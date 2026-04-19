using System.Numerics;

namespace FanKit.Transformer.Input
{
    public readonly struct Pivot2
    {
        readonly double ax, ay;
        readonly double sx, sy;

        public readonly Vector2 Center;
        public readonly float RotationAngle;

        public Pivot2(double x1, double y1, double x2, double y2)
        {
            ax = x1 + x2;
            ay = y1 + y2;

            sx = x2 - x1;
            sy = y2 - y1;

            Center = new Vector2((float)(ax / 2), (float)(ay / 2));
            RotationAngle = (float)System.Math.Atan2(sy, sx);
        }
    }
}