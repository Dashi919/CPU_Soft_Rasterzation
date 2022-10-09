using System;
using CPU_Soft_Rasterization.Math.Vector;

namespace CPU_Soft_Rasterization
{
    public class Light 
    {
        public Vector3f poistion, lightUp, lightFocus, lightRight, color;

        public float intensity;

        public Light(Vector3f poistion,Vector3f lightFocus,float intensity)
        {
            this.color = new Vector3f(1);
            this.lightFocus = lightFocus.normalize();
            lightRight = new Vector3f(-1, 0, 0);
            lightUp = this.lightFocus.crossProduct(lightRight).normalize();
            this.poistion = poistion;
            this.intensity = intensity;
        }


        public Vector3f GetRadiance(Vector3f targetPosition)
        {
            // float distance = (poistion - targetPosition).Distance();
            //  return color * intensity / (distance * distance);
            return color;
        }
    }
}
