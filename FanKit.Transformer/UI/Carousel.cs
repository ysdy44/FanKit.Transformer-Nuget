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

        readonly Vector2 x0;
        readonly Vector4 yl;
        readonly Vector4 yr;

        readonly Quadrilateral ql;
        readonly Quadrilateral qr;

        // 0.0 ~ 1.0
        //
        // 0.0: Rectangle
        // 1.0: Quadrilateral
        public Carousel(float destinationWidth, float destinationHeight, float skew = 0.5f)
        {
            w2 = destinationWidth / 2;
            h2 = destinationHeight / 2;
            h4 = destinationHeight / 4;

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

        public CarouselItem1 ToItem1(SizeMatrix sourceNormalize, float centerX, float centerY, float amount)
        {
            return new CarouselItem1(this, sourceNormalize, centerX, centerY, amount);
        }

        public CarouselItem2 ToItem2(SizeMatrix sourceNormalize, int index, float centerX, float centerY, float offsetX, float itemMargin = 60f, float itemSpacing = 110f)
        {
            return new CarouselItem2(this, sourceNormalize, index, centerX, centerY, offsetX, itemMargin, itemSpacing);
        }

        public Quadrilateral GetDockLeftTextureOutline(Vector2 center) => Quadrilateral.Translate(ql, center);
        public Quadrilateral GetDockLeftTextureOutline(float centerX, float centerY) => Quadrilateral.Translate(ql, centerX, centerY);

        public Quadrilateral GetDockRightTextureOutline(Vector2 center) => Quadrilateral.Translate(qr, center);
        public Quadrilateral GetDockRightTextureOutline(float centerX, float centerY) => Quadrilateral.Translate(qr, centerX, centerY);

        // -1.0 ~ +1.0
        //
        // -1.0: Min
        // -0.5: Left
        // +0.0: Center
        // +0.5: Right
        // +1.0: Max
        public Quadrilateral GetFloatTextureOutline(Vector2 center, float amount) => this.Lerp(center.X, center.Y, amount);
        public Quadrilateral GetFloatTextureOutline(float centerX, float centerY, float amount) => this.Lerp(centerX, centerY, amount);

        private Quadrilateral Lerp(float centerX, float centerY, float amount)
        {
            float r = amount * Constants.PI;
            float c = (float)System.Math.Cos(r);

            float v = 1f - c;
            float n = c * this.h2;

            Vector4 y = new Vector4
            {
                X = -h2 + h4 * amount,
                Y = -h2 - h4 * amount,
                Z = h2 + h4 * amount,
                W = h2 - h4 * amount,
            };

            return new Quadrilateral
            {
                LeftTop = new Vector2(v * x0.X - n + centerX, y.X + centerY),
                RightTop = new Vector2(v * x0.Y + n + centerX, y.Y + centerY),
                RightBottom = new Vector2(v * x0.Y + n + centerX, y.Z + centerY),
                LeftBottom = new Vector2(v * x0.X - n + centerX, y.W + centerY),
            };
        }
    }
}