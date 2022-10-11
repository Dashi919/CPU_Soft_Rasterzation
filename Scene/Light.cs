using System;
using CPU_Soft_Rasterization.Math.Martix;
using CPU_Soft_Rasterization.Math.Vector;

namespace CPU_Soft_Rasterization
{
    public class Light 
    {
        public Vector3f position, lightUp, lightFocus, lightRight, color;

        public float intensity;


        public Light(Vector3f position, Vector3f lightFocus,float intensity)
        {
            this.color = new Vector3f(1);
            this.lightFocus = (lightFocus).normalize();
            lightUp = new Vector3f(0, 0, 1);
            lightRight = this.lightFocus.crossProduct(lightUp).normalize();
            this.position = position;
            this.intensity = intensity;
        }


        public Vector3f GetAttenuation(Vector3f targetPosition)
        {
            // float distance = (position - targetPosition).Distance();
            //  return color * intensity / (distance * distance);
            return color;
        }

        public Martix4f GetWorldToLightMatrix()
        {
            Martix4f TView = new Martix4f(1, 0, 0, -position.x,
                                          0, 1, 0, -position.y,
                                          0, 0, 1, -position.z,
                                          0, 0, 0, 1);
            Martix4f RView = new Martix4f(lightRight, lightUp, -lightFocus);

            return   RView * TView;
        }
    }
}
