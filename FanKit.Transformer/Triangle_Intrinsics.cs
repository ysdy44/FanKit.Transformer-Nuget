using System.Numerics;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace FanKit.Transformer
{
    partial struct Triangle
    {
        public Vector2 LeftTop;
        public Vector2 RightTop;
        public Vector2 LeftBottom;

        #region Constructors
        public Triangle(Vector2 point)
        {
            this.LeftTop = point;
            this.RightTop = point;
            this.LeftBottom = point;
        }

        public Triangle(Vector2 leftTop, Vector2 rightTop, Vector2 leftBottom)
        {
            this.LeftTop = leftTop;
            this.RightTop = rightTop;
            this.LeftBottom = leftBottom;
        }

        public Triangle(Vector2 leftTop, Vector2 rightTop, Vector2 leftBottom, Matrix3x2 matrix)
        {
            this.LeftTop = Vector2.Transform(leftTop, matrix);
            this.RightTop = Vector2.Transform(rightTop, matrix);
            this.LeftBottom = Vector2.Transform(leftBottom, matrix);
        }

        public Triangle(Vector2 leftTop, Vector2 rightTop, Vector2 leftBottom, ICanvasMatrix canvasMatrix)
        {
            this.LeftTop = canvasMatrix.Transform(leftTop);
            this.RightTop = canvasMatrix.Transform(rightTop);
            this.LeftBottom = canvasMatrix.Transform(leftBottom);
        }

        public Triangle(Quadrilateral quad)
        {
            this.LeftTop = quad.LeftTop;
            this.RightTop = quad.RightTop;
            this.LeftBottom = quad.LeftBottom;
        }

        public Triangle(Quadrilateral quad, Matrix3x2 matrix)
        {
            this.LeftTop = Vector2.Transform(quad.LeftTop, matrix);
            this.RightTop = Vector2.Transform(quad.RightTop, matrix);
            this.LeftBottom = Vector2.Transform(quad.LeftBottom, matrix);
        }

        public Triangle(Quadrilateral quad, ICanvasMatrix canvasMatrix)
        {
            this.LeftTop = canvasMatrix.Transform(quad.LeftTop);
            this.RightTop = canvasMatrix.Transform(quad.RightTop);
            this.LeftBottom = canvasMatrix.Transform(quad.LeftBottom);
        }

        public Triangle(float width, float height)
        {
            this.LeftTop = Vector2.Zero;
            this.RightTop = new Vector2(width, 0f);
            this.LeftBottom = new Vector2(0f, height);
        }

        public Triangle(float width, float height, Matrix3x2 matrix)
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
        }

        public Triangle(float width, float height, Vector2 postion)
        {
            this.LeftTop = postion;
            this.RightTop = new Vector2(postion.X + width, postion.Y);
            this.LeftBottom = new Vector2(postion.X, postion.Y + height);
        }

        public Triangle(Rectangle rect)
        {
            this.LeftTop = new Vector2(rect.X, rect.Y);
            this.RightTop = new Vector2(rect.X + rect.Width, rect.Y);
            this.LeftBottom = new Vector2(rect.X, rect.Y + rect.Height);
        }

        public Triangle(float left, float top, float right, float bottom)
        {
            this.LeftTop = new Vector2(left, top);
            this.RightTop = new Vector2(right, top);
            this.LeftBottom = new Vector2(left, bottom);
        }

        public Triangle(Bounds bounds)
        {
            this.LeftTop = new Vector2(bounds.Left, bounds.Top);
            this.RightTop = new Vector2(bounds.Right, bounds.Top);
            this.LeftBottom = new Vector2(bounds.Left, bounds.Bottom);
        }

        public Triangle(XElement element)
        {
            this.LeftTop = Vector2.Zero;
            this.RightTop = Vector2.Zero;
            this.LeftBottom = Vector2.Zero;

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
                new XAttribute(nameof(Vector2.Y), this.LeftBottom.Y)));
        #endregion Constructors

        #region Public Instance Methods
        public bool Equals(Triangle other)
        {
            return
                this.LeftTop.X == other.LeftTop.X && this.LeftTop.Y == other.LeftTop.Y &&
                this.RightTop.X == other.RightTop.X && this.RightTop.Y == other.RightTop.Y &&
                this.LeftBottom.X == other.LeftBottom.X && this.LeftBottom.Y == other.LeftBottom.Y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Triangle Translate(Vector2 translate)
        {
            return this + translate;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Triangle TranslateX(float translateX)
        {
            return new Triangle
            {
                LeftTop = new Vector2(this.LeftTop.X + translateX, this.LeftTop.Y),
                RightTop = new Vector2(this.RightTop.X + translateX, this.RightTop.Y),
                LeftBottom = new Vector2(this.LeftBottom.X + translateX, this.LeftBottom.Y),
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Triangle TranslateY(float translateY)
        {
            return new Triangle
            {
                LeftTop = new Vector2(this.LeftTop.X, this.LeftTop.Y + translateY),
                RightTop = new Vector2(this.RightTop.X, this.RightTop.Y + translateY),
                LeftBottom = new Vector2(this.LeftBottom.X, this.LeftBottom.Y + translateY),
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Triangle MovePoint(TrianglePointKind kind, Vector2 point, bool isExtend = false)
        {
            if (isExtend)
            {
                switch (kind)
                {
                    case TrianglePointKind.LeftTop:
                        {
                            float x0 = this.RightTop.X - point.X;
                            float x1 = this.RightTop.X - this.LeftTop.X;
                            float x2 = this.RightTop.X - this.LeftBottom.X;

                            float y0 = this.RightTop.Y - point.Y;
                            float y1 = this.RightTop.Y - this.LeftTop.Y;
                            float y2 = this.RightTop.Y - this.LeftBottom.Y;

                            float x = x1 * x2 + y1 * y2;
                            float y = x0 * x2 + y0 * y2;
                            float z = x2 * x2 + y2 * y2;

                            float v = x / z;
                            float u = y / z;

                            Vector2 vf = new Vector2(x1 - x2 * v, y1 - y2 * v);
                            Vector2 uf = new Vector2(x0 - x2 * u, y0 - y2 * u);

                            float scale = ((vf.X < 0f) == (uf.X < 0f) && (uf.Y < 0f) == (uf.Y < 0f)) ?
                                (uf.Length() / vf.Length()) :
                                -(uf.Length() / vf.Length());

                            return new Triangle
                            {
                                LeftTop = point,
                                RightTop = new Vector2
                                {
                                    X = point.X + x1 * scale,
                                    Y = point.Y + y1 * scale,
                                },
                                LeftBottom = new Vector2
                                {
                                    X = point.X + (this.LeftBottom.X - this.LeftTop.X) * scale,
                                    Y = point.Y + (this.LeftBottom.Y - this.LeftTop.Y) * scale,
                                },
                            };
                        }
                    case TrianglePointKind.RightTop:
                        {
                            float x0 = this.LeftBottom.X - point.X;
                            float x1 = this.LeftBottom.X - this.RightTop.X;
                            float x2 = this.LeftBottom.X - this.LeftTop.X;

                            float y0 = this.LeftBottom.Y - point.Y;
                            float y1 = this.LeftBottom.Y - this.RightTop.Y;
                            float y2 = this.LeftBottom.Y - this.LeftTop.Y;

                            float x = x1 * x2 + y1 * y2;
                            float y = x0 * x2 + y0 * y2;
                            float z = x2 * x2 + y2 * y2;

                            float v = x / z;
                            float u = y / z;

                            Vector2 vf = new Vector2(x1 - x2 * v, y1 - y2 * v);
                            Vector2 uf = new Vector2(x0 - x2 * u, y0 - y2 * u);

                            float scale = ((vf.X < 0f) == (uf.X < 0f) && (uf.Y < 0f) == (uf.Y < 0f)) ?
                                (uf.Length() / vf.Length()) :
                                -(uf.Length() / vf.Length());

                            return new Triangle
                            {
                                RightTop = point,
                                LeftBottom = new Vector2
                                {
                                    X = point.X + x1 * scale,
                                    Y = point.Y + y1 * scale,
                                },
                                LeftTop = new Vector2
                                {
                                    X = point.X + (this.LeftTop.X - this.RightTop.X) * scale,
                                    Y = point.Y + (this.LeftTop.Y - this.RightTop.Y) * scale,
                                },
                            };
                        }
                    case TrianglePointKind.LeftBottom:
                        {
                            float x0 = this.LeftTop.X - point.X;
                            float x1 = this.LeftTop.X - this.LeftBottom.X;
                            float x2 = this.LeftTop.X - this.RightTop.X;

                            float y0 = this.LeftTop.Y - point.Y;
                            float y1 = this.LeftTop.Y - this.LeftBottom.Y;
                            float y2 = this.LeftTop.Y - this.RightTop.Y;

                            float x = x1 * x2 + y1 * y2;
                            float y = x0 * x2 + y0 * y2;
                            float z = x2 * x2 + y2 * y2;

                            float v = x / z;
                            float u = y / z;

                            Vector2 vf = new Vector2(x1 - x2 * v, y1 - y2 * v);
                            Vector2 uf = new Vector2(x0 - x2 * u, y0 - y2 * u);

                            float scale = ((vf.X < 0f) == (uf.X < 0f) && (uf.Y < 0f) == (uf.Y < 0f)) ?
                                (uf.Length() / vf.Length()) :
                                -(uf.Length() / vf.Length());

                            return new Triangle
                            {
                                LeftBottom = point,
                                LeftTop = new Vector2
                                {
                                    X = point.X + x1 * scale,
                                    Y = point.Y + y1 * scale,
                                },
                                RightTop = new Vector2
                                {
                                    X = point.X + (this.RightTop.X - this.LeftBottom.X) * scale,
                                    Y = point.Y + (this.RightTop.Y - this.LeftBottom.Y) * scale,
                                },
                            };
                        }
                    default:
                        return this;
                }
            }
            else
            {
                switch (kind)
                {
                    case TrianglePointKind.LeftTop:
                        return new Triangle
                        {
                            LeftTop = point,
                            RightTop = this.RightTop,
                            LeftBottom = this.LeftBottom,
                        };
                    case TrianglePointKind.RightTop:
                        return new Triangle
                        {
                            LeftTop = this.LeftTop,
                            RightTop = point,
                            LeftBottom = this.LeftBottom,
                        };
                    case TrianglePointKind.LeftBottom:
                        return new Triangle
                        {
                            LeftTop = this.LeftTop,
                            RightTop = this.RightTop,
                            LeftBottom = point,
                        };
                    default:
                        return this;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Triangle MoveChannel(TriangleChannelKind kind, float channel)
        {
            switch (kind)
            {
                case TriangleChannelKind.LeftTopX:
                    return new Triangle
                    {
                        LeftTop = new Vector2(channel, this.LeftTop.Y),
                        RightTop = this.RightTop,
                        LeftBottom = this.LeftBottom,
                    };
                case TriangleChannelKind.LeftTopY:
                    return new Triangle
                    {
                        LeftTop = new Vector2(this.LeftTop.X, channel),
                        RightTop = this.RightTop,
                        LeftBottom = this.LeftBottom,
                    };
                case TriangleChannelKind.RightTopX:
                    return new Triangle
                    {
                        LeftTop = this.LeftTop,
                        RightTop = new Vector2(channel, this.RightTop.Y),
                        LeftBottom = this.LeftBottom,
                    };
                case TriangleChannelKind.RightTopY:
                    return new Triangle
                    {
                        LeftTop = this.LeftTop,
                        RightTop = new Vector2(this.RightTop.X, channel),
                        LeftBottom = this.LeftBottom,
                    };
                case TriangleChannelKind.LeftBottomX:
                    return new Triangle
                    {
                        LeftTop = this.LeftTop,
                        RightTop = this.RightTop,
                        LeftBottom = new Vector2(channel, this.LeftBottom.Y),
                    };
                case TriangleChannelKind.LeftBottomY:
                    return new Triangle
                    {
                        LeftTop = this.LeftTop,
                        RightTop = this.RightTop,
                        LeftBottom = new Vector2(this.LeftBottom.X, channel),
                    };
                default:
                    return this;
            }
        }
        #endregion Public Instance Methods

        #region Public Static Operators
        public static Triangle operator +(Triangle left, Vector2 right)
        {
            return new Triangle
            {
                LeftTop = new Vector2(left.LeftTop.X + right.X, left.LeftTop.Y + right.Y),
                RightTop = new Vector2(left.RightTop.X + right.X, left.RightTop.Y + right.Y),
                LeftBottom = new Vector2(left.LeftBottom.X + right.X, left.LeftBottom.Y + right.Y),
            };
        }

        public static Triangle operator +(Vector2 left, Triangle right)
        {
            return new Triangle
            {
                LeftTop = new Vector2(left.X + right.LeftTop.X, left.Y + right.LeftTop.Y),
                RightTop = new Vector2(left.X + right.RightTop.X, left.Y + right.RightTop.Y),
                LeftBottom = new Vector2(left.X + right.LeftBottom.X, left.Y + right.LeftBottom.Y),
            };
        }

        public static Triangle operator +(Triangle left, Triangle right)
        {
            return new Triangle
            {
                LeftTop = new Vector2(left.LeftTop.X + right.LeftTop.X, left.LeftTop.Y + right.LeftTop.Y),
                RightTop = new Vector2(left.RightTop.X + right.RightTop.X, left.RightTop.Y + right.RightTop.Y),
                LeftBottom = new Vector2(left.LeftBottom.X + right.LeftBottom.X, left.LeftBottom.Y + right.LeftBottom.Y),
            };
        }

        public static Triangle operator -(Triangle left, Vector2 right)
        {
            return new Triangle
            {
                LeftTop = new Vector2(left.LeftTop.X - right.X, left.LeftTop.Y - right.Y),
                RightTop = new Vector2(left.RightTop.X - right.X, left.RightTop.Y - right.Y),
                LeftBottom = new Vector2(left.LeftBottom.X - right.X, left.LeftBottom.Y - right.Y),
            };
        }

        public static Triangle operator -(Vector2 left, Triangle right)
        {
            return new Triangle
            {
                LeftTop = new Vector2(left.X - right.LeftTop.X, left.Y - right.LeftTop.Y),
                RightTop = new Vector2(left.X - right.RightTop.X, left.Y - right.RightTop.Y),
                LeftBottom = new Vector2(left.X - right.LeftBottom.X, left.Y - right.LeftBottom.Y),
            };
        }

        public static Triangle operator -(Triangle left, Triangle right)
        {
            return new Triangle
            {
                LeftTop = new Vector2(left.LeftTop.X - right.LeftTop.X, left.LeftTop.Y - right.LeftTop.Y),
                RightTop = new Vector2(left.RightTop.X - right.RightTop.X, left.RightTop.Y - right.RightTop.Y),
                LeftBottom = new Vector2(left.LeftBottom.X - right.LeftBottom.X, left.LeftBottom.Y - right.LeftBottom.Y),
            };
        }

        public static Triangle operator *(Triangle left, Matrix3x2 right)
        {
            return new Triangle
            {
                LeftTop = Vector2.Transform(left.LeftTop, right),
                RightTop = Vector2.Transform(left.RightTop, right),
                LeftBottom = Vector2.Transform(left.LeftBottom, right),
            };
        }

        public static Triangle operator *(Triangle left, Vector2 right)
        {
            return new Triangle
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
            };
        }

        public static Triangle operator *(Triangle left, float right)
        {
            return new Triangle
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
            };
        }

        public static Triangle operator *(float left, Triangle right)
        {
            return new Triangle
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
            };
        }

        public static Triangle operator /(Triangle left, Vector2 right)
        {
            return new Triangle
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
            };
        }

        public static Triangle operator /(Triangle left, float right)
        {
            return new Triangle
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
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Triangle operator -(Triangle value)
        {
            return new Triangle
            {
                LeftTop = new Vector2(-value.LeftTop.X, -value.LeftTop.Y),
                RightTop = new Vector2(-value.RightTop.X, -value.RightTop.Y),
                LeftBottom = new Vector2(-value.LeftBottom.X, -value.LeftBottom.Y),
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Triangle left, Triangle right)
        {
            return left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Triangle left, Triangle right)
        {
            return !(left == right);
        }
        #endregion Public Static Operators
    }
}