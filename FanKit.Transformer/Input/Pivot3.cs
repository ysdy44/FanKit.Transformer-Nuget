using System.Numerics;

namespace FanKit.Transformer.Input
{
    public readonly struct Pivot3
    {
        readonly double ax, ay;
        readonly double sx, sy;

        public readonly Vector2 Center;
        public readonly float RotationAngle;
        public readonly float Radius;

        public Pivot3(double x1, double y1, double x2, double y2)
        {
            ax = x1 + x2;
            ay = y1 + y2;

            sx = x2 - x1;
            sy = y2 - y1;

            this.Center = new Vector2((float)(ax / 2), (float)(ay / 2));
            this.RotationAngle = (float)System.Math.Atan2(sy, sx);
            this.Radius = (float)System.Math.Sqrt(sx * sx + sy * sy);
        }
    }
}