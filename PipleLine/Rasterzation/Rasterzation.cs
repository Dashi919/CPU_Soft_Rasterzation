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
                    int r, g, b;

                    var frameBuffer = scene.framebuffers[scene.GetScreenIndex(i, j)];
                    r = (int)(frameBuffer.colorBuffer.x * 255);
                    g = (int)(frameBuffer.colorBuffer.y * 255);
                    b = (int)(frameBuffer.colorBuffer.z * 255);
                    frameBuffer.Clear();
                    Color color = Color.FromArgb(1, r, g, b);
                    bitmap.SetPixel(i, j, color);
                }
            }
        }

    }
}
