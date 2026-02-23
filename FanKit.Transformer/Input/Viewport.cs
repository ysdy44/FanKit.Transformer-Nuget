using System.Numerics;

namespace FanKit.Transformer.Input
{
    /* 
     * ExtentHeight
     * 获取包含盘区垂直大小的一个值。
     * 
     * ExtentWidth
     * 获取包含盘区水平大小的值。
     * 
     * 
     * ScrollableHeight
     * 获取一个值，该值表示可滚动的内容元素的垂直大小。
     * 
     * ScrollableWidth
     * 获取一个值，该值表示可滚动的内容元素的水平大小。
     * 
     * 
     * ViewportHeight
     * 获取包含内容视区垂直大小的值。 
     * 
     * ViewportWidth
     * 获取包含内容视区水平大小的值。
     * */

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