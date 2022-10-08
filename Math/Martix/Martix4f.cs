using System;

using CPU_Soft_Rasterization.Math.Vector;

namespace CPU_Soft_Rasterization.Math.Martix
{
    public class Martix4f
    {
        float[,] data;

        public Martix4f(float a11, float a12, float a13, float a14,
            float a21, float a22, float a23, float a24,
            float a31, float a32, float a33, float a34,
            float a41, float a42, float a43, float a44)
        {
            data = new float[4, 4] {
                    { a11, a12, a13, a14 },
                    { a21, a22, a23, a24 },
                    { a31, a32, a33, a34 },
                    { a41, a42, a43, a44 }};
        }

        public Martix4f(Vector3f row1, Vector3f row2, Vector3f row3)
        {
            data = new float[4, 4] {
                    { row1.x, row1.y, row1.z,  0 },
                    { row2.x, row2.y, row2.z,  0 },
                    { row3.x, row3.y, row3.z , 0 },
                    { 0     , 0     , 0      , 1 } };
        }
        public static Martix4f Identity()
        {
            return new Martix4f(1, 0, 0, 0,
                                0, 1, 0, 0,
                                0, 0, 1, 0,
                                0, 0, 0, 1);
        }
        public float this[int index1, int index2]
        {
            get => data[index1, index2];
            set => data[index1, index2] = value;
        }
        //列
        public Vector4f Col(int col)
        {
            if ( 0 < col && col < 5)
            { return new Vector4f(data[0, col-1], data[1, col-1], data[2, col - 1], data[3, col - 1]); }
            return null;
            
        }
        //行
        public Vector4f Row(int row)
        {
            if (0 < row && row < 5)
            { 
                return new Vector4f(data[row-1, 0], data[row-1, 1], data[row-1, 2], data[row-1, 3]);
            }
            return null;
        }
        public static Martix4f operator *(Martix4f mat1, Martix4f mat2)
        {
            float a11 = mat1.Row(1).DotProduct(mat2.Col(1));
            float a12 = mat1.Row(1).DotProduct(mat2.Col(2));
            float a13 = mat1.Row(1).DotProduct(mat2.Col(3));
            float a14 = mat1.Row(1).DotProduct(mat2.Col(4));

            float a21 = mat1.Row(2).DotProduct(mat2.Col(1));
            float a22 = mat1.Row(2).DotProduct(mat2.Col(2));
            float a23 = mat1.Row(2).DotProduct(mat2.Col(3));
            float a24 = mat1.Row(2).DotProduct(mat2.Col(4));

            float a31 = mat1.Row(3).DotProduct(mat2.Col(1));
            float a32 = mat1.Row(3).DotProduct(mat2.Col(2));
            float a33 = mat1.Row(3).DotProduct(mat2.Col(3));
            float a34 = mat1.Row(3).DotProduct(mat2.Col(4));


            float a41 = mat1.Row(4).DotProduct(mat2.Col(1));
            float a42 = mat1.Row(4).DotProduct(mat2.Col(2));
            float a43 = mat1.Row(4).DotProduct(mat2.Col(3));
            float a44 = mat1.Row(4).DotProduct(mat2.Col(4));


            return new Martix4f(a11, a12, a13, a14, a21, a22, a23, a24, a31, a32, a33, a34, a41, a42, a43, a44);
        }

        public static Vector4f operator *(Martix4f mat, Vector4f vec)
        {
            float x = mat.Row(1).DotProduct(vec);
            float y = mat.Row(2).DotProduct(vec);
            float z = mat.Row(3).DotProduct(vec);
            float w = mat.Row(4).DotProduct(vec);
            return new Vector4f(x, y, z, w);
        }

        public static Martix4f TranslateMat(Vector3f trans)
        {
            return new Martix4f(1, 0, 0, trans.x,
                                0, 1, 0, trans.y,
                                0, 0, 1, trans.z,
                                0, 0, 0, 1);
        }

        public static Martix4f RotateMat(float angle, Vector3f axis)
        {
            float distance = axis.Distance();

            float xangle = angle * axis.x / distance * MathF.PI / 180;
            Martix4f xmat = new Martix4f(1, 0, 0, 0,
                                         0, -MathF.Cos(xangle), MathF.Sin(xangle), 0,
                                         0, MathF.Sin(xangle), MathF.Cos(xangle), 0,
                                         0, 0, 0, 1);
            float yangle = angle * axis.y / distance * MathF.PI / 180;

            Martix4f ymat = new Martix4f(MathF.Cos(yangle), 0, MathF.Sin(yangle), 0,
                                         0, 1, 0, 0,
                                         MathF.Sin(yangle), 0, -MathF.Cos(yangle), 0,
                                         0, 0, 0, 1);
            float zangle = angle * axis.z / distance * MathF.PI / 180;

            Martix4f zmat = new Martix4f(-MathF.Cos(zangle), MathF.Sin(zangle), 0, 0,
                                          MathF.Sin(zangle), MathF.Cos(zangle), 0, 0,
                                         0, 0, 1, 0,
                                         0, 0, 0, 1);
            return zmat * (ymat * xmat);
        }


        public static Martix4f ScaleMat(Vector3f scale)
        {
            return new Martix4f(scale.x, 0, 0, 0,
                                0, scale.y, 0, 0,
                                0, 0, scale.z, 0,
                                0, 0, 0, 1);
        }



        public static Martix4f OrthogonalMartix(float left, float right, float bottom, float top, float near, float far)
        {

            Martix4f translate = TranslateMat(new Vector3f(-(right + left) / 2, -(bottom + top) / 2, -(near + far) / 2));
            Martix4f scale = ScaleMat(new Vector3f(2 / (right - left), 2 / (top - bottom), 2 / (near - far)));
            return scale * translate;
        }
    }
}
