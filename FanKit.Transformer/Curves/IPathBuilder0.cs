using FanKit.Transformer.Cache;
using FanKit.Transformer.Controllers;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Curves
{
    public interface IPathBuilder0
    {
        void BeginFigure(Vector2 startPoint);
        void AddCubicBezier(Vector2 controlPoint1, Vector2 controlPoint2, Vector2 endPoint);
        void AddQuadraticBezier(Vector2 controlPoint, Vector2 endPoint);
        void AddLine(Vector2 endPoint);
        void EndFigure(bool isClosed);
    }

    partial class PathBuilderExtensions
    {
        private static void CreatePoint(IPathBuilder0 pathBuilder, Segment0 previous, Segment0 next)
            => AddBezier(pathBuilder, previous.IsSmooth, next.IsSmooth, previous.Point, next.Point);

        private static void CreatePoint(IPathBuilder0 pathBuilder, Segment1 previous, Segment1 next)
            => AddBezier(pathBuilder, previous.IsSmooth, next.IsSmooth, previous.Point, next.Point);

        private static void CreateActual(IPathBuilder0 pathBuilder, Segment1 previous, Segment1 next)
            => AddBezier(pathBuilder, previous.IsSmooth, next.IsSmooth, previous.Actual, next.Actual);

        private static void CreateRaw(IPathBuilder0 pathBuilder, Segment2 previous, Segment2 next)
            => AddBezier(pathBuilder, previous.IsSmooth, next.IsSmooth, previous.Raw, next.Raw);

        private static void CreateMap(IPathBuilder0 pathBuilder, Segment2 previous, Segment2 next)
            => AddBezier(pathBuilder, previous.IsSmooth, next.IsSmooth, previous.Map, next.Map);

        private static void CreateRaw(IPathBuilder0 pathBuilder, Segment3 previous, Segment3 next)
            => AddBezier(pathBuilder, previous.IsSmooth, next.IsSmooth, previous.Raw, next.Raw);

        private static void CreateActual(IPathBuilder0 pathBuilder, Segment3 previous, Segment3 next)
            => AddBezier(pathBuilder, previous.IsSmooth, next.IsSmooth, previous.Actual, next.Actual);

        private static void AddBezier(IPathBuilder0 pathBuilder, bool previousIsSmooth, bool nextIsSmooth, Node previousPoint, Node nextPoint)
        {
            if (nextIsSmooth)
            {
                if (previousIsSmooth)
                    pathBuilder.AddCubicBezier(previousPoint.RightControlPoint, nextPoint.LeftControlPoint, nextPoint.Point);
                else
                    pathBuilder.AddQuadraticBezier(nextPoint.LeftControlPoint, nextPoint.Point);
            }
            else
            {
                if (previousIsSmooth)
                    pathBuilder.AddQuadraticBezier(previousPoint.RightControlPoint, nextPoint.Point);
                else
                    pathBuilder.AddLine(nextPoint.Point);
            }
        }

        #region ClosestPointer
        public static void CreatePreviousPath(this IPathBuilder0 pathBuilder, ClosestPointer closest)
        {
            // ?

            pathBuilder.BeginFigure(closest.Previous.Point);

            AddBezier(pathBuilder, closest.PreviousIsSmooth, closest.NextIsSmooth, closest.Previous, closest.Current);

            pathBuilder.EndFigure(Open);

            // return
        }

        public static void CreateNextPath(this IPathBuilder0 pathBuilder, ClosestPointer closest)
        {
            // ?

            pathBuilder.BeginFigure(closest.Current.Point);

            AddBezier(pathBuilder, closest.PreviousIsSmooth, closest.NextIsSmooth, closest.Current, closest.Next);

            pathBuilder.EndFigure(Open);

            // return
        }
        #endregion

        #region Node
        public static void CreatePath(this IPathBuilder0 pathBuilder, List<Node> segments, bool isClosed)
        {
            CreatePointPath(pathBuilder, segments, isClosed);
        }

        private static void CreatePointPath(IPathBuilder0 pathBuilder, List<Node> segments, bool isClosed)
        {
            // ?

            Node first = segments[0];
            pathBuilder.BeginFigure(first.Point);

            for (int i = 1; i < segments.Count; i++)
            {
                Node previous = segments[i - 1];
                Node next = segments[i];
                AddBezier(pathBuilder, true, true, previous, next);
            }

            if (isClosed)
            {
                Node last = segments[segments.Count - 1];
                AddBezier(pathBuilder, true, true, last, first);

                pathBuilder.EndFigure(Closed);
            }
            else
            {
                pathBuilder.EndFigure(Open);
            }

            // return
        }
        #endregion

        #region Segment0
        public static void CreatePath(this IPathBuilder0 pathBuilder, List<Segment0> figures, bool isClosed)
        {
            CreatePointPath(pathBuilder, figures, isClosed);
        }

        private static void CreatePointPath(IPathBuilder0 pathBuilder, List<Segment0> segments, bool isClosed)
        {
            // ?

            Segment0 first = segments[0];
            pathBuilder.BeginFigure(first.Point.Point);

            for (int i = 1; i < segments.Count; i++)
            {
                Segment0 previous = segments[i - 1];
                Segment0 next = segments[i];
                CreatePoint(pathBuilder, previous, next);
            }

            if (isClosed)
            {
                Segment0 last = segments[segments.Count - 1];
                CreatePoint(pathBuilder, last, first);

                pathBuilder.EndFigure(Closed);
            }
            else
            {
                pathBuilder.EndFigure(Open);
            }

            // return
        }
        #endregion

        #region Segment1
        public static void CreatePath(this IPathBuilder0 pathBuilder, NodePointUnits unit, List<Segment1> segments, bool isClosed)
        {
            switch (unit)
            {
                case NodePointUnits.Normal:
                    CreatePointPath(pathBuilder, segments, isClosed);
                    break;
                case NodePointUnits.Actual:
                    CreateActualPath(pathBuilder, segments, isClosed);
                    break;
                default:
                    break;
            }
        }

        private static void CreatePointPath(IPathBuilder0 pathBuilder, List<Segment1> segments, bool isClosed)
        {
            // ?

            Segment1 first = segments[0];
            pathBuilder.BeginFigure(first.Point.Point);

            for (int i = 1; i < segments.Count; i++)
            {
                Segment1 previous = segments[i - 1];
                Segment1 next = segments[i];
                CreatePoint(pathBuilder, previous, next);
            }

            if (isClosed)
            {
                Segment1 last = segments[segments.Count - 1];
                CreatePoint(pathBuilder, last, first);

                pathBuilder.EndFigure(Closed);
            }
            else
            {
                pathBuilder.EndFigure(Open);
            }

            // return
        }

        private static void CreateActualPath(IPathBuilder0 pathBuilder, List<Segment1> segments, bool isClosed)
        {
            // ?

            Segment1 first = segments[0];
            pathBuilder.BeginFigure(first.Actual.Point);

            for (int i = 1; i < segments.Count; i++)
            {
                Segment1 previous = segments[i - 1];
                Segment1 next = segments[i];
                CreateActual(pathBuilder, previous, next);
            }

            if (isClosed)
            {
                Segment1 last = segments[segments.Count - 1];
                CreateActual(pathBuilder, last, first);

                pathBuilder.EndFigure(Closed);
            }
            else
            {
                pathBuilder.EndFigure(Open);
            }

            // return
        }
        #endregion

        #region Segment2
        public static void CreatePath(this IPathBuilder0 pathBuilder, NodePointUnits unit, IEnumerable<Figure2> figures)
        {
            switch (unit)
            {
                case NodePointUnits.Normal:
                    CreateRawPath(pathBuilder, figures);
                    break;
                case NodePointUnits.Actual:
                    CreateMapPath(pathBuilder, figures);
                    break;
                default:
                    break;
            }
        }

        private static void CreateRawPath(IPathBuilder0 pathBuilder, IEnumerable<Figure2> figures)
        {
            // ?

            foreach (Figure2 figure in figures)
            {
                Segment2 first = figure.Segments[0];
                pathBuilder.BeginFigure(first.Raw.Point);

                for (int i = 1; i < figure.Segments.Count; i++)
                {
                    Segment2 previous = figure.Segments[i - 1];
                    Segment2 next = figure.Segments[i];
                    CreateRaw(pathBuilder, previous, next);
                }

                if (figure.IsClosed)
                {
                    Segment2 last = figure.Segments[figure.Segments.Count - 1];
                    CreateRaw(pathBuilder, last, first);

                    pathBuilder.EndFigure(Closed);
                }
                else
                {
                    pathBuilder.EndFigure(Open);
                }
            }

            // return
        }

        private static void CreateMapPath(IPathBuilder0 pathBuilder, IEnumerable<Figure2> figures)
        {
            // ?

            foreach (Figure2 figure in figures)
            {
                Segment2 first = figure.Segments[0];
                pathBuilder.BeginFigure(first.Map.Point);

                for (int i = 1; i < figure.Segments.Count; i++)
                {
                    Segment2 previous = figure.Segments[i - 1];
                    Segment2 next = figure.Segments[i];
                    CreateMap(pathBuilder, previous, next);
                }

                if (figure.IsClosed)
                {
                    Segment2 last = figure.Segments[figure.Segments.Count - 1];
                    CreateMap(pathBuilder, last, first);

                    pathBuilder.EndFigure(Closed);
                }
                else
                {
                    pathBuilder.EndFigure(Open);
                }
            }

            // return
        }
        #endregion

        #region Segment3
        public static void CreatePath(this IPathBuilder0 pathBuilder, NodePointUnits unit, IEnumerable<Figure3> figures)
        {
            switch (unit)
            {
                case NodePointUnits.Normal:
                    CreateRawPath(pathBuilder, figures);
                    break;
                case NodePointUnits.Actual:
                    CreateActualPath(pathBuilder, figures);
                    break;
                default:
                    break;
            }
        }

        private static void CreateRawPath(IPathBuilder0 pathBuilder, IEnumerable<Figure3> figures)
        {
            // ?

            foreach (Figure3 figure in figures)
            {
                Segment3 first = figure.Segments[0];
                pathBuilder.BeginFigure(first.Raw.Point);

                for (int i = 1; i < figure.Segments.Count; i++)
                {
                    Segment3 previous = figure.Segments[i - 1];
                    Segment3 next = figure.Segments[i];
                    CreateRaw(pathBuilder, previous, next);
                }

                if (figure.IsClosed)
                {
                    Segment3 last = figure.Segments[figure.Segments.Count - 1];
                    CreateRaw(pathBuilder, last, first);

                    pathBuilder.EndFigure(Closed);
                }
                else
                {
                    pathBuilder.EndFigure(Open);
                }
            }

            // return
        }

        private static void CreateActualPath(IPathBuilder0 pathBuilder, IEnumerable<Figure3> figures)
        {
            // ?

            foreach (Figure3 figure in figures)
            {
                Segment3 first = figure.Segments[0];
                pathBuilder.BeginFigure(first.Actual.Point);

                for (int i = 1; i < figure.Segments.Count; i++)
                {
                    Segment3 previous = figure.Segments[i - 1];
                    Segment3 next = figure.Segments[i];
                    CreateActual(pathBuilder, previous, next);
                }

                if (figure.IsClosed)
                {
                    Segment3 last = figure.Segments[figure.Segments.Count - 1];
                    CreateActual(pathBuilder, last, first);

                    pathBuilder.EndFigure(Closed);
                }
                else
                {
                    pathBuilder.EndFigure(Open);
                }
            }

            // return
        }
        #endregion

        // ---------------------------------------------- Geometry 0 ---------------------------------------------- //

        #region Rectangle
        public static void CreateRectangle(this IPathBuilder0 pathBuilder, Box0 bounds)
        {
            CreateRectangleCore(pathBuilder, bounds.LeftTop, bounds.RightTop, bounds.RightBottom, bounds.LeftBottom);
        }

        public static void CreateRectangle(this IPathBuilder0 pathBuilder, Box0 bounds, Matrix3x2 matrix)
        {
            CreateRectangle(pathBuilder, bounds.LeftTop, bounds.RightTop, bounds.RightBottom, bounds.LeftBottom, matrix);
        }

        public static void CreateRectangle(this IPathBuilder0 pathBuilder, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom)
        {
            CreateRectangleCore(pathBuilder, leftTop, rightTop, rightBottom, leftBottom);
        }

        public static void CreateRectangle(this IPathBuilder0 pathBuilder, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, Matrix3x2 matrix)
        {
            CreateRectangleCore(pathBuilder, Vector2.Transform(leftTop, matrix), Vector2.Transform(rightTop, matrix), Vector2.Transform(rightBottom, matrix), Vector2.Transform(leftBottom, matrix));
        }

        private static void CreateRectangleCore(IPathBuilder0 pathBuilder, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom)
        {
            // Points
            pathBuilder.BeginFigure(leftTop);
            pathBuilder.AddLine(rightTop);
            pathBuilder.AddLine(rightBottom);
            pathBuilder.AddLine(leftBottom);

            // Closed
            pathBuilder.EndFigure(Closed);
        }
        #endregion

        #region Ellipse
        public static void CreateEllipse(this IPathBuilder0 pathBuilder, Box1 bounds)
        {
            CreateEllipseCore(pathBuilder, bounds.CenterLeft, bounds.CenterTop, bounds.CenterRight, bounds.CenterBottom);
        }

        public static void CreateEllipse(this IPathBuilder0 pathBuilder, Box1 bounds, Matrix3x2 matrix)
        {
            CreateEllipse(pathBuilder, bounds.CenterLeft, bounds.CenterTop, bounds.CenterRight, bounds.CenterBottom, matrix);
        }

        public static void CreateEllipse(this IPathBuilder0 pathBuilder, Vector2 centerLeft, Vector2 centerTop, Vector2 centerRight, Vector2 centerBottom)
        {
            CreateEllipseCore(pathBuilder, centerLeft, centerTop, centerRight, centerBottom);
        }

        public static void CreateEllipse(this IPathBuilder0 pathBuilder, Vector2 centerLeft, Vector2 centerTop, Vector2 centerRight, Vector2 centerBottom, Matrix3x2 matrix)
        {
            CreateEllipseCore(pathBuilder, Vector2.Transform(centerLeft, matrix), Vector2.Transform(centerTop, matrix), Vector2.Transform(centerRight, matrix), Vector2.Transform(centerBottom, matrix));
        }

        private static void CreateEllipseCore(IPathBuilder0 pathBuilder, Vector2 centerLeft, Vector2 centerTop, Vector2 centerRight, Vector2 centerBottom)
        {
            // HV
            Vector2 horizontal = (centerRight - centerLeft);
            Vector2 horizontal276 = horizontal * Z276; // vector * Z552 / 2

            Vector2 vertical = (centerBottom - centerTop);
            Vector2 vertical276 = vertical * Z276; // vector * Z552 / 2

            // Control
            Vector2 left1 = centerLeft + vertical276;
            Vector2 left2 = centerLeft - vertical276;
            Vector2 top1 = centerTop - horizontal276;
            Vector2 top2 = centerTop + horizontal276;
            Vector2 right1 = centerRight - vertical276;
            Vector2 right2 = centerRight + vertical276;
            Vector2 bottom1 = centerBottom + horizontal276;
            Vector2 bottom2 = centerBottom - horizontal276;

            // Path
            pathBuilder.BeginFigure(centerBottom);
            pathBuilder.AddCubicBezier(bottom2, left1, centerLeft);
            pathBuilder.AddCubicBezier(left2, top1, centerTop);
            pathBuilder.AddCubicBezier(top2, right1, centerRight);
            pathBuilder.AddCubicBezier(right2, bottom1, centerBottom);
            pathBuilder.EndFigure(Closed);
        }
        #endregion

        // ---------------------------------------------- Geometry 1 ---------------------------------------------- //

        #region RoundRectangle
        public static void CreateRoundRectangle(this IPathBuilder0 pathBuilder, Box1 bounds, float cornerRadius = 0.12f)
        {
            CreateRoundRectangleCore(pathBuilder,
                bounds.LeftTop,
                bounds.RightTop,
                bounds.RightBottom,
                bounds.LeftBottom,

                bounds.CenterLeft,
                bounds.CenterTop,
                bounds.CenterRight,
                bounds.CenterBottom,

                cornerRadius);
        }

        public static void CreateRoundRectangle(this IPathBuilder0 pathBuilder, Box1 bounds, Matrix3x2 matrix, float cornerRadius = 0.12f)
        {
            CreateRoundRectangleCore(pathBuilder,
                Vector2.Transform(bounds.LeftTop, matrix),
                Vector2.Transform(bounds.RightTop, matrix),
                Vector2.Transform(bounds.RightBottom, matrix),
                Vector2.Transform(bounds.LeftBottom, matrix),

                Vector2.Transform(bounds.CenterLeft, matrix),
                Vector2.Transform(bounds.CenterTop, matrix),
                Vector2.Transform(bounds.CenterRight, matrix),
                Vector2.Transform(bounds.CenterBottom, matrix),

                cornerRadius);
        }

        private static void CreateRoundRectangleCore(IPathBuilder0 pathBuilder,
            Vector2 leftTop,
            Vector2 rightTop,
            Vector2 rightBottom,
            Vector2 leftBottom,

            Vector2 centerLeft,
            Vector2 centerTop,
            Vector2 centerRight,
            Vector2 centerBottom,

            float cornerRadius)
        {
            // Horizontal
            Vector2 horizontal = (centerRight - centerLeft);
            float horizontalLength = horizontal.Length();
            Vector2 horizontalUnit = horizontal / horizontalLength;
            // Vertical
            Vector2 vertical = (centerBottom - centerTop);
            float verticalLength = vertical.Length();
            Vector2 verticalUnit = vertical / verticalLength;

            // Control
            float minLength = System.Math.Min(horizontalLength, verticalLength);
            float minLength2 = cornerRadius * minLength;

            Vector2 horizontal2 = minLength2 * horizontalUnit;
            Vector2 horizontal448 = horizontal2 * 0.448f; // vector / (1 - 4 * 0.552f)
            Vector2 vertical2 = minLength2 * verticalUnit;
            Vector2 vertical448 = vertical2 * 0.448f; // vector /  (1 - 4 * 0.552f)

            Vector2 leftTop_Left = leftTop + vertical2;
            Vector2 leftTop_Left2 = leftTop + vertical448;
            Vector2 leftTop_Top = leftTop + horizontal2;
            Vector2 leftTop_Top1 = leftTop + horizontal448;

            Vector2 rightTop_Top = rightTop - horizontal2;
            Vector2 rightTop_Top2 = rightTop - horizontal448;
            Vector2 rightTop_Right = rightTop + vertical2;
            Vector2 rightTop_Right1 = rightTop + vertical448;

            Vector2 rightBottom_Right = rightBottom - vertical2;
            Vector2 rightBottom_Right2 = rightBottom - vertical448;
            Vector2 rightBottom_Bottom = rightBottom - horizontal2;
            Vector2 rightBottom_Bottom1 = rightBottom - horizontal448;

            Vector2 leftBottom_Bottom = leftBottom + horizontal2;
            Vector2 leftBottom_Bottom2 = leftBottom + horizontal448;
            Vector2 leftBottom_Left = leftBottom - vertical2;
            Vector2 leftBottom_Left1 = leftBottom - vertical448;

            // Path
            pathBuilder.BeginFigure(leftTop_Left);

            pathBuilder.AddCubicBezier(leftTop_Left2, leftTop_Top1, leftTop_Top);
            pathBuilder.AddLine(rightTop_Top);

            pathBuilder.AddCubicBezier(rightTop_Top2, rightTop_Right1, rightTop_Right);
            pathBuilder.AddLine(rightBottom_Right);

            pathBuilder.AddCubicBezier(rightBottom_Right2, rightBottom_Bottom1, rightBottom_Bottom);
            pathBuilder.AddLine(leftBottom_Bottom);

            pathBuilder.AddCubicBezier(leftBottom_Bottom2, leftBottom_Left1, leftBottom_Left);
            pathBuilder.AddLine(leftBottom_Left);

            // Closed
            pathBuilder.EndFigure(Closed);
        }
        #endregion

        #region Triangle
        public static void CreateTriangle(this IPathBuilder0 pathBuilder, Box0 bounds, float center = 0.5f)
        {
            CreateTriangleCore(pathBuilder, bounds.LeftTop, bounds.RightTop, bounds.RightBottom, bounds.LeftBottom, center);
        }

        public static void CreateTriangle(this IPathBuilder0 pathBuilder, Box0 bounds, Matrix3x2 matrix, float center = 0.5f)
        {
            CreateTriangleCore(pathBuilder, Vector2.Transform(bounds.LeftTop, matrix), Vector2.Transform(bounds.RightTop, matrix), Vector2.Transform(bounds.RightBottom, matrix), Vector2.Transform(bounds.LeftBottom, matrix), center);
        }

        public static void CreateTriangle(this IPathBuilder0 pathBuilder, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, float center)
        {
            CreateTriangleCore(pathBuilder, leftTop, rightTop, rightBottom, leftBottom, center);
        }

        public static void CreateTriangle(this IPathBuilder0 pathBuilder, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, Matrix3x2 matrix, float center)
        {
            CreateTriangleCore(pathBuilder, Vector2.Transform(leftTop, matrix), Vector2.Transform(rightTop, matrix), Vector2.Transform(rightBottom, matrix), Vector2.Transform(leftBottom, matrix), center);
        }

        private static void CreateTriangleCore(IPathBuilder0 pathBuilder, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, float center)
        {
            float minusValue = 1.0f - center;
            Vector2 center2 = leftTop * minusValue + rightTop * center;

            // Points
            pathBuilder.BeginFigure(center2);
            pathBuilder.AddLine(rightBottom);
            pathBuilder.AddLine(leftBottom);

            // Closed
            pathBuilder.EndFigure(Closed);
        }
        #endregion

        #region Diamond
        public static void CreateDiamond(this IPathBuilder0 pathBuilder, Box1 bounds, float mid = 0.5f)
        {
            CreateDiamondCore(pathBuilder,
                 bounds.LeftTop,
                 bounds.RightTop,
                 bounds.RightBottom,
                 bounds.LeftBottom,

                 bounds.CenterLeft,
                 bounds.CenterRight,

                 mid
            );
        }

        public static void CreateDiamond(this IPathBuilder0 pathBuilder, Box1 bounds, Matrix3x2 matrix, float mid = 0.5f)
        {
            CreateDiamondCore(pathBuilder,
               Vector2.Transform(bounds.LeftTop, matrix),
               Vector2.Transform(bounds.RightTop, matrix),
               Vector2.Transform(bounds.RightBottom, matrix),
               Vector2.Transform(bounds.LeftBottom, matrix),

               Vector2.Transform(bounds.CenterLeft, matrix),
               Vector2.Transform(bounds.CenterRight, matrix),

               mid
            );
        }

        private static void CreateDiamondCore(IPathBuilder0 pathBuilder,
            Vector2 leftTop,
            Vector2 rightTop,
            Vector2 rightBottom,
            Vector2 leftBottom,

            Vector2 centerLeft,
            Vector2 centerRight,

            float mid)
        {
            float minusValue = 1.0f - mid;
            Vector2 top = leftTop * minusValue + rightTop * mid;
            Vector2 bottom = leftBottom * minusValue + rightBottom * mid;

            // Points
            pathBuilder.BeginFigure(centerLeft);
            pathBuilder.AddLine(top);
            pathBuilder.AddLine(centerRight);
            pathBuilder.AddLine(bottom);

            // Closed
            pathBuilder.EndFigure(Closed);
        }
        #endregion

        // ---------------------------------------------- Geometry 2 ---------------------------------------------- //

        #region Pentagon
        public static void CreatePentagon(this IPathBuilder0 pathBuilder, Triangle bounds, int points = 5, float startAngle = R0)
        {
            Matrix3x2 oneMatrix = bounds.Normalize();

            CreatePentagonCore(pathBuilder, points, startAngle, oneMatrix);
        }

        public static void CreatePentagon(this IPathBuilder0 pathBuilder, Triangle bounds, Matrix3x2 matrix, int points = 5, float startAngle = R0)
        {
            Matrix3x2 oneMatrix = bounds.Normalize();
            Matrix3x2 oneMatrix2 = oneMatrix * matrix;

            CreatePentagonCore(pathBuilder, points, startAngle, oneMatrix2);
        }

        private static void CreatePentagonCore(IPathBuilder0 pathBuilder, int points, float startAngle, Matrix3x2 oneMatrix)
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
        public static void CreateStar(this IPathBuilder0 pathBuilder, Triangle bounds, int points = 5, float innerRadius = 0.4f, float startAngle = R0)
        {
            Matrix3x2 oneMatrix = bounds.Normalize();

            CreateStarCore(pathBuilder, points, innerRadius, startAngle, oneMatrix);
        }

        public static void CreateStar(this IPathBuilder0 pathBuilder, Triangle bounds, Matrix3x2 matrix, int points = 5, float innerRadius = 0.4f, float startAngle = R0)
        {
            Matrix3x2 oneMatrix = bounds.Normalize();
            Matrix3x2 oneMatrix2 = oneMatrix * matrix;

            CreateStarCore(pathBuilder, points, innerRadius, startAngle, oneMatrix2);
        }

        private static void CreateStarCore(IPathBuilder0 pathBuilder, int points, float innerRadius, float startAngle, Matrix3x2 oneMatrix)
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
        public static void CreateCog(this IPathBuilder0 pathBuilder, Triangle bounds, int count = 8, float innerRadius = 0.7f, float tooth = 0.3f, float notch = 0.6f)
        {
            Matrix3x2 oneMatrix = bounds.Normalize();

            CreateCogCore(pathBuilder, count, innerRadius, tooth, notch, oneMatrix);
        }

        public static void CreateCog(this IPathBuilder0 pathBuilder, Triangle bounds, Matrix3x2 matrix, int count = 8, float innerRadius = 0.7f, float tooth = 0.3f, float notch = 0.6f)
        {
            Matrix3x2 oneMatrix = bounds.Normalize();
            Matrix3x2 oneMatrix2 = oneMatrix * matrix;

            CreateCogCore(pathBuilder, count, innerRadius, tooth, notch, oneMatrix2);
        }

        private static void CreateCogCore(IPathBuilder0 pathBuilder, int count, float innerRadius, float tooth, float notch, Matrix3x2 oneMatrix)
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

        // ---------------------------------------------- Geometry 3 ---------------------------------------------- //

        #region Donut
        public static void CreateDonut(this IPathBuilder0 pathBuilder, Box1 bounds, float holeRadius = 0.5f)
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

        public static void CreateDonut(this IPathBuilder0 pathBuilder, Box1 bounds, Matrix3x2 matrix, float holeRadius = 0.5f)
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

        private static void CreateDonutCore(IPathBuilder0 pathBuilder, Box1 bounds, float holeRadius, Vector2 center)
        {
            // Donut
            Matrix3x2 holeMatrix = Matrix3x2.CreateTranslation(-center) * Matrix3x2.CreateScale(holeRadius) * Matrix3x2.CreateTranslation(center);
            CreateEllipse(pathBuilder, bounds, holeMatrix);
        }
        #endregion

        #region Pie
        public static void CreatePie(this IPathBuilder0 pathBuilder, Box1 bounds, float startAngle = R0, float sweepAngle = R270)
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

        public static void CreatePie(this IPathBuilder0 pathBuilder, Box1 bounds, Matrix3x2 matrix, float startAngle = R0, float sweepAngle = R270)
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

        private static void CreatePieCore(IPathBuilder0 pathBuilder, Matrix3x2 oneMatrix, float startAngle, float sweepAngle)
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
        public static void CreateCookie(this IPathBuilder0 pathBuilder, Box1 bounds, float innerRadius = 0.5f, float startAngle = R0, float sweepAngle = R270)
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

        public static void CreateCookie(this IPathBuilder0 pathBuilder, Box1 bounds, Matrix3x2 matrix, float innerRadius = 0.5f, float startAngle = R0, float sweepAngle = R270)
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

        private static void CreateCookieCore(IPathBuilder0 pathBuilder, Matrix3x2 oneMatrix, float innerRadius, float startAngle, float sweepAngle)
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

        // ---------------------------------------------- Geometry 4 ---------------------------------------------- //

        #region Arrow
        public static void CreateArrow(this IPathBuilder0 pathBuilder, Box2 bounds, bool isAbsolute = false, float width = 10f, float value = 0.5f, GeometryArrowTailType leftTail = GeometryArrowTailType.None, GeometryArrowTailType rightTail = GeometryArrowTailType.Arrow)
        {
            Vector2 center = bounds.Center;
            Vector2 centerLeft = bounds.CenterLeft;
            Vector2 centerRight = bounds.CenterRight;

            // horizontal
            Vector2 horizontal = new Vector2(bounds.HorizontalX, bounds.HorizontalY);
            float horizontalLength = bounds.HorizontalLength;
            // vertical
            Vector2 vertical = new Vector2(bounds.VerticalX, bounds.VerticalY);
            float verticalLength = bounds.VerticalLength;

            Vector2 widthVector = GetArrowWidthVector(isAbsolute, width, value, vertical, verticalLength);

            Vector2 focusVector = GetArrowFocusVector(verticalLength, horizontalLength, horizontal);
            Vector2 leftFocusTransform = (bounds.CenterLeft + focusVector);
            Vector2 rightFocusTransform = (bounds.CenterRight - focusVector);

            CreateArrowCore(pathBuilder,
                widthVector + bounds.Center - center,

                // Left
                centerLeft,
                bounds.LeftBottom,

                bounds.LeftTop,
                leftFocusTransform - centerLeft,
                leftFocusTransform,

                // Right
                centerRight,
                bounds.RightBottom,

                bounds.RightTop,
                rightFocusTransform - centerRight,
                rightFocusTransform,

                leftTail,
                rightTail);
        }

        public static void CreateArrow(this IPathBuilder0 pathBuilder, Box2 bounds, Matrix3x2 matrix, bool isAbsolute = false, float width = 10f, float value = 0.5f, GeometryArrowTailType leftTail = GeometryArrowTailType.None, GeometryArrowTailType rightTail = GeometryArrowTailType.Arrow)
        {
            Vector2 center = Vector2.Transform(bounds.Center, matrix);
            Vector2 centerLeft = Vector2.Transform(bounds.CenterLeft, matrix);
            Vector2 centerRight = Vector2.Transform(bounds.CenterRight, matrix);

            // horizontal
            Vector2 horizontal = new Vector2(bounds.HorizontalX, bounds.HorizontalY);
            float horizontalLength = bounds.HorizontalLength;
            // vertical
            Vector2 vertical = new Vector2(bounds.VerticalX, bounds.VerticalY);
            float verticalLength = bounds.VerticalLength;

            Vector2 widthVector = GetArrowWidthVector(isAbsolute, width, value, vertical, verticalLength);

            Vector2 focusVector = GetArrowFocusVector(verticalLength, horizontalLength, horizontal);
            Vector2 leftFocusTransform = Vector2.Transform(bounds.CenterLeft + focusVector, matrix);
            Vector2 rightFocusTransform = Vector2.Transform(bounds.CenterRight - focusVector, matrix);

            CreateArrowCore(pathBuilder,
                Vector2.Transform(widthVector + bounds.Center, matrix) - center,

                // Left
                centerLeft,
                Vector2.Transform(bounds.LeftBottom, matrix),

                Vector2.Transform(bounds.LeftTop, matrix),
                (leftFocusTransform - centerLeft),
                leftFocusTransform,

                // Right
                centerRight,
                Vector2.Transform(bounds.RightBottom, matrix),

                Vector2.Transform(bounds.RightTop, matrix),
                (rightFocusTransform - centerRight),
                rightFocusTransform,

                leftTail,
                rightTail);
        }

        private static void CreateArrowCore(IPathBuilder0 pathBuilder,
            Vector2 widthVectorTransform,

            // Left
            Vector2 centerLeft,
            Vector2 leftBottom,

            Vector2 leftTop,
            Vector2 leftVector,
            Vector2 leftFocusTransform,

            // Right
            Vector2 centerRight,
            Vector2 rightBottom,

            Vector2 rightTop,
            Vector2 rightVector,
            Vector2 rightFocusTransform,

           GeometryArrowTailType leftTail, GeometryArrowTailType rightTail)
        {
            if (leftTail == GeometryArrowTailType.Arrow && rightTail == GeometryArrowTailType.Arrow)
            {
                pathBuilder.BeginFigure(centerLeft); // L

                pathBuilder.AddLine(leftTop + leftVector); // LT
                pathBuilder.AddLine(leftFocusTransform - widthVectorTransform); // C LT

                pathBuilder.AddLine(rightFocusTransform - widthVectorTransform); // C RT
                pathBuilder.AddLine(rightTop + rightVector); // RT

                pathBuilder.AddLine(centerRight); // R

                pathBuilder.AddLine(rightBottom + rightVector); // RB
                pathBuilder.AddLine(rightFocusTransform + widthVectorTransform); // C RB

                pathBuilder.AddLine(leftFocusTransform + widthVectorTransform); // C LB
                pathBuilder.AddLine(leftBottom + leftVector); // LB

                // Closed
                pathBuilder.AddLine(centerLeft); // L
            }
            else if (leftTail == GeometryArrowTailType.Arrow && rightTail == GeometryArrowTailType.None)
            {
                pathBuilder.BeginFigure(centerLeft); // L

                pathBuilder.AddLine(leftTop + leftVector); // LT
                pathBuilder.AddLine(leftFocusTransform - widthVectorTransform); // C LT

                pathBuilder.AddLine(centerRight - widthVectorTransform); // RT
                pathBuilder.AddLine(centerRight + widthVectorTransform); // RB

                pathBuilder.AddLine(leftFocusTransform + widthVectorTransform); // C LB
                pathBuilder.AddLine(leftBottom + leftVector); // LB
            }
            else if (leftTail == GeometryArrowTailType.None && rightTail == GeometryArrowTailType.Arrow)
            {
                pathBuilder.BeginFigure(centerRight); // R

                pathBuilder.AddLine(rightTop + rightVector); // RT
                pathBuilder.AddLine(rightFocusTransform - widthVectorTransform); // C RT

                pathBuilder.AddLine(centerLeft - widthVectorTransform); // LT
                pathBuilder.AddLine(centerLeft + widthVectorTransform); // LB

                pathBuilder.AddLine(rightFocusTransform + widthVectorTransform); // C RB
                pathBuilder.AddLine(rightBottom + rightVector); // RB
            }
            else
            {
                pathBuilder.BeginFigure(centerLeft + widthVectorTransform); // LB
                pathBuilder.AddLine(centerLeft - widthVectorTransform); // LT
                pathBuilder.AddLine(centerRight - widthVectorTransform); // RT
                pathBuilder.AddLine(centerRight + widthVectorTransform); // RB
            }

            // Closed
            pathBuilder.EndFigure(Closed);
        }
        #endregion

        #region Capsule
        public static void CreateCapsule(this IPathBuilder0 pathBuilder, Box2 bounds)
        {
            Vector2 centerLeft = bounds.CenterLeft;
            Vector2 centerTop = bounds.CenterTop;
            Vector2 centerRight = bounds.CenterRight;
            Vector2 centerBottom = bounds.CenterBottom;

            // Horizontal
            Vector2 horizontal = new Vector2(bounds.HorizontalX, bounds.HorizontalY);
            float horizontalLength = bounds.HorizontalLength;
            Vector2 horizontalUnit = horizontal / horizontalLength;
            // Vertical
            Vector2 vertical = new Vector2(bounds.VerticalX, bounds.VerticalY);
            float verticalLength = bounds.VerticalLength;

            if (horizontalLength < verticalLength) CreateEllipseCore(pathBuilder, bounds.CenterLeft, bounds.CenterTop, bounds.CenterRight, bounds.CenterBottom);

            CreateCapsuleCore(pathBuilder,
                verticalLength,
                horizontalUnit,

                centerTop,
                centerLeft,
                centerRight,
                centerBottom,

                bounds.LeftTop,
                bounds.RightTop,
                bounds.RightBottom,
                bounds.LeftBottom
            );
        }

        public static void CreateCapsule(this IPathBuilder0 pathBuilder, Box1 bounds, Matrix3x2 matrix)
        {
            Vector2 leftTop = Vector2.Transform(bounds.LeftTop, matrix);
            Vector2 rightTop = Vector2.Transform(bounds.RightTop, matrix);
            Vector2 rightBottom = Vector2.Transform(bounds.RightBottom, matrix);
            Vector2 leftBottom = Vector2.Transform(bounds.LeftBottom, matrix);

            Vector2 centerLeft = Vector2.Transform(bounds.CenterLeft, matrix);
            Vector2 centerTop = Vector2.Transform(bounds.CenterTop, matrix);
            Vector2 centerRight = Vector2.Transform(bounds.CenterRight, matrix);
            Vector2 centerBottom = Vector2.Transform(bounds.CenterBottom, matrix);

            // Horizontal
            Vector2 horizontal = (centerRight - centerLeft);
            float horizontalLength = horizontal.Length();
            Vector2 horizontalUnit = horizontal / horizontalLength;
            // Vertical
            Vector2 vertical = (centerBottom - centerTop);
            float verticalLength = vertical.Length();

            if (horizontalLength < verticalLength) CreateEllipse(pathBuilder, bounds, matrix);

            CreateCapsuleCore(pathBuilder,
                verticalLength,
                horizontalUnit,

                centerTop,
                centerLeft,
                centerRight,
                centerBottom,

                leftTop,
                rightTop,
                rightBottom,
                leftBottom);
        }

        private static void CreateCapsuleCore(IPathBuilder0 pathBuilder,
            float verticalLength,
            Vector2 horizontalUnit,

            Vector2 centerTop,
            Vector2 centerLeft,
            Vector2 centerRight,
            Vector2 centerBottom,

            Vector2 leftTop,
            Vector2 rightTop,
            Vector2 rightBottom,
            Vector2 leftBottom)
        {
            // Horizontal
            Vector2 horizontal2 = 0.5f * verticalLength * horizontalUnit;
            Vector2 horizontal448 = horizontal2 * 0.448f; // vector / (1 - 0.552f)
            // Vertical
            Vector2 vertical276 = (centerBottom - centerTop) * 0.276f; // vector / 2 * 0.552f

            // Control
            Vector2 left2 = centerLeft - vertical276;
            Vector2 leftTop_Top = leftTop + horizontal2;
            Vector2 leftTop_Top1 = leftTop + horizontal448;

            Vector2 rightTop_Top = rightTop - horizontal2;
            Vector2 rightTop_Top2 = rightTop - horizontal448;
            Vector2 right1 = centerRight - vertical276;

            Vector2 right2 = centerRight + vertical276;
            Vector2 rightBottom_Bottom = rightBottom - horizontal2;
            Vector2 rightBottom_Bottom1 = rightBottom - horizontal448;

            Vector2 leftBottom_Bottom = leftBottom + horizontal2;
            Vector2 leftBottom_Bottom2 = leftBottom + horizontal448;
            Vector2 left1 = centerLeft + vertical276;

            // Path
            pathBuilder.BeginFigure(centerLeft);

            pathBuilder.AddCubicBezier(left2, leftTop_Top1, leftTop_Top);
            pathBuilder.AddLine(rightTop_Top);

            pathBuilder.AddCubicBezier(rightTop_Top2, right1, centerRight);

            pathBuilder.AddCubicBezier(right2, rightBottom_Bottom1, rightBottom_Bottom);
            pathBuilder.AddLine(leftBottom_Bottom);

            pathBuilder.AddCubicBezier(leftBottom_Bottom2, left1, centerLeft);

            pathBuilder.EndFigure(Closed);
        }
        #endregion

        #region Heart
        public static void CreateHeart(this IPathBuilder0 pathBuilder, Triangle bounds, float spread = 0.8f)
        {
            Matrix3x2 oneMatrix = bounds.Normalize();

            CreateHeartCore(pathBuilder, spread, oneMatrix);
        }

        public static void CreateHeart(this IPathBuilder0 pathBuilder, Triangle bounds, Matrix3x2 matrix, float spread = 0.8f)
        {
            Matrix3x2 oneMatrix = bounds.Normalize();
            Matrix3x2 oneMatrix2 = oneMatrix * matrix;

            CreateHeartCore(pathBuilder, spread, oneMatrix2);
        }

        private static void CreateHeartCore(IPathBuilder0 pathBuilder, float spread, Matrix3x2 oneMatrix)
        {
            Vector2 bottom = new Vector2(0, 1);

            Vector2 leftBottom = new Vector2(-0.84f, 0.178f);
            Vector2 leftBottom2 = leftBottom + new Vector2(-0.2f, -0.2f);

            Vector2 leftTop = new Vector2(-0.84f, -0.6f);
            Vector2 leftTop1 = leftTop + new Vector2(-0.2f, 0.2f);
            Vector2 leftTop2 = leftTop + new Vector2(0.2f, -0.2f);

            Vector2 top1 = new Vector2(-0.2f, -0.8f);
            Vector2 topSpread = HeartTopSpread(spread);
            Vector2 top2 = new Vector2(0.2f, -0.8f);

            Vector2 rightTop = new Vector2(0.84f, -0.6f);
            Vector2 rightTop1 = rightTop + new Vector2(-0.2f, -0.2f);
            Vector2 rightTop2 = rightTop + new Vector2(0.2f, 0.2f);

            Vector2 rightBottom = new Vector2(0.84f, 0.178f);
            Vector2 rightBottom1 = rightBottom + new Vector2(0.2f, -0.2f);

            // Path
            pathBuilder.BeginFigure(Vector2.Transform(bottom, oneMatrix));
            pathBuilder.AddLine(Vector2.Transform(leftBottom, oneMatrix));

            pathBuilder.AddCubicBezier(Vector2.Transform(leftBottom2, oneMatrix), Vector2.Transform(leftTop1, oneMatrix), Vector2.Transform(leftTop, oneMatrix));

            pathBuilder.AddCubicBezier(Vector2.Transform(leftTop2, oneMatrix), Vector2.Transform(top1, oneMatrix), Vector2.Transform(topSpread, oneMatrix));
            pathBuilder.AddCubicBezier(Vector2.Transform(top2, oneMatrix), Vector2.Transform(rightTop1, oneMatrix), Vector2.Transform(rightTop, oneMatrix));

            pathBuilder.AddCubicBezier(Vector2.Transform(rightTop2, oneMatrix), Vector2.Transform(rightBottom1, oneMatrix), Vector2.Transform(rightBottom, oneMatrix));
            pathBuilder.EndFigure(Closed);
        }
        #endregion

        // ---------------------------------------------- Geometry 5 ---------------------------------------------- //

        #region Arc
        public static void CreateArc(this IPathBuilder0 pathBuilder, Triangle bounds, float startAngle = R0, float sweepAngle = R270)
        {
            Matrix3x2 oneMatrix = bounds.Normalize();

            CreateArcCore(pathBuilder, oneMatrix, startAngle, sweepAngle, true, true);
            pathBuilder.EndFigure(Open);
        }

        public static void CreateArc(this IPathBuilder0 pathBuilder, Triangle bounds, Matrix3x2 matrix, float startAngle = R0, float sweepAngle = R270)
        {
            Matrix3x2 oneMatrix = bounds.Normalize();
            Matrix3x2 oneMatrix2 = oneMatrix * matrix;

            CreateArcCore(pathBuilder, oneMatrix2, startAngle, sweepAngle, true, true);
            pathBuilder.EndFigure(Open);
        }

        private static void CreateArcCore(IPathBuilder0 pathBuilder, Matrix3x2 oneMatrix, float startAngle, float sweepAngle, bool isBegin, bool isClosed = false)
        {
            float start = startAngle + R90;
            Rotation2x2 r = new Rotation2x2(start);

            Vector2 centerRight = new Vector2(1, 0);
            Vector2 centerLeft = new Vector2(-1, 0);
            Vector2 centerBottom = new Vector2(0, 1);
            Vector2 centerTop = new Vector2(0, -1);

            // A Ellipse has left, top, right, bottom four nodes.
            // 
            // Control points on the left and right sides of the node.
            // 
            // The distance of the control point 
            // is 0.552f times
            // the length of the square edge.

            // HV
            Vector2 horizontal = (centerRight - centerLeft);
            Vector2 horizontal276 = horizontal * Z276; // vector * Z552 / 2

            Vector2 vertical = (centerBottom - centerTop);
            Vector2 vertical276 = vertical * Z276; // vector * Z552 / 2

            // Control
            Vector2 left1 = centerLeft + vertical276;
            Vector2 left2 = centerLeft - vertical276;
            Vector2 top1 = centerTop - horizontal276;
            Vector2 top2 = centerTop + horizontal276;
            Vector2 right1 = centerRight - vertical276;
            Vector2 right2 = centerRight + vertical276;
            Vector2 bottom1 = centerBottom + horizontal276;
            Vector2 bottom2 = centerBottom - horizontal276;

            switch (GetArcMode(sweepAngle))
            {
                case P360:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(bottom2), oneMatrix), Vector2.Transform(r.T2(left1), oneMatrix), Vector2.Transform(r.T2(centerLeft), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(left2), oneMatrix), Vector2.Transform(r.T2(top1), oneMatrix), Vector2.Transform(r.T2(centerTop), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(top2), oneMatrix), Vector2.Transform(r.T2(right1), oneMatrix), Vector2.Transform(r.T2(centerRight), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(right2), oneMatrix), Vector2.Transform(r.T2(bottom1), oneMatrix), Vector2.Transform(r.T2(centerBottom), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                    }
                    break;
                case P270T360:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(bottom2), oneMatrix), Vector2.Transform(r.T2(left1), oneMatrix), Vector2.Transform(r.T2(centerLeft), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(left2), oneMatrix), Vector2.Transform(r.T2(top1), oneMatrix), Vector2.Transform(r.T2(centerTop), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(top2), oneMatrix), Vector2.Transform(r.T2(right1), oneMatrix), Vector2.Transform(r.T2(centerRight), oneMatrix));

                        float scale = Z552 * (sweepAngle - R270) / RQ;
                        float sweep = sweepAngle - R90;
                        Rotation2x2 arc = new Rotation2x2(sweep);
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(centerBottom * scale + centerRight), oneMatrix), Vector2.Transform(r.T2(arc.NCP(scale)), oneMatrix), Vector2.Transform(r.T2(arc.NP()), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T2(arc.NP()), oneMatrix));
                    }
                    break;
                case P270:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(bottom2), oneMatrix), Vector2.Transform(r.T2(left1), oneMatrix), Vector2.Transform(r.T2(centerLeft), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(left2), oneMatrix), Vector2.Transform(r.T2(top1), oneMatrix), Vector2.Transform(r.T2(centerTop), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(top2), oneMatrix), Vector2.Transform(r.T2(right1), oneMatrix), Vector2.Transform(r.T2(centerRight), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T2(centerRight), oneMatrix));
                    }
                    break;
                case P180T270:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(bottom2), oneMatrix), Vector2.Transform(r.T2(left1), oneMatrix), Vector2.Transform(r.T2(centerLeft), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(left2), oneMatrix), Vector2.Transform(r.T2(top1), oneMatrix), Vector2.Transform(r.T2(centerTop), oneMatrix));

                        float scale = Z552 * (sweepAngle - R180) / RQ;
                        float sweep = sweepAngle - R90;
                        Rotation2x2 arc = new Rotation2x2(sweep);
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(centerRight * scale + centerTop), oneMatrix), Vector2.Transform(r.T2(arc.NCP(scale)), oneMatrix), Vector2.Transform(r.T2(arc.NP()), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T2(arc.NP()), oneMatrix));
                    }
                    break;
                case P180:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(bottom2), oneMatrix), Vector2.Transform(r.T2(left1), oneMatrix), Vector2.Transform(r.T2(centerLeft), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(left2), oneMatrix), Vector2.Transform(r.T2(top1), oneMatrix), Vector2.Transform(r.T2(centerTop), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T2(centerTop), oneMatrix));
                    }
                    break;
                case P90T180:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(bottom2), oneMatrix), Vector2.Transform(r.T2(left1), oneMatrix), Vector2.Transform(r.T2(centerLeft), oneMatrix));

                        float scale = Z552 * (sweepAngle - R90) / RQ;
                        float sweep = sweepAngle - R90;
                        Rotation2x2 arc = new Rotation2x2(sweep);
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(centerTop * scale + centerLeft), oneMatrix), Vector2.Transform(r.T2(arc.NCP(scale)), oneMatrix), Vector2.Transform(r.T2(arc.NP()), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T2(arc.NP()), oneMatrix));
                    }
                    break;
                case P90:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(bottom2), oneMatrix), Vector2.Transform(r.T2(left1), oneMatrix), Vector2.Transform(r.T2(centerLeft), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T2(centerLeft), oneMatrix));
                    }
                    break;
                case P0T90:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T2(centerBottom), oneMatrix));

                        float scale = Z552 * (sweepAngle - R0) / RQ;
                        float sweep = sweepAngle - R90;
                        Rotation2x2 arc = new Rotation2x2(sweep);
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(centerLeft * scale + centerBottom), oneMatrix), Vector2.Transform(r.T2(arc.NCP(scale)), oneMatrix), Vector2.Transform(r.T2(arc.NP()), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T2(arc.NP()), oneMatrix));
                    }
                    break;
                case Z0:
                    break;
                case N0T90:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T3(centerBottom), oneMatrix));

                        float scale = Z552 * (-sweepAngle - R0) / RQ;
                        float sweep = -sweepAngle - R90;
                        Rotation2x2 arc = new Rotation2x2(sweep);
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(centerLeft * scale + centerBottom), oneMatrix), Vector2.Transform(r.T3(arc.NCP(scale)), oneMatrix), Vector2.Transform(r.T3(arc.NP()), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T3(arc.NP()), oneMatrix));
                    }
                    break;
                case N90:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(bottom2), oneMatrix), Vector2.Transform(r.T3(left1), oneMatrix), Vector2.Transform(r.T3(centerLeft), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T3(centerLeft), oneMatrix));
                    }
                    break;
                case N90T180:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(bottom2), oneMatrix), Vector2.Transform(r.T3(left1), oneMatrix), Vector2.Transform(r.T3(centerLeft), oneMatrix));

                        float scale = Z552 * (-sweepAngle - R90) / RQ;
                        float sweep = -sweepAngle - R90;
                        Rotation2x2 arc = new Rotation2x2(sweep);
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(centerTop * scale + centerLeft), oneMatrix), Vector2.Transform(r.T3(arc.NCP(scale)), oneMatrix), Vector2.Transform(r.T3(arc.NP()), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T3(arc.NP()), oneMatrix));
                    }
                    break;
                case N180:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(bottom2), oneMatrix), Vector2.Transform(r.T3(left1), oneMatrix), Vector2.Transform(r.T3(centerLeft), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(left2), oneMatrix), Vector2.Transform(r.T3(top1), oneMatrix), Vector2.Transform(r.T3(centerTop), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T3(centerTop), oneMatrix));
                    }
                    break;
                case N180T270:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(bottom2), oneMatrix), Vector2.Transform(r.T3(left1), oneMatrix), Vector2.Transform(r.T3(centerLeft), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(left2), oneMatrix), Vector2.Transform(r.T3(top1), oneMatrix), Vector2.Transform(r.T3(centerTop), oneMatrix));

                        float scale = Z552 * (-sweepAngle - R180) / RQ;
                        float sweep = -sweepAngle - R90;
                        Rotation2x2 arc = new Rotation2x2(sweep);
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(centerRight * scale + centerTop), oneMatrix), Vector2.Transform(r.T3(arc.NCP(scale)), oneMatrix), Vector2.Transform(r.T3(arc.NP()), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T3(arc.NP()), oneMatrix));
                    }
                    break;
                case N270:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(bottom2), oneMatrix), Vector2.Transform(r.T3(left1), oneMatrix), Vector2.Transform(r.T3(centerLeft), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(left2), oneMatrix), Vector2.Transform(r.T3(top1), oneMatrix), Vector2.Transform(r.T3(centerTop), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(top2), oneMatrix), Vector2.Transform(r.T3(right1), oneMatrix), Vector2.Transform(r.T3(centerRight), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T3(centerRight), oneMatrix));
                    }
                    break;
                case N270T360:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(bottom2), oneMatrix), Vector2.Transform(r.T3(left1), oneMatrix), Vector2.Transform(r.T3(centerLeft), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(left2), oneMatrix), Vector2.Transform(r.T3(top1), oneMatrix), Vector2.Transform(r.T3(centerTop), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(top2), oneMatrix), Vector2.Transform(r.T3(right1), oneMatrix), Vector2.Transform(r.T3(centerRight), oneMatrix));

                        float scale = Z552 * (-sweepAngle - R270) / RQ;
                        float sweep = -sweepAngle - R90;
                        Rotation2x2 arc = new Rotation2x2(sweep);
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(centerBottom * scale + centerRight), oneMatrix), Vector2.Transform(r.T3(arc.NCP(scale)), oneMatrix), Vector2.Transform(r.T3(arc.NP()), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T3(arc.NP()), oneMatrix));
                    }
                    break;
                case N360:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(bottom2), oneMatrix), Vector2.Transform(r.T3(left1), oneMatrix), Vector2.Transform(r.T3(centerLeft), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(left2), oneMatrix), Vector2.Transform(r.T3(top1), oneMatrix), Vector2.Transform(r.T3(centerTop), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(top2), oneMatrix), Vector2.Transform(r.T3(right1), oneMatrix), Vector2.Transform(r.T3(centerRight), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(right2), oneMatrix), Vector2.Transform(r.T3(bottom1), oneMatrix), Vector2.Transform(r.T3(centerBottom), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}