using System.Numerics;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace FanKit.Transformer
{
    partial struct Quadrilateral
    {
        public Vector2 LeftTop;
        public Vector2 RightTop;
        public Vector2 LeftBottom;
        public Vector2 RightBottom;

        #region Constructors
        public Quadrilateral(Vector2 point)
        {
            this.LeftTop = point;
            this.RightTop = point;
            this.LeftBottom = point;
            this.RightBottom = point;
        }

        public Quadrilateral(Vector2 leftTop, Vector2 rightTop, Vector2 leftBottom)
        {
            this.LeftTop = leftTop;
            this.RightTop = rightTop;
            this.LeftBottom = leftBottom;

            this.RightBottom = new Vector2(this.RightTop.X + this.LeftBottom.X - this.LeftTop.X,
                this.RightTop.Y + this.LeftBottom.Y - this.LeftTop.Y); // Triangle -> Transformer
        }

        public Quadrilateral(Vector2 leftTop, Vector2 rightTop, Vector2 leftBottom, Matrix3x2 matrix)
        {
            this.LeftTop = Vector2.Transform(leftTop, matrix);
            this.RightTop = Vector2.Transform(rightTop, matrix);
            this.LeftBottom = Vector2.Transform(leftBottom, matrix);

            this.RightBottom = new Vector2(this.RightTop.X + this.LeftBottom.X - this.LeftTop.X,
                this.RightTop.Y + this.LeftBottom.Y - this.LeftTop.Y); // Triangle -> Transformer
        }

        public Quadrilateral(Vector2 leftTop, Vector2 rightTop, Vector2 leftBottom, ICanvasMatrix matrix)
        {
            this.LeftTop = matrix.Transform(leftTop);
            this.RightTop = matrix.Transform(rightTop);
            this.LeftBottom = matrix.Transform(leftBottom);

            this.RightBottom = new Vector2(this.RightTop.X + this.LeftBottom.X - this.LeftTop.X,
                this.RightTop.Y + this.LeftBottom.Y - this.LeftTop.Y); // Triangle -> Transformer
        }

        public Quadrilateral(Triangle triangle)
        {
            this.LeftTop = triangle.LeftTop;
            this.RightTop = triangle.RightTop;
            this.LeftBottom = triangle.LeftBottom;

            this.RightBottom = new Vector2(this.RightTop.X + this.LeftBottom.X - this.LeftTop.X,
                this.RightTop.Y + this.LeftBottom.Y - this.LeftTop.Y); // Triangle -> Transformer
        }

        public Quadrilateral(Triangle triangle, Matrix3x2 matrix)
        {
            this.LeftTop = Vector2.Transform(triangle.LeftTop, matrix);
            this.RightTop = Vector2.Transform(triangle.RightTop, matrix);
            this.LeftBottom = Vector2.Transform(triangle.LeftBottom, matrix);

            this.RightBottom = new Vector2(this.RightTop.X + this.LeftBottom.X - this.LeftTop.X,
                this.RightTop.Y + this.LeftBottom.Y - this.LeftTop.Y); // Triangle -> Transformer
        }

        public Quadrilateral(Triangle triangle, ICanvasMatrix matrix)
        {
            this.LeftTop = matrix.Transform(triangle.LeftTop);
            this.RightTop = matrix.Transform(triangle.RightTop);
            this.LeftBottom = matrix.Transform(triangle.LeftBottom);

            this.RightBottom = new Vector2(this.RightTop.X + this.LeftBottom.X - this.LeftTop.X,
                this.RightTop.Y + this.LeftBottom.Y - this.LeftTop.Y); // Triangle -> Transformer
        }

        public Quadrilateral(float width, float height)
        {
            this.LeftTop = Vector2.Zero;
            this.RightTop = new Vector2(width, 0f);
            this.LeftBottom = new Vector2(0f, height);
            this.RightBottom = new Vector2(width, height);
        }

        public Quadrilateral(float width, float height, Matrix3x2 matrix)
        {
            this.LeftTop = new Vector2(
                matrix.M31,
                matrix.M32);
            this.RightTop = new Vector2(
                width * matrix.M11 + matrix.M31,
                width * matrix.M12 + matrix.M32);
            this.LeftBottom = new Vector2(
                height * matrix.M21 + matrix.M31,
                height * matrix.M22 + matrix.M32);

            this.RightBottom = new Vector2(this.RightTop.X + this.LeftBottom.X - this.LeftTop.X,
                this.RightTop.Y + this.LeftBottom.Y - this.LeftTop.Y); // Triangle -> Transformer
        }

        public Quadrilateral(float width, float height, Vector2 postion)
        {
            this.LeftTop = postion;
            this.RightTop = new Vector2(postion.X + width, postion.Y);
            this.LeftBottom = new Vector2(postion.X, postion.Y + height);
            this.RightBottom = new Vector2(postion.X + width, postion.Y + height);
        }

        public Quadrilateral(Rectangle rect)
        {
            this.LeftTop = new Vector2(rect.X, rect.Y);
            this.RightTop = new Vector2(rect.X + rect.Width, rect.Y);
            this.LeftBottom = new Vector2(rect.X, rect.Y + rect.Height);
            this.RightBottom = new Vector2(this.RightTop.X, this.LeftBottom.Y);
        }

        public Quadrilateral(float left, float top, float right, float bottom)
        {
            this.LeftTop = new Vector2(left, top);
            this.RightTop = new Vector2(right, top);
            this.LeftBottom = new Vector2(left, bottom);
            this.RightBottom = new Vector2(right, bottom);
        }

        public Quadrilateral(Bounds bounds)
        {
            this.LeftTop = new Vector2(bounds.Left, bounds.Top);
            this.RightTop = new Vector2(bounds.Right, bounds.Top);
            this.LeftBottom = new Vector2(bounds.Left, bounds.Bottom);
            this.RightBottom = new Vector2(bounds.Right, bounds.Bottom);
        }

        //public Quadrilateral(Vector2 pointA, Vector2 pointB)
        //{
        //    Bounds bounds = new Bounds(pointA, pointB);

        //    this.LeftTop = new Vector2(bounds.Left, bounds.Top);
        //    this.RightTop = new Vector2(bounds.Right, bounds.Top);
        //    this.LeftBottom = new Vector2(bounds.Left, bounds.Bottom);
        //    this.RightBottom = new Vector2(bounds.Right, bounds.Bottom);
        //}

        //public Quadrilateral(Vector2 pointA, Vector2 pointB, bool keepRatio, bool centeredScaling)
        //{
        //    Bounds bounds = new Bounds(pointA, pointB, centeredScaling, keepRatio);

        //    this.LeftTop = new Vector2(bounds.Left, bounds.Top);
        //    this.RightTop = new Vector2(bounds.Right, bounds.Top);
        //    this.LeftBottom = new Vector2(bounds.Left, bounds.Bottom);
        //    this.RightBottom = new Vector2(bounds.Right, bounds.Bottom);
        //}

        //public Quadrilateral(Bounds bounds)
        //{
        //    this.LeftTop = new Vector2(bounds.Left, bounds.Top);
        //    this.RightTop = new Vector2(bounds.Right, bounds.Top);
        //    this.LeftBottom = new Vector2(bounds.Left, bounds.Bottom);
        //    this.RightBottom = new Vector2(bounds.Right, bounds.Bottom);
        //}

        public Quadrilateral(XElement element)
        {
            this.LeftTop = Vector2.Zero;
            this.RightTop = Vector2.Zero;
            this.LeftBottom = Vector2.Zero;
            this.RightBottom = Vector2.Zero;

            foreach (XElement item in element.Elements())
            {
                switch (item.Name.LocalName)
                {
                    case nameof(LeftTop):
                        foreach (XAttribute child in item.Attributes())
                        {
                            switch (child.Name.LocalName)
                            {
                                case nameof(Vector2.X): this.LeftTop.X = (float)child; break;
                                case nameof(Vector2.Y): this.LeftTop.Y = (float)child; break;
                                default: break;
                            }
                        }
                        break;
                    case nameof(RightTop):
                        foreach (XAttribute child in item.Attributes())
                        {
                            switch (child.Name.LocalName)
                            {
                                case nameof(Vector2.X): this.RightTop.X = (float)child; break;
                                case nameof(Vector2.Y): this.RightTop.Y = (float)child; break;
                                default: break;
                            }
                        }
                        break;
                    case nameof(LeftBottom):
                        foreach (XAttribute child in item.Attributes())
                        {
                            switch (child.Name.LocalName)
                            {
                                case nameof(Vector2.X): this.LeftBottom.X = (float)child; break;
                                case nameof(Vector2.Y): this.LeftBottom.Y = (float)child; break;
                                default: break;
                            }
                        }
                        break;
                    case nameof(RightBottom):
                        foreach (XAttribute child in item.Attributes())
                        {
                            switch (child.Name.LocalName)
                            {
                                case nameof(Vector2.X): this.RightBottom.X = (float)child; break;
                                case nameof(Vector2.Y): this.RightBottom.Y = (float)child; break;
                                default: break;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public XElement Save(string elementName) => new XElement(elementName,
            new XElement(nameof(LeftTop),
                new XAttribute(nameof(Vector2.X), this.LeftTop.X),
                new XAttribute(nameof(Vector2.Y), this.LeftTop.Y)),
            new XElement(nameof(RightTop),
                new XAttribute(nameof(Vector2.X), this.RightTop.X),
                new XAttribute(nameof(Vector2.Y), this.RightTop.Y)),
            new XElement(nameof(LeftBottom),
                new XAttribute(nameof(Vector2.X), this.LeftBottom.X),
                new XAttribute(nameof(Vector2.Y), this.LeftBottom.Y)),
            new XElement(nameof(RightBottom),
                new XAttribute(nameof(Vector2.X), this.RightBottom.X),
                new XAttribute(nameof(Vector2.Y), this.RightBottom.Y)));
        #endregion Constructors

        #region Public Instance Methods
        public bool Equals(Quadrilateral other)
        {
            return
                this.LeftTop.X == other.LeftTop.X && this.LeftTop.Y == other.LeftTop.Y &&
                this.RightTop.X == other.RightTop.X && this.RightTop.Y == other.RightTop.Y &&
                this.LeftBottom.X == other.LeftBottom.X && this.LeftBottom.Y == other.LeftBottom.Y &&
                this.RightBottom.X == other.RightBottom.X && this.RightBottom.Y == other.RightBottom.Y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Quadrilateral Translate(Vector2 translate)
        {
            return this + translate;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Quadrilateral TranslateX(float translateX)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(this.LeftTop.X + translateX, this.LeftTop.Y),
                RightTop = new Vector2(this.RightTop.X + translateX, this.RightTop.Y),
                LeftBottom = new Vector2(this.LeftBottom.X + translateX, this.LeftBottom.Y),
                RightBottom = new Vector2(this.RightBottom.X + translateX, this.RightBottom.Y),
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Quadrilateral TranslateY(float translateY)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(this.LeftTop.X, this.LeftTop.Y + translateY),
                RightTop = new Vector2(this.RightTop.X, this.RightTop.Y + translateY),
                LeftBottom = new Vector2(this.LeftBottom.X, this.LeftBottom.Y + translateY),
                RightBottom = new Vector2(this.RightBottom.X, this.RightBottom.Y + translateY),
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Quadrilateral MovePoint(QuadrilateralPointKind kind, Vector2 point)
        {
            switch (kind)
            {
                case QuadrilateralPointKind.LeftTop:
                    return new Quadrilateral
                    {
                        LeftTop = point,
                        RightTop = this.RightTop,
                        LeftBottom = this.LeftBottom,
                        RightBottom = this.RightBottom,
                    };
                case QuadrilateralPointKind.RightTop:
                    return new Quadrilateral
                    {
                        LeftTop = this.LeftTop,
                        RightTop = point,
                        LeftBottom = this.LeftBottom,
                        RightBottom = this.RightBottom,
                    };
                case QuadrilateralPointKind.LeftBottom:
                    return new Quadrilateral
                    {
                        LeftTop = this.LeftTop,
                        RightTop = this.RightTop,
                        LeftBottom = point,
                        RightBottom = this.RightBottom,
                    };
                case QuadrilateralPointKind.RightBottom:
                    return new Quadrilateral
                    {
                        LeftTop = this.LeftTop,
                        RightTop = this.RightTop,
                        LeftBottom = this.LeftBottom,
                        RightBottom = point,
                    };
                default:
                    return this;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Quadrilateral MoveChannel(QuadrilateralChannelKind kind, float channel)
        {
            switch (kind)
            {
                case QuadrilateralChannelKind.LeftTopX:
                    return new Quadrilateral
                    {
                        LeftTop = new Vector2(channel, this.LeftTop.Y),
                        RightTop = this.RightTop,
                        LeftBottom = this.LeftBottom,
                        RightBottom = this.RightBottom,
                    };
                case QuadrilateralChannelKind.LeftTopY:
                    return new Quadrilateral
                    {
                        LeftTop = new Vector2(this.LeftTop.X, channel),
                        RightTop = this.RightTop,
                        LeftBottom = this.LeftBottom,
                        RightBottom = this.RightBottom,
                    };
                case QuadrilateralChannelKind.RightTopX:
                    return new Quadrilateral
                    {
                        LeftTop = this.LeftTop,
                        RightTop = new Vector2(channel, this.RightTop.Y),
                        LeftBottom = this.LeftBottom,
                        RightBottom = this.RightBottom,
                    };
                case QuadrilateralChannelKind.RightTopY:
                    return new Quadrilateral
                    {
                        LeftTop = this.LeftTop,
                        RightTop = new Vector2(this.RightTop.X, channel),
                        LeftBottom = this.LeftBottom,
                        RightBottom = this.RightBottom,
                    };
                case QuadrilateralChannelKind.LeftBottomX:
                    return new Quadrilateral
                    {
                        LeftTop = this.LeftTop,
                        RightTop = this.RightTop,
                        LeftBottom = new Vector2(channel, this.LeftBottom.Y),
                        RightBottom = this.RightBottom,
                    };
                case QuadrilateralChannelKind.LeftBottomY:
                    return new Quadrilateral
                    {
                        LeftTop = this.LeftTop,
                        RightTop = this.RightTop,
                        LeftBottom = new Vector2(this.LeftBottom.X, channel),
                        RightBottom = this.RightBottom,
                    };
                case QuadrilateralChannelKind.RightBottomX:
                    return new Quadrilateral
                    {
                        LeftTop = this.LeftTop,
                        RightTop = this.RightTop,
                        LeftBottom = this.LeftBottom,
                        RightBottom = new Vector2(channel, this.RightBottom.Y),
                    };
                case QuadrilateralChannelKind.RightBottomY:
                    return new Quadrilateral
                    {
                        LeftTop = this.LeftTop,
                        RightTop = this.RightTop,
                        LeftBottom = this.LeftBottom,
                        RightBottom = new Vector2(this.RightBottom.X, channel),
                    };
                default:
                    return this;
            }
        }
        #endregion Public Instance Methods

        #region Public Static Operators
        public static Quadrilateral operator +(Quadrilateral left, Vector2 right)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(left.LeftTop.X + right.X, left.LeftTop.Y + right.Y),
                RightTop = new Vector2(left.RightTop.X + right.X, left.RightTop.Y + right.Y),
                LeftBottom = new Vector2(left.LeftBottom.X + right.X, left.LeftBottom.Y + right.Y),
                RightBottom = new Vector2(left.RightBottom.X + right.X, left.RightBottom.Y + right.Y),
            };
        }

        public static Quadrilateral operator +(Quadrilateral left, Quadrilateral right)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(left.LeftTop.X + right.LeftTop.X, left.LeftTop.Y + right.LeftTop.Y),
                RightTop = new Vector2(left.RightTop.X + right.RightTop.X, left.RightTop.Y + right.RightTop.Y),
                LeftBottom = new Vector2(left.LeftBottom.X + right.LeftBottom.X, left.LeftBottom.Y + right.LeftBottom.Y),
                RightBottom = new Vector2(left.RightBottom.X + right.RightBottom.X, left.RightBottom.Y + right.RightBottom.Y),
            };
        }

        public static Quadrilateral operator +(Vector2 left, Quadrilateral right)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(left.X + right.LeftTop.X, left.Y + right.LeftTop.Y),
                RightTop = new Vector2(left.X + right.RightTop.X, left.Y + right.RightTop.Y),
                LeftBottom = new Vector2(left.X + right.LeftBottom.X, left.Y + right.LeftBottom.Y),
                RightBottom = new Vector2(left.X + right.RightBottom.X, left.Y + right.RightBottom.Y),
            };
        }

        public static Quadrilateral operator -(Quadrilateral left, Vector2 right)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(left.LeftTop.X - right.X, left.LeftTop.Y - right.Y),
                RightTop = new Vector2(left.RightTop.X - right.X, left.RightTop.Y - right.Y),
                LeftBottom = new Vector2(left.LeftBottom.X - right.X, left.LeftBottom.Y - right.Y),
                RightBottom = new Vector2(left.RightBottom.X - right.X, left.RightBottom.Y - right.Y),
            };
        }

        public static Quadrilateral operator -(Vector2 left, Quadrilateral right)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(left.X - right.LeftTop.X, left.Y - right.LeftTop.Y),
                RightTop = new Vector2(left.X - right.RightTop.X, left.Y - right.RightTop.Y),
                LeftBottom = new Vector2(left.X - right.LeftBottom.X, left.Y - right.LeftBottom.Y),
                RightBottom = new Vector2(left.X - right.RightBottom.X, left.Y - right.RightBottom.Y),
            };
        }

        public static Quadrilateral operator -(Quadrilateral left, Quadrilateral right)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(left.LeftTop.X - right.LeftTop.X, left.LeftTop.Y - right.LeftTop.Y),
                RightTop = new Vector2(left.RightTop.X - right.RightTop.X, left.RightTop.Y - right.RightTop.Y),
                LeftBottom = new Vector2(left.LeftBottom.X - right.LeftBottom.X, left.LeftBottom.Y - right.LeftBottom.Y),
                RightBottom = new Vector2(left.RightBottom.X - right.RightBottom.X, left.RightBottom.Y - right.RightBottom.Y),
            };
        }

        public static Quadrilateral operator *(Quadrilateral left, Matrix3x2 right)
        {
            return new Quadrilateral
            {
                LeftTop = Vector2.Transform(left.LeftTop, right),
                RightTop = Vector2.Transform(left.RightTop, right),
                LeftBottom = Vector2.Transform(left.LeftBottom, right),
                RightBottom = Vector2.Transform(left.RightBottom, right),
            };
        }

        public static Quadrilateral operator *(Quadrilateral left, Vector2 right)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(
                    left.LeftTop.X * right.X,
                    left.LeftTop.Y * right.Y),
                RightTop = new Vector2(
                    left.RightTop.X * right.X,
                    left.RightTop.Y * right.Y),
                LeftBottom = new Vector2(
                    left.LeftBottom.X * right.X,
                    left.LeftBottom.Y * right.Y),
                RightBottom = new Vector2(
                    left.RightBottom.X * right.X,
                    left.RightBottom.Y * right.Y),
            };
        }

        public static Quadrilateral operator *(Quadrilateral left, float right)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(
                    left.LeftTop.X * right,
                    left.LeftTop.Y * right),
                RightTop = new Vector2(
                    left.RightTop.X * right,
                    left.RightTop.Y * right),
                LeftBottom = new Vector2(
                    left.LeftBottom.X * right,
                    left.LeftBottom.Y * right),
                RightBottom = new Vector2(
                    left.RightBottom.X * right,
                    left.RightBottom.Y * right),
            };
        }

        public static Quadrilateral operator *(float left, Quadrilateral right)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(
                    left * right.LeftTop.X,
                    left * right.LeftTop.Y),
                RightTop = new Vector2(
                    left * right.RightTop.X,
                    left * right.RightTop.Y),
                LeftBottom = new Vector2(
                    left * right.LeftBottom.X,
                    left * right.LeftBottom.Y),
                RightBottom = new Vector2(
                    left * right.RightBottom.X,
                    left * right.RightBottom.Y),
            };
        }

        public static Quadrilateral operator /(Quadrilateral left, Vector2 right)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(
                    left.LeftTop.X / right.X,
                    left.LeftTop.Y / right.Y),
                RightTop = new Vector2(
                    left.RightTop.X / right.X,
                    left.RightTop.Y / right.Y),
                LeftBottom = new Vector2(
                    left.LeftBottom.X / right.X,
                    left.LeftBottom.Y / right.Y),
                RightBottom = new Vector2(
                    left.RightBottom.X / right.X,
                    left.RightBottom.Y / right.Y),
            };
        }

        public static Quadrilateral operator /(Quadrilateral left, float right)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(
                    left.LeftTop.X / right,
                    left.LeftTop.Y / right),
                RightTop = new Vector2(
                    left.RightTop.X / right,
                    left.RightTop.Y / right),
                LeftBottom = new Vector2(
                    left.LeftBottom.X / right,
                    left.LeftBottom.Y / right),
                RightBottom = new Vector2(
                    left.RightBottom.X / right,
                    left.RightBottom.Y / right),
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quadrilateral operator -(Quadrilateral value)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(-value.LeftTop.X, -value.LeftTop.Y),
                RightTop = new Vector2(-value.RightTop.X, -value.RightTop.Y),
                LeftBottom = new Vector2(-value.LeftBottom.X, -value.LeftBottom.Y),
                RightBottom = new Vector2(-value.RightBottom.X, -value.RightBottom.Y),
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Quadrilateral left, Quadrilateral right)
        {
            return left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Quadrilateral left, Quadrilateral right)
        {
            return !(left == right);
        }
        #endregion Public Static Operators
    }
}