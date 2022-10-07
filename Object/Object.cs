using System;
using CPU_Soft_Rasterization.Math.Vector;

namespace CPU_Soft_Rasterization
{
    public abstract class Object
    {
        public Transform transform;

        public Triangle[] triangles;

        public Material material;


        public abstract bool IsInsideObjcet(Vector3f pos);
    
    }
}
