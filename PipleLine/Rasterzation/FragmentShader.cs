using System;
using CPU_Soft_Rasterization.Math.Vector;

namespace CPU_Soft_Rasterization
{
    public class FragmentShader
    {
        private FrameBuffer m_fragment;
        private Light[] m_lights;
        private ShadowMaping? m_shadowmap;
        private Vector3f m_pos;
        private Material m_material;
        private Camera m_camera;
        public FragmentShader(FrameBuffer buffer)
        {
            m_fragment = buffer;
            m_material = buffer.objectBuffer != null ? buffer.objectBuffer.material : new Material(Material.MaterialType.SkyBox);
            m_pos = buffer.vertexPos;
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
            //lights
            for (int i = 0; i < m_lights.Length; i++)
            {
                var radiance = m_lights[i].GetRadiance(m_pos);
                var lightDir = (m_lights[i].poistion - m_pos).normalize();
                var viewDir = (m_camera.position - m_pos).normalize();
                var halfVertor = (lightDir + viewDir).normalize();
                Vector3f diffuse = radiance.CwiseProduct(m_material.diffuse) * MathF.Max(m_fragment.normalBuffer.DotProduct(lightDir),0);
                Vector3f specular = radiance.CwiseProduct(m_material.specular) * MathF.Max(MathF.Pow(m_fragment.normalBuffer.DotProduct(halfVertor), m_material.roughness),0);
                finalColor += specular + diffuse;
            }

            //shadow
            if (m_shadowmap != null && m_shadowmap.IsGenetated())
            {
                finalColor *= m_shadowmap.GetVisability(m_pos);
            }

            return baseColor.CwiseProduct(finalColor.Clamp(Vector3f.Zero(),Vector3f.Identity()));
        }
    }
}
