using System;
using CPU_Soft_Rasterization.Math.Martix;
using CPU_Soft_Rasterization.Math.Vector;

namespace CPU_Soft_Rasterization
{
    public class Camera
    {
        public Vector3f position;
        public Vector3f camLookAt;
        public Vector3f upDir;
        public Vector3f rightDir;

        public float maxDepth = 1000f;

        public Camera(Vector3f pos, Vector3f lookAt, Vector3f upDir)
        {
            position = pos;
            this.camLookAt = lookAt.normalize();
            this.upDir = upDir.normalize();

        }

        public Martix4f GetViewMatrix()
        {
            rightDir = camLookAt.crossProduct(upDir).normalize();
            Martix4f TView = new Martix4f( 1, 0, 0, -position.x,
                                           0, 1, 0, -position.y,
                                           0, 0, 1, -position.z,
                                           0, 0, 0, 1);
            Martix4f RView = new Martix4f(rightDir, upDir, -camLookAt);
            return TView * RView;
         } 
        
    }
}
