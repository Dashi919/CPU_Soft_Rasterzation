using System;

namespace CPU_Soft_Rasterization.Main
{
    public class Ticker
    {

        private Scene scene;
        private Time timer;
        public enum RenderType
        {
            Rasterzation,
            Raytracing
        }

        private Ticker()
        {

        }
        private void Tick(Time deltaTime)
        {


        }

        private void LogicTick(Time deltaTime)
        {

        }

        private void GraphicTick(Time deltaTime)
        {

        }

        public void Render(RenderType renderType)
        {
            switch (renderType)
            {
                case RenderType.Rasterzation:
                    //scene.Rasterization();
                    break;
                case RenderType.Raytracing:
                    scene.RayTracing();
                    break;
            }
        }
    }
}
