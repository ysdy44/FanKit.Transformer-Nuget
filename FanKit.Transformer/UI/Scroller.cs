using FanKit.Transformer.Mathematics;
using System.Numerics;

namespace FanKit.Transformer.UI
{
    public struct Scroller
    {
        public ScrollerState State;
        public Quadrilateral Bounds;
        public Quadrilateral Float;

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

        public float ToOpacity(float width)
        {
            float diff0 = System.Math.Abs(this.Float.RightBottom.X - this.Bounds.LeftBottom.X);
            float diff1 = System.Math.Abs(this.Float.RightTop.X - this.Bounds.LeftTop.X);
            float opacity = 2f * System.Math.Max(diff0, diff1) / width;

            float inv = 1f - opacity;
            return 1f - inv * inv;
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