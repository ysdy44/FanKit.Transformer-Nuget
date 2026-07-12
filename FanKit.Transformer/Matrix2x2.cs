using System;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace FanKit.Transformer
{
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct Matrix2x2
    {
        #region Public Instance Properties
        public bool IsEmpty
        {
            get
            {
                return this.TranslateX == 0f &
                  this.TranslateY == 0f &
                  this.ScaleX == 0f &
                  this.ScaleY == 0f;
            }
        }

        public bool IsIdentity
        {
            get
            {
                return this.TranslateX == 0f &
                  this.TranslateY == 0f &
                  this.ScaleX == 1f &
                  this.ScaleY == 1f;
            }
        }
        #endregion Public Instance Properties

        #region Public Static Properties
        static readonly Matrix2x2 identity = new Matrix2x2
        {
            ScaleX = 1f,
            ScaleY = 1f,
            TranslateX = 0f,
            TranslateY = 0f,
        };

        public static Matrix2x2 Identity
        {
            get { return identity; }
        }
        #endregion Public Static Properties

        #region Public instance methods
        public override int GetHashCode()
        {
            int hashCode = 1368381790;
            hashCode = hashCode * -1521134295 + TranslateX.GetHashCode();
            hashCode = hashCode * -1521134295 + TranslateY.GetHashCode();
            hashCode = hashCode * -1521134295 + ScaleX.GetHashCode();
            hashCode = hashCode * -1521134295 + ScaleY.GetHashCode();
            return hashCode;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Matrix2x2))
                return false;
            return Equals((Matrix2x2)obj);
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
            return String.Format(formatProvider, "{{TranslateX:{0} TranslateY:{1} ScaleX:{2} ScaleY:{3}}}",
                this.TranslateX.ToString(format, formatProvider),
                this.TranslateY.ToString(format, formatProvider),
                this.ScaleX.ToString(format, formatProvider),
                this.ScaleY.ToString(format, formatProvider));
        }

        public Matrix3x2 ToMatrix3x2() => new Matrix3x2
        {
            M11 = this.ScaleX,
            M12 = 0,
            M21 = 0,
            M22 = this.ScaleY,
            M31 = this.TranslateX,
            M32 = this.TranslateY,
        };
        #endregion Public instance methods

        #region Public Static Methods
        public static Matrix2x2 CreateTranslation(Vector2 position)
        {
            return new Matrix2x2
            {
                ScaleX = 1.0f,
                ScaleY = 1.0f,

                TranslateX = position.X,
                TranslateY = position.Y
            };
        }

        public static Matrix2x2 CreateTranslation(float xPosition, float yPosition)
        {
            return new Matrix2x2
            {
                ScaleX = 1.0f,
                ScaleY = 1.0f,

                TranslateX = xPosition,
                TranslateY = yPosition
            };
        }

        public static Matrix2x2 CreateScale(float xScale, float yScale) => new Matrix2x2
        {
            ScaleX = xScale,
            ScaleY = yScale,
            TranslateX = 0.0f,
            TranslateY = 0.0f
        };

        public static Matrix2x2 CreateScale(float xScale, float yScale, Vector2 centerPoint) => new Matrix2x2
        {
            ScaleX = xScale,
            ScaleY = yScale,
            TranslateX = centerPoint.X * (1 - xScale),
            TranslateY = centerPoint.Y * (1 - yScale)
        };

        public static Matrix2x2 CreateScale(Vector2 scales) => new Matrix2x2
        {
            ScaleX = scales.X,
            ScaleY = scales.Y,
            TranslateX = 0.0f,
            TranslateY = 0.0f
        };

        public static Matrix2x2 CreateScale(Vector2 scales, Vector2 centerPoint) => new Matrix2x2
        {
            ScaleX = scales.X,
            ScaleY = scales.Y,
            TranslateX = centerPoint.X * (1 - scales.X),
            TranslateY = centerPoint.Y * (1 - scales.Y)
        };

        public static Matrix2x2 CreateScale(float scale) => new Matrix2x2
        {
            ScaleX = scale,
            ScaleY = scale,
            TranslateX = 0.0f,
            TranslateY = 0.0f
        };

        public static Matrix2x2 CreateScale(float scale, Vector2 centerPoint) => new Matrix2x2
        {
            ScaleX = scale,
            ScaleY = scale,
            TranslateX = centerPoint.X * (1 - scale),
            TranslateY = centerPoint.Y * (1 - scale)
        };
        #endregion Public Static Methods
    }
}