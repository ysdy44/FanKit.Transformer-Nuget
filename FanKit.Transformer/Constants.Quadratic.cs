using System;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer
{
    partial class Constants
    {
        // Static
        internal static Vector2 QuadraticPoint(Vector2 p0, Vector2 p1, Vector2 p2, float t, float i)
        {
            float i2 = i * i;
            float t2 = t * t;
            float it2 = 2f * i * t;

            return new Vector2
            {
                X = t2 * p2.X +
                     it2 * p1.X +
                     i2 * p0.X,
                Y = t2 * p2.Y +
                     it2 * p1.Y +
                     i2 * p0.Y,
            };
        }

        internal static List<float> QuadraticExtrema(float p0, float p1, float p2)
        {
            List<float> tValues = new List<float>();

            float a = p0 - 2 * p1 + p2;
            float b = p1 - p0;

            if (Math.Abs(a) > float.Epsilon)
            {
                float t = -b / a;
                if (t >= 0 && t <= 1)
                    tValues.Add(t);
            }

            return tValues;
        }
    }
}