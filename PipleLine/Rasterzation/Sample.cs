using System;
using CPU_Soft_Rasterization.Math.Vector;

namespace CPU_Soft_Rasterization
{

    public class Sample
    {
        private Scene scene;
        private Triangle[] sampleTriangles;

        public Sample(Scene scene)
        {
            this.scene = scene;
        }


        public void SetSampleTriangle(Triangle[] triangles)
        {
            sampleTriangles = triangles;
        }
        public void DoSample()
        {
            if (sampleTriangles == null || sampleTriangles.Length == 0)
            {
                return;
            }
            for (int j = 0; j < sampleTriangles.Length; j++)
            {
                var triangle = sampleTriangles[j];
                var boundingBox = CreateBoudingBox(triangle);
          
                for (int x = (int)boundingBox.x; x < (int)boundingBox.y; x++)
                {
                    for (int y = (int)boundingBox.z; y < (int)boundingBox.w; y++)
                    {
                        var point = new Vector3f((float)(x + 0.5), (float)(y + 0.5), 0);
                        if (triangle.IsInsideTriangle(point))
                        {
                            Vertex[] v = triangle.vertices;
                            Vector3f abc = triangle.ComputeBarycentricCoordinateInViewPort(point);
                            float zbuffer = v[0].screenPos.z * abc.x + v[1].screenPos.z * abc.y + v[2].screenPos.z * abc.z;
                            float w = v[0].screenPos.w * abc.x + v[1].screenPos.w * abc.y + v[2].screenPos.w * abc.z;
                            zbuffer *= w;
                            int bufferx = x + scene.width / 2;
                            int buffery = y + scene.height / 2;
                            if (zbuffer < scene.GetDepthBuffer(bufferx, buffery))
                            {
                                Vector3f color = v[0].color * abc.x + v[1].color * abc.y + v[2].color * abc.z;
                                Vector3f normal = v[0].worldNormal * abc.x + v[1].worldNormal * abc.y + v[2].worldNormal * abc.z;
                                Vector4f vertexPos = v[0].worldPos * abc.x + v[1].worldPos * abc.y + v[2].worldPos * abc.z;
                                scene.SetDepthBuffer(bufferx, buffery, zbuffer);
                                scene.SetColorBuffer(bufferx, buffery, color);
                                scene.SetNormalBuffer(bufferx, buffery, normal.normalize());
                                scene.SetVertexBuffer(bufferx, buffery, vertexPos);
                                scene.SetObjcetBuffer(bufferx, buffery, triangle.GetParentObject());
                            }
                        }
                    }
                }
            }
        }

        public Vector4f CreateBoudingBox(Triangle triangle)
        {
            float minX, minY, maxX, maxY, halfWidth, halfHeight;
            Vector3f pos1 = triangle.vertices[0].screenPos.toVector3();
            Vector3f pos2 = triangle.vertices[1].screenPos.toVector3();
            Vector3f pos3 = triangle.vertices[2].screenPos.toVector3();
            halfWidth = scene.width / 2;
            halfHeight = scene.height / 2;

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
