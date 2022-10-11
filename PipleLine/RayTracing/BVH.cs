using System;
using CPU_Soft_Rasterization.Math.Vector;

namespace CPU_Soft_Rasterization
{
    public class BVHTree
    {
        private Scene m_scene;
        public BVHTree(Scene scene)
        {
            m_scene = scene;
        }

        public void BuildBVH()
        {
            
        }

        public struct BVHNode
        {
            object[] objects;
        };

        public struct BVHLeaf{
            Vector3f pmin, pmax;
        };
    }
}
