using System;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace FanKit.Transformer
{
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct Rectangle
    {
        #region Public Instance Properties
        public bool IsEmpty
        {
            get
            {
                return this.X == 0f &
                  this.Y == 0f &
                  this.Width == 0f &
                  this.Height == 0f;
            }
        }

        public bool IsIdentity
        {
            get
            {
                return this.X == 0f &
                  this.Y == 0f &
                  this.Width == 1f &
                  this.Height == 1f;
            }
        }

        public bool IsWidthZero
        {
            get
            {
                return this.Width == 0f;
            }
        }

        public bool IsHeightZero
        {
            get
            {
                return this.Height == 0f;
            }
        }

        public float Left
        {
            get
            {
                return this.X;
            }
        }

        public float Top
        {
            get
            {
                return this.Y;
            }
        }

        public float Right
        {
            get
            {
                return this.X + this.Width;
            }
        }

        public float Bottom
        {
            get
            {
                return this.Y + this.Height;
            }
        }
        #endregion Public Instance Properties

        #region Public Static Properties
        static readonly Rectangle empty = new Rectangle
        {
            X = 0f,
            Y = 0f,
            Width = 0f,
            Height = 0f,
        };

        static readonly Rectangle identity = new Rectangle
        {
            X = 0f,
            Y = 0f,
            Width = 1f,
            Height = 1f,
        };

        public static Rectangle Empty
        {
            get { return empty; }
        }

        public static Rectangle Identity
        {
            get { return identity; }
        }
        #endregion Public Static Properties

        #region Public instance methods
        public override int GetHashCode()
        {
            int hashCode = 466501756;
            hashCode = hashCode * -1521134295 + this.X.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Y.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Width.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Height.GetHashCode();
            return hashCode;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Rectangle))
                return false;
            return Equals((Rectangle)obj);
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
            return String.Format(formatProvider, "{{X:{0} Y:{1} Width:{2} Height:{3}}}",
                this.X.ToString(format, formatProvider),
                this.Y.ToString(format, formatProvider),
                this.Width.ToString(format, formatProvider),
                this.Height.ToString(format, formatProvider));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2[] To3Points() => new Vector2[]
        {
            new Vector2(this.X, this.Y),
            new Vector2(this.X + this.Width, this.Y),
            new Vector2(this.X, this.Y + this.Height),
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2[] To4Points() => new Vector2[]
        {
            new Vector2(this.X, this.Y),
            new Vector2(this.X + this.Width, this.Y),
            new Vector2(this.X + this.Width, this.Y + this.Height),
            new Vector2(this.X, this.Y + this.Height),
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Bounds ToBounds() => new Bounds
        {
            Left = this.X,
            Top = this.Y,
            Right = this.X + this.Width,
            Bottom = this.Y + this.Height,
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix3x2 ToMatrix3x2() => new Matrix3x2
        {
            M11 = this.Width,
            M12 = 0,
            M21 = 0,
            M22 = this.Height,
            M31 = this.X,
            M32 = this.Y,
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4 ToIdentity() => new Vector4
        {
            X = 1f / this.Width,
            Y = 1f / this.Height,
            Z = -this.X / this.Width,
            W = -this.Y / this.Height,
        };

        public bool Contains(Vector2 point)
            => point.X > this.X
            && point.Y > this.Y
            && point.X < this.X + this.Width
            && point.Y < this.Y + this.Height;
        #endregion Public instance methods

        #region Public operator methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rectangle Add(Rectangle left, Vector2 right)
        {
            return left + right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rectangle Add(Rectangle left, Rectangle right)
        {
            return left + right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rectangle Subtract(Rectangle left, Vector2 right)
        {
            return left - right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rectangle Subtract(Vector2 left, Rectangle right)
        {
            return left - right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rectangle Subtract(Rectangle left, Rectangle right)
        {
            return left - right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rectangle Multiply(Rectangle left, Vector2 right)
        {
            return left * right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rectangle Multiply(Rectangle left, Single right)
        {
            return left * right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rectangle Multiply(Single left, Rectangle right)
        {
            return left * right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rectangle Divide(Rectangle left, Vector2 right)
        {
            return left / right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rectangle Divide(Rectangle left, Single divisor)
        {
            return left / divisor;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rectangle Negate(Rectangle value)
        {
            return -value;
        }
        #endregion Public operator methods
    }
}