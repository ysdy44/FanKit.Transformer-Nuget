using System.Numerics;

namespace FanKit.Transformer.Input
{
    public struct Viewport
    {
        // Canvas
        public float ExtentWidth;
        public float ExtentHeight;

        // UI
        public float ViewportX;
        public float ViewportY;

        public float ViewportWidth;
        public float ViewportHeight;

        public Fit ToFit(FitUnits unit, float scale = 0.9f)
        {
            return new Fit(this, unit, scale);
        }
    }
}