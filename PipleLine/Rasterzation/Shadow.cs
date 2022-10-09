using System;
using System.Numerics;
using System.Windows;
using CPU_Soft_Rasterization.Math.Martix;
using CPU_Soft_Rasterization.Math.Vector;

namespace CPU_Soft_Rasterization
{
    public class ShadowMaping
    {
        private readonly Scene m_scene;
        private Light mainLight;
        private Martix4f LightVPMartix;
        private ShadowMap map;
        private bool m_IsGenerated = false;
        private Vector2f[] randomSamplers;
        private float halfWidth, halfHeight;
        private Martix4f viewPortMartix;
        private const int sample_nums = 16;
        private const int rings_nums = 10;
        private const float light_world_size = 0.05f;
        private const float light_frustrum_width = 10.0f;
        private const float light_size_uv = light_world_size / light_frustrum_width;
        private const float near_plane = 0.1f;


        public ShadowMaping(Scene scene)
        {
            m_scene = scene;
            mainLight = scene.lights[0];
            LightVPMartix = CalculateLightMV();
        }
        public struct ShadowMap
        {
            public int width, height;
            public float[] depths;
        }

        private Martix4f CalculateLightMV()
        {
            
            Martix4f LightVP;
            Martix4f translateMartix = Martix4f.TranslateMat(-mainLight.poistion);
            Martix4f rotateMartix = new Martix4f(mainLight.lightRight, mainLight.lightUp, -mainLight.lightFocus);
            Martix4f orthoMartix = Martix4f.OrthogonalMartix(-10, 10, -10, 10, 0.1f, 1000f);

            LightVP = orthoMartix * translateMartix * rotateMartix;
            return LightVP;
        }
        public void GenerateShadowMap()
        {
            m_IsGenerated = false;
            map = new ShadowMap();
            map.width = m_scene.width;
            map.height = m_scene.height;
            map.depths = new float[map.width * map.height];
            halfWidth = map.width * 0.5f;
            halfHeight = map.height * 0.5f;
            viewPortMartix = new Martix4f(halfWidth, 0, 0, 0,
                                                   0, halfHeight, 0, 0,
                                                   0, 0, 1, 0,
                                                   0, 0, 0, 1);

            for (int i = 0; i < m_scene.sceneObjs.Count; i++)
            {
                if (m_scene.sceneObjs[i].isLight)
                    continue;
                for (int j = 0; j < m_scene.sceneObjs[i].triangles.Length; j++)
                {
                    var triangle = m_scene.sceneObjs[i].triangles[j];
                    Vector4f pos1 = viewPortMartix * WolrdToLightMat() * triangle.vertices[0].worldPos;
                    Vector4f pos2 = viewPortMartix * WolrdToLightMat() * triangle.vertices[1].worldPos;
                    Vector4f pos3 = viewPortMartix * WolrdToLightMat() * triangle.vertices[2].worldPos;
                    Vector4f[] vetices = new Vector4f[] { pos1, pos2, pos3 };
                    var boundingBox = CreateBoudingBox(vetices);


                    for (int x = (int)boundingBox.x; x < (int)boundingBox.y; x++)
                    {
                        for (int y = (int)boundingBox.z; y < (int)boundingBox.w; y++)
                        {
                            var point = new Vector3f((float)(x + 0.5), (float)(y + 0.5), 0);


                            if (IsInsideTriangle(point, vetices))
                            {

                                Vector3f abc = ComputeBarycentricCoordinateInViewPort(point, vetices);
                                float depth = vetices[0].z * abc.x + vetices[1].z * abc.y + vetices[2].z * abc.z;
                                
                                if (map.depths[GetIndex2(x, y)] == 0.0f || depth < map.depths[GetIndex2(x, y)])
                                {

                                    map.depths[GetIndex2(x, y)] = depth;
                                }

                            }

                        }
                    }
                }

            }
            m_IsGenerated = true;

        }

        public bool IsInsideTriangle(Vector3f point, Vector4f[] vertices)
        {
            float c1 = (point.x - vertices[0].x) * (vertices[0].y - vertices[1].y)
                - (vertices[0].x - vertices[1].x) * (point.y - vertices[0].y);
            float c2 = (point.x - vertices[1].x) * (vertices[1].y - vertices[2].y)
                - (vertices[1].x - vertices[2].x) * (point.y - vertices[1].y);
            float c3 = (point.x - vertices[2].x) * (vertices[2].y - vertices[0].y)
                - (vertices[2].x - vertices[0].x) * (point.y - vertices[2].y);
            return (c1 >= 0 && c2 >= 0 && c3 >= 0) || (c1 <= 0 && c2 <= 0 && c3 <= 0);
        }

        public Vector3f ComputeBarycentricCoordinateInViewPort(Vector3f point, Vector4f[] vertices)
        {
            float c1 = (point.x * (vertices[1].y - vertices[2].y) + (vertices[2].x - vertices[1].x) * point.y + vertices[1].x * vertices[2].y - vertices[2].x * vertices[1].y) / (vertices[0].x * (vertices[1].y - vertices[2].y) + (vertices[2].x - vertices[1].x) * vertices[0].y + vertices[1].x * vertices[2].y - vertices[2].x * vertices[1].y);
            float c2 = (point.x * (vertices[2].y - vertices[0].y) + (vertices[0].x - vertices[2].x) * point.y + vertices[2].x * vertices[0].y - vertices[0].x * vertices[2].y) / (vertices[1].x * (vertices[2].y - vertices[0].y) + (vertices[0].x - vertices[2].x) * vertices[1].y + vertices[2].x * vertices[0].y - vertices[0].x * vertices[2].y);
            float c3 = (point.x * (vertices[0].y - vertices[1].y) + (vertices[1].x - vertices[0].x) * point.y + vertices[0].x * vertices[1].y - vertices[1].x * vertices[0].y) / (vertices[2].x * (vertices[0].y - vertices[1].y) + (vertices[1].x - vertices[0].x) * vertices[2].y + vertices[0].x * vertices[1].y - vertices[1].x * vertices[0].y);
            return new Vector3f(c1, c2, c3);
        }

