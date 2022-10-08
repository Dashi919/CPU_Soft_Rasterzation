using System;


namespace CPU_Soft_Rasterization.Math.Vector
{
    public class Vector2f
    {
        public float x, y;
        public Vector2f(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        static Vector2f Identity()
        {
            return new Vector2f(1, 1);
        }
     
        Vector2f normalize()
        {
            float distance = Distance();
            return new Vector2f(x / distance, y / distance);
        }

        public float Distance()
        {
            return MathF.Sqrt(x * x + y * y);
        }

        public float dotProduct(Vector2f vector)
        {
            return x * vector.x + y * vector.y;
        }
        public static Vector2f operator +(Vector2f a, Vector2f b)
        {
            return new Vector2f(a.x + b.x, a.y + b.y);
        }
        public static Vector2f operator -(Vector2f a, Vector2f b)
        {
            return new Vector2f(a.x - b.x, a.y - b.y);
        }
    }
}
