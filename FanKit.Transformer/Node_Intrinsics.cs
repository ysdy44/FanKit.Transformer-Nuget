using System.Numerics;
using System.Runtime.CompilerServices;

namespace FanKit.Transformer
{
    partial struct Node
    {
        public Vector2 Point;
        public Vector2 LeftControlPoint;
        public Vector2 RightControlPoint;

        #region Constructors
        public Node(Vector2 point)
        {
            this.Point = point;
            this.LeftControlPoint = point;
            this.RightControlPoint = point;
        }

        public Node(Vector2 point, Vector2 leftControlPoint, Vector2 rightControlPoint)
        {
            this.Point = point;
            this.LeftControlPoint = leftControlPoint;
            this.RightControlPoint = rightControlPoint;
        }

        public Node(Vector2 point, Vector2 controlPoint, bool isLeftControlPoint)
        {
            if (isLeftControlPoint)
            {
                this.Point = point;
                this.LeftControlPoint = controlPoint;
                this.RightControlPoint = new Vector2(point.X + point.X - controlPoint.X,
                    point.Y + point.Y - controlPoint.Y);
            }
            else
            {
                this.Point = point;
                this.LeftControlPoint = new Vector2(point.X + point.X - controlPoint.X,
                    point.Y + point.Y - controlPoint.Y);
                this.RightControlPoint = controlPoint;
            }
        }

        /*
        internal Node(NodeController controller, Vector2 point, bool disconnected)
        {
            this.Point = controller.p0;

            switch (controller.c.m)
            {
                case ControlPointMode.LeftEqual:
                    this.LeftControlPoint = point;

                    if (disconnected)
                    {
                        this.RightControlPoint = controller.cp0;
                    }
                    else
                    {
                        this.RightControlPoint = new Vector2(controller.p0.X + controller.p0.X - point.X,
                                            controller.p0.Y + controller.p0.Y - point.Y);
                    }
                    break;
                case ControlPointMode.RightEqual:
                    this.RightControlPoint = point;

                    if (disconnected)
                    {
                        this.LeftControlPoint = controller.cp0;
                    }
                    else
                    {
                        this.LeftControlPoint = new Vector2(controller.p0.X + controller.p0.X - point.X,
                                        controller.p0.Y + controller.p0.Y - point.Y);
                    }
                    break;
                case ControlPointMode.LeftRatio:
                    this.LeftControlPoint = point;

                    if (disconnected)
                    {
                        this.RightControlPoint = controller.cp0;
                    }
                    else
                    {
                        Vector2 p = point - controller.p0;

                        float s = controller.c.b / p.Length();
                        Vector2 v = s * p;

                        this.RightControlPoint = controller.p0 - v;
                    }
                    break;
                case ControlPointMode.RightRatio:
                    {
                        this.RightControlPoint = point;

                        if (disconnected)
                        {
                            this.LeftControlPoint = controller.cp0;
                        }
                        else
                        {
                            Vector2 p = point - controller.p0;

                            float s = controller.c.b / p.Length();
                            Vector2 v = s * p;

                            this.LeftControlPoint = controller.p0 - v;
                        }
                    }
                    break;
                case ControlPointMode.LeftAngleEqual:
                    {
                        Vector2 p = point - controller.p0;
                        float l = p.X * controller.c.x.X + p.Y * controller.c.x.Y;

                        float s = l / controller.c.b;
                        Vector2 v = controller.c.x * s;

                        this.LeftControlPoint = controller.p0 + v;

                        if (disconnected)
                        {
                            this.RightControlPoint = controller.cp0;
                        }
                        else
                        {
                            this.RightControlPoint = controller.p0 - v;
                        }
                    }
                    break;
                case ControlPointMode.RightAngleEqual:
                    {
                        Vector2 p = point - controller.p0;
                        float l = p.X * controller.c.x.X + p.Y * controller.c.x.Y;

                        float s = l / controller.c.b;
                        Vector2 v = controller.c.x * s;

                        this.RightControlPoint = controller.p0 + v;

                        if (disconnected)
                        {
                            this.LeftControlPoint = controller.cp0;
                        }
                        else
                        {
                            this.LeftControlPoint = controller.p0 - v;
                        }
                    }
                    break;
                case ControlPointMode.LeftAngleRatio:
                    {
                        Vector2 p = point - controller.p0;
                        float l1 = p.X * controller.c.x.X + p.Y * controller.c.x.Y;

                        float s1 = l1 / controller.c.a;
                        Vector2 v1 = s1 * controller.c.x;

                        this.LeftControlPoint = controller.p0 + v1;

                        if (disconnected)
                        {
                            this.RightControlPoint = controller.cp0;
                        }
                        else
                        {
                            float l2 = v1.Length();

                            float s2 = controller.c.b / l2;
                            Vector2 v2 = s2 * v1;

                            this.RightControlPoint = controller.p0 - v2;
                        }
                    }
                    break;
                case ControlPointMode.RightAngleRatio:
                    {
                        Vector2 p = point - controller.p0;
                        float l1 = p.X * controller.c.x.X + p.Y * controller.c.x.Y;

                        float s1 = l1 / controller.c.a;
                        Vector2 v1 = s1 * controller.c.x;

                        this.RightControlPoint = controller.p0 + v1;

                        if (disconnected)
                        {
                            this.LeftControlPoint = controller.cp0;
                        }
                        else
                        {
                            float l2 = v1.Length();

                            float s2 = controller.c.b / l2;
                            Vector2 v2 = s2 * v1;

                            this.LeftControlPoint = controller.p0 - v2;
                        }
                    }
                    break;
                case ControlPointMode.LeftLength:
                    {
                        Vector2 p = point - controller.p0;
                        float l = p.Length();

                        float s = controller.c.a / l;
                        Vector2 b = s * p;

                        this.LeftControlPoint = controller.p0 + b;

                        if (disconnected)
                        {
                            this.RightControlPoint = controller.cp0;
                        }
                        else
                        {
                            this.RightControlPoint = controller.p0 - b;
                        }
                    }
                    break;
                case ControlPointMode.RightLength:
                    {
                        Vector2 p = point - controller.p0;
                        float l = p.Length();

                        float s = controller.c.a / l;
                        Vector2 v = s * p;

                        this.RightControlPoint = controller.p0 + v;

                        if (disconnected)
                        {
                            this.LeftControlPoint = controller.cp0;
                        }
                        else
                        {
                            this.LeftControlPoint = controller.p0 - v;
                        }
                    }
                    break;
                default:
                    {
                        this.LeftControlPoint = point;

                        this.RightControlPoint = controller.cp0;
                    }
                    break;
            }
        }
         */
        #endregion Constructors

