using FanKit.Transformer.Mathematics;

namespace FanKit.Transformer.Input
{
    public struct Fit
    {
        public Coordinate Coord;

        public Rectangle Bounds;

        public float TranslateX => Coord.TranslateX;

        public float TranslateY => Coord.TranslateY;

        public float ScaleFactor => Coord.ScaleFactor;

        internal Fit(Viewport viewport, FitUnits unit, float scale)
        {
            // 1. ScaleFactor
            if (unit != FitUnits.CanvasTransformer0)
            {
                float sx = viewport.ViewportWidth / viewport.ExtentWidth;
                float sy = viewport.ViewportHeight / viewport.ExtentHeight;
                Coord.ScaleFactor = System.Math.Min(sx, sy) * scale;
                Coord.InverseScaleFactor = 1f / Coord.ScaleFactor;
            }
            else
            {
                Coord.ScaleFactor = 1f;
                Coord.InverseScaleFactor = 1f;
            }

            // 2. Width & Height
            if (unit == FitUnits.CanvasTransformer1 || unit == FitUnits.Thumbnail)
            {
                Bounds.Width = Coord.ScaleFactor * viewport.ExtentWidth;
                Bounds.Height = Coord.ScaleFactor * viewport.ExtentHeight;
            }
            else
            {
                Bounds.Width = viewport.ExtentWidth;
                Bounds.Height = viewport.ExtentHeight;
            }

            // 3. X & Y
            Bounds.X = Coord.TranslateX = viewport.ViewportX + (viewport.ViewportWidth - Bounds.Width) / 2f;
            Bounds.Y = Coord.TranslateY = viewport.ViewportY + (viewport.ViewportHeight - Bounds.Height) / 2f;
        }
    }
}