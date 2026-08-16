using FanKit.Transformer.Mathematics;
using System.Numerics;

namespace FanKit.Transformer.UI
{
    public struct Scroller
    {
        public ScrollerState State;
        public Quadrilateral Bounds;
        public Quadrilateral Float;

        public float ToOpacity(float width)
        {
            float diff0 = System.Math.Abs(this.Float.RightBottom.X - this.Bounds.LeftBottom.X);
            float diff1 = System.Math.Abs(this.Float.RightTop.X - this.Bounds.LeftTop.X);
            float opacity = 2f * System.Math.Max(diff0, diff1) / width;

            float inv = 1f - opacity;
            return 1f - inv * inv;
        }

        public Linear ToFloatLinear(float distance)
        {
            float centerX = (this.Float.RightTop.X + this.Float.RightBottom.X) / 2f;
            float centerY = (this.Float.RightTop.Y + this.Float.RightBottom.Y) / 2f;

            float x = this.Float.RightBottom.X - this.Float.RightTop.X;
            float y = this.Float.RightTop.Y - this.Float.RightBottom.Y;

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

        public Matrix3x2 GetFloatTransformMatrix(ScrollerBounds bounds, float bitmapWidth, float bitmapHeight, bool isFlipX)
        {
            float scaleX = bounds.WidthHalf / bitmapWidth;
            float scaleY = bounds.Height / bitmapHeight;

            float x = this.Float.LeftBottom.X - this.Float.LeftTop.X;
            float y = this.Float.LeftBottom.Y - this.Float.LeftTop.Y;

            float radians = (float)System.Math.Atan2(y, x) - Constants.PIOver2;

            switch (this.State)
            {
                case ScrollerState.BottomTriangle:
                    float ds = x * x + y * y;
                    float d = (float)System.Math.Sqrt(ds);

                    float px = this.Float.LeftBottom.X - bounds.Height * x / d;
                    float py = this.Float.LeftBottom.Y - bounds.Height * y / d;

                    if (isFlipX)
                    {
                        return Matrix3x2.CreateScale(-scaleX, scaleY)
                        * Matrix3x2.CreateTranslation(bounds.WidthHalf, 0)
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
                        * Matrix3x2.CreateTranslation(bounds.WidthHalf, 0)
                        * Matrix3x2.CreateRotation(radians)
                        * Matrix3x2.CreateTranslation(this.Float.LeftTop);
                    }
                    else
                    {
                        return Matrix3x2.CreateScale(scaleX, scaleY)
                        * Matrix3x2.CreateRotation(radians)
                        * Matrix3x2.CreateTranslation(this.Float.LeftTop);
                    }
            }
        }

        public Vector2[] ToLeftPoints()
        {
            switch (this.State)
            {
                case ScrollerState.TopTriangle:
                    return new Vector2[]
                    {
                        this.Bounds.LeftTop,
                        this.Float.RightTop,
                        this.Float.RightBottom,

                        this.Bounds.RightBottom,

                        this.Bounds.LeftBottom,
                    };
                case ScrollerState.BottomTriangle:
                    return new Vector2[]
                    {
                        this.Bounds.LeftTop,

                        this.Bounds.RightTop,

                        this.Float.RightTop,
                        this.Float.RightBottom,
                        this.Bounds.LeftBottom,
                    };
                default:
                    return new Vector2[]
                    {
                        this.Bounds.LeftTop,
                        this.Float.RightTop,
                        this.Float.RightBottom,
                        this.Bounds.LeftBottom,
                    };
            }
        }

        public Vector2[] ToRightPoints()
        {
            switch (this.State)
            {
                case ScrollerState.TopTriangle:
                    return new Vector2[]
                    {
                        this.Float.RightTop,
                        this.Bounds.RightTop,
                        this.Float.RightBottom,
                    };
                case ScrollerState.BottomTriangle:
                    return new Vector2[]
                    {
                        this.Float.RightTop,
                        this.Bounds.RightBottom,
                        this.Float.RightBottom,
                    };
                default:
                    return new Vector2[]
                    {
                        this.Float.RightTop,
                        this.Bounds.RightTop,
                        this.Bounds.RightBottom,
                        this.Float.RightBottom,
                    };
            }
        }

        public Vector2[] ToFloatPoints()
        {
            switch (this.State)
            {
                case ScrollerState.TopTriangle:
                    return new Vector2[]
                    {
                        this.Float.LeftTop,
                        this.Float.RightTop,
                        //this.Float.RightBottom,
                        this.Float.LeftBottom,
                    };
                case ScrollerState.BottomTriangle:
                    return new Vector2[]
                    {
                        this.Float.LeftTop,
                        //this.Float.RightTop,
                        this.Float.RightBottom,
                        this.Float.LeftBottom,
                    };
                default:
                    return new Vector2[]
                    {
                        this.Float.LeftTop,
                        this.Float.RightTop,
                        this.Float.RightBottom,
                        this.Float.LeftBottom,
                    };
            }
        }
    }
}