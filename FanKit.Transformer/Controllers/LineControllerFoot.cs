using System.Numerics;

namespace FanKit.Transformer.Controllers
{
    public readonly struct LineControllerFoot
    {
        // Point
        readonly float apX;
        readonly float apY;
        readonly float c;

        // Offset
        readonly float x;
        readonly float y;

        readonly float ap2X;
        readonly float ap2Y;
        readonly float c2;

        // Foot
        readonly float t;
        public readonly Vector2 Foot;

        public LineControllerFoot(LineController r, Vector2 linePoint, Vector2 point, Vector2 diff)
        {
            // Point
            apX = linePoint.X - point.X;
            apY = linePoint.Y - point.Y;
            c = apY * r.abY + apX * r.abX;

            // Offset
            if (c < 0f)
            {
                x = point.X + diff.X;
                y = point.Y + diff.Y;
            }
            else
            {
                x = point.X - diff.X;
                y = point.Y - diff.Y;
            }

            ap2X = linePoint.X - x;
            ap2Y = linePoint.Y - y;
            c2 = ap2Y * r.abY + ap2X * r.abX;

            // Foot
            t = c2 / r.xy;
            Foot = new Vector2(linePoint.X - r.abX * t,
                   linePoint.Y - r.abY * t);
        }

    }
}