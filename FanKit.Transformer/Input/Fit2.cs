using FanKit.Transformer.Mathematics;
using System.Numerics;

namespace FanKit.Transformer.Input
{
    public readonly struct Fit2
    {
        readonly Vector2 s; // Scale

        public readonly Coordinate Coord;

        public Fit2(Viewport viewport, float padding)
        {
            s = viewport.ToFitScales();

            // 1. ScaleFactor
            Coord.ScaleFactor = System.Math.Min(s.X, s.Y) * padding;
            Coord.InverseScaleFactor = 1f / Coord.ScaleFactor;

            // 2. Translate
            Coord.Translate = viewport.ToFitTranslation();
        }
    }
}