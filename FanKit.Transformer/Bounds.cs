using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace FanKit.Transformer
{
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct Bounds : IEquatable<Bounds>
    {
        #region Public Instance Properties
        public bool IsEmpty
        {
            get
            {
                return this.Left == 0f &
                  this.Top == 0f &
                  this.Right == 0f &
                  this.Bottom == 0f;
            }
        }

        public bool IsInfinity
        {
            get
            {
                return this.Left == float.MaxValue &
                  this.Top == float.MaxValue &
                  this.Right == float.MinValue &
                  this.Bottom == float.MinValue;
            }
        }

        public bool IsWidthZero
        {
            get
            {
                return this.Left == this.Right;
            }
        }

        public bool IsHeightZero
        {
            get
            {
                return this.Top == this.Bottom;
            }
        }
        #endregion Public Instance Properties

        #region Public Static Properties
        static readonly Bounds empty = new Bounds
        {
            Left = 0f,
            Top = 0f,
            Right = 0f,
            Bottom = 0f,
        };

        static readonly Bounds infinity = new Bounds
        {
            Left = float.MaxValue,
            Top = float.MaxValue,
            Right = float.MinValue,
            Bottom = float.MinValue,
        };

        public static Bounds Empty
        {
            get { return empty; }
        }

        public static Bounds Infinity
        {
            get { return infinity; }
        }
        #endregion Public Static Properties

        #region Public instance methods
        public override int GetHashCode()
        {
            int hashCode = -1819631549;
            hashCode = hashCode * -1521134295 + this.Left.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Top.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Right.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Bottom.GetHashCode();
            return hashCode;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Bounds))
                return false;
            return Equals((Bounds)obj);
        }

        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return String.Format(formatProvider, "{{Left:{0} Top:{1} Right:{2} Bottom:{3}}}",
                this.Left.ToString(format, formatProvider),
                this.Top.ToString(format, formatProvider),
                this.Right.ToString(format, formatProvider),
                this.Bottom.ToString(format, formatProvider));
        }

        public Vector2[] To3Points() => new Vector2[]
        {
            new Vector2(this.Left, this.Top),
            new Vector2(this.Right, this.Top),
            new Vector2(this.Left, this.Bottom),
        };
        public Vector2[] To4Points() => new Vector2[]
        {
            new Vector2(this.Left, this.Top),
            new Vector2(this.Right, this.Top),
            new Vector2(this.Right, this.Bottom),
            new Vector2(this.Left, this.Bottom),
        };
        public Bounds ToBounds() => new Bounds
        {
            Left = System.Math.Min(this.Right, this.Left),
            Top = System.Math.Min(this.Bottom, this.Top),
            Right = System.Math.Max(this.Right, this.Left),
            Bottom = System.Math.Max(this.Bottom, this.Top),
        };
        public Rectangle ToRectangle() => new Rectangle
        {
            X = System.Math.Min(this.Right, this.Left),
            Y = System.Math.Min(this.Bottom, this.Top),
            Width = System.Math.Abs(this.Right - this.Left),
            Height = System.Math.Abs(this.Bottom - this.Top),
        };
        public Matrix2x2 Normalize() => new Matrix2x2
        {
            TranslateX = this.Left,
            TranslateY = this.Top,
            ScaleX = this.Right - this.Left,
            ScaleY = this.Bottom - this.Top,
        };

        public float CenterX() => (this.Left + this.Right) / 2f;
        public float CenterY() => (this.Top + this.Bottom) / 2f;
        public Vector2 Center() => new Vector2((this.Left + this.Right) / 2f, (this.Top + this.Bottom) / 2f);

        public Vector2 CenterLeft() => new Vector2(this.Left, (this.Top + this.Bottom) / 2f);
        public Vector2 CenterTop() => new Vector2((this.Left + this.Right) / 2f, this.Top);
        public Vector2 CenterRight() => new Vector2(this.Right, (this.Top + this.Bottom) / 2f);
        public Vector2 CenterBottom() => new Vector2((this.Left + this.Right) / 2f, this.Bottom);

        public float MinX() => System.Math.Min(this.Left, this.Right);
        public float MaxX() => System.Math.Max(this.Left, this.Right);
        public float MinY() => System.Math.Min(this.Top, this.Bottom);
        public float MaxY() => System.Math.Max(this.Top, this.Bottom);

        public float Horizontal() => this.Right - this.Left;
        public float Vertical() => this.Bottom - this.Top;

        public Bounds Expand(float expander) => new Bounds
        {
            Left = this.Left - expander,
            Top = this.Top - expander,

            Right = this.Right + expander,
            Bottom = this.Bottom + expander,
        };
        public Bounds Shrink(float shrinker) => new Bounds
        {
            Left = this.Left + shrinker,
            Top = this.Top + shrinker,

            Right = this.Right - shrinker,
            Bottom = this.Bottom - shrinker,
        };

        public Bounds ClipToBounds(float boundsWidth, float boundsHeight) => new Bounds
        {
            Left = System.Math.Min(System.Math.Max(this.Left, 0f), boundsWidth),
            Top = System.Math.Min(System.Math.Max(this.Top, 0f), boundsHeight),
            Right = System.Math.Min(System.Math.Max(this.Left, 0f), boundsWidth),
            Bottom = System.Math.Min(System.Math.Max(this.Top, 0f), boundsHeight),
        };
        public Bounds ClipToBounds(Bounds bounds) => new Bounds
        {
            Left = System.Math.Min(System.Math.Max(this.Left, bounds.Left), bounds.Right),
            Top = System.Math.Min(System.Math.Max(this.Top, bounds.Top), bounds.Bottom),
            Right = System.Math.Min(System.Math.Max(this.Left, bounds.Left), bounds.Right),
            Bottom = System.Math.Min(System.Math.Max(this.Top, bounds.Top), bounds.Bottom),
        };

        public bool Contains(Vector2 point)
        {
            return point.X > this.Left == point.X < this.Right && point.Y > this.Top == point.Y < this.Bottom;
        }

        public bool Contains(Bounds bounds)
        {
            return CH(bounds.Left, bounds.Right) && CV(bounds.Top, bounds.Bottom);
        }

        private bool CH(float left, float right)
        {
            if (left < right)
            {
                if (this.Left < this.Right)
                    return !(left < this.Left) && (right < this.Right);
                else
                    return (right < this.Left) && !(left < this.Right);
            }
            else
            {
                if (this.Left < this.Right)
                    return !(right < this.Left) && (left < this.Right);
                else
                    return (left < this.Left) && !(right < this.Right);
            }
        }

        private bool CV(float top, float bottom)
        {
            if (top < bottom)
            {
                if (this.Top < this.Bottom)
                    return !(top < this.Top) && (bottom < this.Bottom);
                else
                    return (bottom < this.Top) && !(top < this.Bottom);
            }
            else
            {
                if (this.Top < this.Bottom)
                    return !(bottom < this.Top) && (top < this.Bottom);
                else
                    return (top < this.Top) && !(bottom < this.Bottom);
            }
        }

        public bool Contains(Triangle quad)
        {
            float x0 = quad.LeftTop.X;
            float y0 = quad.LeftTop.Y;

            float x1 = quad.RightTop.X;
            float y1 = quad.RightTop.Y;

            float x2 = quad.LeftBottom.X;
            float y2 = quad.LeftBottom.Y;

            float x3 = quad.RightTop.X + quad.LeftBottom.X - quad.LeftTop.X; // Triangle -> Transformer
            float y3 = quad.RightTop.Y + quad.LeftBottom.Y - quad.LeftTop.Y; // Triangle -> Transformer

            switch (Comparer<float>.Default.Compare(this.Right, this.Left))
            {
                case 1:
                    switch (Comparer<float>.Default.Compare(this.Bottom, this.Top))
                    {
                        case 1:
                            return x0 >= this.Left && x0 <= this.Right && y0 >= this.Top && y0 <= this.Bottom &&
                                x1 >= this.Left && x1 <= this.Right && y1 >= this.Top && y1 <= this.Bottom &&
                                x2 >= this.Left && x2 <= this.Right && y2 >= this.Top && y2 <= this.Bottom &&
                                x3 >= this.Left && x3 <= this.Right && y3 >= this.Top && y3 <= this.Bottom;
                        case -1:
                            return x0 >= this.Left && x0 <= this.Right && y0 >= this.Bottom && y0 <= this.Top &&
                                x1 >= this.Left && x1 <= this.Right && y1 >= this.Bottom && y1 <= this.Top &&
                                x2 >= this.Left && x2 <= this.Right && y2 >= this.Bottom && y2 <= this.Top &&
                                x3 >= this.Left && x3 <= this.Right && y3 >= this.Bottom && y3 <= this.Top;
                        default:
                            return false;
                    }
                case -1:
                    switch (Comparer<float>.Default.Compare(this.Bottom, this.Top))
                    {
                        case 1:
                            return x0 >= this.Right && x0 <= this.Left && y0 >= this.Top && y0 <= this.Bottom &&
                                x1 >= this.Right && x1 <= this.Left && y1 >= this.Top && y1 <= this.Bottom &&
                                x2 >= this.Right && x2 <= this.Left && y2 >= this.Top && y2 <= this.Bottom &&
                                x3 >= this.Right && x3 <= this.Left && y3 >= this.Top && y3 <= this.Bottom;
                        case -1:
                            return x0 >= this.Right && x0 <= this.Left && y0 >= this.Bottom && y0 <= this.Top &&
                                x1 >= this.Right && x1 <= this.Left && y1 >= this.Bottom && y1 <= this.Top &&
                                x2 >= this.Right && x2 <= this.Left && y2 >= this.Bottom && y2 <= this.Top &&
                                x3 >= this.Right && x3 <= this.Left && y3 >= this.Bottom && y3 <= this.Top;
                        default:
                            return false;
                    }
                default:
                    return false;
            }
        }

        public bool Contains(Quadrilateral quad)
        {
            float x0 = quad.LeftTop.X;
            float y0 = quad.LeftTop.Y;

            float x1 = quad.RightTop.X;
            float y1 = quad.RightTop.Y;

            float x2 = quad.LeftBottom.X;
            float y2 = quad.LeftBottom.Y;

            float x3 = quad.RightBottom.X;
            float y3 = quad.RightBottom.Y;

            switch (Comparer<float>.Default.Compare(this.Right, this.Left))
            {
                case 1:
                    switch (Comparer<float>.Default.Compare(this.Bottom, this.Top))
                    {
                        case 1:
                            return x0 >= this.Left && x0 <= this.Right && y0 >= this.Top && y0 <= this.Bottom &&
                                x1 >= this.Left && x1 <= this.Right && y1 >= this.Top && y1 <= this.Bottom &&
                                x2 >= this.Left && x2 <= this.Right && y2 >= this.Top && y2 <= this.Bottom &&
                                x3 >= this.Left && x3 <= this.Right && y3 >= this.Top && y3 <= this.Bottom;
                        case -1:
                            return x0 >= this.Left && x0 <= this.Right && y0 >= this.Bottom && y0 <= this.Top &&
                                x1 >= this.Left && x1 <= this.Right && y1 >= this.Bottom && y1 <= this.Top &&
                                x2 >= this.Left && x2 <= this.Right && y2 >= this.Bottom && y2 <= this.Top &&
                                x3 >= this.Left && x3 <= this.Right && y3 >= this.Bottom && y3 <= this.Top;
                        default:
                            return false;
                    }
                case -1:
                    switch (Comparer<float>.Default.Compare(this.Bottom, this.Top))
                    {
                        case 1:
                            return x0 >= this.Right && x0 <= this.Left && y0 >= this.Top && y0 <= this.Bottom &&
                                x1 >= this.Right && x1 <= this.Left && y1 >= this.Top && y1 <= this.Bottom &&
                                x2 >= this.Right && x2 <= this.Left && y2 >= this.Top && y2 <= this.Bottom &&
                                x3 >= this.Right && x3 <= this.Left && y3 >= this.Top && y3 <= this.Bottom;
                        case -1:
                            return x0 >= this.Right && x0 <= this.Left && y0 >= this.Bottom && y0 <= this.Top &&
                                x1 >= this.Right && x1 <= this.Left && y1 >= this.Bottom && y1 <= this.Top &&
                                x2 >= this.Right && x2 <= this.Left && y2 >= this.Bottom && y2 <= this.Top &&
                                x3 >= this.Right && x3 <= this.Left && y3 >= this.Bottom && y3 <= this.Top;
                        default:
                            return false;
                    }
                default:
                    return false;
            }
        }

        public bool ContainsPoint(Vector2 point)
        {
            float x = point.X;
            float y = point.Y;

            switch (Comparer<float>.Default.Compare(this.Right, this.Left))
            {
                case 1:
                    switch (Comparer<float>.Default.Compare(this.Bottom, this.Top))
                    {
                        case 1:
                            return x >= this.Left && x <= this.Right && y >= this.Top && y <= this.Bottom;
                        case -1:
                            return x >= this.Left && x <= this.Right && y >= this.Bottom && y <= this.Top;
                        default:
                            return false;
                    }
                case -1:
                    switch (Comparer<float>.Default.Compare(this.Bottom, this.Top))
                    {
                        case 1:
                            return x >= this.Right && x <= this.Left && y >= this.Top && y <= this.Bottom;
                        case -1:
                            return x >= this.Right && x <= this.Left && y >= this.Bottom && y <= this.Top;
                        default:
                            return false;
                    }
                default:
                    return false;
            }
        }

        public QuadrilateralContainsNodeMode ContainsNode(Vector2 point, float minSelectedLengthSquared = 144f)
        {
            float x = point.X;
            float y = point.Y;

            float ax = x - this.Right;
            float ay = y - this.Bottom;

            float a2 = ax * ax + ay * ay;
            if (a2 < minSelectedLengthSquared)
                return QuadrilateralContainsNodeMode.RightBottom;

            float bx = x - this.Left;
            float by = y - this.Bottom;

            float b2 = bx * bx + by * by;
            if (b2 < minSelectedLengthSquared)
                return QuadrilateralContainsNodeMode.LeftBottom;

            float cx = x - this.Right;
            float cy = y - this.Top;

            float c2 = cx * cx + cy * cy;
            if (c2 < minSelectedLengthSquared)
                return QuadrilateralContainsNodeMode.RightTop;

            float dx = x - this.Left;
            float dy = y - this.Top;

            float d2 = dx * dx + dy * dy;
            if (d2 < minSelectedLengthSquared)
                return QuadrilateralContainsNodeMode.LeftTop;

            switch (Comparer<float>.Default.Compare(this.Right, this.Left))
            {
                case 1:
                    switch (Comparer<float>.Default.Compare(this.Bottom, this.Top))
                    {
                        case 1:
                            if (x >= this.Left && x <= this.Right && y >= this.Top && y <= this.Bottom)
                                return QuadrilateralContainsNodeMode.Contains;
                            else
                                return QuadrilateralContainsNodeMode.None;
                        case -1:
                            if (x >= this.Left && x <= this.Right && y >= this.Bottom && y <= this.Top)
                                return QuadrilateralContainsNodeMode.Contains;
                            else
                                return QuadrilateralContainsNodeMode.None;
                        default:
                            return QuadrilateralContainsNodeMode.None;
                    }
                case -1:
                    switch (Comparer<float>.Default.Compare(this.Bottom, this.Top))
                    {
                        case 1:
                            if (x >= this.Right && x <= this.Left && y >= this.Top && y <= this.Bottom)
                                return QuadrilateralContainsNodeMode.Contains;
                            else
                                return QuadrilateralContainsNodeMode.None;
                        case -1:
                            if (x >= this.Right && x <= this.Left && y >= this.Bottom && y <= this.Top)
                                return QuadrilateralContainsNodeMode.Contains;
                            else
                                return QuadrilateralContainsNodeMode.None;
                        default:
                            return QuadrilateralContainsNodeMode.None;
                    }
                default:
                    return QuadrilateralContainsNodeMode.None;
            }
        }
        #endregion Public instance methods

        #region Public Static Methods
        public static Bounds Inflate(Bounds bounds, float x, float y)
        {
            return new Bounds
            {
                Left = bounds.Left - x,
                Top = bounds.Top - y,

                Right = bounds.Right + x,
                Bottom = bounds.Bottom + y,
            };
        }

        public static Bounds Union(Bounds a, Vector2 b)
        {
            return new Bounds
            {
                Left = Math.Min(a.Left, b.X),
                Top = Math.Min(a.Top, b.Y),

                Right = Math.Max(a.Right, b.X),
                Bottom = Math.Max(a.Bottom, b.Y),
            };
        }
        public static Bounds Union(Bounds a, Bounds b)
        {
            return new Bounds
            {
                Left = System.Math.Min(a.Left, b.Left),
                Top = System.Math.Min(a.Top, b.Top),

                Right = System.Math.Max(a.Right, b.Right),
                Bottom = System.Math.Max(a.Bottom, b.Bottom),
            };
        }

        public static Bounds Intersect(Bounds a, Vector2 b)
        {
            return new Bounds
            {
                Left = System.Math.Max(a.Left, b.X),
                Top = System.Math.Max(a.Top, b.Y),

                Right = System.Math.Min(a.Right, b.X),
                Bottom = System.Math.Min(a.Bottom, b.Y),
            };
        }
        public static Bounds Intersect(Bounds a, Bounds b)
        {
            return new Bounds
            {
                Left = System.Math.Max(a.Left, b.Left),
                Top = System.Math.Max(a.Top, b.Top),

                Right = System.Math.Min(a.Right, b.Right),
                Bottom = System.Math.Min(a.Bottom, b.Bottom),
            };
        }

        public static Bounds Translate(Bounds bounds, float translateX, float translateY)
        {
            return new Bounds
            {
                Left = bounds.Left + translateX,
                Top = bounds.Top + translateY,
                Right = bounds.Right + translateX,
                Bottom = bounds.Bottom + translateY,
            };
        }

        public static Bounds Translate(Bounds bounds, Vector2 translate)
        {
            return bounds + translate;
        }

        public static Bounds TranslateX(Bounds bounds, float translateX)
        {
            return new Bounds
            {
                Left = bounds.Left + translateX,
                Top = bounds.Top,
                Right = bounds.Right + translateX,
                Bottom = bounds.Bottom,
            };
        }

        public static Bounds TranslateY(Bounds bounds, float translateY)
        {
            return new Bounds
            {
                Left = bounds.Left,
                Top = bounds.Top + translateY,
                Right = bounds.Right,
                Bottom = bounds.Bottom + translateY,
            };
        }

        public static Bounds Scale(Bounds bounds, float xScale, float yScale)
        {
            return new Bounds
            {
                Left = bounds.Left * xScale,
                Top = bounds.Top * yScale,
                Right = bounds.Right * xScale,
                Bottom = bounds.Bottom * yScale,
            };
        }

        public static Bounds Scale(Bounds bounds, float scale)
        {
            return bounds * scale;
        }

        public static Bounds Scale(Bounds bounds, float scale, Vector2 centerPoint)
        {
            return new Bounds
            {
                Left = (bounds.Left - centerPoint.X) * scale + centerPoint.X,
                Top = (bounds.Top - centerPoint.Y) * scale + centerPoint.Y,
                Right = (bounds.Right - centerPoint.X) * scale + centerPoint.X,
                Bottom = (bounds.Bottom - centerPoint.Y) * scale + centerPoint.Y,
            };
        }

        public static Bounds Scale(Bounds bounds, Vector2 scales)
        {
            return bounds * scales;
        }

        public static Bounds Scale(Bounds bounds, Vector2 scales, Vector2 centerPoint)
        {
            return new Bounds
            {
                Left = (bounds.Left - centerPoint.X) * scales.X + centerPoint.X,
                Top = (bounds.Top - centerPoint.Y) * scales.Y + centerPoint.Y,
                Right = (bounds.Right - centerPoint.X) * scales.X + centerPoint.X,
                Bottom = (bounds.Bottom - centerPoint.Y) * scales.Y + centerPoint.Y,
            };
        }

        public static Bounds Rotate(Bounds bounds, Rotation2x2 rotation)
        {
            return new Bounds
            {
                Left = bounds.Left * rotation.C - bounds.Left * rotation.S,
                Top = bounds.Top * rotation.S + bounds.Top * rotation.C,
                Right = bounds.Right * rotation.C - bounds.Right * rotation.S,
                Bottom = bounds.Bottom * rotation.S + bounds.Bottom * rotation.C,
            };
        }

        public static Bounds Transform(Bounds bounds, Matrix2x2 matrix)
        {
            return new Bounds
            {
                Left = matrix.TranslateX + matrix.ScaleX * bounds.Left,
                Top = matrix.TranslateY + matrix.ScaleY * bounds.Top,

                Right = matrix.TranslateX + matrix.ScaleX * bounds.Right,
                Bottom = matrix.TranslateY + matrix.ScaleY * bounds.Bottom
            };
        }

        public static Bounds Transform(Bounds bounds, Vector2 translate, float scale)
        {
            return new Bounds
            {
                Left = bounds.Left * scale + translate.X,
                Top = bounds.Top * scale + translate.Y,
                Right = bounds.Right * scale + translate.X,
                Bottom = bounds.Bottom * scale + translate.Y,
            };
        }
        public static Bounds Transform(Bounds bounds, Vector2 translate, float scale, Vector2 centerPoint)
        {
            return new Bounds
            {
                Left = (bounds.Left - centerPoint.X) * scale + centerPoint.X + translate.X,
                Top = (bounds.Top - centerPoint.Y) * scale + centerPoint.Y + translate.Y,
                Right = (bounds.Right - centerPoint.X) * scale + centerPoint.X + translate.X,
                Bottom = (bounds.Bottom - centerPoint.Y) * scale + centerPoint.Y + translate.Y,
            };
        }

        public static Bounds Transform(Bounds bounds, Vector2 translate, Vector2 scales)
        {
            return new Bounds
            {
                Left = bounds.Left * scales.X + translate.X,
                Top = bounds.Top * scales.Y + translate.Y,
                Right = bounds.Right * scales.X + translate.X,
                Bottom = bounds.Bottom * scales.Y + translate.Y,
            };
        }
        public static Bounds Transform(Bounds bounds, Vector2 translate, Vector2 scales, Vector2 centerPoint)
        {
            return new Bounds
            {
                Left = (bounds.Left - centerPoint.X) * scales.X + centerPoint.X + translate.X,
                Top = (bounds.Top - centerPoint.Y) * scales.Y + centerPoint.Y + translate.Y,
                Right = (bounds.Right - centerPoint.X) * scales.X + centerPoint.X + translate.X,
                Bottom = (bounds.Bottom - centerPoint.Y) * scales.Y + centerPoint.Y + translate.Y,
            };
        }
        #endregion Public Static Methods

        #region Public operator methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds Add(Bounds left, Vector2 right)
        {
            return left + right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds Add(Bounds left, Bounds right)
        {
            return left + right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds Subtract(Bounds left, Vector2 right)
        {
            return left - right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds Subtract(Vector2 left, Bounds right)
        {
            return left - right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds Subtract(Bounds left, Bounds right)
        {
            return left - right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds Multiply(Bounds left, Vector2 right)
        {
            return left * right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds Multiply(Bounds left, Single right)
        {
            return left * right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds Multiply(Single left, Bounds right)
        {
            return left * right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds Divide(Bounds left, Vector2 right)
        {
            return left / right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds Divide(Bounds left, Single divisor)
        {
            return left / divisor;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds Negate(Bounds value)
        {
            return -value;
        }
        #endregion Public operator methods
    }
}