using FanKit.Transformer.Cache;
using System.Numerics;

namespace FanKit.Transformer.Curves
{
    partial class PathBuilderExtensions
    {
        #region Donut
        public static void CreateDonut(this IPathBuilder pathBuilder, Box1 bounds, float holeRadius = 0.5f)
        {
            bool zeroHoleRadius = holeRadius == 0f;
            CreateEllipse(pathBuilder, bounds);

            if (zeroHoleRadius)
                return;
            else
            {
                Vector2 center = bounds.Center;

                CreateDonutCore(pathBuilder, bounds, holeRadius, center);
            }
        }

        public static void CreateDonut(this IPathBuilder pathBuilder, Box1 bounds, Matrix3x2 matrix, float holeRadius = 0.5f)
        {
            bool zeroHoleRadius = holeRadius == 0f;
            CreateEllipse(pathBuilder, bounds, matrix);

            if (zeroHoleRadius)
                return;
            else
            {
                Vector2 center = Vector2.Transform(bounds.Center, matrix);

                CreateDonutCore(pathBuilder, bounds, holeRadius, center);
            }
        }

        private static void CreateDonutCore(IPathBuilder pathBuilder, Box1 bounds, float holeRadius, Vector2 center)
        {
            // Donut
            Matrix3x2 holeMatrix = Matrix3x2.CreateTranslation(-center) * Matrix3x2.CreateScale(holeRadius) * Matrix3x2.CreateTranslation(center);
            CreateEllipse(pathBuilder, bounds, holeMatrix);
        }
        #endregion

        #region Pie
        public static void CreatePie(this IPathBuilder pathBuilder, Box1 bounds, float startAngle = R0, float sweepAngle = R270)
        {
            bool zeroSweepAngle = sweepAngle == 0f;

            if (zeroSweepAngle)
                CreateEllipse(pathBuilder, bounds);
            else
            {
                Matrix3x2 oneMatrix = bounds.Normalize();

                CreatePieCore(pathBuilder, oneMatrix, startAngle, sweepAngle);
            }
        }

        public static void CreatePie(this IPathBuilder pathBuilder, Box1 bounds, Matrix3x2 matrix, float startAngle = R0, float sweepAngle = R270)
        {
            bool zeroSweepAngle = sweepAngle == 0f;

            if (zeroSweepAngle)
                CreateEllipse(pathBuilder, bounds, matrix);
            else
            {
                Matrix3x2 oneMatrix = bounds.Normalize();
                Matrix3x2 oneMatrix2 = oneMatrix * matrix;

                CreatePieCore(pathBuilder, oneMatrix2, startAngle, sweepAngle);
            }
        }

        private static void CreatePieCore(IPathBuilder pathBuilder, Matrix3x2 oneMatrix, float startAngle, float sweepAngle)
        {
            pathBuilder.BeginFigure(oneMatrix.Translation);

            // tooth point
            CreateArcCore(pathBuilder, oneMatrix, startAngle, sweepAngle, false);

            pathBuilder.AddLine(oneMatrix.Translation);

            // Closed
            pathBuilder.EndFigure(Closed);
        }
        #endregion

        #region Cookie
        public static void CreateCookie(this IPathBuilder pathBuilder, Box1 bounds, float innerRadius = 0.5f, float startAngle = R0, float sweepAngle = R270)
        {
            bool zeroInnerRadius = innerRadius == 0f;
            bool zeroSweepAngle = sweepAngle == 0f;

            if (zeroSweepAngle)
            {
                CreateEllipse(pathBuilder, bounds);

                if (zeroInnerRadius)
                    return;
                else
                {
                    Vector2 center = bounds.Center;

                    CreateDonutCore(pathBuilder, bounds, innerRadius, center);
                }
            }
            else
            {
                Matrix3x2 oneMatrix = bounds.Normalize();

                if (zeroInnerRadius)
                    CreatePieCore(pathBuilder, oneMatrix, startAngle, sweepAngle);
                else
                    CreateCookieCore(pathBuilder, oneMatrix, innerRadius, startAngle, sweepAngle);
            }
        }

        public static void CreateCookie(this IPathBuilder pathBuilder, Box1 bounds, Matrix3x2 matrix, float innerRadius = 0.5f, float startAngle = R0, float sweepAngle = R270)
        {
            bool zeroInnerRadius = innerRadius == 0f;
            bool zeroSweepAngle = sweepAngle == 0f;

            if (zeroSweepAngle)
            {
                CreateEllipse(pathBuilder, bounds, matrix);

                if (zeroInnerRadius)
                    return;
                else
                {
                    Vector2 center = Vector2.Transform(bounds.Center, matrix);

                    CreateDonutCore(pathBuilder, bounds, innerRadius, center);
                }
            }
            else
            {
                Matrix3x2 oneMatrix = bounds.Normalize();
                Matrix3x2 oneMatrix2 = oneMatrix * matrix;

                if (zeroInnerRadius)
                    CreatePieCore(pathBuilder, oneMatrix2, startAngle, sweepAngle);
                else
                    CreateCookieCore(pathBuilder, oneMatrix2, innerRadius, startAngle, sweepAngle);
            }
        }

        private static void CreateCookieCore(IPathBuilder pathBuilder, Matrix3x2 oneMatrix, float innerRadius, float startAngle, float sweepAngle)
        {
            // notch point
            Matrix3x2 innerOneMatrix = Matrix3x2.CreateScale(innerRadius) * oneMatrix;
            CreateArcCore(pathBuilder, innerOneMatrix, R360 - sweepAngle, -sweepAngle, true);

            // tooth point
            CreateArcCore(pathBuilder, oneMatrix, startAngle, sweepAngle, false);

            // Closed
            pathBuilder.EndFigure(Closed);
        }
        #endregion
    }
}