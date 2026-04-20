using System;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer
{
    partial class Constants
    {
        // Static
        internal static Vector2 CubicPoint(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t, float i)
        {
            float i2 = i * i;
            float i3 = i2 * i;

            float t2 = t * t;
            float t3 = t2 * t;

            return new Vector2
            {
                X = i3 * p0.X +
                     3 * i2 * t * p1.X +
                     3 * i * t2 * p2.X +
                     t3 * p3.X,
                Y = i3 * p0.Y +
                     3 * i2 * t * p1.Y +
                     3 * i * t2 * p2.Y +
                     t3 * p3.Y,
            };
        }

        internal static List<float> CubicExtrema(float p0, float p1, float p2, float p3)
        {
            List<float> tValues = new List<float>();

            float a = 3 * (-p0 + 3 * p1 - 3 * p2 + p3);
            float b = 6 * (p0 - 2 * p1 + p2);
            float c = 3 * (p1 - p0);

            CubicEquation(a, b, c, tValues);

            return tValues;
        }

        private static void CubicEquation(float a, float b, float c, List<float> solutions)
        {
            if (Math.Abs(a) < float.Epsilon)
            {
                if (Math.Abs(b) > float.Epsilon)
                {
                    float t = -c / b;
                    if (t >= 0 && t <= 1)
                        solutions.Add(t);
                }
                return;
            }

            float discriminant = b * b - 4 * a * c;

            if (discriminant < 0)
                return;

            if (Math.Abs(discriminant) < float.Epsilon)
            {
                float t = -b / (2 * a);
                if (t >= 0 && t <= 1)
                    solutions.Add(t);
            }
            else
            {
                float sqrtDiscriminant = (float)Math.Sqrt(discriminant);
                float t1 = (-b + sqrtDiscriminant) / (2 * a);
                float t2 = (-b - sqrtDiscriminant) / (2 * a);

                if (t1 >= 0 && t1 <= 1)
                    solutions.Add(t1);

                if (t2 >= 0 && t2 <= 1)
                    solutions.Add(t2);
            }
        }

        #region GaussLegendre_Order5
        const float weight0o5 = 0.236926885f;
        const float weight1o5 = 0.4786286705f;
        const float weight2o5 = 0.5688888889f;
        const float weight3o5 = 0.4786286705f;
        const float weight4o5 = 0.236926885f;

        const float a0o5 = -2.725141f;
        const float a1o5 = -1.775166f;
        const float a2o5 = -0.75f;
        const float a3o5 = -0.1597579f;
        const float a4o5 = -0.006601658f;

        const float b0o5 = 2.456884f;
        const float b1o5 = 0.7100897f;
        const float b2o5 = -0.75f;
        const float b3o5 = -0.9053183f;
        const float b4o5 = -0.2616553f;

        const float c0o5 = 0.2616555f;
        const float c1o5 = 0.9053183f;
        const float c2o5 = 0.75f;
        const float c3o5 = -0.7100897f;
        const float c4o5 = -2.456884f;

        const float d0o5 = 0.006601666f;
        const float d1o5 = 0.1597579f;
        const float d2o5 = 0.75f;
        const float d3o5 = 1.775166f;
        const float d4o5 = 2.725141f;

        internal static float CubicLengthOrder5(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            float x0o5 = p0.X * a0o5 + p1.X * b0o5 + p2.X * c0o5 + p3.X * d0o5;
            float x1o5 = p0.X * a1o5 + p1.X * b1o5 + p2.X * c1o5 + p3.X * d1o5;
            float x2o5 = p0.X * a2o5 + p1.X * b2o5 + p2.X * c2o5 + p3.X * d2o5;
            float x3o5 = p0.X * a3o5 + p1.X * b3o5 + p2.X * c3o5 + p3.X * d3o5;
            float x4o5 = p0.X * a4o5 + p1.X * b4o5 + p2.X * c4o5 + p3.X * d4o5;

            float y0o5 = p0.Y * a0o5 + p1.Y * b0o5 + p2.Y * c0o5 + p3.Y * d0o5;
            float y1o5 = p0.Y * a1o5 + p1.Y * b1o5 + p2.Y * c1o5 + p3.Y * d1o5;
            float y2o5 = p0.Y * a2o5 + p1.Y * b2o5 + p2.Y * c2o5 + p3.Y * d2o5;
            float y3o5 = p0.Y * a3o5 + p1.Y * b3o5 + p2.Y * c3o5 + p3.Y * d3o5;
            float y4o5 = p0.Y * a4o5 + p1.Y * b4o5 + p2.Y * c4o5 + p3.Y * d4o5;

            Vector2 derivative0o5 = new Vector2(x0o5, y0o5);
            Vector2 derivative1o5 = new Vector2(x1o5, y1o5);
            Vector2 derivative2o5 = new Vector2(x2o5, y2o5);
            Vector2 derivative3o5 = new Vector2(x3o5, y3o5);
            Vector2 derivative4o5 = new Vector2(x4o5, y4o5);

            float speed0o5 = derivative0o5.Length();
            float speed1o5 = derivative1o5.Length();
            float speed2o5 = derivative2o5.Length();
            float speed3o5 = derivative3o5.Length();
            float speed4o5 = derivative4o5.Length();

            return (weight0o5 * speed0o5
                + weight1o5 * speed1o5
                + weight2o5 * speed2o5
                + weight3o5 * speed3o5
                + weight4o5 * speed4o5) * 0.5f;
        }
        #endregion

        #region GaussLegendre_Order7
        const float weight0o7 = 0.1294849662f;
        const float weight1o7 = 0.2797053915f;
        const float weight2o7 = 0.3818300505f;
        const float weight3o7 = 0.4179591837f;
        const float weight4o7 = 0.3818300505f;
        const float weight5o7 = 0.2797053915f;
        const float weight6o7 = 0.1294849662f;

        const float a0o7 = -2.849266f;
        const float a1o7 = -2.274698f;
        const float a2o7 = -1.482301f;
        const float a3o7 = -0.75f;
        const float a4o7 = -0.264765f;
        const float a5o7 = -0.05010461f;
        const float a6o7 = -0.001942506f;

        const float b0o7 = 2.700475f;
        const float b1o7 = 1.599501f;
        const float b2o7 = 0.2293659f;
        const float b3o7 = -0.75f;
        const float b4o7 = -0.9881696f;
        const float b5o7 = -0.6250927f;
        const float b6o7 = -0.1468488f;

        const float c0o7 = 0.1468488f;
        const float c1o7 = 0.6250926f;
        const float c2o7 = 0.9881696f;
        const float c3o7 = 0.75f;
        const float c4o7 = -0.2293659f;
        const float c5o7 = -1.599501f;
        const float c6o7 = -2.700475f;

        const float d0o7 = 0.001942506f;
        const float d1o7 = 0.05010459f;
        const float d2o7 = 0.264765f;
        const float d3o7 = 0.75f;
        const float d4o7 = 1.482301f;
        const float d5o7 = 2.274698f;
        const float d6o7 = 2.849266f;

        internal static float CubicLengthOrder7(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            float x0o7 = p0.X * a0o7 + p1.X * b0o7 + p2.X * c0o7 + p3.X * d0o7;
            float x1o7 = p0.X * a1o7 + p1.X * b1o7 + p2.X * c1o7 + p3.X * d1o7;
            float x2o7 = p0.X * a2o7 + p1.X * b2o7 + p2.X * c2o7 + p3.X * d2o7;
            float x3o7 = p0.X * a3o7 + p1.X * b3o7 + p2.X * c3o7 + p3.X * d3o7;
            float x4o7 = p0.X * a4o7 + p1.X * b4o7 + p2.X * c4o7 + p3.X * d4o7;
            float x5o7 = p0.X * a5o7 + p1.X * b5o7 + p2.X * c5o7 + p3.X * d5o7;
            float x6o7 = p0.X * a6o7 + p1.X * b6o7 + p2.X * c6o7 + p3.X * d6o7;

            float y0o7 = p0.Y * a0o7 + p1.Y * b0o7 + p2.Y * c0o7 + p3.Y * d0o7;
            float y1o7 = p0.Y * a1o7 + p1.Y * b1o7 + p2.Y * c1o7 + p3.Y * d1o7;
            float y2o7 = p0.Y * a2o7 + p1.Y * b2o7 + p2.Y * c2o7 + p3.Y * d2o7;
            float y3o7 = p0.Y * a3o7 + p1.Y * b3o7 + p2.Y * c3o7 + p3.Y * d3o7;
            float y4o7 = p0.Y * a4o7 + p1.Y * b4o7 + p2.Y * c4o7 + p3.Y * d4o7;
            float y5o7 = p0.Y * a5o7 + p1.Y * b5o7 + p2.Y * c5o7 + p3.Y * d5o7;
            float y6o7 = p0.Y * a6o7 + p1.Y * b6o7 + p2.Y * c6o7 + p3.Y * d6o7;

            Vector2 derivative0o7 = new Vector2(x0o7, y0o7);
            Vector2 derivative1o7 = new Vector2(x1o7, y1o7);
            Vector2 derivative2o7 = new Vector2(x2o7, y2o7);
            Vector2 derivative3o7 = new Vector2(x3o7, y3o7);
            Vector2 derivative4o7 = new Vector2(x4o7, y4o7);
            Vector2 derivative5o7 = new Vector2(x5o7, y5o7);
            Vector2 derivative6o7 = new Vector2(x6o7, y6o7);

            float speed0o7 = derivative0o7.Length();
            float speed1o7 = derivative1o7.Length();
            float speed2o7 = derivative2o7.Length();
            float speed3o7 = derivative3o7.Length();
            float speed4o7 = derivative4o7.Length();
            float speed5o7 = derivative5o7.Length();
            float speed6o7 = derivative6o7.Length();

            return (weight0o7 * speed0o7
                + weight1o7 * speed1o7
                + weight2o7 * speed2o7
                + weight3o7 * speed3o7
                + weight4o7 * speed4o7
                + weight5o7 * speed5o7
                + weight6o7 * speed6o7) * 0.5f;
        }
        #endregion
    }
}