        #region Public Instance Methods
        public bool Equals(Node other)
        {
            return
                this.Point.X == other.Point.X && this.Point.Y == other.Point.Y &&
                this.LeftControlPoint.X == other.LeftControlPoint.X && this.LeftControlPoint.Y == other.LeftControlPoint.Y &&
                this.RightControlPoint.X == other.RightControlPoint.X && this.RightControlPoint.Y == other.RightControlPoint.Y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Node Translate(Vector2 translate)
        {
            return this + translate;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Node TranslateX(float translateX)
        {
            return new Node
            {
                Point = new Vector2(this.Point.X + translateX, this.Point.Y),
                LeftControlPoint = new Vector2(this.LeftControlPoint.X + translateX, this.LeftControlPoint.Y),
                RightControlPoint = new Vector2(this.RightControlPoint.X + translateX, this.RightControlPoint.Y),
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Node TranslateY(float translateY)
        {
            return new Node
            {
                Point = new Vector2(this.Point.X, this.Point.Y + translateY),
                LeftControlPoint = new Vector2(this.LeftControlPoint.X, this.LeftControlPoint.Y + translateY),
                RightControlPoint = new Vector2(this.RightControlPoint.X, this.RightControlPoint.Y + translateY),
            };
        }
        #endregion Public Instance Methods

        #region Public Static Operators
        public static Node operator +(Node left, Vector2 right)
        {
            return new Node
            {
                Point = new Vector2(left.Point.X + right.X, left.Point.Y + right.Y),
                LeftControlPoint = new Vector2(left.LeftControlPoint.X + right.X, left.LeftControlPoint.Y + right.Y),
                RightControlPoint = new Vector2(left.RightControlPoint.X + right.X, left.RightControlPoint.Y + right.Y),
            };
        }

        public static Node operator +(Vector2 left, Node right)
        {
            return new Node
            {
                Point = new Vector2(left.X + right.Point.X, left.Y + right.Point.Y),
                LeftControlPoint = new Vector2(left.X + right.LeftControlPoint.X, left.Y + right.LeftControlPoint.Y),
                RightControlPoint = new Vector2(left.X + right.RightControlPoint.X, left.Y + right.RightControlPoint.Y),
            };
        }

        public static Node operator +(Node left, Node right)
        {
            return new Node
            {
                Point = new Vector2(left.Point.X + right.Point.X, left.Point.Y + right.Point.Y),
                LeftControlPoint = new Vector2(left.LeftControlPoint.X + right.LeftControlPoint.X, left.LeftControlPoint.Y + right.LeftControlPoint.Y),
                RightControlPoint = new Vector2(left.RightControlPoint.X + right.RightControlPoint.X, left.RightControlPoint.Y + right.RightControlPoint.Y),
            };
        }

        public static Node operator -(Node left, Vector2 right)
        {
            return new Node
            {
                Point = new Vector2(left.Point.X - right.X, left.Point.Y - right.Y),
                LeftControlPoint = new Vector2(left.LeftControlPoint.X - right.X, left.LeftControlPoint.Y - right.Y),
                RightControlPoint = new Vector2(left.RightControlPoint.X - right.X, left.RightControlPoint.Y - right.Y),
            };
        }

        public static Node operator -(Vector2 left, Node right)
        {
            return new Node
            {
                Point = new Vector2(left.X - right.Point.X, left.Y - right.Point.Y),
                LeftControlPoint = new Vector2(left.X - right.LeftControlPoint.X, left.Y - right.LeftControlPoint.Y),
                RightControlPoint = new Vector2(left.X - right.RightControlPoint.X, left.Y - right.RightControlPoint.Y),
            };
        }

        public static Node operator -(Node left, Node right)
        {
            return new Node
            {
                Point = new Vector2(left.Point.X - right.Point.X, left.Point.Y - right.Point.Y),
                LeftControlPoint = new Vector2(left.LeftControlPoint.X - right.LeftControlPoint.X, left.LeftControlPoint.Y - right.LeftControlPoint.Y),
                RightControlPoint = new Vector2(left.RightControlPoint.X - right.RightControlPoint.X, left.RightControlPoint.Y - right.RightControlPoint.Y),
            };
        }

        public static Node operator *(Node left, Matrix3x2 right)
        {
            return new Node
            {
                Point = Vector2.Transform(left.Point, right),
                LeftControlPoint = Vector2.Transform(left.LeftControlPoint, right),
                RightControlPoint = Vector2.Transform(left.RightControlPoint, right),
            };
        }

        public static Node operator *(Node left, Vector2 right)
        {
            return new Node
            {
                Point = new Vector2(
                    left.Point.X * right.X,
                    left.Point.Y * right.Y),
                LeftControlPoint = new Vector2(
                    left.LeftControlPoint.X * right.X,
                    left.LeftControlPoint.Y * right.Y),
                RightControlPoint = new Vector2(
                    left.RightControlPoint.X * right.X,
                    left.RightControlPoint.Y * right.Y),
            };
        }

        public static Node operator *(Node left, float right)
        {
            return new Node
            {
                Point = new Vector2(
                    left.Point.X * right,
                    left.Point.Y * right),
                LeftControlPoint = new Vector2(
                    left.LeftControlPoint.X * right,
                    left.LeftControlPoint.Y * right),
                RightControlPoint = new Vector2(
                    left.RightControlPoint.X * right,
                    left.RightControlPoint.Y * right),
            };
        }

        public static Node operator *(float left, Node right)
        {
            return new Node
            {
                Point = new Vector2(
                    left * right.Point.X,
                    left * right.Point.Y),
                LeftControlPoint = new Vector2(
                    left * right.LeftControlPoint.X,
                    left * right.LeftControlPoint.Y),
                RightControlPoint = new Vector2(
                    left * right.RightControlPoint.X,
                    left * right.RightControlPoint.Y),
            };
        }

        public static Node operator /(Node left, Vector2 right)
        {
            return new Node
            {
                Point = new Vector2(
                    left.Point.X / right.X,
                    left.Point.Y / right.Y),
                LeftControlPoint = new Vector2(
                    left.LeftControlPoint.X / right.X,
                    left.LeftControlPoint.Y / right.Y),
                RightControlPoint = new Vector2(
                    left.RightControlPoint.X / right.X,
                    left.RightControlPoint.Y / right.Y),
            };
        }

        public static Node operator /(Node left, float right)
        {
            return new Node
            {
                Point = new Vector2(
                    left.Point.X / right,
                    left.Point.Y / right),
                LeftControlPoint = new Vector2(
                    left.LeftControlPoint.X / right,
                    left.LeftControlPoint.Y / right),
                RightControlPoint = new Vector2(
                    left.RightControlPoint.X / right,
                    left.RightControlPoint.Y / right),
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Node operator -(Node value)
        {
            return new Node
            {
                Point = new Vector2(-value.Point.X, -value.Point.Y),
                LeftControlPoint = new Vector2(-value.LeftControlPoint.X, -value.LeftControlPoint.Y),
                RightControlPoint = new Vector2(-value.RightControlPoint.X, -value.RightControlPoint.Y),
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Node left, Node right)
        {
            return left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Node left, Node right)
        {
            return !(left == right);
        }
        #endregion Public Static Operators
    }
}