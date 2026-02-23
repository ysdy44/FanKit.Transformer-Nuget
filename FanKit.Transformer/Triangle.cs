using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace FanKit.Transformer
{
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct Triangle : IEquatable<Triangle>
    {
        #region Public Instance Properties
        public bool IsEmpty
        {
            get
            {
                return this.LeftTop.X == 0f & this.LeftTop.Y == 0f &
                  this.RightTop.X == 0f & this.RightTop.Y == 0f &
                  this.LeftBottom.X == 0f & this.LeftBottom.Y == 0f;
            }
        }

        public bool IsInfinity
        {
            get
            {
                return this.LeftTop.X == 0f & this.LeftTop.Y == 0f &
                  this.RightTop.X == 1f & this.RightTop.Y == 0f &
                  this.LeftBottom.X == 0f & this.LeftBottom.Y == 1f;
            }
        }

        public bool IsHorizontalZero
        {
            get
            {
                return this.RightTop.X == this.LeftTop.X &&
                    this.RightTop.Y == this.LeftTop.Y;
            }
        }

        public bool IsVerticalZero
        {
            get
            {
                return this.LeftBottom.X == this.LeftTop.X &&
                    this.LeftBottom.Y == this.LeftTop.Y;
            }
        }
        #endregion Public Instance Properties

        #region Public Static Properties
        static readonly Triangle empty = new Triangle
        {
            LeftTop = Vector2.Zero,
            RightTop = Vector2.Zero,
            LeftBottom = Vector2.Zero,
        };

        static readonly Triangle identity = new Triangle
        {
            LeftTop = Vector2.Zero,
            RightTop = Vector2.UnitX,
            LeftBottom = Vector2.UnitY,
        };

        public static Triangle Empty
        {
            get { return empty; }
        }

        public static Triangle Identity
        {
            get { return identity; }
        }
        #endregion Public Static Properties

        #region Public instance methods
        public override int GetHashCode()
        {
            int hashCode = -1276852854;
            hashCode = hashCode * -1521134295 + this.LeftTop.GetHashCode();
            hashCode = hashCode * -1521134295 + this.RightTop.GetHashCode();
            hashCode = hashCode * -1521134295 + this.LeftBottom.GetHashCode();
            return hashCode;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Triangle))
                return false;
            return Equals((Triangle)obj);
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

            new Vector2(this.RightTop.X + this.LeftBottom.X - this.LeftTop.X,
                this.RightTop.Y + this.LeftBottom.Y - this.LeftTop.Y), // Triangle -> Transformer
            
            this.LeftBottom,
        };
        public Bounds ToBounds() => new Bounds
        {
            Left = System.Math.Min(System.Math.Min(this.LeftTop.X, this.RightTop.X), this.LeftBottom.X),
            Top = System.Math.Min(System.Math.Min(this.LeftTop.Y, this.RightTop.Y), this.LeftBottom.Y),
            Right = System.Math.Max(System.Math.Max(this.LeftTop.X, this.RightTop.X), this.LeftBottom.X),
            Bottom = System.Math.Max(System.Math.Max(this.LeftTop.Y, this.RightTop.Y), this.LeftBottom.Y),
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

            RightBottom = new Vector2(this.RightTop.X + this.LeftBottom.X - this.LeftTop.X,
                this.RightTop.Y + this.LeftBottom.Y - this.LeftTop.Y), // Triangle -> Transformer
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

        public float CenterX() => (this.RightTop.X + this.LeftBottom.X) / 2f;
        public float CenterY() => (this.RightTop.Y + this.LeftBottom.Y) / 2f;
        public Vector2 Center() => new Vector2((this.RightTop.X + this.LeftBottom.X) / 2f,
            (this.RightTop.Y + this.LeftBottom.Y) / 2f);

        public Vector2 CenterLeft() => new Vector2((this.LeftTop.X + this.LeftBottom.X) / 2f,
            (this.LeftTop.Y + this.LeftBottom.Y) / 2f);
        public Vector2 CenterTop() => new Vector2((this.LeftTop.X + this.RightTop.X) / 2f,
            (this.LeftTop.Y + this.RightTop.Y) / 2f);
        public Vector2 CenterRight() => new Vector2(this.RightTop.X + (this.LeftBottom.X - this.LeftTop.X) / 2f,
            this.RightTop.Y + (this.LeftBottom.Y - this.LeftTop.Y) / 2f);
        public Vector2 CenterBottom() => new Vector2(this.LeftBottom.X + (this.RightTop.X - this.LeftTop.X) / 2f,
            this.LeftBottom.Y + (this.RightTop.Y - this.LeftTop.Y) / 2f);

        //public float MinX() => System.Math.Min(System.Math.Min(this.A.X, this.R.X), System.Math.Min((this.R.X + this.B.X - this.A.X), this.B.X));
        //public float MaxX() => System.Math.Max(System.Math.Max(this.A.X, this.R.X), System.Math.Max((this.R.X + this.B.X - this.A.X), this.B.X));
        //public float MinY() => System.Math.Min(System.Math.Min(this.A.Y, this.R.Y), System.Math.Min((this.R.Y + this.B.Y - this.A.Y), this.B.Y));
        //public float MaxY() => System.Math.Max(System.Math.Max(this.A.Y, this.R.Y), System.Math.Max((this.R.Y + this.B.Y - this.A.Y), this.B.Y));

        public Vector2 Horizontal() => new Vector2(this.RightTop.X - this.LeftTop.X,
             this.RightTop.Y - this.LeftTop.Y);
        public Vector2 Vertical() => new Vector2(this.LeftBottom.X - this.LeftTop.X,
             this.LeftBottom.Y - this.LeftTop.Y);

        //public Controllers.Controller CacheTransform(TransformMode mode) => new Controllers.Controller(this, mode);
        //public Controllers.Controller CacheRotation(Vector2 point) => new Controllers.Controller(this, point);

        //public Controllers.FreeController CacheFreeTransform(FreeTransformMode mode) => new Controllers.FreeController(this, mode, 8f);

        //public Triangle Transform(Controllers.Controller controller, Vector2 point, bool keepRatio, bool centeredScaling) => controller.Transform(this, point, keepRatio, centeredScaling);

        //public Triangle MovePoint(FreeController controller, Vector2 point) => controller.MovePoint(this, point);
        //public Triangle MovePointOfConvexQuadrilateral(FreeController controller, Vector2 point) => controller.MovePointOfConvexQuadrilateral(this, point);

        public Triangle Expand(Triangle expander) => new Triangle
        {
            LeftTop = this.LeftTop - expander.LeftTop,
            RightTop = this.RightTop - expander.RightTop,
            LeftBottom = this.LeftBottom - expander.LeftBottom,
        };
        public Triangle Shrink(Triangle shrinker) => new Triangle
        {
            LeftTop = this.LeftTop + shrinker.LeftTop,
            RightTop = this.RightTop + shrinker.RightTop,
            LeftBottom = this.LeftBottom + shrinker.LeftBottom,
        };

        public bool ContainsPoint(Vector2 point)
        {
            float x = point.X;
            float y = point.Y;

            // Corners
            Vector2 b = this.LeftBottom;
            Vector2 c = this.RightTop;
            Vector2 d = this.LeftTop;

            switch (Comparer<float>.Default.Compare((b.X - d.X) * (c.Y - b.Y), (b.Y - d.Y) * (c.X - b.X)))
            {
                case 1:
                    return (c.X - b.X) * (y - b.Y) > (c.Y - b.Y) * (x - b.X) &&
                        (d.X - c.X) * (y - c.Y) > (d.Y - c.Y) * (x - c.X) &&
                        (b.X - d.X) * (y - d.Y) > (b.Y - d.Y) * (x - d.X);
                case -1:
                    return (b.X - d.X) * (y - b.Y) < (b.Y - d.Y) * (x - b.X) &&
                        (c.X - b.X) * (y - c.Y) < (c.Y - b.Y) * (x - c.X) &&
                        (d.X - c.X) * (y - d.Y) < (d.Y - c.Y) * (x - d.X);
                default:
                    return false;
            }
        }

        public TriangleContainsNodeMode ContainsNode(Vector2 point, float minSelectedLengthSquared = 144f)
        {
            float x = point.X;
            float y = point.Y;

            Vector2 b = this.LeftBottom;

            float bx = x - b.X;
            float by = y - b.Y;

            float b2 = bx * bx + by * by;
            if (b2 < minSelectedLengthSquared)
                return TriangleContainsNodeMode.LeftBottom;

            Vector2 c = this.RightTop;

            float cx = x - c.X;
            float cy = y - c.Y;

            float c2 = cx * cx + cy * cy;
            if (c2 < minSelectedLengthSquared)
                return TriangleContainsNodeMode.RightTop;

            Vector2 d = this.LeftTop;

            float dx = x - d.X;
            float dy = y - d.Y;

            float d2 = dx * dx + dy * dy;
            if (d2 < minSelectedLengthSquared)
                return TriangleContainsNodeMode.LeftTop;

            switch (Comparer<float>.Default.Compare((b.X - d.X) * (c.Y - b.Y), (b.Y - d.Y) * (c.X - b.X)))
            {
                case 1:
                    if ((c.X - b.X) * (y - b.Y) > (c.Y - b.Y) * (x - b.X) &&
                        (d.X - c.X) * (y - c.Y) > (d.Y - c.Y) * (x - c.X) &&
                        (b.X - d.X) * (y - d.Y) > (b.Y - d.Y) * (x - d.X))
                        return TriangleContainsNodeMode.Contains;
                    else
                        return TriangleContainsNodeMode.None;
                case -1:
                    if ((b.X - d.X) * (y - b.Y) < (b.Y - d.Y) * (x - b.X) &&
                        (c.X - b.X) * (y - c.Y) < (c.Y - b.Y) * (x - c.X) &&
                        (d.X - c.X) * (y - d.Y) < (d.Y - c.Y) * (x - d.X))
                        return TriangleContainsNodeMode.Contains;
                    else
                        return TriangleContainsNodeMode.None;
                default:
                    return TriangleContainsNodeMode.None;
            }
        }
        #endregion Public instance methods

        #region Public Static Methods
        public static Triangle Translate(Triangle triangle, float translateX, float translateY)
        {
            return new Triangle
            {
                LeftTop = new Vector2(triangle.LeftTop.X + translateX, triangle.LeftTop.Y + translateY),
                RightTop = new Vector2(triangle.RightTop.X + translateX, triangle.RightTop.Y + translateY),
                LeftBottom = new Vector2(triangle.LeftBottom.X + translateX, triangle.LeftBottom.Y + translateY),
            };
        }

        public static Triangle Translate(Triangle triangle, Vector2 translate)
        {
            return triangle + translate;
        }

        public static Triangle TranslateX(Triangle triangle, float translateX)
        {
            return new Triangle
            {
                LeftTop = new Vector2(triangle.LeftTop.X + translateX, triangle.LeftTop.Y),
                RightTop = new Vector2(triangle.RightTop.X + translateX, triangle.RightTop.Y),
                LeftBottom = new Vector2(triangle.LeftBottom.X + translateX, triangle.LeftBottom.Y),
            };
        }

        public static Triangle TranslateY(Triangle triangle, float translateY)
        {
            return new Triangle
            {
                LeftTop = new Vector2(triangle.LeftTop.X, triangle.LeftTop.Y + translateY),
                RightTop = new Vector2(triangle.RightTop.X, triangle.RightTop.Y + translateY),
                LeftBottom = new Vector2(triangle.LeftBottom.X, triangle.LeftBottom.Y + translateY),
            };
        }

        public static Triangle Scale(Triangle triangle, float xScale, float yScale)
        {
            return new Triangle
            {
                LeftTop = new Vector2(
                    triangle.LeftTop.X * xScale,
                    triangle.LeftTop.Y * yScale),
                RightTop = new Vector2(
                    triangle.RightTop.X * xScale,
                    triangle.RightTop.Y * yScale),
                LeftBottom = new Vector2(
                    triangle.LeftBottom.X * xScale,
                    triangle.LeftBottom.Y * yScale),
            };
        }

        public static Triangle Scale(Triangle triangle, float scale)
        {
            return triangle * scale;
        }

        public static Triangle Scale(Triangle triangle, float scale, Vector2 centerPoint)
        {
            return new Triangle
            {
                LeftTop = new Vector2((triangle.LeftTop.X - centerPoint.X) * scale + centerPoint.X,
                (triangle.LeftTop.Y - centerPoint.Y) * scale + centerPoint.Y),

                RightTop = new Vector2((triangle.RightTop.X - centerPoint.X) * scale + centerPoint.X,
                (triangle.RightTop.Y - centerPoint.Y) * scale + centerPoint.Y),

                LeftBottom = new Vector2((triangle.LeftBottom.X - centerPoint.X) * scale + centerPoint.X,
                (triangle.LeftBottom.Y - centerPoint.Y) * scale + centerPoint.Y),
            };
        }

        public static Triangle Scale(Triangle triangle, Vector2 scales)
        {
            return new Triangle
            {
                LeftTop = new Vector2(
                    triangle.LeftTop.X * scales.X,
                    triangle.LeftTop.Y * scales.Y),
                RightTop = new Vector2(
                    triangle.RightTop.X * scales.X,
                    triangle.RightTop.Y * scales.Y),
                LeftBottom = new Vector2(
                    triangle.LeftBottom.X * scales.X,
                    triangle.LeftBottom.Y * scales.Y),
            };
        }
        public static Triangle Scale(Triangle triangle, Vector2 scales, Vector2 centerPoint)
        {
            return new Triangle
            {
                LeftTop = new Vector2((triangle.LeftTop.X - centerPoint.X) * scales.X + centerPoint.X,
                (triangle.LeftTop.Y - centerPoint.Y) * scales.Y + centerPoint.Y),

                RightTop = new Vector2((triangle.RightTop.X - centerPoint.X) * scales.X + centerPoint.X,
                (triangle.RightTop.Y - centerPoint.Y) * scales.Y + centerPoint.Y),

                LeftBottom = new Vector2((triangle.LeftBottom.X - centerPoint.X) * scales.X + centerPoint.X,
                (triangle.LeftBottom.Y - centerPoint.Y) * scales.Y + centerPoint.Y),
            };
        }

        public static Triangle Rotate(Triangle triangle, Rotation2x2 rotation)
        {
            return new Triangle
            {
                LeftTop = new Vector2(triangle.LeftTop.X * rotation.C - triangle.LeftTop.Y * rotation.S,
                triangle.LeftTop.X * rotation.S + triangle.LeftTop.Y * rotation.C),

                RightTop = new Vector2(triangle.RightTop.X * rotation.C - triangle.RightTop.Y * rotation.S,
                triangle.RightTop.X * rotation.S + triangle.RightTop.Y * rotation.C),

                LeftBottom = new Vector2(triangle.LeftBottom.X * rotation.C - triangle.LeftBottom.Y * rotation.S,
                triangle.LeftBottom.X * rotation.S + triangle.LeftBottom.Y * rotation.C),
            };
        }

        public static Triangle Transform(Triangle triangle, Matrix3x2 matrix) => triangle * matrix;

        public static Triangle Transform(Triangle triangle, Vector2 translate, float scale)
        {
            return new Triangle
            {
                LeftTop = new Vector2(triangle.LeftTop.X * scale + translate.X,
                triangle.LeftTop.Y * scale + translate.Y),

                RightTop = new Vector2(triangle.RightTop.X * scale + translate.X,
                triangle.RightTop.Y * scale + translate.Y),

                LeftBottom = new Vector2(triangle.LeftBottom.X * scale + translate.X,
                triangle.LeftBottom.Y * scale + translate.Y),
            };
        }
        public static Triangle Transform(Triangle triangle, Vector2 translate, float scale, Vector2 centerPoint)
        {
            return new Triangle
            {
                LeftTop = new Vector2((triangle.LeftTop.X - centerPoint.X) * scale + centerPoint.X + translate.X,
                (triangle.LeftTop.Y - centerPoint.Y) * scale + centerPoint.Y + translate.Y),

                RightTop = new Vector2((triangle.RightTop.X - centerPoint.X) * scale + centerPoint.X + translate.X,
                (triangle.RightTop.Y - centerPoint.Y) * scale + centerPoint.Y + translate.Y),

                LeftBottom = new Vector2((triangle.LeftBottom.X - centerPoint.X) * scale + centerPoint.X + translate.X,
                (triangle.LeftBottom.Y - centerPoint.Y) * scale + centerPoint.Y + translate.Y),
            };
        }

        public static Triangle Transform(Triangle triangle, Vector2 translate, Vector2 scales)
        {
            return new Triangle
            {
                LeftTop = new Vector2(triangle.LeftTop.X * scales.X + translate.X,
                triangle.LeftTop.Y * scales.Y + translate.Y),

                RightTop = new Vector2(triangle.RightTop.X * scales.X + translate.X,
                triangle.RightTop.Y * scales.Y + translate.Y),

                LeftBottom = new Vector2(triangle.LeftBottom.X * scales.X + translate.X,
                triangle.LeftBottom.Y * scales.Y + translate.Y),
            };
        }
        public static Triangle Transform(Triangle triangle, Vector2 translate, Vector2 scales, Vector2 centerPoint)
        {
            return new Triangle
            {
                LeftTop = new Vector2((triangle.LeftTop.X - centerPoint.X) * scales.X + centerPoint.X + translate.X,
                (triangle.LeftTop.Y - centerPoint.Y) * scales.Y + centerPoint.Y + translate.Y),

                RightTop = new Vector2((triangle.RightTop.X - centerPoint.X) * scales.X + centerPoint.X + translate.X,
                (triangle.RightTop.Y - centerPoint.Y) * scales.Y + centerPoint.Y + translate.Y),

                LeftBottom = new Vector2((triangle.LeftBottom.X - centerPoint.X) * scales.X + centerPoint.X + translate.X,
                (triangle.LeftBottom.Y - centerPoint.Y) * scales.Y + centerPoint.Y + translate.Y),
            };
        }
        #endregion Public Static Methods

        #region Public operator methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Triangle Add(Triangle left, Vector2 right)
        {
            return left + right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Triangle Add(Triangle left, Triangle right)
        {
            return left + right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Triangle Subtract(Triangle left, Vector2 right)
        {
            return left - right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Triangle Subtract(Vector2 left, Triangle right)
        {
            return left - right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Triangle Subtract(Triangle left, Triangle right)
        {
            return left - right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Triangle Multiply(Triangle left, Vector2 right)
        {
            return left * right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Triangle Multiply(Triangle left, Single right)
        {
            return left * right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Triangle Multiply(Single left, Triangle right)
        {
            return left * right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Triangle Divide(Triangle left, Vector2 right)
        {
            return left / right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Triangle Divide(Triangle left, Single divisor)
        {
            return left / divisor;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Triangle Negate(Triangle value)
        {
            return -value;
        }
        #endregion Public operator methods
    }
}