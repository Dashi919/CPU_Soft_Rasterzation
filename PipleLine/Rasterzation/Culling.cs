using System;
using System.Collections.Generic;
using CPU_Soft_Rasterization.Math.Vector;

namespace CPU_Soft_Rasterization
{
    public class Culling
    {
        Scene scene;
        public Culling(Scene scene)
        {
            this.scene = scene;
        }

        public Triangle[] Cull()
        {
            return CullTriangles(CullObjcets());
        }


        private Object[] CullObjcets()
        {
            List<Object> cullObjs = new List<Object>();
            var camera = scene.camera;
            var sceneObjs = scene.sceneObjs;
            for (int i = 0; i < sceneObjs.Count; i++)
            {
                if (IsObjInsideCam(sceneObjs[i], camera))
                {
                    cullObjs.Add(sceneObjs[i]);
                }
            }
            return cullObjs.ToArray();
        }


        private Triangle[] CullTriangles(Object[] objs)
        {
            List<Triangle> cullTriangles = new List<Triangle>();
            for(int i  = 0;i < objs.Length; i++)
            {
                for(int j = 0; j < objs[i].triangles.Length; j++)
                {
                    var triangle = objs[i].triangles[j];
                    if(IsTraingleInsideCam(triangle))
                    {
                        cullTriangles.Add(triangle);
                    }
                }
            }

            return cullTriangles.ToArray();
        }

        private bool IsObjInsideCam(Object obj, Camera cam)
        {
            var objPos = obj.transform.position;
            var camPos = cam.position;
            var camToObjDir = (objPos - camPos).normalize();
            if (cam.camDir.DotProduct(camToObjDir) < 0)
                return false;
            float distance = MathF.Abs((camPos - objPos).Distance());
            if (distance > cam.maxDepth)
                return false;

            return true;
        }


        private bool IsTraingleInsideCam(Triangle triangle)
        {
            var vextices = triangle.vertices;
            for (int i = 0; i < vextices.Length; i++)
            {
                if (IsPointInsideCam(vextices[i].screenPos.toVector3()))
                {
                    return true;
                }

            }

            return false;

        }


        private bool IsPointInsideCam(Vector3f point)
        {
            float halfWidth = scene.width / 2;
            float halfHeight = scene.height / 2;
            if (MathF.Abs(point.x) < halfWidth && MathF.Abs(point.y) < halfHeight)
                return true;
            return false;
        }
    }
}
