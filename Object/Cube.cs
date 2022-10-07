using System;
using System.Reflection.Metadata;
using System.Windows.Media.Media3D;
using CPU_Soft_Rasterization.Math.Vector;

namespace CPU_Soft_Rasterization
{
    public class Cube : Object
    {
        static readonly float[] standerVerticesAndNormal = {
        -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
         0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
         0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
         0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
        -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,

        -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
         0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
         0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
         0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
        -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
        -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,

        -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
        -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
        -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
        -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
        -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
        -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,

         0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
         0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
         0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
         0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
         0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
         0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,

        -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
         0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
         0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
         0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,

        -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
         0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
         0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
         0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
        -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
        -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f
    };

        private Vector3f m_color;

        private Vector3f pmin, pmax;

        public Cube(Vector3f pos, Vector3f color)
        {
            transform = new Transform(pos);
            triangles = new Triangle[12];
            material = new Material(Material.MaterialType.Metal);
            int index = 0;

            for (int i = 0; i < 12; i++)
            {
                Vertex[] vertices = new Vertex[3];
                for (int j = 0; j < 3; j++)
                {

                    vertices[j] = new Vertex(
                        new Vector3f(
                        standerVerticesAndNormal[0 + index], 
                        standerVerticesAndNormal[1 + index], 
                        standerVerticesAndNormal[2 + index]),
                        new Vector3f(
                        standerVerticesAndNormal[3 + index], 
                        standerVerticesAndNormal[4 + index],
                        standerVerticesAndNormal[5 + index]),
                        color);
                    index += 6;

                }
                triangles[i] = new Triangle(vertices,this);

            }
            m_color = color;
            pmin = pos - new Vector3f(0.5f);
            pmax = pos + new Vector3f(0.5f);

        }



        public Cube(Vector3f pos, Vector3f rotation, Vector3f scale)
        {
            transform = new Transform(pos, rotation, scale);
            m_color = new Vector3f(1f);
            triangles = new Triangle[12];
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    triangles[i].vertices[j] = new Vertex( new Vector3f((standerVerticesAndNormal[0 + (35 - i * j) * 6] * scale.x), standerVerticesAndNormal[1 + (35 - i * j) * 6] * scale.y, standerVerticesAndNormal[2 + (35 - i * j) * 6] * scale.y),
                        new Vector3f(standerVerticesAndNormal[3 + (35 - i * j) * 6], standerVerticesAndNormal[4 + (35 - i * j) * 6], standerVerticesAndNormal[5 + (35 - i * j) * 6]), m_color);
                }
            }
            pmin = pos - new Vector3f(0.5f);
            pmax = pos + new Vector3f(0.5f);



        }

        public override bool IsInsideObjcet(Vector3f pos)
        {
            if (pos.x <= pmax.x && pos.x >= pmin.x)
            {
                if (pos.y <= pmax.y && pos.y >= pmin.y)
                {
                    if (pos.z <= pmax.z && pos.z >= pmin.z)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


    }
}
