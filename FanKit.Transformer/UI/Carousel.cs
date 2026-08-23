using FanKit.Transformer.Mathematics;
using System.Numerics;

namespace FanKit.Transformer.UI
{
    // 1. Orgin
    // (110) - (110) - (110) + (110) - (110) - (110)

    // 2. Actual
    // (110) - (110) - (110+60) + (60+110) - (110) - (110)

    // 3. Field
    // ......Left + Right......
    // ......60) + (60......
    public readonly struct Carousel
    {
        readonly float w2;
        readonly float h2;
        readonly float h4;
        readonly float h8;

        readonly Vector2 x0;
        readonly Vector4 yl;
        readonly Vector4 yr;

         readonly Quadrilateral ql;
        readonly Quadrilateral qr;

        // 0.0 ~ 1.0
        //
        // 0.0: Rectangle
        // 1.0: Quadrilateral
        public Carousel(float itemWidth, float itemHeight, float rotationXAngleInDegrees = 45f)
        {
            float skew;

            if (rotationXAngleInDegrees <= 0f)
            {
                skew = 0f;
            }
            else if (rotationXAngleInDegrees >= 90f)
            {
                skew = 1f;
            }
            else
            {
                skew = rotationXAngleInDegrees / 90f;
            }

            w2 = itemWidth / 2f;
            h2 = itemHeight / 2f;
            h4 = itemHeight / 4f;
            h8 = itemHeight / 8f;

            x0 = new Vector2
            {
                X = -w2 + w2 * skew,
                Y = w2 - w2 * skew,
            };
            yl = new Vector4
            {
                X = -h2 - h4 * skew,
                Y = -h2 + h4 * skew,
                Z = h2 - h4 * skew,
                W = h2 + h4 * skew,
            };
            yr = new Vector4
            {
                X = -h2 + h4 * skew,
                Y = -h2 - h4 * skew,
                Z = h2 + h4 * skew,
                W = h2 - h4 * skew,
            };

            ql = new Quadrilateral
            {
                LeftTop = new Vector2(x0.X, yl.X),
                RightTop = new Vector2(x0.Y, yl.Y),
                RightBottom = new Vector2(x0.Y, yl.Z),
                LeftBottom = new Vector2(x0.X, yl.W),
            };
            qr = new Quadrilateral
            {
                LeftTop = new Vector2(x0.X, yr.X),
                RightTop = new Vector2(x0.Y, yr.Y),
                RightBottom = new Vector2(x0.Y, yr.Z),
                LeftBottom = new Vector2(x0.X, yr.W),
            };
        }

        public CarouselItem ToItem(SizeMatrix sourceNormalize, Vector2 center, float rotattionXAmount)
        {
            return new CarouselItem(this, sourceNormalize, center, rotattionXAmount);
        }

        public CarouselItem ToItem(SizeMatrix sourceNormalize, Vector2 center, int index, float offsetX, float itemMargin = 60f, float itemSpacing = 110f)
        {
            return new CarouselItem(this, sourceNormalize, center, index, offsetX, itemMargin, itemSpacing);
        }

        public Quadrilateral GetDockLeftTextureOutline(Vector2 center) => Quadrilateral.Translate(ql, center);
        internal Quadrilateral GetDockLeftTextureOutline(float centerX, float centerY) => Quadrilateral.Translate(ql, centerX, centerY);

        public Quadrilateral GetDockRightTextureOutline(Vector2 center) => Quadrilateral.Translate(qr, center);
        internal Quadrilateral GetDockRightTextureOutline(float centerX, float centerY) => Quadrilateral.Translate(qr, centerX, centerY);

        // -1.0 ~ +1.0
        //
        // -1.0: Min
        // -0.5: Left
        // +0.0: Center
        // +0.5: Right
        // +1.0: Max
        public Quadrilateral GetFloatTextureOutline(Vector2 center, float rotattionXAmount) => this.Lerp(center.X, center.Y, rotattionXAmount);
        internal Quadrilateral GetFloatTextureOutline(float centerX, float centerY, float rotattionXAmount) => this.Lerp(centerX, centerY, rotattionXAmount);

        private Quadrilateral Lerp(float centerX, float centerY, float amount)
        {
            float r = amount * Constants.PIOver2;
            float c = (float)System.Math.Cos(r);

            float v = 1f - c;
            float n = c * this.h2;

            float x1 = v * x0.X - n + centerX;
            float x2 = v * x0.Y + n + centerX;

            float y = h8 * amount;

            return new Quadrilateral
            {
                LeftTop = new Vector2(x1, -h2 + y + centerY),
                RightTop = new Vector2(x2, -h2 - y + centerY),
                RightBottom = new Vector2(x2, h2 + y + centerY),
                LeftBottom = new Vector2(x1, h2 - y + centerY),
            };
        }
    }
}