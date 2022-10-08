using System;
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
        private Vector3f lightUp, lightFocus, lightRight;
        private Vector2f[] randomSamplers;


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
            lightUp = new Vector3f(0, 1, 0);
            lightFocus = new Vector3f(0, 0, 0);
            LightVPMartix = CalculateLightMV();
        }
        public struct ShadowMap
        {
            public int width, height;
            public float[] depths;
        }

        private Martix4f CalculateLightMV()
        {
            lightRight = lightFocus.crossProduct(lightUp);
            Martix4f LightVP;
            Martix4f translateMartix = Martix4f.TranslateMat(-mainLight.poistion);
            Martix4f rotateMartix = new Martix4f(lightRight, lightUp, -lightFocus);
            Martix4f orthoMartix = Martix4f.OrthogonalMartix(-100f,100f,-100f,100f,0.1f,1000f);
            LightVP = orthoMartix * translateMartix * rotateMartix;
            return LightVP;
        }
        public void GenerateShadowMap()
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
                    var pos = m_scene.sceneObjs[i].transform.position;
                    var boundingBox = CreateBoudingBox(triangle,pos);
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
                                if (map.depths[GetIndex(x, y)] == 0f || map.depths[GetIndex(x, y)] > depth)
                                {
                                    map.depths[GetIndex(x, y)] = depth;
                                }

                            }
                            
                        }
                    }
                }

            }
            m_IsGenerated = true;

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
            Vector4f coords = WolrdToLightMat() * pos.PointToVector4();
            if (coords.x < 0 || coords.y < 0 || coords.x > 1 || coords.y > 1)
            {
                return 0;
            }
            else
            {
                if (isSoftShadow)
                {
                    return SoftShadows(coords);

                }
                return 1;
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
            int screenX =(int)(x * map.width);
            int screenY = (int)(y * map.height);
            return GetIndex(screenX, screenY);
        }
        

        public Vector4f CreateBoudingBox(Triangle triangle,Vector3f objPos)
        {
            float minX, minY, maxX, maxY;
            var LightMVP = WolrdToLightMat() * m_scene.GetModelMartix(m_scene.renderTick,objPos);
            Vector3f pos1 = (LightMVP * triangle.vertices[0].pos.PointToVector4()).toVector3();
            Vector3f pos2 = (LightMVP * triangle.vertices[1].pos.PointToVector4()).toVector3();
            Vector3f pos3 = (LightMVP * triangle.vertices[2].pos.PointToVector4()).toVector3();


            minX = pos1.x < pos2.x ? pos1.x : pos2.x;
            minX = minX < pos3.x ? minX : pos3.x;
            minX = minX < 0 ? 0 : minX;

            maxX = pos1.x > pos2.x ? pos1.x : pos2.x;
            maxX = maxX > pos3.x ? maxX : pos3.x;
            maxX = maxX > map.width ? map.width : maxX;


            minY = pos1.y < pos2.y ? pos1.y : pos2.y;
            minY = minY < pos3.y ? minY : pos3.y;
            minY = minY < 0 ? 0 : minY;


            maxY = pos1.y > pos2.y ? pos1.y : pos2.y;
            maxY = maxY > pos3.y ? maxY : pos3.y;
            maxY = maxY > map.height ? map.height : maxY;


            return new Vector4f(minX, maxX, minY, maxY);
        }
    }
}
