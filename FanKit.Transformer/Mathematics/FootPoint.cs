using System.Numerics;

namespace FanKit.Transformer.Mathematics
{
    public readonly struct FootPoint
    {
        public readonly Vector2 Point;
        public readonly Vector2 LinePoint0;
        public readonly Vector2 LinePoint1;

        readonly float x;
        readonly float y;
        readonly float s;

        readonly float a;
        readonly float b;
        readonly float p;

        //public readonly float Judge;
        public readonly float Value;
        public readonly Vector2 Foot;

        public FootPoint(Vector2 point, Vector2 linePoint0, Vector2 linePoint1)
        {
            Point = point;
            LinePoint0 = linePoint0;
            LinePoint1 = linePoint1;

            x = LinePoint0.X - LinePoint1.X;
            y = LinePoint0.Y - LinePoint1.Y;
            s = x * x + y * y;

            a = LinePoint0.X - Point.X;
            b = LinePoint0.Y - Point.Y;
            p = a * x + b * y;

            //Judge = x * b - y * a;
            Value = p / s;
            Foot = new Vector2
            {
                X = LinePoint0.X - x * Value,
                Y = LinePoint0.Y - y * Value,
            };
        }
    }
}