using System;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Mathematics
{
    public struct Cubic
    {
        public Vector2 C0;
        public Vector2 C1;
        public Vector2 C2;
        public Vector2 C3;

        public float Length(CubicGaussLegendre order = CubicGaussLegendre.Order5)
        {
            switch (order)
            {
                case CubicGaussLegendre.Order5:
                    return Constants.CubicLengthOrder5(C0, C1, C2, C3);
                case CubicGaussLegendre.Order7:
                    return Constants.CubicLengthOrder7(C0, C1, C2, C3);
                default:
                    return 0f;
            }
        }

        public Quadratic Quadratic(float t, float i)
        {
            return new Quadratic
            {
                Q0 = new Vector2(C1.X * t + C0.X * i, C1.Y * t + C0.Y * i),
                Q1 = new Vector2(C2.X * t + C1.X * i, C2.Y * t + C1.Y * i),
                Q2 = new Vector2(C3.X * t + C2.X * i, C3.Y * t + C2.Y * i),
            };
        }

        public Vector2 Lerp(float t, float i)
        {
            float i2 = i * i;
            float i3 = i2 * i;

            float t2 = t * t;
            float t3 = t2 * t;

            return new Vector2
            {
                X = i3 * C0.X +
                     3 * i2 * t * C1.X +
                     3 * i * t2 * C2.X +
                     t3 * C3.X,
                Y = i3 * C0.Y +
                     3 * i2 * t * C1.Y +
                     3 * i * t2 * C2.Y +
                     t3 * C3.Y,
            };
        }

        // B(t) = (1-t)³P0 + 3(1-t)²tP1 + 3(1-t)t²P2 + t³P3
        public float FindClosest(Vector2 point, float tolerance = 0.0001f, int maxIterations = 20)
        {
            const int initialSubdivisions = 100;
            float bestT = 0;
            float minDistanceSq = float.MaxValue;

            for (int k = 0; k <= initialSubdivisions; k++)
            {
                float t = (float)k / initialSubdivisions;
                float i = 1f - t;
                Vector2 curvePoint = Lerp(t, i);
                float distanceSq = Vector2.DistanceSquared(point, curvePoint);

                if (distanceSq < minDistanceSq)
                {
                    minDistanceSq = distanceSq;
                    bestT = t;
                }
            }

            return RefineNewton(point, bestT, tolerance, maxIterations);
        }

        private float RefineNewton(Vector2 point, float initialT, float tolerance, int maxIterations)
        {
            float t = initialT;

            for (int k = 0; k < maxIterations; k++)
            {
                float i = 1f - t;
                Vector2 curvePoint = Lerp(t, i);
                Vector2 firstDeriv = FirstDerivative(t, i);
                Vector2 secondDeriv = SecondDerivative(t, i);

                // f(t) = (B(t) - P) · B'(t)
                Vector2 diff = new Vector2(curvePoint.X - point.X, curvePoint.Y - point.Y);
                float f = diff.X * firstDeriv.X + diff.Y * firstDeriv.Y;

                // f'(t) = B'(t) · B'(t) + (B(t)-P) · B''(t)
                float fPrime = firstDeriv.X * firstDeriv.X + firstDeriv.Y * firstDeriv.Y +
                  diff.X * secondDeriv.X + diff.Y * secondDeriv.Y;

                if (System.Math.Abs(fPrime) < float.Epsilon)
                    break;

                // t_{n+1} = t_n - f(t_n)/f'(t_n)
                float delta = f / fPrime;
                t -= delta;

                t = System.Math.Max(0, System.Math.Min(1, t));

                if (System.Math.Abs(delta) < tolerance)
                    break;
            }

            return t;
        }

        // B'(t) = 3(1-t)²(P1-P0) + 6(1-t)t(P2-P1) + 3t²(P3-P2)
        private Vector2 FirstDerivative(float t, float i)
        {
            float i2 = i * i;
            float t2 = t * t;

            float it = i * t;

            return new Vector2
            {
                X = 3 * i2 * (C1.X - C0.X) +
                    6 * it * (C2.X - C1.X) +
                    3 * t2 * (C3.X - C2.X),
                Y = 3 * i2 * (C1.Y - C0.Y) +
                    6 * it * (C2.Y - C1.Y) +
                    3 * t2 * (C3.Y - C2.Y),
            };
        }

        // B''(t) = 6(1-t)(P2-2P1+P0) + 6t(P3-2P2+P1)
        private Vector2 SecondDerivative(float t, float i)
        {
            return new Vector2
            {
                X = 6 * i * (C2.X - 2 * C1.X + C0.X) +
                    6 * t * (C3.X - 2 * C2.X + C1.X),
                Y = 6 * i * (C2.Y - 2 * C1.Y + C0.Y) +
                    6 * t * (C3.Y - 2 * C2.Y + C1.Y)
            };
        }

        // Static
        public static Vector2 Point(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t, float i)
        {
            return Constants.CubicPoint(p0, p1, p2, p3, t, i);
        }

        public static List<float> Extrema(float p0, float p1, float p2, float p3)
        {
            return Constants.CubicExtrema(p0, p1, p2, p3);
        }
    }
}