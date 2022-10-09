using System;
using CPU_Soft_Rasterization.Math.Martix;
using CPU_Soft_Rasterization.Math.Vector;

namespace CPU_Soft_Rasterization
{
    public class VertexShader
    {
        Martix4f MVP;
        Martix4f ModelMatrix;
        Vertex m_vertex;
        float width, height;
        public VertexShader(Vertex vertex)
        {
            m_vertex = vertex;
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

        public void SetModelMatrix(Martix4f matrix)
        {
            ModelMatrix = matrix;
        }
        public void Shade()
        {
            m_vertex.worldPos = ModelMatrix * m_vertex.localPos.PointToVector4();
            m_vertex.worldNormal = (ModelMatrix * m_vertex.normal.DirToVector4()).toVector3();

            m_vertex.screenPos = MVP * m_vertex.localPos.PointToVector4();
            //局部坐标转换为世界坐标
            //齐次除法
            m_vertex.screenPos /= m_vertex.screenPos.w;
            //视口变换
            m_vertex.screenPos.x *=  0.5f * width;
            m_vertex.screenPos.y *=  0.5f * height;
         

        }
    }
}
