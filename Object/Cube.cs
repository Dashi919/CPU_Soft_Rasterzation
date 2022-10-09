using System;
using System.Reflection.Metadata;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using CPU_Soft_Rasterization.Math.Martix;
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
            material.baseColor = color;
            pmin = pos - new Vector3f(0.5f);
            pmax = pos + new Vector3f(0.5f);
            Martix4f rotation = Martix4f.RotateMat(transform.rotation);
            Martix4f translate = Martix4f.TranslateMat(transform.position);
            Martix4f scale = Martix4f.ScaleMat(transform.scale);
            modelMatrix = translate * rotation * scale;
        }



        public Cube(Vector3f pos, Vector3f rotation, Vector3f scale)
        {
            transform = new Transform(pos, rotation, scale);
            triangles = new Triangle[12];
            material = new Material(Material.MaterialType.Metal);
            material.baseColor = new Vector3f(1f);
            int index = 0;
            for (int i = 0; i < 12; i++)
            {
                Vertex[] vertices = new Vertex[3];
                for (int j = 0; j < 3; j++)
                {

                    vertices[j] = new Vertex( 
                        new Vector3f(
                        standerVerticesAndNormal[0 + index] , 
                        standerVerticesAndNormal[1 + index] , 
                        standerVerticesAndNormal[2 + index]),
                        new Vector3f(
                        standerVerticesAndNormal[3 + index],
                        standerVerticesAndNormal[4 + index], 
                        standerVerticesAndNormal[5 + index]), 
                        material.baseColor);
                        index += 6;
                }
                triangles[i] = new Triangle(vertices, this);

            }
            material.baseColor = new Vector3f(0.5f);
            pmin = pos - new Vector3f(0.5f);
            pmax = pos + new Vector3f(0.5f);
            Martix4f rotateMat = Martix4f.RotateMat(transform.rotation);
            Martix4f translateMat = Martix4f.TranslateMat(transform.position);
            Martix4f scaleMat = Martix4f.ScaleMat(transform.scale);
            modelMatrix = rotateMat * translateMat * scaleMat;


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
