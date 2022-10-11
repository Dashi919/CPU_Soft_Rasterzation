using System;
using CPU_Soft_Rasterization.Math.Vector;
using CPU_Soft_Rasterization.Math.Martix;

namespace CPU_Soft_Rasterization
{
    public abstract class Object
    {
        public bool isLight = false;
        public Martix4f modelMatrix;
        public Transform transform { get; set; }

        public Triangle[] triangles;

        public Material material { get; set; }

        public abstract bool IsInsideObjcet(Vector3f pos);
    
    }
}
