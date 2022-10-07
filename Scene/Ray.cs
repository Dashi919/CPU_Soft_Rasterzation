using System;
using CPU_Soft_Rasterization.Math.Vector;

namespace CPU_Soft_Rasterization
{
    public class Ray
    {
        public float tmin, tmax,depth;
        public Vector3f origin,direction;
        public Ray(Vector3f ori,Vector3f dir,float depth)
        {
            origin = ori;
            direction = dir;
            this.depth = depth;
        }

        public Insertion Cast(Scene scene)
        {
            Insertion insert = new Insertion();

            return insert;
        }



    }
    public struct Insertion
    {
        public bool isHappened;
        public object insertObj;
        public Vector3f insertPoint, insertNormal, insertColor;
    }
}
