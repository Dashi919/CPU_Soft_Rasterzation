using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using CPU_Soft_Rasterization.Math.Vector;
using CPU_Soft_Rasterization.Math.Martix;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Media.Animation;

namespace CPU_Soft_Rasterization
{
    public class Scene
    {
        public float renderTick
        {
           get { return m_renderTick; }
           private set { m_renderTick = value; }
        }
        private float m_renderTick = 0;
       
        public enum RenderType
        {
            Rasterization,
            RayTracing,
        }


        public Camera camera;
        public List<Light> lights;
        public FrameBuffer[] framebuffers;
        public List<Object> sceneObjs;
        public int width, height;
        public float fov, znear, zfar, aspectRatio;
        private BVHTree bvhTree;
        private RenderType m_renderType;
        public bool isShowShadowMap = false;

        public Scene(int width, int height, float fov, float znear, float zfar)
        {
            this.width = width;
            this.height = height;
            this.fov = fov;
            this.znear = znear;
            this.zfar = zfar;
            this.aspectRatio = width / height;
            this.framebuffers = new FrameBuffer[width * height];
            for (int i = 0; i < framebuffers.Length; i++)
            {
                framebuffers[i] = new FrameBuffer();
            }
            lights = new List<Light>();
            sceneObjs = new List<Object>();
            m_renderType = RenderType.Rasterization;
        }

        public int GetObjcetLastIndex()
        {
            return sceneObjs.Count;
        }

        public void Tick(Bitmap bitmap)
        {
           
            switch (m_renderType)
            {
                case RenderType.Rasterization:
                    Rasterization(bitmap);
                    break;
                case RenderType.RayTracing:
                    RayTracing();
                    break;
            }
            renderTick++;
        }

        public void SetRenderType(RenderType type)
        {
            m_renderType = type;
        }
        public void AddLight(Light light)
        {
            lights.Add(light);
        }

        public void AddCam(Camera cam)
        {
            if (camera == null)
                camera = cam;

        }

        public void AddObject(Object obj)
        {
            sceneObjs.Add(obj);
        }

        public void DeleteObject(Object obj)
        {
            sceneObjs.Remove(obj);
        }

        public bool CheckIsReadyToRender()
        {
            if (lights.Count < 1)
                return false;
            if (camera == null)
                return false;
            if (sceneObjs.Count < 1)
                return false;
            return true;
        }

        public int GetScreenIndex(int x, int y)
        {
            return y * width + x;
        }

        public float GetDepthBuffer(int x, int y)
        {
            return framebuffers[GetScreenIndex(x, y)].depthBuffer;
        }

        public void SetDepthBuffer(int x, int y, float z)
        {
            framebuffers[GetScreenIndex(x, y)].depthBuffer = z;
        }
        public void SetColorBuffer(int x, int y, Vector3f color)
        {
            framebuffers[GetScreenIndex(x, y)].colorBuffer = color;

        }
        public void SetNormalBuffer(int x, int y, Vector3f normal)
        {
            framebuffers[GetScreenIndex(x, y)].normalBuffer = normal;
        }

        public void SetObjcetBuffer(int x, int y, Object obj)
        {
            framebuffers[GetScreenIndex(x, y)].objectBuffer = obj;
        }

        public void SetVertexBuffer(int x, int y, Vector3f vPos)
        {
            framebuffers[GetScreenIndex(x, y)].vertexPos = vPos;
        }

        #region Rasterzation

        public Martix4f GetModelMartix(float angle,Vector3f pos)
        {
            angle *= 5 * MathF.PI / 180f;
            Martix4f rotation = new Martix4f(MathF.Cos(angle), 0, MathF.Sin(angle), 0,
                                         0, 1, 0, 0,
                                         MathF.Sin(angle), 0, -MathF.Cos(angle), 0,
                                         0, 0, 0, 1);
            Martix4f translate = Martix4f.TranslateMat(pos);
            return translate * rotation;
        }

