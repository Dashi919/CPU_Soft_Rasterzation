using System;
using CPU_Soft_Rasterization.Math.Vector;

namespace CPU_Soft_Rasterization
{
    public class Light 
    {
        public Vector3f poistion;
        public Vector3f intensity; 

        public Light(Vector3f poistion, Vector3f intensity)
        {
            this.poistion = poistion;
            this.intensity = intensity;
        }


        public Vector3f GetRadiance(Vector3f targetPosition)
        {
            float distance = (poistion - targetPosition).Distance();
            return intensity / (distance * distance);
        }
    }
}
