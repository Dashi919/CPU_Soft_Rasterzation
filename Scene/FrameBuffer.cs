using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPU_Soft_Rasterization.Math.Vector;

namespace CPU_Soft_Rasterization
{
    public class FrameBuffer
    {
        public Vector3f colorBuffer, normalBuffer,vertexPos;
        public float depthBuffer;

        public Object objectBuffer ;

        public FrameBuffer()
        {
            colorBuffer = new Vector3f(1f, 1f, 1f);
            normalBuffer = new Vector3f(0, 0, 0);
            depthBuffer = 1000;
        }


        public void Clear()
        {
            colorBuffer = new Vector3f(1f, 1f, 1f);
            normalBuffer = new Vector3f(0, 0, 0);
            depthBuffer = 1000;
        }
    }
}
