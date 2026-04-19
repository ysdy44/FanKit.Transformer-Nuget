using System.Numerics;

namespace FanKit.Transformer.Input
{
    public readonly struct Pivot1
    {
        readonly double ax, ay;
        readonly double sx, sy;

        public readonly Vector2 Center;
        public readonly float Radius;

        public Pivot1(double x1, double y1, double x2, double y2)
        {
            ax = x1 + x2;
            ay = y1 + y2;

            sx = x2 - x1;
            sy = y2 - y1;

            Center = new Vector2((float)(ax / 2), (float)(ay / 2));
            Radius = (float)System.Math.Sqrt(sx * sx + sy * sy);
        }
    }
}