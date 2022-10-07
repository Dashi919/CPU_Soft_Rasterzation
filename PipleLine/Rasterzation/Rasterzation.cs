using System;
using System.Drawing;
using System.Windows.Media.Media3D;

namespace CPU_Soft_Rasterization
{
    public class Rasterization
    {
        private Scene scene;
        
        public Rasterization(Scene scene)
        {
            this.scene = scene;
          
        }

        public void Render(Bitmap bitmap)
        {

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    var frameBuffer = scene.framebuffers[scene.GetScreenIndex(i, j)];
                    int r = (int)(frameBuffer.colorBuffer.x * 255);
                    int g = (int)(frameBuffer.colorBuffer.y * 255);
                    int b = (int)(frameBuffer.colorBuffer.z * 255);
                    Color color = Color.FromArgb(1, r, g, b);
                    bitmap.SetPixel(i, j, color);
                    frameBuffer.Clear();
                }
            }
        }

      
    }
}
