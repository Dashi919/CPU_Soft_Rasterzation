using System;
using CPU_Soft_Rasterization.Math.Vector;

namespace CPU_Soft_Rasterization
{
    public class Triangle : Mesh
    {
        public Vertex[] vertices;
        public readonly int vertexCount = 3;
        public Vertex[] fragments;
    
        public Triangle(Vertex[] vertex,Object parent)
        {
            parentObjcet = parent;
            if (vertex.Length == 3)
            {
                vertices = new Vertex[3];
                for (int i = 0; i < vertexCount; i++)
                {
                    vertices[i] = vertex[i];
                }
            }
        }

        public bool IsInsideTriangle(Vector3f point)
        {
            float c1 = (point.x - vertices[0].screenPos.x) * (vertices[0].screenPos.y - vertices[1].screenPos.y) 
                - (vertices[0].screenPos.x - vertices[1].screenPos.x) * (point.y - vertices[0].screenPos.y);
            float c2 = (point.x - vertices[1].screenPos.x) * (vertices[1].screenPos.y - vertices[2].screenPos.y) 
                - (vertices[1].screenPos.x - vertices[2].screenPos.x) * (point.y - vertices[1].screenPos.y);
            float c3 = (point.x - vertices[2].screenPos.x) * (vertices[2].screenPos.y - vertices[0].screenPos.y) 
                - (vertices[2].screenPos.x - vertices[0].screenPos.x) * (point.y - vertices[2].screenPos.y);
            return (c1 >= 0 && c2 >= 0 && c3 >= 0) || (c1 <= 0 && c2 <= 0 && c3 <= 0);
        }

        public Vector3f ComputeBarycentricCoordinateInViewPort(Vector3f point)
        {
            Vector3f a = vertices[0].screenPos.toVector3();
            Vector3f b = vertices[1].screenPos.toVector3();
            Vector3f c = vertices[2].screenPos.toVector3();
            float c1 = (point.x * (b.y - c.y) + (c.x - b.x) * point.y + b.x * c.y - c.x * b.y) / (a.x * (b.y - c.y) + (c.x - b.x) * a.y + b.x * c.y - c.x * b.y);
            float c2 = (point.x * (c.y - a.y) + (a.x - c.x) * point.y + c.x * a.y - a.x * c.y) / (b.x * (c.y - a.y) + (a.x - c.x) * b.y + c.x * a.y - a.x * c.y);
            float c3 = (point.x * (a.y - b.y) + (b.x - a.x) * point.y + a.x * b.y - b.x * a.y) / (c.x * (a.y - b.y) + (b.x - a.x) * c.y + a.x * b.y - b.x * a.y);
            return new Vector3f(c1, c2, c3);
        }

    }
}
