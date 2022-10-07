using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Drawing.Imaging;
using System.Timers;
using System.Threading;
using System.IO;
using CPU_Soft_Rasterization.Math.Vector;
using System.ComponentModel;
using System.Reflection;

namespace CPU_Soft_Rasterization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread renderTickThread;
        Scene mainScene;
        bool IsChanged;
        float oldMouseX, oldMouseY, camMoveSpeed, camRotateSpeed;
        BindingList<SceneObject> sceneObjectList;
        delegate void MoveCamera(Vector3f distance);
        MoveCamera cameraController;
        public MainWindow()
        {
            InitializeComponent();
            sceneObjectList = new BindingList<SceneObject>();
            hierachy.ItemsSource = sceneObjectList;

            camMoveSpeed = 5;
            camRotateSpeed = 0.1f;
            mainScene = new Scene((int)sceneImage.Width, (int)sceneImage.Height, 90.0f, 0.1f, 1000.0f);
            Camera mainCam = new Camera(new Vector3f(0, 0, -5), new Vector3f(0, 0, 1), new Vector3f(0, 1, 0));
            mainScene.AddCam(mainCam);
            Light mainLight = new Light(new Vector3f(10), new Vector3f(500));
            mainScene.AddLight(mainLight);
            Light secLight = new Light(new Vector3f(-10), new Vector3f(500));
            mainScene.AddLight(secLight);
            Cube cube = new Cube(new Vector3f(-1, -1, 2), new Vector3f(0, 0, 1f));
            Add_Scene_Objcet(cube, "Cube");




            renderTickThread = new Thread(() =>
                {
                    while (true)
                    {

                        if (mainScene.CheckIsReadyToRender())
                        {
                            Bitmap bitmap = new Bitmap(mainScene.width, mainScene.height);

                            mainScene.Tick(bitmap);
                            Action action = () =>
                            {
                                sceneImage.Source = BitmapToImageSource(bitmap);
                            };
                            sceneImage.Dispatcher.BeginInvoke(action);
                        }
                       
                    }
                });
            renderTickThread.IsBackground = true;
            renderTickThread.Start();

        }

        private bool CheckInput()
        {
            return false;
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            int index = mainScene.GetObjcetLastIndex();
            Cube cube = new Cube(new Vector3f(index), new Vector3f(1f / index));
            Add_Scene_Objcet(cube, "Cube" + index.ToString());
            IsChanged = true;

        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (hierachy.ItemsSource != null)
            {
                SceneObject sj = (SceneObject)hierachy.SelectedItem;
                mainScene.DeleteObject(sj.obj);
                sceneObjectList.Remove(sj);
                IsChanged = true;
            }
        }

        private void RaytrcingButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("进行离线光追吗？可能花费时间比较长！", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                //mainScene.RayTracing();
                MessageBox.Show("功能爆肝开发中");
            }
        }


        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowObjcetProperties((SceneObject)hierachy.SelectedItem);
        }



        private void ShowObjcetProperties(SceneObject sj)
        {
            inspector.Items.Clear();
            Type type = typeof(Object);
            System.Reflection.PropertyInfo[] propertyInfo = type.GetProperties();
            foreach (var info in propertyInfo)
            {
                inspector.Items.Add(info.Name.ToString());
            }
        }



        public void Add_Scene_Objcet(Object obj, string name)
        {
            SceneObject sj = new SceneObject();
            sj.obj = obj;
            sj.Title = name;
            sceneObjectList.Add(sj);
            mainScene.AddObject(obj);
        }

    
        public class SceneObject
        {
            public string Title { get; set; }
            public Object obj { get; set; }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {

            var point = e.GetPosition(this);
            float x = (float)point.X;
            float y = (float)point.Y;
            if(oldMouseX!= x && oldMouseY != y)
            {
                Vector3f rotation = new Vector3f(x - oldMouseX, y - oldMouseY,0) * camRotateSpeed;
                cameraController = mainScene.RotateCam;
                cameraController(rotation);
                oldMouseX = x;
                oldMouseY = y;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Vector3f? camMove =  null;
            if (e.Key == Key.W)
            {
               camMove = mainScene.GetCamForward() * camMoveSpeed;
         
            }
            else if (e.Key == Key.S)
            {
                camMove = mainScene.GetCamForward() * -camMoveSpeed;


            }
            else if (e.Key == Key.A)
            {
                 camMove = -mainScene.GetCamRight() * camMoveSpeed;
            }
            else if (e.Key == Key.D)

            {
               camMove = mainScene.GetCamRight() * camMoveSpeed;

            }
            if (camMove != null)
            {
                cameraController = mainScene.MoveCam;
                cameraController(camMove);
            }
        }
    }
}
