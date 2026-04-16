using FanKit.Transformer.Mathematics;
using System.Numerics;

namespace FanKit.Transformer.UI
{
    public readonly struct ScrollerBounds
    {
        public readonly float Left;
        public readonly float Top;

        public readonly float Right;
        public readonly float Bottom;

        public readonly float Width;
        public readonly float Height;

        public readonly float WidthHalf;
        public readonly float CenterX;

        public ScrollerBounds(float x, float y, float width, float height)
        {
            this.Left = x;
            this.Top = y;

            this.Right = x + width;
            this.Bottom = y + height;

            this.Width = width;
            this.Height = height;

            this.WidthHalf = width / 2;
            this.CenterX = x + this.WidthHalf;
        }

        public ScrollerDirection GetDirection(float x) => BeginDirection(x);
        internal ScrollerDirection EndDirection(float x) => x > this.CenterX ? ScrollerDirection.PageUp : ScrollerDirection.None;
        internal ScrollerDirection BeginDirection(float x) => x > this.CenterX ? ScrollerDirection.PageDown : ScrollerDirection.None;

        public Linear ToLeftLinear(float distance)
        {
            return new Linear
            {
                L0 = new Vector2(this.CenterX - distance, this.Top),
                L1 = new Vector2(this.CenterX, this.Top),
            };
        }
        public Linear ToRightLinear(float distance)
        {
            return new Linear
            {
                L0 = new Vector2(this.CenterX, this.Top),
                L1 = new Vector2(this.CenterX + distance, this.Top),
            };
        }

        public Vector2 GetLeftScales(float width, float height)
        {
            float scaleX = this.WidthHalf / width;
            float scaleY = this.Height / height;
           
            return new Vector2
            {
                X = scaleX,
                Y = scaleY,
            };
        }

        public Vector2 GetRightScales(float width, float height)
        {
            float scaleX = this.WidthHalf / width;
            float scaleY = this.Height / height;

            return new Vector2
            {
                X = scaleX,
                Y = scaleY,
            };
        }

        public Matrix3x2 GetFloatTransformMatrix(Scroller quad, float width, float height)
        {
            float scaleX = this.WidthHalf / width;
            float scaleY = this.Height / height;

            float x = quad.Float.LeftBottom.X - quad.Float.LeftTop.X;
            float y = quad.Float.LeftBottom.Y - quad.Float.LeftTop.Y;

            float radians = (float)(System.Math.Atan2(y, x) - System.Math.PI / 2);

            switch (quad.State)
            {
                case ScrollerState.BottomTriangle:
                    return Matrix3x2.CreateTranslation(0f, -height)
                         * Matrix3x2.CreateScale(scaleX, scaleY)
                         * Matrix3x2.CreateRotation(radians)
                         * Matrix3x2.CreateTranslation(quad.Float.LeftBottom);
                default:
                    return Matrix3x2.CreateScale(scaleX, scaleY)
                        * Matrix3x2.CreateRotation(radians)
                        * Matrix3x2.CreateTranslation(quad.Float.LeftTop);
            }
        }

        private static float InterX(float px, float py, float invk, float x) => py + invk * (px - x);
        private static float InterY(float px, float py, float k, float y) => px + k * (py - y);
        private static Vector2 MirrorPoint(Vector2 point, Vector2 linePoint1, Vector2 linePoint2)
        {
            FootPoint footPoint = new FootPoint(point, linePoint1, linePoint2);
            return footPoint.Foot + footPoint.Foot - point;
        }

        public Scroller Scroll(float startingX)
        {
            if (startingX >= this.Right)
            {
                return new Scroller
                {
                    State = ScrollerState.RightOutside
                };
            }
            else if (this.Right + startingX <= this.Left + this.Right)
            {
                return new Scroller
                {
                    State = ScrollerState.LeftOutside,
                };
            }
            else
            {
                float x = (this.Right + startingX) / 2f;
                Vector2 top = new Vector2(x, this.Top);
                Vector2 bottom = new Vector2(x, this.Bottom);

                Vector2 rightTop = new Vector2(this.Right, this.Top);
                Vector2 rightBottom = new Vector2(this.Right, this.Bottom);
                return new Scroller
                {
                    State = ScrollerState.Quadrilateral,
                    Bounds = new Quadrilateral
                    {
                        LeftTop = new Vector2(this.CenterX, this.Top),
                        RightTop = new Vector2(this.Right, this.Top),
                        LeftBottom = new Vector2(this.CenterX, this.Bottom),
                        RightBottom = new Vector2(this.Right, this.Bottom),
                    },
                    Float = new Quadrilateral
                    {
                        LeftTop = MirrorPoint(rightTop, top, bottom),
                        RightTop = top,
                        RightBottom = bottom,
                        LeftBottom = MirrorPoint(rightBottom, top, bottom)
                    }
                };
            }
        }
        public Scroller Scroll(float startingY, Vector2 point)
        {
            if (startingY == point.Y)
                return Scroll(point.X);

            float startingX = this.Right;

            float centerX = (startingX + point.X) / 2;
            float centerY = (startingY + point.Y) / 2;

            float vectorX = startingX - point.X;
            float vectorY = startingY - point.Y;

            // k = vectorX / vectorY
            // f(x) = k * (x - centerX) + centerY;
            float k = vectorY / vectorX;
            float topX = InterY(centerX, centerY, k, this.Top);
            float bottomX = InterY(centerX, centerY, k, this.Bottom);

            bool isBottomTriangle = topX >= this.Right;
            bool IsTopTriangle = bottomX >= this.Right;

            if (isBottomTriangle)
            {
                if (IsTopTriangle)
                {
                    return new Scroller
                    {
                        State = ScrollerState.RightOutside
                    };
                }
                else
                {
                    float invk = vectorX / vectorY;
                    float rightY = InterX(centerX, centerY, invk, this.Right);

                    Vector2 right = new Vector2
                    {
                        X = this.Right,
                        Y = rightY
                    };
                    Vector2 bottom = new Vector2
                    {
                        X = System.Math.Max(this.CenterX, bottomX),
                        Y = this.Bottom
                    };

                    Vector2 rightBottom = new Vector2(this.Right, this.Bottom);
                    return new Scroller
                    {
                        State = ScrollerState.BottomTriangle,
                        Bounds = new Quadrilateral
                        {
                            LeftTop = new Vector2(this.CenterX, this.Top),
                            RightTop = new Vector2(this.Right, this.Top),
                            LeftBottom = new Vector2(this.CenterX, this.Bottom),
                            RightBottom = new Vector2(this.Right, this.Bottom)
                        },
                        Float = new Quadrilateral
                        {
                            LeftTop = right,
                            RightTop = right,
                            RightBottom = bottom,
                            LeftBottom = MirrorPoint(rightBottom, right, bottom)
                        },
                    };
                }
            }
            else
            {
                if (IsTopTriangle)
                {
                    float invk = vectorX / vectorY;
                    float rightY = InterX(centerX, centerY, invk, this.Right);

                    Vector2 top = new Vector2
                    {
                        X = System.Math.Max(this.CenterX, topX),
                        Y = this.Top
                    };
                    Vector2 right = new Vector2
                    {
                        X = this.Right,
                        Y = rightY
                    };

                    Vector2 rightTop = new Vector2(this.Right, this.Top);
                    return new Scroller
                    {
                        State = ScrollerState.TopTriangle,
                        Bounds = new Quadrilateral
                        {
                            LeftTop = new Vector2(this.CenterX, this.Top),
                            RightTop = new Vector2(this.Right, this.Top),
                            LeftBottom = new Vector2(this.CenterX, this.Bottom),
                            RightBottom = new Vector2(this.Right, this.Bottom)
                        },
                        Float = new Quadrilateral
                        {
                            LeftTop = MirrorPoint(rightTop, right, top),
                            RightTop = top,
                            RightBottom = right,
                            LeftBottom = right
                        }
                    };
                }
                else
                {
                    bool IsTop = topX <= this.CenterX;
                    bool isBottom = bottomX <= this.CenterX;

                    if (IsTop)
                    {
                        if (isBottom)
                        {
                            return new Scroller
                            {
                                State = ScrollerState.LeftOutside,
                            };
                        }
                    }

                    Vector2 top = new Vector2(IsTop ? this.CenterX : topX, this.Top);
                    Vector2 bottom = new Vector2(isBottom ? this.CenterX : bottomX, this.Bottom);

                    Vector2 rightTop = new Vector2(this.Right, this.Top);
                    Vector2 rightBottom = new Vector2(this.Right, this.Bottom);
                    return new Scroller
                    {
                        State = ScrollerState.Quadrilateral,
                        Bounds = new Quadrilateral
                        {
                            LeftTop = new Vector2(this.CenterX, this.Top),
                            RightTop = new Vector2(this.Right, this.Top),
                            LeftBottom = new Vector2(this.CenterX, this.Bottom),
                            RightBottom = new Vector2(this.Right, this.Bottom)
                        },
                        Float = new Quadrilateral
                        {
                            LeftTop = MirrorPoint(rightTop, top, bottom),
                            RightTop = top,
                            RightBottom = bottom,
                            LeftBottom = MirrorPoint(rightBottom, top, bottom)
                        }
                    };
                }
            }
        }
    }
}