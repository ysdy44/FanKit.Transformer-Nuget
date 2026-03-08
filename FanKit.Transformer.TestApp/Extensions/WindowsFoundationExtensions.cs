namespace FanKit.Transformer.TestApp
{
    public static class WindowsFoundationExtensions
    {
        public static Windows.Foundation.Rect ToRect(this Rectangle rectangle) => new Windows.Foundation.Rect
        {
            X = rectangle.X,
            Y = rectangle.Y,
            Width = rectangle.Width,
            Height = rectangle.Height,
        };

        public static Windows.Foundation.Rect ToRect(this Bounds bounds) => new Windows.Foundation.Rect
        {
            X = System.Math.Min(bounds.Right, bounds.Left),
            Y = System.Math.Min(bounds.Bottom, bounds.Top),
            Width = System.Math.Abs(bounds.Right - bounds.Left),
            Height = System.Math.Abs(bounds.Bottom - bounds.Top),
        };
    }
}