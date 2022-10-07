using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPU_Soft_Rasterization.Math.Vector;

namespace CPU_Soft_Rasterization
{
    public class Material
    {
        private MaterialType m_type;
        public enum MaterialType
        {
            Metal,
            Mirror,
            SkyBox,
        }

        public Material(MaterialType type)
        {
            m_type = type;
            switch (m_type)
            {
                case MaterialType.Metal:
                    diffuse = new Vector3f(0.9f);
                    specular = new Vector3f(0.2f);
                    roughness = 128f;
                    break;
                case MaterialType.Mirror:
                    break;
                case MaterialType.SkyBox:
                    diffuse = new Vector3f(1f);
                    specular = new Vector3f(1f);
                    roughness = 1f;
                    break;
                default:
                    break;
            }
        }

        public Vector4f baseColor;
        public Vector3f diffuse;
        public Vector3f specular;
        public float roughness;
       
    }
}
