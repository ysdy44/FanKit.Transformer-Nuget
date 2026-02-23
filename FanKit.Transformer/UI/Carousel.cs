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

        public Quadrilateral LeftBox(Vector2 center) => Quadrilateral.Translate(ql, center);
        public Quadrilateral LeftBox(float centerX, float centerY) => Quadrilateral.Translate(ql, centerX, centerY);

        public Quadrilateral RightBox(Vector2 center) => Quadrilateral.Translate(qr, center);
        public Quadrilateral RightBox(float centerX, float centerY) => Quadrilateral.Translate(qr, centerX, centerY);

        // -1.0 ~ +1.0
        //
        // -1.0: Min
        // -0.5: Left
        // +0.0: Center
        // +0.5: Right
        // +1.0: Max
        public Quadrilateral LerpBox(Vector2 center, float amout) => LB(center.X, center.Y, amout);
        public Quadrilateral LerpBox(float centerX, float centerY, float amout) => LB(centerX, centerY, amout);

        public Quadrilateral LB(float centerX, float centerY, float amout)
        {
            float r = amout * (float)System.Math.PI;
            float c = (float)System.Math.Cos(r);

            float v = 1f - c;
            float n = c * this.h2;

            Vector4 y = new Vector4
            {
                X = -h2 + h4 * amout,
                Y = -h2 - h4 * amout,
                Z = h2 + h4 * amout,
                W = h2 - h4 * amout,
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