        public Martix4f GetViewMartix()
        {
            return camera.GetViewMatrix();
        }
        public Martix4f GetProjectionMartix()
        {

            Martix4f projcection = new Martix4f(1 / (MathF.Tan(fov * MathF.PI / 360) * aspectRatio), 0, 0, 0,
                                                0, 1 / (MathF.Tan(fov * MathF.PI / 360)), 0, 0,
                                                0, 0, -(zfar + znear) / (znear - zfar), -(2 * znear * zfar) / (znear - zfar),
                                                0, 0, -1, 0);
            return projcection;
        }


        public void ShowOrHideShadowMap()
        {
            isShowShadowMap = !isShowShadowMap;
        }

        public Martix4f GetMVPMatrix(Vector3f pos)
        {
            return GetProjectionMartix() * GetViewMartix() * GetModelMartix(renderTick, pos);
        }


        public Vector3f GetCamForward()
        {
            Vector3f camForward = camera.camLookAt;
            return camForward;
        }

        public Vector3f GetCamRight()
        {
            Vector3f camRight = camera.camLookAt.crossProduct(camera.upDir).normalize();
            return camRight;
        }

        public void MoveCam(Vector3f distance)
        {
           camera.position += distance;
        }

        public void RotateCam(Vector3f rotation)
        {
/*
            //水平
            float horizonAngle = rotation.x;

            camera.camLookAt =  new Martix3f(MathF.Cos(horizonAngle), 0, MathF.Sin(horizonAngle),
                                         0, 1, 0,
                                         MathF.Sin(horizonAngle), 0, -MathF.Cos(horizonAngle)) * camera.camLookAt;

            //垂直
            float veticalAngle = rotation.y;
            camera.upDir = new Martix3f(1, 0, 0,
                                        0, -MathF.Cos(veticalAngle), MathF.Sin(veticalAngle),
                                      0, MathF.Sin(veticalAngle), MathF.Cos(veticalAngle)) * camera.upDir;

            */
        }

        public void Rasterization(Bitmap bitmap)
        {
            //Vertex Shader
            for (int i = 0; i < sceneObjs.Count; i++)
            {
                for (int j = 0; j < sceneObjs[i].triangles.Length; j++)
                {
                    for (int l = 0; l < sceneObjs[i].triangles[j].vertexCount; l++)
                    {
                        {
                            VertexShader vertexShader = new VertexShader(sceneObjs[i].triangles[j].vertices[l]);
                            vertexShader.SetMVP(GetMVPMatrix(sceneObjs[i].transform.position));
                            vertexShader.SetSceneHW(width, height);                            
                            vertexShader.Shade();

                        }
                    }
                }

            }

            ShadowMaping shadowMap = new ShadowMaping(this);

           // Task shadowTask = Task.Factory.StartNew(() =>
           // {
                //ShadowMap
            //    shadowMap.GenerateShadowMap();

            //});
          

            //Culling
            Culling culling = new Culling(this);
            var cullTriangles = culling.Cull();


            //Sampling
            Sample sample = new Sample(this);
            sample.SetSampleTriangle(cullTriangles);
            sample.DoSample();



           // shadowTask.Wait();
            //FragmentShader
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    FragmentShader fragmentShader = new FragmentShader(framebuffers[GetScreenIndex(x, y)]);
                    //fragmentShader.SetShadow(shadowMap);
                    fragmentShader.SetLight(lights.ToArray());
                    fragmentShader.SetCamera(camera);
                    SetColorBuffer(x, y, fragmentShader.Shade());
                }
            }

            //Rasterization
           
            Rasterization rasterization = new Rasterization(this);
            rasterization.Render(bitmap);


            
        }
        #endregion

        #region Raytracing

        public void RayTracing()
        {

        }


        public Insertion CastRay(Ray ray)
        {
            Insertion insert = new Insertion();
            return insert;
        }
        #endregion


    }
}
