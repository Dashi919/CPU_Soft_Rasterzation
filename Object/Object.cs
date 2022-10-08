using System;
using CPU_Soft_Rasterization.Math.Vector;

namespace CPU_Soft_Rasterization
{
    public abstract class Object
    {
        public Transform transform { get; set; }

        public Triangle[] triangles { get; set; }

        public Material material { get; set; }


        public abstract bool IsInsideObjcet(Vector3f pos);
    
    }
}
