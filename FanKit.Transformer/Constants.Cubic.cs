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
    }
}