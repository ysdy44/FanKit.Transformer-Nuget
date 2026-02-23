using FanKit.Transformer.Mathematics;
using System.Numerics;

namespace FanKit.Transformer.Input
{
    public readonly struct Fit1
    {
        readonly Vector2 s; // Scale

        public readonly Coordinate Coord;
        public readonly Rectangle Bounds;

        public Fit1(Viewport viewport, float padding)
        {
            s = viewport.ToFitScales();

            // 1. ScaleFactor
            Coord.ScaleFactor = System.Math.Min(s.X, s.Y) * padding;
            Coord.InverseScaleFactor = 1f / Coord.ScaleFactor;

            // 2. Width & Height
            Bounds.Width = Coord.ScaleFactor * viewport.ExtentWidth;
            Bounds.Height = Coord.ScaleFactor * viewport.ExtentHeight;

            // 3. X & Y
            Coord.Translate = viewport.ToFitTranslation(Bounds.Width, Bounds.Height);

            // 4. Translate
            Bounds.X = Coord.Translate.X;
            Bounds.Y = Coord.Translate.Y;
        }
    }
}