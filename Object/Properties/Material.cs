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
                    ambient = new Vector3f(1.0f, 0.5f, 0.32f);
                    diffuse = new Vector3f(1.0f, 0.5f, 0.31f);
                    specular = new Vector3f(0.5f);
                    roughness = 32f;
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

        public Vector3f baseColor { get; set; }
        public Vector3f ambient { get; set; }

        public Vector3f diffuse { get; set; }
        public Vector3f specular { get; set; }
        public float roughness { get; set; }
       
    }
}
