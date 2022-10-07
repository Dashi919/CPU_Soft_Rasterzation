using System;
using CPU_Soft_Rasterization.Math.Martix;

namespace CPU_Soft_Rasterization.Math.Vector
{
    public class Vector3f
    {
        public float x, y, z;
        public Vector3f(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Vector3f(float xyz)
        {
            x = xyz;
            y = xyz;
            z = xyz;
        }

        public static Vector3f Identity()
        {
            return new Vector3f(1f, 1f, 1f);
        }

        public static Vector3f Zero()
        {
            return new Vector3f(0f, 0f, 0f);
        }
        public float DotProduct(Vector3f vector)
        {
            return x * vector.x + y * vector.y + z * vector.z;
        }

        public Vector3f Clamp(Vector3f min, Vector3f max)
        {
            float newX, newY, newZ;
            if (x < min.x)
                newX = min.x;
            else if (x > max.x)
                newX = max.x;
            else
                newX = x;

            if (y < min.y)
                newY = min.y;
            else if (y > max.y)
                newY = max.y;
            else
                newY = y;

            if (z < min.z)
                newZ = min.z;
            else if (z > max.z)
                newZ = max.z;
            else
                newZ = z;

            return new Vector3f(newX, newY, newZ);
        }


        public Vector3f normalize()
        {
            float distance = Distance();
            return new Vector3f(x / distance, y / distance, z / distance);
        }

        public Vector3f crossProduct(Vector3f vector)
        {
            return new Vector3f(
                y * vector.z - z * vector.y,
                z * vector.x - x * vector.z,
                x * vector.y - y * vector.x);
        }

        //用于颜色的线性叠加
        public Vector3f CwiseProduct(Vector3f vector)
        {
            return new Vector3f(
                x * vector.x,
                y * vector.y,
                z * vector.z);
        }

        public float Distance()
        {
            return MathF.Sqrt(x * x + y * y + z * z);
        }

        public static Vector3f operator +(Vector3f a, Vector3f b)
        {
            return new Vector3f(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        public static Vector3f operator -(Vector3f a, Vector3f b)
        {
            return new Vector3f(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3f operator -(Vector3f a)
        {
            return new Vector3f(-a.x ,- a.y  ,-a.z );
        }

        public static Vector3f operator *(Vector3f a, float b)
        {
            return new Vector3f(a.x * b, a.y * b, a.z * b);
        }

        public static Vector3f operator /(Vector3f a, float b)
        {
            return new Vector3f(a.x / b, a.y / b, a.z / b);
        }
        public Vector4f PointToVector4()
        {
            return new Vector4f(x, y, z, 1);
        }

        public Vector4f DirToVector4()
        {
            return new Vector4f(x, y, z, 0);
        }
    }
}
