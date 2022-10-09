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
        public Vector3f? colorBuffer, normalBuffer;
        public Vector4f? vertexPos;
        public float depthBuffer;

        public Object? objectBuffer ;

        public FrameBuffer()
        {

            depthBuffer = 1000;
        }


        public void Clear()
        {
            colorBuffer = null;
            normalBuffer = null;
            objectBuffer = null;
            vertexPos = null;
            depthBuffer = 1000;
        }
    }
}
