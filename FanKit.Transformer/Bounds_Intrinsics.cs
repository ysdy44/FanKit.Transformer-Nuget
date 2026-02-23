using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace FanKit.Transformer
{
    partial struct Bounds
    {
        public float Left;
        public float Top;
        public float Right;
        public float Bottom;

        #region Constructors
        public Bounds(Vector2 point)
        {
            this.Left = point.X;
            this.Top = point.Y;
            this.Right = point.X;
            this.Bottom = point.Y;
        }

        public Bounds(float uniformLength)
        {
            this.Left = uniformLength;
            this.Top = uniformLength;
            this.Right = uniformLength;
            this.Bottom = uniformLength;
        }

        public Bounds(float left, float top, float right, float bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }

        public Bounds(Vector2 point1, Vector2 point2, bool keepRatio, bool centeredScaling)
        {
            if (keepRatio)
            {
                const float root2over2 = 0.70710678f; // 1.41421356 / 2;

                float distance = Vector2.Distance(point1, point2) * root2over2;

                if (centeredScaling)
                {
                    #region KeepRatio & CenteredScaling
                    if (point2.X > point1.X)
                    {
                        this.Left = point1.X - distance;
                        this.Right = point1.X + distance;
                    }
                    else
                    {
                        this.Left = point1.X + distance;
                        this.Right = point1.X - distance;
                    }

                    if (point2.Y > point1.Y)
                    {
                        this.Top = point1.Y - distance;
                        this.Bottom = point1.Y + distance;
                    }
                    else
                    {
                        this.Top = point1.Y + distance;
                        this.Bottom = point1.Y - distance;
                    }
                    #endregion
                }
                else
                {
                    #region KeepRatio
                    this.Left = point1.X;
                    this.Top = point1.Y;

                    if (point2.X > point1.X)
                    {
                        //this.Left = point1.X - distance;
                        this.Right = point1.X + distance;
                    }
                    else
                    {
                        //this.Left = point1.X + distance;
                        this.Right = point1.X - distance;
                    }

                    if (point2.Y > point1.Y)
                    {
                        //this.Top = point1.Y - distance;
                        this.Bottom = point1.Y + distance;
                    }
                    else
                    {
                        //this.Top = point1.Y + distance;
                        this.Bottom = point1.Y - distance;
                    }
                    #endregion
                }
            }
            else
            {
                if (centeredScaling)
                {
                    #region CenteredScaling
                    this.Left = point1.X + point1.X - point2.X;
                    this.Top = point1.Y + point1.Y - point2.Y;
                    #endregion
                }
                else
                {
                    #region None
                    this.Left = point1.X;
                    this.Top = point1.Y;
                    #endregion
                }

                this.Right = point2.X;
                this.Bottom = point2.Y;
            }
        }

        public Bounds(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            this.Left = Math.Min(p0.X, p3.X);
            this.Right = Math.Max(p0.X, p3.X);
            this.Top = Math.Min(p0.Y, p3.Y);
            this.Bottom = Math.Max(p0.Y, p3.Y);

            List<float> tValuesX = Constants.CubicExtrema(p0.X, p1.X, p2.X, p3.X);
            List<float> tValuesY = Constants.CubicExtrema(p0.Y, p1.Y, p2.Y, p3.Y);

            HashSet<float> tValues = new HashSet<float>();
            foreach (float t in tValuesX)
            {
                if (t >= 0 && t <= 1)
                    tValues.Add(t);
            }
            foreach (float t in tValuesY)
            {
                if (t >= 0 && t <= 1)
                    tValues.Add(t);
            }

            tValues.Add(0f);
            tValues.Add(1f);

            foreach (float t in tValues)
            {
                Vector2 point = Constants.CubicPoint(p0, p1, p2, p3, t, 1f - t);
                this.Left = Math.Min(this.Left, point.X);
                this.Right = Math.Max(this.Right, point.X);
                this.Top = Math.Min(this.Top, point.Y);
                this.Bottom = Math.Max(this.Bottom, point.Y);
            }
        }

        public Bounds(Vector2 p0, Vector2 p1, Vector2 p2)
        {
            this.Left = Math.Min(p0.X, p2.X);
            this.Right = Math.Max(p0.X, p2.X);
            this.Top = Math.Min(p0.Y, p2.Y);
            this.Bottom = Math.Max(p0.Y, p2.Y);

            List<float> tValuesX = Constants.QuadraticExtrema(p0.X, p1.X, p2.X);
            List<float> tValuesY = Constants.QuadraticExtrema(p0.Y, p1.Y, p2.Y);

            HashSet<float> tValues = new HashSet<float>();
            foreach (float t in tValuesX)
            {
                if (t >= 0 && t <= 1)
                    tValues.Add(t);
            }
            foreach (float t in tValuesY)
            {
                if (t >= 0 && t <= 1)
                    tValues.Add(t);
            }

            tValues.Add(0f);
            tValues.Add(1f);

            foreach (float t in tValues)
            {
                Vector2 point = Constants.QuadraticPoint(p0, p1, p2, t, 1f - t);
                this.Left = Math.Min(this.Left, point.X);
                this.Right = Math.Max(this.Right, point.X);
                this.Top = Math.Min(this.Top, point.Y);
                this.Bottom = Math.Max(this.Bottom, point.Y);
            }
        }

        public Bounds(Vector2 point1, Vector2 point2)
        {
            this.Left = System.Math.Min(point1.X, point2.X);
            this.Top = System.Math.Min(point1.Y, point2.Y);

            this.Right = System.Math.Max(point1.X, point2.X);
            this.Bottom = System.Math.Max(point1.Y, point2.Y);
        }

        public Bounds(float width, float height)
        {
            this.Left = 0f;
            this.Top = 0f;
            this.Right = width;
            this.Bottom = height;
        }

        public Bounds(float width, float height, Matrix2x2 matrix)
        {
            this.Left = matrix.TranslateX;
            this.Top = matrix.TranslateY;

            this.Right = matrix.TranslateX + matrix.ScaleX * width;
            this.Bottom = matrix.TranslateY + matrix.ScaleY * height;
        }

        public Bounds(float width, float height, Vector2 postion)
        {
            this.Left = postion.X;
            this.Top = postion.Y;
            this.Right = postion.X + width;
            this.Bottom = postion.Y + height;
        }

        public Bounds(float width, float height, Vector2 postion, Matrix2x2 matrix)
        {
            this.Left = matrix.TranslateX + matrix.ScaleX * postion.X;
            this.Top = matrix.TranslateY + matrix.ScaleY * postion.Y;

            this.Right = this.Left + matrix.ScaleX * width;
            this.Bottom = this.Top + matrix.ScaleY * height;
        }

        public Bounds(Rectangle rect)
        {
            this.Left = rect.X;
            this.Top = rect.Y;
            this.Right = rect.X + rect.Width;
            this.Bottom = rect.Y + rect.Height;
        }

        public Bounds(Rectangle rect, Matrix2x2 matrix)
        {
            this.Left = matrix.TranslateX + matrix.ScaleX * rect.X;
            this.Top = matrix.TranslateY + matrix.ScaleY * rect.Y;

            this.Right = this.Left + matrix.ScaleX * rect.Width;
            this.Bottom = this.Top + matrix.ScaleY * rect.Height;
        }

        public Bounds(Bounds bounds)
        {
            this.Left = bounds.Left;
            this.Top = bounds.Top;

            this.Right = bounds.Right;
            this.Bottom = bounds.Bottom;
        }

        public Bounds(Bounds bounds, Matrix2x2 matrix)
        {
            this.Left = matrix.TranslateX + matrix.ScaleX * bounds.Left;
            this.Top = matrix.TranslateY + matrix.ScaleY * bounds.Top;

            this.Right = matrix.TranslateX + matrix.ScaleX * bounds.Right;
            this.Bottom = matrix.TranslateY + matrix.ScaleY * bounds.Bottom;
        }

        public Bounds(Triangle triangle)
        {
            this.Left = float.MaxValue;
            this.Top = float.MaxValue;
            this.Right = float.MinValue;
            this.Bottom = float.MinValue;

            this.Extend(triangle.LeftTop);
            this.Extend(triangle.RightTop);
            this.Extend(triangle.LeftBottom);

            // get bounding rect
            //this.Extend(triangle.RightTop + triangle.LeftBottom - triangle.LeftTop); // Triangle -> Transformer
        }

        public Bounds(Quadrilateral quad)
        {
            this.Left = float.MaxValue;
            this.Top = float.MaxValue;
            this.Right = float.MinValue;
            this.Bottom = float.MinValue;

            this.Extend(quad.LeftTop);
            this.Extend(quad.RightTop);
            this.Extend(quad.LeftBottom);

            this.Extend(quad.RightBottom);
        }

        public Bounds(IEnumerable<Vector2> items)
        {
            this.Left = float.MaxValue;
            this.Top = float.MaxValue;
            this.Right = float.MinValue;
            this.Bottom = float.MinValue;

            foreach (Vector2 item in items)
            {
                this.Extend(item);
            }
        }

        public Bounds(IEnumerable<IEnumerable<Vector2>> itemss)
        {
            this.Left = float.MaxValue;
            this.Top = float.MaxValue;
            this.Right = float.MinValue;
            this.Bottom = float.MinValue;

            foreach (IEnumerable<Vector2> items in itemss)
            {
                foreach (Vector2 item in items)
                {
                    this.Extend(item);
                }
            }
        }

        public Bounds(IEnumerable<Bounds> items)
        {
            this.Left = float.MaxValue;
            this.Top = float.MaxValue;
            this.Right = float.MinValue;
            this.Bottom = float.MinValue;

            foreach (Bounds item in items)
            {
                if (this.Left > item.Left) this.Left = item.Left;
                if (this.Top > item.Top) this.Top = item.Top;
                if (this.Right < item.Right) this.Right = item.Right;
                if (this.Bottom < item.Bottom) this.Bottom = item.Bottom;
            }
        }

        public Bounds(IEnumerable<IEnumerable<Bounds>> itemss)
        {
            this.Left = float.MaxValue;
            this.Top = float.MaxValue;
            this.Right = float.MinValue;
            this.Bottom = float.MinValue;

            foreach (IEnumerable<Bounds> items in itemss)
            {
                foreach (Bounds item in items)
                {
                    if (this.Left > item.Left) this.Left = item.Left;
                    if (this.Top > item.Top) this.Top = item.Top;
                    if (this.Right < item.Right) this.Right = item.Right;
                    if (this.Bottom < item.Bottom) this.Bottom = item.Bottom;
                }
            }
        }

        public Bounds(IEnumerable<Triangle> items)
        {
            this.Left = float.MaxValue;
            this.Top = float.MaxValue;
            this.Right = float.MinValue;
            this.Bottom = float.MinValue;

            foreach (Triangle item in items)
            {
                this.Extend(item.LeftTop);
                this.Extend(item.RightTop);
                this.Extend(item.LeftBottom);

                this.Extend(item.RightTop + item.LeftBottom - item.LeftTop); // Triangle -> Transformer
            }
        }

        public Bounds(IEnumerable<Quadrilateral> items)
        {
            this.Left = float.MaxValue;
            this.Top = float.MaxValue;
            this.Right = float.MinValue;
            this.Bottom = float.MinValue;

            foreach (Quadrilateral item in items)
            {
                this.Extend(item.LeftTop);
                this.Extend(item.RightTop);
                this.Extend(item.LeftBottom);

                this.Extend(item.RightBottom);
            }
        }

        private void Extend(Vector2 point)
        {
            if (this.Left > point.X) this.Left = point.X;
            if (this.Top > point.Y) this.Top = point.Y;
            if (this.Right < point.X) this.Right = point.X;
            if (this.Bottom < point.Y) this.Bottom = point.Y;
        }
        #endregion Constructors

        #region Public Instance Methods
        public bool Equals(Bounds other)
        {
            return
                this.Left == other.Left &&
                this.Top == other.Top &&
                this.Right == other.Right &&
                this.Bottom == other.Bottom;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Bounds MovePoint(QuadrilateralPointKind kind, Vector2 point)
        {
            switch (kind)
            {
                case QuadrilateralPointKind.LeftTop:
                    return new Bounds
                    {
                        Left = point.X,
                        Top = point.Y,

                        Right = this.Right,
                        Bottom = this.Bottom,
                    };
                case QuadrilateralPointKind.RightTop:
                    return new Bounds
                    {
                        Left = this.Left,
                        Top = point.Y,

                        Right = point.X,
                        Bottom = this.Bottom,
                    };
                case QuadrilateralPointKind.LeftBottom:
                    return new Bounds
                    {
                        Left = point.X,
                        Top = this.Top,

                        Right = this.Right,
                        Bottom = point.Y,
                    };
                case QuadrilateralPointKind.RightBottom:
                    return new Bounds
                    {
                        Left = this.Left,
                        Top = this.Top,

                        Right = point.X,
                        Bottom = point.Y,
                    };
                default:
                    return this;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Bounds MoveChannel(BoundsChannelKind kind, float channel)
        {
            switch (kind)
            {
                case BoundsChannelKind.Left:
                    return new Bounds
                    {
                        Left = channel,
                        Top = this.Top,

                        Right = this.Right,
                        Bottom = this.Bottom,
                    };
                case BoundsChannelKind.Top:
                    return new Bounds
                    {
                        Left = this.Left,
                        Top = channel,

                        Right = this.Right,
                        Bottom = this.Bottom,
                    };
                case BoundsChannelKind.Right:
                    return new Bounds
                    {
                        Left = this.Left,
                        Top = this.Top,

                        Right = channel,
                        Bottom = this.Bottom,
                    };
                case BoundsChannelKind.Bottom:
                    return new Bounds
                    {
                        Left = this.Left,
                        Top = this.Top,

                        Right = this.Right,
                        Bottom = channel,
                    };
                default:
                    return this;
            }
        }
        #endregion Public Instance Methods

        #region Public Static Operators
        public static Bounds operator +(Bounds left, Vector2 right)
        {
            return new Bounds
            {
                Left = left.Left + right.X,
                Top = left.Top + right.Y,
                Right = left.Right + right.X,
                Bottom = left.Bottom + right.Y,
            };
        }

        public static Bounds operator +(Vector2 left, Bounds right)
        {
            return new Bounds
            {
                Left = left.X + right.Left,
                Top = left.Y + right.Top,
                Right = left.X + right.Right,
                Bottom = left.Y + right.Bottom,
            };
        }

        public static Bounds operator +(Bounds left, Bounds right)
        {
            return new Bounds
            {
                Left = left.Left + right.Left,
                Top = left.Top + right.Top,
                Right = left.Right + right.Right,
                Bottom = left.Bottom + right.Bottom,
            };
        }

        public static Bounds operator -(Bounds left, Vector2 right)
        {
            return new Bounds
            {
                Left = left.Left - right.X,
                Top = left.Top - right.Y,
                Right = left.Right - right.X,
                Bottom = left.Bottom - right.Y,
            };
        }

        public static Bounds operator -(Vector2 left, Bounds right)
        {
            return new Bounds
            {
                Left = left.X - right.Left,
                Top = left.Y - right.Top,
                Right = left.X - right.Right,
                Bottom = left.Y - right.Bottom,
            };
        }

        public static Bounds operator -(Bounds left, Bounds right)
        {
            return new Bounds
            {
                Left = left.Left - right.Left,
                Top = left.Top - right.Top,
                Right = left.Right - right.Right,
                Bottom = left.Bottom - right.Bottom,
            };
        }

        public static TransformedBounds operator *(Bounds left, Matrix3x2 right)
        {
            return new TransformedBounds(left, right);
        }

        public static FreeTransformedBounds operator *(Bounds left, Matrix4x4 right)
        {
            return new FreeTransformedBounds(left, right);
        }

        public static Bounds operator *(Bounds left, Vector2 right)
        {
            return new Bounds
            {
                Left = left.Left * right.X,
                Top = left.Top * right.Y,
                Right = left.Right * right.X,
                Bottom = left.Bottom * right.Y,
            };
        }

        public static Bounds operator *(Bounds left, float right)
        {
            return new Bounds
            {
                Left = left.Left * right,
                Top = left.Top * right,
                Right = left.Right * right,
                Bottom = left.Bottom * right,
            };
        }

        public static Bounds operator *(float left, Bounds right)
        {
            return new Bounds
            {
                Left = left * right.Left,
                Top = left * right.Top,
                Right = left * right.Right,
                Bottom = left * right.Bottom,
            };
        }

        public static Bounds operator /(Bounds left, Vector2 right)
        {
            return new Bounds
            {
                Left = left.Left / right.X,
                Top = left.Top / right.Y,
                Right = left.Right / right.X,
                Bottom = left.Bottom / right.Y,
            };
        }

        public static Bounds operator /(Bounds left, float right)
        {
            return new Bounds
            {
                Left = left.Left / right,
                Top = left.Top / right,
                Right = left.Right / right,
                Bottom = left.Bottom / right,
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds operator -(Bounds value)
        {
            return new Bounds
            {
                Left = -value.Left,
                Top = -value.Top,
                Right = -value.Right,
                Bottom = -value.Bottom,
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Bounds left, Bounds right)
        {
            return left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Bounds left, Bounds right)
        {
            return !(left == right);
        }
        #endregion Public Static Operators
    }
}