using FanKit.Transformer.Cache;
using System.Numerics;

namespace FanKit.Transformer.Curves
{
    partial class PathBuilderExtensions
    {
        const float Z276 = 0.276114f;
        const float Z552 = 0.55228f;

        #region Rectangle
        public static void CreateRectangle(this IPathBuilder pathBuilder, Box0 bounds)
        {
            CreateRectangleCore(pathBuilder, bounds.LeftTop, bounds.RightTop, bounds.RightBottom, bounds.LeftBottom);
        }

        public static void CreateRectangle(this IPathBuilder pathBuilder, Box0 bounds, Matrix3x2 matrix)
        {
            CreateRectangle(pathBuilder, bounds.LeftTop, bounds.RightTop, bounds.RightBottom, bounds.LeftBottom, matrix);
        }

        public static void CreateRectangle(this IPathBuilder pathBuilder, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom)
        {
            CreateRectangleCore(pathBuilder, leftTop, rightTop, rightBottom, leftBottom);
        }

        public static void CreateRectangle(this IPathBuilder pathBuilder, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, Matrix3x2 matrix)
        {
            CreateRectangleCore(pathBuilder, Vector2.Transform(leftTop, matrix), Vector2.Transform(rightTop, matrix), Vector2.Transform(rightBottom, matrix), Vector2.Transform(leftBottom, matrix));
        }

        private static void CreateRectangleCore(IPathBuilder pathBuilder, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom)
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
        public static void CreateEllipse(this IPathBuilder pathBuilder, Box1 bounds)
        {
            CreateEllipseCore(pathBuilder, bounds.CenterLeft, bounds.CenterTop, bounds.CenterRight, bounds.CenterBottom);
        }

        public static void CreateEllipse(this IPathBuilder pathBuilder, Box1 bounds, Matrix3x2 matrix)
        {
            CreateEllipse(pathBuilder, bounds.CenterLeft, bounds.CenterTop, bounds.CenterRight, bounds.CenterBottom, matrix);
        }

        public static void CreateEllipse(this IPathBuilder pathBuilder, Vector2 centerLeft, Vector2 centerTop, Vector2 centerRight, Vector2 centerBottom)
        {
            CreateEllipseCore(pathBuilder, centerLeft, centerTop, centerRight, centerBottom);
        }

        public static void CreateEllipse(this IPathBuilder pathBuilder, Vector2 centerLeft, Vector2 centerTop, Vector2 centerRight, Vector2 centerBottom, Matrix3x2 matrix)
        {
            CreateEllipseCore(pathBuilder, Vector2.Transform(centerLeft, matrix), Vector2.Transform(centerTop, matrix), Vector2.Transform(centerRight, matrix), Vector2.Transform(centerBottom, matrix));
        }

        private static void CreateEllipseCore(IPathBuilder pathBuilder, Vector2 centerLeft, Vector2 centerTop, Vector2 centerRight, Vector2 centerBottom)
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
    }
}