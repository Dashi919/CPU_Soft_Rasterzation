using System;
using System.Collections.Generic;
using CPU_Soft_Rasterization.Math.Vector;

namespace CPU_Soft_Rasterization
{
    public class Vertex
    {
        public Vector3f localPos;
        public Vector4f worldPos;
        public Vector3f normal;
        public Vector3f worldNormal;
        public Vector3f color;
        public Vector4f screenPos;


        public Vertex(Vector3f pos, Vector3f normal, Vector3f color)
        {
            this.localPos = pos;
            this.normal = normal;
            this.color = color;
        }
    }
}
