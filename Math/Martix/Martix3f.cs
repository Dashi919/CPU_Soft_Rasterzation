using System;
using CPU_Soft_Rasterization.Math.Vector;

namespace CPU_Soft_Rasterization.Math.Martix
{
    public class Martix3f
    {
        public Martix3f(float[,] data)
        {
            this.data = data;
        }
        public Martix3f(float a11, float a12, float a13, float a21, float a22, float a23, float a31, float a32, float a33)
        {
            data = new float[3, 3];
            data[0, 0] = a11;
            data[0, 1] = a12;
            data[0, 2] = a13;
            data[1, 0] = a21;
            data[1, 1] = a22;
            data[1, 2] = a23;
            data[2, 0] = a31;
            data[2, 1] = a32;
            data[2, 2] = a33;

        }

        public Martix3f(Vector3f col1, Vector3f col2, Vector3f col3)
        {
            data = new float[3, 3] { { col1.x, col1.y, col1.z }, { col2.x, col2.y, col2.z }, { col3.x, col3.y, col3.z } };
        }
        float[,] data;

        public float this[int index, int index2]
        {
            get => data[index, index2];
            set => data[index, index2] = value;
        }

        public static Martix3f Identity()
        {
            return new Martix3f(1, 0, 0,
                                0, 1, 0,
                                0, 0, 1);
        }
        /// <summary>
        /// left mutiply
        /// </summary>
        /// <param name="mat1"></param>
        /// <param name="mat2"></param>
        /// <returns></returns>
        public static Martix3f operator *(Martix3f mat1, Martix3f mat2)
        {
            float a11 = mat2.Row(1).DotProduct(mat1.Col(1));
            float a12 = mat2.Row(1).DotProduct(mat1.Col(2));
            float a13 = mat2.Row(1).DotProduct(mat1.Col(3));


            float a21 = mat2.Row(2).DotProduct(mat1.Col(1));
            float a22 = mat2.Row(2).DotProduct(mat1.Col(2));
            float a23 = mat2.Row(2).DotProduct(mat1.Col(3));

            float a31 = mat2.Row(3).DotProduct(mat1.Col(1));
            float a32 = mat2.Row(3).DotProduct(mat1.Col(2));
            float a33 = mat2.Row(3).DotProduct(mat1.Col(3));

            return new Martix3f(a11, a12, a13, a21, a22, a23, a31, a32, a33);
        }

        public static Vector3f operator *(Martix3f mat, Vector3f vector)
        {
            float x = mat.Row(1).DotProduct(vector);
            float y = mat.Row(2).DotProduct(vector);
            float z = mat.Row(3).DotProduct(vector);
            return new Vector3f(x, y, z);
        }

        public Vector3f Col(int col)
        {
            if (0 < col && col < 4)
            { return new Vector3f(data[0, col - 1], data[1, col - 1], data[2, col - 1]); }
            return null;

        }
        //行
        public Vector3f Row(int row)
        {
            if (0 < row && row < 4)
            {
                return new Vector3f(data[row - 1, 0], data[row - 1, 1], data[row - 1, 2]);
            }
            return null;
        }


        public static Martix3f RotateMat(Vector3f angles)
        {

            float xangle = angles.x * MathF.PI / 180;
            Martix3f xmat = new Martix3f(1, 0, 0, 
                                         0, MathF.Cos(xangle), -MathF.Sin(xangle),
                                         0, MathF.Sin(xangle), MathF.Cos(xangle));
            float yangle = angles.x  * MathF.PI / 180;

            Martix3f ymat = new Martix3f(MathF.Cos(yangle), 0, MathF.Sin(yangle),
                                         0, 1, 0,
                                         -MathF.Sin(yangle), 0, MathF.Cos(yangle));
            float zangle = angles.z * MathF.PI / 180;

            Martix3f zmat = new Martix3f(MathF.Cos(zangle), -MathF.Sin(zangle), 0,
                                          MathF.Sin(zangle), MathF.Cos(zangle), 0,
                                         0, 0, 1  );
            return zmat * ymat * xmat;
        }
    }
}