        public bool IsGenetated()
        {
            return m_IsGenerated;
        }

        public Martix4f WolrdToLightMat()
        {
            return LightVPMartix;
        }


        public float SoftShadows(Vector4f coords)
        {
            return PCSS(coords);
        }

        private float PCSS(Vector4f coords)
        {
            Vector2f uv = new Vector2f(coords.x, coords.y);
            randomSamplers = poissonDiskSamping(uv);

            float zReceiver = coords.z;
            float avgBlocker = FindBlocker(uv, zReceiver);
            if (avgBlocker == -1f)
            {
                return 1f;
            }
            float penumbraRatio = (zReceiver - avgBlocker) / avgBlocker;
            float filterSize = penumbraRatio * light_size_uv * near_plane / zReceiver;

            return PCF(uv, zReceiver, filterSize);
        }

        private float PCF(Vector2f uv, float zReceiver, float radius)
        {
            float sum = 0f;
            for (int i = 0; i < sample_nums; i++)
            {
                float depth = GetDepth(GetIndex2(uv.x + randomSamplers[i].x * radius, uv.y + randomSamplers[i].y * radius));
                if (zReceiver <= depth)
                {
                    sum += 1;
                }
            }
            for (int i = 0; i < sample_nums; i++)
            {
                float depth = GetDepth(GetIndex2(uv.x - randomSamplers[i].y * radius, uv.y - randomSamplers[i].x * radius));
                if (zReceiver <= depth)
                {
                    sum += 1;
                }
            }

            return sum / (2 * sample_nums);
        }


        private float FindBlocker(Vector2f uv, float zReceiver)
        {
            float avgBlocker = 0f;
            float searRadius = light_size_uv * (zReceiver - near_plane) / zReceiver;
            int numBlocker = 0;
            for (int i = 0; i < sample_nums; i++)
            {
                float depth = GetDepth(GetIndex2(uv.x + randomSamplers[i].x * searRadius, uv.y + randomSamplers[i].y * searRadius));
                if (depth < zReceiver)
                {
                    avgBlocker += depth;
                    numBlocker++;
                }

            }

            if (numBlocker == 0)
                return -1f;
            return zReceiver / numBlocker;

        }

        private Vector2f[] poissonDiskSamping(Vector2f randomSeed)
        {
            float angle_Step = 2 * MathF.PI * rings_nums / sample_nums;
            float inv_num_samples = 1.0f / sample_nums;
            float angle = rand_2to1(randomSeed);
            float radius = inv_num_samples;
            float radiusStep = radius;
            Vector2f[] samples = new Vector2f[sample_nums];
            for (int i = 0; i < sample_nums; i++)
            {
                samples[i] = new Vector2f(MathF.Cos(angle), MathF.Sin(angle) * MathF.Pow(radius, 0.75f));
                radius += radiusStep;
                angle += angle_Step;
            }

            return samples;
        }


        private float rand_2to1(Vector2f vec)
        {
            float a = 12.9898f, b = 78.233f, c = 43758.5453f;
            float dt = vec.dotProduct(new Vector2f(a, b));
            float sn = dt % MathF.PI;
            return MathF.Truncate(MathF.Sin((sn) * c));
        }

        public float GetVisability(Vector3f pos, bool isSoftShadow)
        {
            Vector4f coords = viewPortMartix * WolrdToLightMat() * pos.PointToVector4();
            if (coords.x < -halfWidth || coords.y < -halfHeight || coords.x > halfWidth || coords.y > halfHeight)
            {

                return 0;
            }
            else
            {
                float depth = GetDepth(GetIndex2(coords.x, coords.y));

                if (coords.z - 0.001 < depth)
                {
                    if (isSoftShadow)
                    {
                        return SoftShadows(coords);

                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    return 0f;
                }
            }
        }

        public float GetDepth(int index)
        {
            return map.depths[index];
        }

        public int GetIndex(int x, int y)
        {
            return y * map.width + x;
        }

        private int GetIndex2(float x, float y)
        {
            int screenX = (int)(x + halfWidth);
            int screenY = (int)(y + halfHeight);
            return GetIndex(screenX, screenY);
        }


        public Vector4f CreateBoudingBox(Vector4f[] vertices)
        {
            float minX, minY, maxX, maxY;
            minX = vertices[0].x < vertices[1].x ? vertices[0].x : vertices[1].x;
            minX = minX < vertices[2].x ? minX : vertices[2].x;
            minX = minX < -halfWidth ? halfWidth : minX;

            maxX = vertices[0].x > vertices[1].x ? vertices[0].x : vertices[1].x;
            maxX = maxX > vertices[2].x ? maxX : vertices[2].x;
            maxX = maxX > halfWidth ? halfWidth : maxX;


            minY = vertices[0].y < vertices[1].y ? vertices[0].y : vertices[1].y;
            minY = minY < vertices[2].y ? minY : vertices[2].y;
            minY = minY < -halfHeight ? -halfHeight : minY;


            maxY = vertices[0].y > vertices[1].y ? vertices[0].y : vertices[1].y;
            maxY = maxY > vertices[2].y ? maxY : vertices[2].y;
            maxY = maxY > halfHeight ? halfHeight : maxY;


            return new Vector4f(minX, maxX, minY, maxY);
        }
    }
}
