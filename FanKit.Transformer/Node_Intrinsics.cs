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