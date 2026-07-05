using FanKit.Transformer.Cache;
using System.Numerics;

namespace FanKit.Transformer.Curves
{
    partial class PathBuilderExtensions
    {
        #region Arrow
        public static void CreateArrow(this IPathBuilder pathBuilder, Box2 bounds, bool isAbsolute = false, float width = 10f, float value = 0.5f, GeometryArrowTailType leftTail = GeometryArrowTailType.None, GeometryArrowTailType rightTail = GeometryArrowTailType.Arrow)
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

        public static void CreateArrow(this IPathBuilder pathBuilder, Box2 bounds, Matrix3x2 matrix, bool isAbsolute = false, float width = 10f, float value = 0.5f, GeometryArrowTailType leftTail = GeometryArrowTailType.None, GeometryArrowTailType rightTail = GeometryArrowTailType.Arrow)
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

        private static void CreateArrowCore(IPathBuilder pathBuilder,
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

        private static Vector2 GetArrowFocusVector(float verticalLength, float horizontalLength, Vector2 horizontal)
        {
            if (verticalLength < horizontalLength)
                return 0.5f * (verticalLength / horizontalLength) * horizontal;
            else
                return 0.5f * horizontal;
        }

        private static Vector2 GetArrowWidthVector(bool isAbsolute, float width2, float value, Vector2 vertical, float verticalLength)
        {
            float width = isAbsolute ? width2 : value * verticalLength;
            return vertical * (width / verticalLength) / 2;
        }
        #endregion

        #region Capsule
        public static void CreateCapsule(this IPathBuilder pathBuilder, Box2 bounds)
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

        public static void CreateCapsule(this IPathBuilder pathBuilder, Box1 bounds, Matrix3x2 matrix)
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

        private static void CreateCapsuleCore(IPathBuilder pathBuilder,
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
        public static void CreateHeart(this IPathBuilder pathBuilder, Triangle bounds, float spread = 0.8f)
        {
            Matrix3x2 oneMatrix = bounds.Normalize();

            CreateHeartCore(pathBuilder, spread, oneMatrix);
        }

        public static void CreateHeart(this IPathBuilder pathBuilder, Triangle bounds, Matrix3x2 matrix, float spread = 0.8f)
        {
            Matrix3x2 oneMatrix = bounds.Normalize();
            Matrix3x2 oneMatrix2 = oneMatrix * matrix;

            CreateHeartCore(pathBuilder, spread, oneMatrix2);
        }

        private static void CreateHeartCore(IPathBuilder pathBuilder, float spread, Matrix3x2 oneMatrix)
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

        private static Vector2 HeartTopSpread(float spread)
        {
            // Rang
            //   x: 0~1
            //   y: 1.0~ - 0.8
            //  y=1 - 1.8x
            float topSpread = 1f - spread * 1.8f;
            return new Vector2(0, topSpread);
        }
        #endregion
    }
}