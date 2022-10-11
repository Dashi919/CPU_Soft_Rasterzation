using System;
using CPU_Soft_Rasterization.Math.Vector;

namespace CPU_Soft_Rasterization
{
    public class FragmentShader
    {
        private FrameBuffer m_fragment;
        private Light[] m_lights;
        private ShadowMaping? m_shadowmap;
        private Vector3f frag_world_pos;
        private Material m_material;
        private Camera m_camera;
        public bool IsShowShadow = true;
        public FragmentShader(FrameBuffer buffer)
        {
            m_fragment = buffer;
            m_material = buffer.objectBuffer != null ? buffer.objectBuffer.material : new Material(Material.MaterialType.SkyBox);
        }

        public void SetLight(Light[] lights)
        {
            m_lights = lights;
        }

        public void SetShadow(ShadowMaping shadowmap)
        {
            m_shadowmap = shadowmap;
        }

        public void SetCamera(Camera camera)
        {
            m_camera = camera;
        }

        public Vector3f Shade()
        {
            Vector3f finalColor = new Vector3f(0f);
            var baseColor = m_fragment.colorBuffer;

            if (m_fragment.objectBuffer == null)
                return finalColor;

            if (m_fragment.objectBuffer.isLight)
                return m_material.baseColor;
            //blinn-phong lights
            frag_world_pos = m_fragment.vertexPos.toVector3();

            for (int i = 0; i < m_lights.Length; i++)
            {
                var radiance = m_lights[i].GetAttenuation(frag_world_pos);
                var lightDir = (m_lights[i].position - frag_world_pos).normalize();
                var viewDir = (m_camera.transform.position - frag_world_pos).normalize();
                var halfVertor = (lightDir + viewDir).normalize();
                Vector3f diffuse = radiance.CwiseProduct(m_material.diffuse) * MathF.Max(m_fragment.normalBuffer.DotProduct(lightDir), 0);
                Vector3f specular = radiance.CwiseProduct(m_material.specular) * MathF.Pow(MathF.Max(m_fragment.normalBuffer.DotProduct(halfVertor), 0), m_material.roughness);
                finalColor += diffuse + specular;
            }

            //shadow
            if (m_shadowmap != null && m_shadowmap.IsGenetated())
            {
            if(IsShowShadow)
               finalColor *= m_shadowmap.GetVisability(frag_world_pos, false);
            }

            return baseColor.CwiseProduct(finalColor.Clamp(Vector3f.Zero(), Vector3f.Identity()));
        }
    }
}
