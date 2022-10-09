using System;
using CPU_Soft_Rasterization.Math.Martix;
using CPU_Soft_Rasterization.Math.Vector;

namespace CPU_Soft_Rasterization
{
    public class Camera
    {
        public Vector3f position;
        public Vector3f camDir;
        public Vector3f upDir;
        public Vector3f rightDir;

        public float maxDepth = 1000f;

        public Camera(Vector3f pos, Vector3f camDir, Vector3f rightDir)
        {
            position = pos;
            this.camDir = camDir.normalize();
            this.rightDir = rightDir.normalize();

        }

        public Martix4f GetViewMatrix()
        {
            upDir = camDir.crossProduct(rightDir).normalize();
            Martix4f TView = new Martix4f( 1, 0, 0, -position.x,
                                           0, 1, 0, -position.y,
                                           0, 0, 1, -position.z,
                                           0, 0, 0, 1);
            Martix4f RView = new Martix4f(rightDir, upDir, -camDir);
            return TView * RView;
         } 
        
    }
}
