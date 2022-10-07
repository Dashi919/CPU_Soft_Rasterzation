using System;
using CPU_Soft_Rasterization.Math.Vector;

namespace CPU_Soft_Rasterization
{
    public class Sphere : Object
    {
        private float m_radius;
        private Vector3f m_center;

        public Sphere (Vector3f center,float radius)
        {
            m_radius = radius;
            m_center = center;
            
        }

        public override bool IsInsideObjcet(Vector3f pos)
        {
            return false;
        }
    }
}
