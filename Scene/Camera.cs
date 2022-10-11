using System;
using CPU_Soft_Rasterization.Math.Martix;
using CPU_Soft_Rasterization.Math.Vector;
using System.Windows;
namespace CPU_Soft_Rasterization
{
    public class Camera
    {
        public Transform transform;

        public Vector3f camDir { get; set; }
        public Vector3f upDir { get; set; }
        
        public Vector3f rightDir;

        public float maxDepth = 1000f;

        public Camera(Vector3f pos, Vector3f camDir, Vector3f upDir)
        {
            transform = new Transform(pos);
            this.camDir =  camDir;
            this.upDir = upDir;
            rightDir = camDir.crossProduct(upDir);

        }

        public Martix4f GetViewMatrix()
        {
            Martix4f modelView = Martix4f.RotateMat(new Vector3f(0,0, 0));

            Martix4f TView = new Martix4f( 1, 0, 0, -transform.position.x,
                                           0, 1, 0, -transform.position.y,
                                           0, 0, 1, -transform.position.z,
                                           0, 0, 0, 1);
            Martix4f RView = new Martix4f(rightDir, upDir, camDir);

            return   RView * TView ;
         } 
        
    }
}
