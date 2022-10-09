using System;

namespace CPU_Soft_Rasterization.Math.Vector
{
    public class Vector4f
    {
        public float x, y, z, w;
        public Vector4f(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vector3f toVector3()
        {
            return new Vector3f(x, y, z);
        }

        public static Vector4f operator +(Vector4f a, Vector4f b)
        {
            return new Vector4f(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        }

        public static Vector4f operator -(Vector4f a, Vector4f b)
        {
            return new Vector4f(a.x - b.x, a.y - b.y, a.z - b.z,a.w - b.w);
        }

        public static Vector4f operator /(Vector4f a, float b)
        {
            return new Vector4f(a.x / b, a.y / b, a.z / b, a.w / b);
        }

        public static Vector4f operator *(Vector4f a, float b)
        {
            return new Vector4f(a.x * b, a.y * b, a.z * b,a.w * b);
        }
        public float DotProduct(Vector4f b)
        {
            return x * b.x + y * b.y + z * b.z + w * b.w ;
        }
    }
}
