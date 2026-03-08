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

        public Fit1 ToFit1(float padding = 0.9f) => new Fit1(this, padding);
        public Fit2 ToFit2(float padding = 0.9f) => new Fit2(this, padding);

        public Bounds ToViewportBounds() => new Bounds(
            this.ViewportX,
            this.ViewportY,
            this.ViewportX + this.ViewportWidth,
            this.ViewportY + this.ViewportHeight);

        internal Vector2 ToFitScales() => new Vector2
        {
            X = this.ViewportWidth / this.ExtentWidth,
            Y = this.ViewportHeight / this.ExtentHeight,
        };
        public Vector2 ToFitTranslation() => new Vector2
        {
            X = this.ViewportX + (this.ViewportWidth - this.ExtentWidth) / 2f,
            Y = this.ViewportY + (this.ViewportHeight - this.ExtentHeight) / 2f
        };
        internal Vector2 ToFitTranslation(float width, float height) => new Vector2
        {
            X = this.ViewportX + (this.ViewportWidth - width) / 2f,
            Y = this.ViewportY + (this.ViewportHeight - height) / 2f
        };
    }
}