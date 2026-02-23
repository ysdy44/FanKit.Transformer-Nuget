using System.Numerics;

namespace FanKit.Transformer.Mathematics
{
    public struct Linear
    {
        public Vector2 L0;
        public Vector2 L1;

        public Vector2 Lerp(float t, float i)
        {
            return new Vector2
            {
                X = t * L1.X + i * L0.X,
                Y = t * L1.Y + i * L0.Y,
            };
        }

        // Extensions
        public Quadratic Quadratic() => new Quadratic
        {
            Q0 = L0,
            Q1 = new Vector2
            {
                X = (L0.X + L1.X) / 2f,
                Y = (L0.Y + L1.Y) / 2f,
            },
            Q2 = L1,
        };

        public Cubic Cubic() => new Cubic
        {
            C0 = L0,
            C1 = new Vector2
            {
                X = (L0.X + L0.X + L1.X) / 3f,
                Y = (L0.Y + L0.Y + L1.Y) / 3f,
            },
            C2 = new Vector2
            {
                X = (L0.X + L1.X + L1.X) / 3f,
                Y = (L0.Y + L1.Y + L1.Y) / 3f,
            },
            C3 = L1,
        };

        public float Foot(Vector2 point)
        {
            float x = L0.X - L1.X;
            float y = L0.Y - L1.Y;
            float s = x * x + y * y;

            float a = L0.X - point.X;
            float b = L0.Y - point.Y;
            float p = a * x + b * y;

            return p / s;
        }
    }
}