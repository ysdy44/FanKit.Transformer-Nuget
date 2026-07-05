using FanKit.Transformer.Cache;
using System.Numerics;

namespace FanKit.Transformer.Curves
{
    partial class PathBuilderExtensions
    {
        #region RoundRectangle
        public static void CreateRoundRectangle(this IPathBuilder pathBuilder, Box1 bounds, float cornerRadius = 0.12f)
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

        public static void CreateRoundRectangle(this IPathBuilder pathBuilder, Box1 bounds, Matrix3x2 matrix, float cornerRadius = 0.12f)
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

        private static void CreateRoundRectangleCore(IPathBuilder pathBuilder,
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
        public static void CreateTriangle(this IPathBuilder pathBuilder, Box0 bounds, float center = 0.5f)
        {
            CreateTriangleCore(pathBuilder, bounds.LeftTop, bounds.RightTop, bounds.RightBottom, bounds.LeftBottom, center);
        }

        public static void CreateTriangle(this IPathBuilder pathBuilder, Box0 bounds, Matrix3x2 matrix, float center = 0.5f)
        {
            CreateTriangleCore(pathBuilder, Vector2.Transform(bounds.LeftTop, matrix), Vector2.Transform(bounds.RightTop, matrix), Vector2.Transform(bounds.RightBottom, matrix), Vector2.Transform(bounds.LeftBottom, matrix), center);
        }

        public static void CreateTriangle(this IPathBuilder pathBuilder, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, float center)
        {
            CreateTriangleCore(pathBuilder, leftTop, rightTop, rightBottom, leftBottom, center);
        }

        public static void CreateTriangle(this IPathBuilder pathBuilder, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, Matrix3x2 matrix, float center)
        {
            CreateTriangleCore(pathBuilder, Vector2.Transform(leftTop, matrix), Vector2.Transform(rightTop, matrix), Vector2.Transform(rightBottom, matrix), Vector2.Transform(leftBottom, matrix), center);
        }

        private static void CreateTriangleCore(IPathBuilder pathBuilder, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, float center)
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
        public static void CreateDiamond(this IPathBuilder pathBuilder, Box1 bounds, float mid = 0.5f)
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

        public static void CreateDiamond(this IPathBuilder pathBuilder, Box1 bounds, Matrix3x2 matrix, float mid = 0.5f)
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

        private static void CreateDiamondCore(IPathBuilder pathBuilder,
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
    }
}