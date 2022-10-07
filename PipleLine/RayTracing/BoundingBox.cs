using System;
using CPU_Soft_Rasterization.Math.Vector;

namespace CPU_Soft_Rasterization
{
    public class BoundingBox
    {
        Vector3f pMax,pMin;

        public bool IsInsert(Ray ray)
        {
            return false;
        }
    }
}
