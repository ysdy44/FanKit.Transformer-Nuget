using FanKit.Transformer.Cache;
using System.Numerics;

namespace FanKit.Transformer.Curves
{
    partial class PathBuilderExtensions
    {
        #region Pentagon
        public static void CreatePentagon(this IPathBuilder pathBuilder, Triangle bounds, int points = 5, float startAngle = R0)
        {
            Matrix3x2 oneMatrix = bounds.Normalize();

            CreatePentagonCore(pathBuilder, points, startAngle, oneMatrix);
        }

        public static void CreatePentagon(this IPathBuilder pathBuilder, Triangle bounds, Matrix3x2 matrix, int points = 5, float startAngle = R0)
        {
            Matrix3x2 oneMatrix = bounds.Normalize();
            Matrix3x2 oneMatrix2 = oneMatrix * matrix;

            CreatePentagonCore(pathBuilder, points, startAngle, oneMatrix2);
        }

        private static void CreatePentagonCore(IPathBuilder pathBuilder, int points, float startAngle, Matrix3x2 oneMatrix)
        {
            float rotation = startAngle - PIOver2;
            float angle = PITwice / points;

            for (int i = 0; i < points; i++)
            {
                // Outer
                Vector2 outer = new Rotation2x2(rotation).Normalize();
                Vector2 outerTransform = Vector2.Transform(outer, oneMatrix);
                if (i == 0)
                    pathBuilder.BeginFigure(outerTransform);
                else
                    pathBuilder.AddLine(outerTransform);
                rotation += angle;
            }

            // Closed
            pathBuilder.EndFigure(Closed);
        }
        #endregion

        #region Star
        public static void CreateStar(this IPathBuilder pathBuilder, Triangle bounds, int points = 5, float innerRadius = 0.4f, float startAngle = R0)
        {
            Matrix3x2 oneMatrix = bounds.Normalize();

            CreateStarCore(pathBuilder, points, innerRadius, startAngle, oneMatrix);
        }

        public static void CreateStar(this IPathBuilder pathBuilder, Triangle bounds, Matrix3x2 matrix, int points = 5, float innerRadius = 0.4f, float startAngle = R0)
        {
            Matrix3x2 oneMatrix = bounds.Normalize();
            Matrix3x2 oneMatrix2 = oneMatrix * matrix;

            CreateStarCore(pathBuilder, points, innerRadius, startAngle, oneMatrix2);
        }

        private static void CreateStarCore(IPathBuilder pathBuilder, int points, float innerRadius, float startAngle, Matrix3x2 oneMatrix)
        {
            float rotation = startAngle - PIOver2;
            float angle = PI / points;

            for (int i = 0; i < points; i++)
            {
                // Outer
                Vector2 outer = new Rotation2x2(rotation).Normalize();
                Vector2 outerTransform = Vector2.Transform(outer, oneMatrix);
                if (i == 0)
                    pathBuilder.BeginFigure(outerTransform);
                else
                    pathBuilder.AddLine(outerTransform);
                rotation += angle;

                // Inner
                Vector2 inner = new Rotation2x2(rotation).Normalize();
                Vector2 inner2 = inner * innerRadius;
                Vector2 inner2Transform = Vector2.Transform(inner2, oneMatrix);
                pathBuilder.AddLine(inner2Transform);
                rotation += angle;
            }

            // Closed
            pathBuilder.EndFigure(Closed);
        }
        #endregion

        #region Cog
        public static void CreateCog(this IPathBuilder pathBuilder, Triangle bounds, int count = 8, float innerRadius = 0.7f, float tooth = 0.3f, float notch = 0.6f)
        {
            Matrix3x2 oneMatrix = bounds.Normalize();

            CreateCogCore(pathBuilder, count, innerRadius, tooth, notch, oneMatrix);
        }

        public static void CreateCog(this IPathBuilder pathBuilder, Triangle bounds, Matrix3x2 matrix, int count = 8, float innerRadius = 0.7f, float tooth = 0.3f, float notch = 0.6f)
        {
            Matrix3x2 oneMatrix = bounds.Normalize();
            Matrix3x2 oneMatrix2 = oneMatrix * matrix;

            CreateCogCore(pathBuilder, count, innerRadius, tooth, notch, oneMatrix2);
        }

        private static void CreateCogCore(IPathBuilder pathBuilder, int count, float innerRadius, float tooth, float notch, Matrix3x2 oneMatrix)
        {
            float angle = PITwice / count; // angle
            float angleTooth = angle * tooth; // angle tooth
            float angleNotch = angle * notch; // angle notch
            float angleDiffHalf = (angleNotch - angleTooth) / 2f; // Half the angle difference between the tooth and the notch

            float rotation = 0f; // Start angle is zero
            int countQuadra = count * 4;

            Vector2 vectorStarting = new Vector2((float)System.Math.Cos(rotation), (float)System.Math.Sin(rotation));

            // Inner
            Vector2 innerStarting = vectorStarting * innerRadius;
            Vector2 innerTransformStarting = Vector2.Transform(innerStarting, oneMatrix);
            pathBuilder.BeginFigure(innerTransformStarting);
            rotation += angleDiffHalf;

            for (int i = 1; i < countQuadra; i++)
            {
                Vector2 vector = new Vector2((float)System.Math.Cos(rotation), (float)System.Math.Sin(rotation));
                int remainder = i % 4; // remainder

                if (remainder == 0) // 凸 left-bottom point
                {
                    // Inner
                    Vector2 inner = vector * innerRadius;
                    Vector2 innerTransform = Vector2.Transform(inner, oneMatrix);
                    pathBuilder.AddLine(innerTransform);
                    rotation += angleDiffHalf;
                }
                else if (remainder == 1) // 凸 left-top point
                {
                    // Outer
                    Vector2 outer = vector;
                    Vector2 outerTransform = Vector2.Transform(vector, oneMatrix);
                    pathBuilder.AddLine(outerTransform);
                    rotation += angleTooth;
                }
                else if (remainder == 2) // 凸 right-top point
                {
                    // Outer
                    Vector2 outer = vector;
                    Vector2 outerTransform = Vector2.Transform(vector, oneMatrix);
                    pathBuilder.AddLine(outerTransform);
                    rotation += angleDiffHalf;
                }
                else if (remainder == 3) // 凸 right-bottom point
                {
                    // Inner
                    Vector2 inner = vector * innerRadius;
                    Vector2 innerTransform = Vector2.Transform(inner, oneMatrix);
                    pathBuilder.AddLine(innerTransform);
                    rotation += angle - angleNotch;
                }
            }

            // Closed
            pathBuilder.EndFigure(Closed);
        }
        #endregion
    }
}