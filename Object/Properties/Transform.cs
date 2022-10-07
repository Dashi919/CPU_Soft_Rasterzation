using System;
using CPU_Soft_Rasterization.Math.Vector;

namespace CPU_Soft_Rasterization
{

    public class Transform
    {
        public Vector3f position;
        public Vector3f rotation;
        public Vector3f scale;

        public Transform(Vector3f position, Vector3f rotation, Vector3f scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }


        public Transform(Vector3f pos)
        {
            position = pos;
            rotation = new Vector3f(0.0f, 0.0f, 0.0f);
            scale = new Vector3f(1.0f, 1.0f, 1.0f);
        }

        public void Translate(Vector3f translate)
        {
            position += translate;
        }

        public void Scale(Vector3f scale)
        {
            this.scale = scale;
        }

        public void Rotate(float angle, Vector3f axis)
        {
            rotation += axis * angle;
        }
    }
}
