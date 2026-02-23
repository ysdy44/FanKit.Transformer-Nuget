using System;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Mathematics
{
    public struct Quadratic
    {
        public Vector2 Q0;
        public Vector2 Q1;
        public Vector2 Q2;

        public Linear Linear(float t, float i)
        {
            return new Linear
            {
                L0 = new Vector2(Q1.X * t + Q0.X * i, Q1.Y * t + Q0.Y * i),
                L1 = new Vector2(Q2.X * t + Q1.X * i, Q2.Y * t + Q1.Y * i),
            };
        }

        public Vector2 Lerp(float t, float i)
        {
            float i2 = i * i;
            float t2 = t * t;
            float it2 = 2f * i * t;

            return new Vector2
            {
                X = t2 * Q2.X +
                     it2 * Q1.X +
                     i2 * Q0.X,
                Y = t2 * Q2.Y +
                     it2 * Q1.Y +
                     i2 * Q0.Y,
            };
        }

        // Extensions
        public Cubic Cubic()
        {
            return new Cubic
            {
                C0 = Q0,
                C1 = new Vector2
                {
                    X = (Q0.X + Q1.X + Q1.X) / 3f,
                    Y = (Q0.Y + Q1.Y + Q1.Y) / 3f,
                },
                C2 = new Vector2
                {
                    X = (Q1.X + Q1.X + Q2.X) / 3f,
                    Y = (Q1.Y + Q1.Y + Q2.Y) / 3f,
                },
                C3 = Q2,
            };
        }

        // B(t) = (1-t)²P0 + 2(1-t)tP1 + t²P2
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
                Vector2 firstDeriv = FirstDerivative(t);
                Vector2 secondDeriv = SecondDerivative();

                Vector2 diff = new Vector2(curvePoint.X - point.X, curvePoint.Y - point.Y);
                float f = diff.X * firstDeriv.X + diff.Y * firstDeriv.Y;
                float fPrime = firstDeriv.X * firstDeriv.X + firstDeriv.Y * firstDeriv.Y +
                  diff.X * secondDeriv.X + diff.Y * secondDeriv.Y;

                if (System.Math.Abs(fPrime) < float.Epsilon)
                    break;

                float delta = f / fPrime;
                t -= delta;
                t = System.Math.Max(0, System.Math.Min(1, t));

                if (System.Math.Abs(delta) < tolerance)
                    break;
            }

            return t;
        }

        // B'(t) = 2(1-t)(P1-P0) + 2t(P2-P1)
        private Vector2 FirstDerivative(float t)
        {
            return new Vector2
            {
                X = 2 * (1 - t) * (Q1.X - Q0.X) + 2 * t * (Q2.X - Q1.X),
                Y = 2 * (1 - t) * (Q1.Y - Q0.Y) + 2 * t * (Q2.Y - Q1.Y),
            };
        }

        // B''(t) = 2(P2-2P1+P0) 
        private Vector2 SecondDerivative()
        {
            return new Vector2
            {
                X = 2 * (Q2.X - 2 * Q1.X + Q0.X),
                Y = 2 * (Q2.Y - 2 * Q1.Y + Q0.Y),
            };
        }

        // Static
        public static Vector2 Point(Vector2 p0, Vector2 p1, Vector2 p2, float t, float i)
        {
            return Constants.QuadraticPoint(p0, p1, p2, t, i);
        }

        public static List<float> Extrema(float p0, float p1, float p2)
        {
            return Constants.QuadraticExtrema(p0, p1, p2);
        }
    }
}