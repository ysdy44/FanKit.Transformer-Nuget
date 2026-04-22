using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace FanKit.Transformer
{
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct Quadrilateral : IEquatable<Quadrilateral>
    {
        #region Public Instance Properties
        public bool IsEmpty
        {
            get
            {
                return this.LeftTop.X == 0f & this.LeftTop.Y == 0f &
                  this.RightTop.X == 0f & this.RightTop.Y == 0f &
                  this.LeftBottom.X == 0f & this.LeftBottom.Y == 0f &
                  this.RightBottom.X == 0f & this.RightBottom.Y == 0f;
            }
        }

        public bool IsIdentity
        {
            get
            {
                return this.LeftTop.X == 0f & this.LeftTop.Y == 0f &
                  this.RightTop.X == 1f & this.RightTop.Y == 0f &
                  this.LeftBottom.X == 0f & this.LeftBottom.Y == 1f &
                  this.RightBottom.X == 1f & this.RightBottom.Y == 1f;
            }
        }

        public bool IsHorizontalZero
        {
            get
            {
                return this.RightTop.X + this.RightBottom.X == this.LeftTop.X + this.LeftBottom.X &&
                    this.RightTop.Y + this.RightBottom.Y == this.LeftTop.Y + this.LeftBottom.Y;
            }
        }

        public bool IsVerticalZero
        {
            get
            {
                return this.LeftBottom.X + this.RightBottom.X == this.LeftTop.X + this.RightTop.X &&
                    this.LeftBottom.Y + this.RightBottom.Y == this.LeftTop.Y + this.RightTop.Y;
            }
        }
        #endregion Public Instance Properties

        #region Public Static Properties
        static readonly Quadrilateral empty = new Quadrilateral
        {
            LeftTop = Vector2.Zero,
            RightTop = Vector2.Zero,
            LeftBottom = Vector2.Zero,
            RightBottom = Vector2.Zero,
        };

        static readonly Quadrilateral identity = new Quadrilateral
        {
            LeftTop = Vector2.Zero,
            RightTop = Vector2.UnitX,
            LeftBottom = Vector2.UnitY,
            RightBottom = Vector2.One,
        };

        public static Quadrilateral Empty
        {
            get { return empty; }
        }

        public static Quadrilateral Identity
        {
            get { return identity; }
        }
        #endregion Public Static Properties

        #region Public instance methods
        public override int GetHashCode()
        {
            int hashCode = -747894596;
            hashCode = hashCode * -1521134295 + this.LeftTop.GetHashCode();
            hashCode = hashCode * -1521134295 + this.RightTop.GetHashCode();
            hashCode = hashCode * -1521134295 + this.LeftBottom.GetHashCode();
            hashCode = hashCode * -1521134295 + this.RightBottom.GetHashCode();
            return hashCode;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Quadrilateral))
                return false;
            return Equals((Quadrilateral)obj);
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
            StringBuilder sb = new StringBuilder();
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            sb.Append('{');

            sb.Append('<');
            sb.Append(this.LeftTop.X.ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(this.LeftTop.Y.ToString(format, formatProvider));
            sb.Append('>');

            sb.Append(' ');

            sb.Append('<');
            sb.Append(this.RightTop.X.ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(this.RightTop.Y.ToString(format, formatProvider));
            sb.Append('>');

            sb.Append(' ');

            sb.Append('<');
            sb.Append(this.LeftBottom.X.ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(this.LeftBottom.Y.ToString(format, formatProvider));
            sb.Append('>');

            sb.Append(' ');

            sb.Append('<');
            sb.Append(this.RightBottom.X.ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(this.RightBottom.Y.ToString(format, formatProvider));
            sb.Append('>');

            sb.Append('}');
            return sb.ToString();
        }

        public Vector2[] To3Points() => new Vector2[]
        {
            this.LeftTop,
            this.RightTop,
            this.LeftBottom,
        };
        public Vector2[] To4Points() => new Vector2[]
        {
            this.LeftTop,
            this.RightTop,
            this.RightBottom,
            this.LeftBottom,
        };
        public Bounds ToBounds() => new Bounds
        {
            Left = System.Math.Min(System.Math.Min(this.LeftTop.X, this.RightTop.X), System.Math.Min(this.RightBottom.X, this.LeftBottom.X)),
            Top = System.Math.Min(System.Math.Min(this.LeftTop.Y, this.RightTop.Y), System.Math.Min(this.RightBottom.Y, this.LeftBottom.Y)),
            Right = System.Math.Max(System.Math.Max(this.LeftTop.X, this.RightTop.X), System.Math.Max(this.RightBottom.X, this.LeftBottom.X)),
            Bottom = System.Math.Max(System.Math.Max(this.LeftTop.Y, this.RightTop.Y), System.Math.Max(this.RightBottom.Y, this.LeftBottom.Y)),
        };
        public Triangle ToTriangle() => new Triangle
        {
            LeftTop = this.LeftTop,
            RightTop = this.RightTop,
            LeftBottom = this.LeftBottom,
        };
        public Quadrilateral ToQuadrilateral() => new Quadrilateral
        {
            LeftTop = this.LeftTop,
            RightTop = this.RightTop,
            LeftBottom = this.LeftBottom,
            RightBottom = this.RightBottom,
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix3x2 Normalize() => new Matrix3x2
        {
            M11 = this.RightTop.X - this.LeftTop.X,
            M12 = this.RightTop.Y - this.LeftTop.Y,
            M21 = this.LeftBottom.X - this.LeftTop.X,
            M22 = this.LeftBottom.Y - this.LeftTop.Y,
            M31 = this.LeftTop.X,
            M32 = this.LeftTop.Y,
        };

        public float CenterX() => (this.LeftTop.X + this.RightTop.X + this.RightBottom.X + this.LeftBottom.X) / 4f;
        public float CenterY() => (this.LeftTop.Y + this.RightTop.Y + this.RightBottom.Y + this.LeftBottom.Y) / 4f;
        public Vector2 Center() => new Vector2((this.LeftTop.X + this.RightTop.X + this.RightBottom.X + this.LeftBottom.X) / 4f,
            (this.LeftTop.Y + this.RightTop.Y + this.RightBottom.Y + this.LeftBottom.Y) / 4f);

        public Vector2 CenterLeft() => new Vector2((this.LeftTop.X + this.LeftBottom.X) / 2f,
            (this.LeftTop.Y + this.LeftBottom.Y) / 2f);
        public Vector2 CenterTop() => new Vector2((this.LeftTop.X + this.RightTop.X) / 2f,
            (this.LeftTop.Y + this.RightTop.Y) / 2f);
        public Vector2 CenterRight() => new Vector2((this.RightTop.X + this.RightBottom.X) / 2f,
            (this.RightTop.Y + this.RightBottom.Y) / 2f);
        public Vector2 CenterBottom() => new Vector2((this.RightBottom.X + this.LeftBottom.X) / 2f,
            (this.RightBottom.Y + this.LeftBottom.Y) / 2f);

        public float MinX() => System.Math.Min(System.Math.Min(this.LeftTop.X, this.RightTop.X), System.Math.Min(this.RightBottom.X, this.LeftBottom.X));
        public float MaxX() => System.Math.Max(System.Math.Max(this.LeftTop.X, this.RightTop.X), System.Math.Max(this.RightBottom.X, this.LeftBottom.X));
        public float MinY() => System.Math.Min(System.Math.Min(this.LeftTop.Y, this.RightTop.Y), System.Math.Min(this.RightBottom.Y, this.LeftBottom.Y));
        public float MaxY() => System.Math.Max(System.Math.Max(this.LeftTop.Y, this.RightTop.Y), System.Math.Max(this.RightBottom.Y, this.LeftBottom.Y));

        public Vector2 Horizontal() => new Vector2((this.RightTop.X + this.RightBottom.X - this.LeftTop.X - this.LeftBottom.X) / 2f,
             (this.RightTop.Y + this.RightBottom.Y - this.LeftTop.Y - this.LeftBottom.Y) / 2f);
        public Vector2 Vertical() => new Vector2((this.LeftBottom.X + this.RightBottom.X - this.LeftTop.X - this.RightTop.X) / 2f,
             (this.LeftBottom.Y + this.RightBottom.Y - this.LeftTop.Y - this.RightTop.Y) / 2f);

        //public TransformController CacheTransform(TransformMode mode) => new TransformController(this, mode);
        //public TransformController CacheRotation(Vector2 point) => new TransformController(this, point);

        //public FreeTransformController CacheFreeTransform(FreeTransformMode mode) => new FreeTransformController(this, mode, 8f);

        //public Quadrilateral Transform(TransformController controller, Vector2 point, bool keepRatio, bool centeredScaling) => controller.Transform(this, point, keepRatio, centeredScaling);

        //public Quadrilateral MovePoint(FreeTransformController controller, Vector2 point) => controller.MovePoint(this, point);
        //public Quadrilateral MovePointOfConvexQuadrilateral(FreeTransformController controller, Vector2 point) => controller.MovePointOfConvexQuadrilateral(this, point);

        public Quadrilateral Expand(Quadrilateral expander) => new Quadrilateral
        {
            LeftTop = this.LeftTop - expander.LeftTop,
            RightTop = this.RightTop - expander.RightTop,
            LeftBottom = this.LeftBottom - expander.LeftBottom,
            RightBottom = this.RightBottom - expander.RightBottom,
        };
        public Quadrilateral Shrink(Quadrilateral shrinker) => new Quadrilateral
        {
            LeftTop = this.LeftTop + shrinker.LeftTop,
            RightTop = this.RightTop + shrinker.RightTop,
            LeftBottom = this.LeftBottom + shrinker.LeftBottom,
            RightBottom = this.RightBottom + shrinker.RightBottom,
        };

        public Quadrilateral ClipToBounds(float boundsWidth, float boundsHeight) => new Quadrilateral
        {
            LeftTop = new Vector2(
                System.Math.Min(System.Math.Max(this.LeftTop.X, 0f), boundsWidth),
                System.Math.Min(System.Math.Max(this.LeftTop.Y, 0f), boundsHeight)
            ),
            RightTop = new Vector2(
                System.Math.Min(System.Math.Max(this.RightTop.X, 0f), boundsWidth),
                System.Math.Min(System.Math.Max(this.RightTop.Y, 0f), boundsHeight)
            ),
            LeftBottom = new Vector2(
                System.Math.Min(System.Math.Max(this.LeftBottom.X, 0f), boundsWidth),
                System.Math.Min(System.Math.Max(this.LeftBottom.Y, 0f), boundsHeight)
            ),
            RightBottom = new Vector2(
                System.Math.Min(System.Math.Max(this.RightBottom.X, 0f), boundsWidth),
                System.Math.Min(System.Math.Max(this.RightBottom.Y, 0f), boundsHeight)
            ),
        };
        public Quadrilateral ClipToBounds(Bounds bounds) => new Quadrilateral
        {
            LeftTop = new Vector2(
                System.Math.Min(System.Math.Max(this.LeftTop.X, bounds.Left), bounds.Right),
                System.Math.Min(System.Math.Max(this.LeftTop.Y, bounds.Top), bounds.Bottom)
            ),
            RightTop = new Vector2(
                System.Math.Min(System.Math.Max(this.RightTop.X, bounds.Left), bounds.Right),
                System.Math.Min(System.Math.Max(this.RightTop.Y, bounds.Top), bounds.Bottom)
            ),
            LeftBottom = new Vector2(
                System.Math.Min(System.Math.Max(this.LeftBottom.X, bounds.Left), bounds.Right),
                System.Math.Min(System.Math.Max(this.LeftBottom.Y, bounds.Top), bounds.Bottom)
            ),
            RightBottom = new Vector2(
                System.Math.Min(System.Math.Max(this.RightBottom.X, bounds.Left), bounds.Right),
                System.Math.Min(System.Math.Max(this.RightBottom.Y, bounds.Top), bounds.Bottom)
            ),
        };

        public bool ContainsPoint(Vector2 point)
        {
            float x = point.X;
            float y = point.Y;

            // Corners
            Vector2 a = this.RightBottom;
            Vector2 b = this.LeftBottom;
            Vector2 c = this.RightTop;
            Vector2 d = this.LeftTop;

            switch (Comparer<float>.Default.Compare((d.X - b.X) * (y - b.Y), (d.Y - b.Y) * (x - b.X)))
            {
                case 1:
                    return (c.X - d.X) * (y - d.Y) > (c.Y - d.Y) * (x - d.X) &&
                        (a.X - c.X) * (y - c.Y) > (a.Y - c.Y) * (x - c.X) &&
                        (b.X - a.X) * (y - a.Y) > (b.Y - a.Y) * (x - a.X);
                case -1:
                    return (c.X - d.X) * (y - d.Y) < (c.Y - d.Y) * (x - d.X) &&
                        (a.X - c.X) * (y - c.Y) < (a.Y - c.Y) * (x - c.X) &&
                        (b.X - a.X) * (y - a.Y) < (b.Y - a.Y) * (x - a.X);
                default:
                    return false;
            }
        }

        public QuadrilateralContainsNodeMode ContainsNode(Vector2 point, float minSelectedLengthSquared = 144f)
        {
            float x = point.X;
            float y = point.Y;

            Vector2 a = this.RightBottom;

            float ax = x - a.X;
            float ay = y - a.Y;

            float a2 = ax * ax + ay * ay;
            if (a2 < minSelectedLengthSquared)
                return QuadrilateralContainsNodeMode.RightBottom;

            Vector2 b = this.LeftBottom;

            float bx = x - b.X;
            float by = y - b.Y;

            float b2 = bx * bx + by * by;
            if (b2 < minSelectedLengthSquared)
                return QuadrilateralContainsNodeMode.LeftBottom;

            Vector2 c = this.RightTop;

            float cx = x - c.X;
            float cy = y - c.Y;

            float c2 = cx * cx + cy * cy;
            if (c2 < minSelectedLengthSquared)
                return QuadrilateralContainsNodeMode.RightTop;

            Vector2 d = this.LeftTop;

            float dx = x - d.X;
            float dy = y - d.Y;

            float d2 = dx * dx + dy * dy;
            if (d2 < minSelectedLengthSquared)
                return QuadrilateralContainsNodeMode.LeftTop;

            switch (Comparer<float>.Default.Compare((d.X - b.X) * (y - b.Y), (d.Y - b.Y) * (x - b.X)))
            {
                case 1:
                    if ((c.X - d.X) * (y - d.Y) > (c.Y - d.Y) * (x - d.X) &&
                        (a.X - c.X) * (y - c.Y) > (a.Y - c.Y) * (x - c.X) &&
                        (b.X - a.X) * (y - a.Y) > (b.Y - a.Y) * (x - a.X))
                        return QuadrilateralContainsNodeMode.Contains;
                    else
                        return QuadrilateralContainsNodeMode.None;
                case -1:
                    if ((c.X - d.X) * (y - d.Y) < (c.Y - d.Y) * (x - d.X) &&
                        (a.X - c.X) * (y - c.Y) < (a.Y - c.Y) * (x - c.X) &&
                        (b.X - a.X) * (y - a.Y) < (b.Y - a.Y) * (x - a.X))
                        return QuadrilateralContainsNodeMode.Contains;
                    else
                        return QuadrilateralContainsNodeMode.None;
                default:
                    return QuadrilateralContainsNodeMode.None;
            }
        }
        #endregion Public instance methods

        #region Public Static Methods
        public static Quadrilateral Translate(Quadrilateral quad, float translateX, float translateY)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(quad.LeftTop.X + translateX, quad.LeftTop.Y + translateY),
                RightTop = new Vector2(quad.RightTop.X + translateX, quad.RightTop.Y + translateY),
                LeftBottom = new Vector2(quad.LeftBottom.X + translateX, quad.LeftBottom.Y + translateY),
                RightBottom = new Vector2(quad.RightBottom.X + translateX, quad.RightBottom.Y + translateY),
            };
        }

        public static Quadrilateral Translate(Quadrilateral quad, Vector2 translate)
        {
            return quad + translate;
        }

        public static Quadrilateral TranslateX(Quadrilateral quad, float translateX)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(quad.LeftTop.X + translateX, quad.LeftTop.Y),
                RightTop = new Vector2(quad.RightTop.X + translateX, quad.RightTop.Y),
                LeftBottom = new Vector2(quad.LeftBottom.X + translateX, quad.LeftBottom.Y),
                RightBottom = new Vector2(quad.RightBottom.X + translateX, quad.RightBottom.Y),
            };
        }

        public static Quadrilateral TranslateY(Quadrilateral quad, float translateY)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(quad.LeftTop.X, quad.LeftTop.Y + translateY),
                RightTop = new Vector2(quad.RightTop.X, quad.RightTop.Y + translateY),
                LeftBottom = new Vector2(quad.LeftBottom.X, quad.LeftBottom.Y + translateY),
                RightBottom = new Vector2(quad.RightBottom.X, quad.RightBottom.Y + translateY),
            };
        }

        public static Quadrilateral Scale(Quadrilateral quad, float xScale, float yScale)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(
                    quad.LeftTop.X * xScale,
                    quad.LeftTop.Y * yScale),
                RightTop = new Vector2(
                    quad.RightTop.X * xScale,
                    quad.RightTop.Y * yScale),
                LeftBottom = new Vector2(
                    quad.LeftBottom.X * xScale,
                    quad.LeftBottom.Y * yScale),
                RightBottom = new Vector2(
                    quad.RightBottom.X * xScale,
                    quad.RightBottom.Y * yScale),
            };
        }

        public static Quadrilateral Scale(Quadrilateral quad, float scale)
        {
            return quad * scale;
        }

        public static Quadrilateral Scale(Quadrilateral quad, float scale, Vector2 centerPoint)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2((quad.LeftTop.X - centerPoint.X) * scale + centerPoint.X,
                (quad.LeftTop.Y - centerPoint.Y) * scale + centerPoint.Y),

                RightTop = new Vector2((quad.RightTop.X - centerPoint.X) * scale + centerPoint.X,
                (quad.RightTop.Y - centerPoint.Y) * scale + centerPoint.Y),

                LeftBottom = new Vector2((quad.LeftBottom.X - centerPoint.X) * scale + centerPoint.X,
                (quad.LeftBottom.Y - centerPoint.Y) * scale + centerPoint.Y),

                RightBottom = new Vector2((quad.RightBottom.X - centerPoint.X) * scale + centerPoint.X,
                (quad.RightBottom.Y - centerPoint.Y) * scale + centerPoint.Y),
            };
        }

        public static Quadrilateral Scale(Quadrilateral quad, Vector2 scales)
        {
            return quad * scales;
        }

        public static Quadrilateral Scale(Quadrilateral quad, Vector2 scales, Vector2 centerPoint)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2((quad.LeftTop.X - centerPoint.X) * scales.X + centerPoint.X,
                (quad.LeftTop.Y - centerPoint.Y) * scales.Y + centerPoint.Y),

                RightTop = new Vector2((quad.RightTop.X - centerPoint.X) * scales.X + centerPoint.X,
                (quad.RightTop.Y - centerPoint.Y) * scales.Y + centerPoint.Y),

                LeftBottom = new Vector2((quad.LeftBottom.X - centerPoint.X) * scales.X + centerPoint.X,
                (quad.LeftBottom.Y - centerPoint.Y) * scales.Y + centerPoint.Y),

                RightBottom = new Vector2((quad.RightBottom.X - centerPoint.X) * scales.X + centerPoint.X,
                (quad.RightBottom.Y - centerPoint.Y) * scales.Y + centerPoint.Y),
            };
        }

        public static Quadrilateral Rotate(Quadrilateral quad, Rotation2x2 rotation)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(quad.LeftTop.X * rotation.C - quad.LeftTop.Y * rotation.S,
                quad.LeftTop.X * rotation.S + quad.LeftTop.Y * rotation.C),

                RightTop = new Vector2(quad.RightTop.X * rotation.C - quad.RightTop.Y * rotation.S,
                quad.RightTop.X * rotation.S + quad.RightTop.Y * rotation.C),

                LeftBottom = new Vector2(quad.LeftBottom.X * rotation.C - quad.LeftBottom.Y * rotation.S,
                quad.LeftBottom.X * rotation.S + quad.LeftBottom.Y * rotation.C),

                RightBottom = new Vector2(quad.RightBottom.X * rotation.C - quad.RightBottom.Y * rotation.S,
                quad.RightBottom.X * rotation.S + quad.RightBottom.Y * rotation.C),
            };
        }

        public static Quadrilateral Transform(Quadrilateral quad, Matrix3x2 matrix) => quad * matrix;

        public static Quadrilateral Transform(Quadrilateral quad, Vector2 translate, float scale)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(quad.LeftTop.X * scale + translate.X,
                quad.LeftTop.Y * scale + translate.Y),

                RightTop = new Vector2(quad.RightTop.X * scale + translate.X,
                quad.RightTop.Y * scale + translate.Y),

                LeftBottom = new Vector2(quad.LeftBottom.X * scale + translate.X,
                quad.LeftBottom.Y * scale + translate.Y),

                RightBottom = new Vector2(quad.RightBottom.X * scale + translate.X,
                quad.RightBottom.Y * scale + translate.Y),
            };
        }
        public static Quadrilateral Transform(Quadrilateral quad, Vector2 translate, float scale, Vector2 centerPoint)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2((quad.LeftTop.X - centerPoint.X) * scale + centerPoint.X + translate.X,
                (quad.LeftTop.Y - centerPoint.Y) * scale + centerPoint.Y + translate.Y),

                RightTop = new Vector2((quad.RightTop.X - centerPoint.X) * scale + centerPoint.X + translate.X,
                (quad.RightTop.Y - centerPoint.Y) * scale + centerPoint.Y + translate.Y),

                LeftBottom = new Vector2((quad.LeftBottom.X - centerPoint.X) * scale + centerPoint.X + translate.X,
                (quad.LeftBottom.Y - centerPoint.Y) * scale + centerPoint.Y + translate.Y),

                RightBottom = new Vector2((quad.RightBottom.X - centerPoint.X) * scale + centerPoint.X + translate.X,
                (quad.RightBottom.Y - centerPoint.Y) * scale + centerPoint.Y + translate.Y),
            };
        }

        public static Quadrilateral Transform(Quadrilateral quad, Vector2 translate, Vector2 scales)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(quad.LeftTop.X * scales.X + translate.X,
                quad.LeftTop.Y * scales.Y + translate.Y),

                RightTop = new Vector2(quad.RightTop.X * scales.X + translate.X,
                quad.RightTop.Y * scales.Y + translate.Y),

                LeftBottom = new Vector2(quad.LeftBottom.X * scales.X + translate.X,
                quad.LeftBottom.Y * scales.Y + translate.Y),

                RightBottom = new Vector2(quad.RightBottom.X * scales.X + translate.X,
                quad.RightBottom.Y * scales.Y + translate.Y),
            };
        }
        public static Quadrilateral Transform(Quadrilateral quad, Vector2 translate, Vector2 scales, Vector2 centerPoint)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2((quad.LeftTop.X - centerPoint.X) * scales.X + centerPoint.X + translate.X,
                (quad.LeftTop.Y - centerPoint.Y) * scales.Y + centerPoint.Y + translate.Y),

                RightTop = new Vector2((quad.RightTop.X - centerPoint.X) * scales.X + centerPoint.X + translate.X,
                (quad.RightTop.Y - centerPoint.Y) * scales.Y + centerPoint.Y + translate.Y),

                LeftBottom = new Vector2((quad.LeftBottom.X - centerPoint.X) * scales.X + centerPoint.X + translate.X,
                (quad.LeftBottom.Y - centerPoint.Y) * scales.Y + centerPoint.Y + translate.Y),

                RightBottom = new Vector2((quad.RightBottom.X - centerPoint.X) * scales.X + centerPoint.X + translate.X,
                (quad.RightBottom.Y - centerPoint.Y) * scales.Y + centerPoint.Y + translate.Y),
            };
        }
        #endregion Public Static Methods

        #region Public operator methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quadrilateral Add(Quadrilateral left, Vector2 right)
        {
            return left + right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quadrilateral Add(Quadrilateral left, Quadrilateral right)
        {
            return left + right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quadrilateral Subtract(Quadrilateral left, Vector2 right)
        {
            return left - right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quadrilateral Subtract(Vector2 left, Quadrilateral right)
        {
            return left - right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quadrilateral Subtract(Quadrilateral left, Quadrilateral right)
        {
            return left - right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quadrilateral Multiply(Quadrilateral left, Vector2 right)
        {
            return left * right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quadrilateral Multiply(Quadrilateral left, Single right)
        {
            return left * right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quadrilateral Multiply(Single left, Quadrilateral right)
        {
            return left * right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quadrilateral Divide(Quadrilateral left, Vector2 right)
        {
            return left / right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quadrilateral Divide(Quadrilateral left, Single divisor)
        {
            return left / divisor;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quadrilateral Negate(Quadrilateral value)
        {
            return -value;
        }
        #endregion Public operator methods
    }
}