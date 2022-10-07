using System;
using CPU_Soft_Rasterization.Math.Martix;
using CPU_Soft_Rasterization.Math.Vector;

namespace CPU_Soft_Rasterization
{
    public class VertexShader
    {
        Martix4f MVP;
        Vertex m_vertex;
        float width, height;
        public VertexShader(Vertex vertex)
        {
            m_vertex = vertex;
        }
        public Vector3f GetVertexpos()
        {
            return m_vertex.pos;
        }

        public void SetSceneHW(float w,float h)
        {
            width = w;
            height = h;
        }

        public void SetMVP(Martix4f matrix)
        {
            MVP = matrix;
        }
        public void Shade()
        {
            m_vertex.screenPos = MVP * m_vertex.pos.PointToVector4();

            //齐次除法
            m_vertex.screenPos /= m_vertex.screenPos.w;
            //视口变换
            m_vertex.screenPos.x *=  0.5f * width;
            m_vertex.screenPos.y *=  0.5f * height;
         

        }
    }
}
