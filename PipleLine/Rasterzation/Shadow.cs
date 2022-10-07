using System;
using System.Windows.Media.Media3D;
using CPU_Soft_Rasterization.Math.Martix;
using CPU_Soft_Rasterization.Math.Vector;
using static System.Formats.Asn1.AsnWriter;

namespace CPU_Soft_Rasterization
{
    public class ShadowMaping
    {
        private readonly Scene m_scene;
        private Light mainLight;
        private Martix4f wolrdToLightMat;
        private ShadowMap map;
        private bool m_IsGenerated = false;
        public ShadowMaping(Scene scene)
        {
            m_scene = scene;
            mainLight = scene.lights[0];
        }
        public struct ShadowMap
        {
            public int width, height;
            public float[] depths;
            public float[] visabilitys;
        }

        public void GenerateShadowMap(bool IsHardShadow)
        {

            if (IsHardShadow)
                HardShadow(out map);
            else
                SoftShadows(out map);
        }

        public bool IsGenetated()
        {
            return m_IsGenerated;
        }

        public Martix4f WolrdToLightMat()
        {
            return wolrdToLightMat;
        }


        public void HardShadow(out ShadowMap map)
        {
            map = new ShadowMap();
            map.width = m_scene.width;
            map.height = m_scene.height;
            map.depths = new float[map.width * map.height];
            for (int i = 0; i < m_scene.sceneObjs.Count; i++)
            {
                for (int j = 0; j < m_scene.sceneObjs[i].triangles.Length; j++)
                {
                    var triangle = m_scene.sceneObjs[i].triangles[j];
                    var boundingBox = CreateBoudingBox(triangle);
                    for (int x = (int)boundingBox.x; x < (int)boundingBox.y; x++)
                    {
                        for (int y = (int)boundingBox.z; y < (int)boundingBox.z; y++)
                        {
                            var point = new Vector3f((float)(x + 0.5), (float)(y + 0.5), 0);
                            if (triangle.IsInsideTriangle(point))
                            {
                                Vertex[] v = triangle.vertices;
                                Vector3f abc = triangle.ComputeBarycentricCoordinateInViewPort(point);
                                float depth = v[0].screenPos.z * abc.x + v[1].screenPos.z * abc.y + v[2].screenPos.z * abc.z;
                                float w = v[0].screenPos.w * abc.x + v[1].screenPos.w * abc.y + v[2].screenPos.w * abc.z;
                                depth *= w;
                                if (map.depths[GetIndex(x, y)] == 0f ||map.depths[GetIndex(x,y)] > depth)
                                {
                                    map.depths[GetIndex(x,y)] = depth;
                                }

                            }
                        }
                    }
                }

            }
            m_IsGenerated = true;
        }
        public void SoftShadows(out ShadowMap map)
        {
            map = new ShadowMap();
            map.width = m_scene.width;
            map.height = m_scene.height;
            map.depths = new float[map.width * map.height];

            PCSS(ref map);
        }

        public void PCSS(ref ShadowMap shadowMap)
        {
            int kenelRadius = 7;
            for (int x = 0; x < shadowMap.width; x++)
            {
                for (int y = 0; y < shadowMap.height; y++)
                {

                    int xbegin = x - (kenelRadius - 1) / 2 > 0 ? x - (kenelRadius - 1) / 2 : 0;
                    int xEnd = x + (kenelRadius - 1) / 2 < shadowMap.width ? x + (kenelRadius - 1) / 2 : shadowMap.width;
                    int ybegin = y - (kenelRadius - 1) / 2 > 0 ? y - (kenelRadius - 1) / 2 : 0;
                    int yEnd = y + (kenelRadius - 1) / 2 < shadowMap.height ? y + (kenelRadius - 1) / 2 : shadowMap.height;
                    int weight = 0;
                    float sumvisbitlity = 0f;
                    for (int i = xbegin; x < xEnd; x++)
                    {
                        for (int j = ybegin; j < yEnd; j++)
                        {
                            sumvisbitlity += shadowMap.depths[GetIndex(i, j)];

                            weight += 1;
                        }
                    }
                    shadowMap.visabilitys[GetIndex(x, y)] = sumvisbitlity / weight;
                }
            }
        }


        public float GetVisability(Vector3f pos)
        {
            Vector4f lightPos = WolrdToLightMat() * pos.PointToVector4();
            Vector2f uv = new Vector2f(lightPos.x, lightPos.y);
            if (uv.x < 0 || uv.y < 0 || uv.x > map.width || uv.y > map.height)
            {
                return 0;
            }
            else
            {
                return map.visabilitys[GetIndex((int)uv.x,(int)uv.y)];
            }
        }


        private int GetIndex(int x, int y)
        {
            return y * map.width + x; ;
            
        }


        public Vector4f CreateBoudingBox(Triangle triangle)
        {
            float minX, minY, maxX, maxY, halfWidth, halfHeight;
            Vector3f pos1 = triangle.vertices[0].screenPos.toVector3();
            Vector3f pos2 = triangle.vertices[1].screenPos.toVector3();
            Vector3f pos3 = triangle.vertices[2].screenPos.toVector3();
            halfWidth = m_scene.width / 2;
            halfHeight = m_scene.height / 2;

            minX = pos1.x < pos2.x ? pos1.x : pos2.x;
            minX = minX < pos3.x ? minX : pos3.x;
            minX = minX < -halfWidth ? -halfWidth : minX;

            maxX = pos1.x > pos2.x ? pos1.x : pos2.x;
            maxX = maxX > pos3.x ? maxX : pos3.x;
            maxX = maxX > halfWidth ? halfWidth : maxX;


            minY = pos1.y < pos2.y ? pos1.y : pos2.y;
            minY = minY < pos3.y ? minY : pos3.y;
            minY = minY < -halfHeight ? -halfHeight : minY;


            maxY = pos1.y > pos2.y ? pos1.y : pos2.y;
            maxY = maxY > pos3.y ? maxY : pos3.y;
            maxY = maxY > halfHeight ? halfHeight : maxY;


            return new Vector4f(minX, maxX, minY, maxY);
        }
    }
}
