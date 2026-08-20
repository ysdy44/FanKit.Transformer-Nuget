using FanKit.Transformer.Mathematics;
using System.Numerics;

namespace FanKit.Transformer.UI
{
    public struct Scroller
    {
        internal const byte l = 0; // LeftOutside
        internal const byte r = 1; // RightOutside
        internal const byte q = 2; // Quadrilateral
        internal const byte t = 3; // TopTriangle
        internal const byte m = 4; // BottomTriangle

        internal byte s;
        internal ScrollerBounds b;
        internal Quadrilateral f;

        public ScrollerState State
        {
            get
            {
                switch (this.s)
                {
                    case l: return ScrollerState.DockLeft;
                    case r: return ScrollerState.DockRight;
                    default: return ScrollerState.Float;
                }
            }
        }
        public Quadrilateral DockBounds => this.b.Bounds;
        public Quadrilateral FloatBounds => this.f;

        public float GetFloatShadowOpacity()
        {
            float diff0 = System.Math.Abs(this.f.RightBottom.X - this.b.CenterX);
            float diff1 = System.Math.Abs(this.f.RightTop.X - this.b.CenterX);

            if (diff0 < float.Epsilon && diff1 < float.Epsilon)
                return 0f;

            float opacity = 2f * System.Math.Max(diff0, diff1) / this.b.Width;

            float inv = 1f - opacity;
            return 1f - inv * inv;
        }

        public Linear GetFloatLinearGradientBrushPoints(float distance)
        {
            float centerX = (this.f.RightTop.X + this.f.RightBottom.X) / 2f;
            float centerY = (this.f.RightTop.Y + this.f.RightBottom.Y) / 2f;

            float x = this.f.RightBottom.X - this.f.RightTop.X;
            float y = this.f.RightTop.Y - this.f.RightBottom.Y;

            // Normalize
            float square = x * x + y * y;
            float inv = distance / (float)System.Math.Sqrt(square);

            return new Linear
            {
                L0 = new Vector2
                {
                    X = centerX + y * inv,
                    Y = centerY + x * inv,
                },
                L1 = new Vector2
                {
                    X = centerX,
                    Y = centerY,
                },
            };
        }

        public Matrix3x2 GetFloatTextureTransformMatrix(float bitmapWidth, float bitmapHeight, bool isFlipX)
        {
            float scaleX = this.b.WidthHalf / bitmapWidth;
            float scaleY = this.b.Height / bitmapHeight;

            float x = this.f.LeftBottom.X - this.f.LeftTop.X;
            float y = this.f.LeftBottom.Y - this.f.LeftTop.Y;

            float radians = (float)System.Math.Atan2(y, x) - Constants.PIOver2;

            switch (this.s)
            {
                case m:
                    float ds = x * x + y * y;
                    float d = (float)System.Math.Sqrt(ds);

                    float px = this.f.LeftBottom.X - this.b.Height * x / d;
                    float py = this.f.LeftBottom.Y - this.b.Height * y / d;

                    if (isFlipX)
                    {
                        return Matrix3x2.CreateScale(-scaleX, scaleY)
                        * Matrix3x2.CreateTranslation(this.b.WidthHalf, 0)
                        * Matrix3x2.CreateRotation(radians)
                        * Matrix3x2.CreateTranslation(px, py);
                    }
                    else
                    {
                        return Matrix3x2.CreateScale(scaleX, scaleY)
                        * Matrix3x2.CreateRotation(radians)
                        * Matrix3x2.CreateTranslation(px, py);
                    }
                default:
                    if (isFlipX)
                    {
                        return Matrix3x2.CreateScale(-scaleX, scaleY)
                        * Matrix3x2.CreateTranslation(this.b.WidthHalf, 0)
                        * Matrix3x2.CreateRotation(radians)
                        * Matrix3x2.CreateTranslation(this.f.LeftTop);
                    }
                    else
                    {
                        return Matrix3x2.CreateScale(scaleX, scaleY)
                        * Matrix3x2.CreateRotation(radians)
                        * Matrix3x2.CreateTranslation(this.f.LeftTop);
                    }
            }
        }

        public Vector2[] GetLeftTextureOutlines()
        {
            switch (this.s)
            {
                case t:
                    return new Vector2[]
                    {
                        this.b.LeftTop,
                        this.f.RightTop,
                        this.f.RightBottom,

                        this.b.RightBottom,

                        this.b.LeftBottom,
                    };
                case m:
                    return new Vector2[]
                    {
                        this.b.LeftTop,

                        this.b.RightTop,

                        this.f.RightTop,
                        this.f.RightBottom,
                        this.b.LeftBottom,
                    };
                default:
                    return new Vector2[]
                    {
                        this.b.LeftTop,
                        this.f.RightTop,
                        this.f.RightBottom,
                        this.b.LeftBottom,
                    };
            }
        }

        public Vector2[] GetRightTextureOutlines()
        {
            switch (this.s)
            {
                case t:
                    return new Vector2[]
                    {
                        this.f.RightTop,
                        this.b.RightTop,
                        this.f.RightBottom,
                    };
                case m:
                    return new Vector2[]
                    {
                        this.f.RightTop,
                        this.b.RightBottom,
                        this.f.RightBottom,
                    };
                default:
                    return new Vector2[]
                    {
                        this.f.RightTop,
                        this.b.RightTop,
                        this.b.RightBottom,
                        this.f.RightBottom,
                    };
            }
        }

        public Vector2[] GetFloatTextureOutlines()
        {
            switch (this.s)
            {
                case t:
                    return new Vector2[]
                    {
                        this.f.LeftTop,
                        this.f.RightTop,
                        //this.f.RightBottom,
                        this.f.LeftBottom,
                    };
                case m:
                    return new Vector2[]
                    {
                        this.f.LeftTop,
                        //this.f.RightTop,
                        this.f.RightBottom,
                        this.f.LeftBottom,
                    };
                default:
                    return new Vector2[]
                    {
                        this.f.LeftTop,
                        this.f.RightTop,
                        this.f.RightBottom,
                        this.f.LeftBottom,
                    };
            }
        }
    }
}