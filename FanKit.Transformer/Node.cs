using System;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace FanKit.Transformer
{
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct Node : IEquatable<Node>
    {
        #region Public instance methods
        public override int GetHashCode()
        {
            int hashCode = -1303386182;
            hashCode = hashCode * -1521134295 + this.Point.GetHashCode();
            hashCode = hashCode * -1521134295 + this.LeftControlPoint.GetHashCode();
            hashCode = hashCode * -1521134295 + this.RightControlPoint.GetHashCode();
            return hashCode;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Node))
                return false;
            return Equals((Node)obj);
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
            sb.Append(this.Point.X.ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(this.Point.Y.ToString(format, formatProvider));
            sb.Append('>');

            sb.Append(' ');

            sb.Append('<');
            sb.Append(this.LeftControlPoint.X.ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(this.LeftControlPoint.Y.ToString(format, formatProvider));
            sb.Append('>');

            sb.Append(' ');

            sb.Append('<');
            sb.Append(this.RightControlPoint.X.ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(this.RightControlPoint.Y.ToString(format, formatProvider));
            sb.Append('>');

            sb.Append('}');
            return sb.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Node MovePoint(Vector2 point)
        {
            return new Node
            {
                Point = point,
                LeftControlPoint = point + this.LeftControlPoint - this.Point,
                RightControlPoint = point + this.RightControlPoint - this.Point,
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Node MoveLeftControlPoint(Vector2 point)
        {
            return new Node
            {
                Point = this.Point,
                LeftControlPoint = point,
                RightControlPoint = this.Point + this.Point - point,
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Node MoveRightControlPoint(Vector2 point)
        {
            return new Node
            {
                Point = this.Point,
                LeftControlPoint = this.Point + this.Point - point,
                RightControlPoint = point,
            };
        }
        #endregion Public instance methods

        #region Public Static Methods
        public static Node Translate(Node node, Vector2 translate)
        {
            return node + translate;
        }

        public static Node Translate(Node node, float translateX, float translateY)
        {
            return new Node
            {
                Point = new Vector2(node.Point.X + translateX, node.Point.Y + translateY),
                LeftControlPoint = new Vector2(node.LeftControlPoint.X + translateX, node.LeftControlPoint.Y + translateY),
                RightControlPoint = new Vector2(node.RightControlPoint.X + translateX, node.RightControlPoint.Y + translateY),
            };
        }

        public static Node TranslateX(Node node, float translateX)
        {
            return new Node
            {
                Point = new Vector2(node.Point.X + translateX, node.Point.Y),
                LeftControlPoint = new Vector2(node.LeftControlPoint.X + translateX, node.LeftControlPoint.Y),
                RightControlPoint = new Vector2(node.RightControlPoint.X + translateX, node.RightControlPoint.Y),
            };
        }

        public static Node TranslateY(Node node, float translateY)
        {
            return new Node
            {
                Point = new Vector2(node.Point.X, node.Point.Y + translateY),
                LeftControlPoint = new Vector2(node.LeftControlPoint.X, node.LeftControlPoint.Y + translateY),
                RightControlPoint = new Vector2(node.RightControlPoint.X, node.RightControlPoint.Y + translateY),
            };
        }

        public static Node Scale(Node node, float xScale, float yScale)
        {
            return new Node
            {
                Point = new Vector2(
                    node.Point.X * xScale,
                    node.Point.Y * yScale),
                LeftControlPoint = new Vector2(
                    node.LeftControlPoint.X * xScale,
                    node.LeftControlPoint.Y * yScale),
                RightControlPoint = new Vector2(
                    node.RightControlPoint.X * xScale,
                    node.RightControlPoint.Y * yScale),
            };
        }

        public static Node Scale(Node node, float scale)
        {
            return node * scale;
        }

        public static Node Scale(Node node, float scale, Vector2 centerPoint)
        {
            return new Node
            {
                Point = new Vector2((node.Point.X - centerPoint.X) * scale + centerPoint.X,
                (node.Point.Y - centerPoint.Y) * scale + centerPoint.Y),

                LeftControlPoint = new Vector2((node.LeftControlPoint.X - centerPoint.X) * scale + centerPoint.X,
                (node.LeftControlPoint.Y - centerPoint.Y) * scale + centerPoint.Y),

                RightControlPoint = new Vector2((node.RightControlPoint.X - centerPoint.X) * scale + centerPoint.X,
                (node.RightControlPoint.Y - centerPoint.Y) * scale + centerPoint.Y),
            };
        }

        public static Node Scale(Node node, Vector2 scales)
        {
            return new Node
            {
                Point = new Vector2(
                    node.Point.X * scales.X,
                    node.Point.Y * scales.Y),
                LeftControlPoint = new Vector2(
                    node.LeftControlPoint.X * scales.X,
                    node.LeftControlPoint.Y * scales.Y),
                RightControlPoint = new Vector2(
                    node.RightControlPoint.X * scales.X,
                    node.RightControlPoint.Y * scales.Y),
            };
        }
        public static Node Scale(Node node, Vector2 scales, Vector2 centerPoint)
        {
            return new Node
            {
                Point = new Vector2((node.Point.X - centerPoint.X) * scales.X + centerPoint.X,
                (node.Point.Y - centerPoint.Y) * scales.Y + centerPoint.Y),

                LeftControlPoint = new Vector2((node.LeftControlPoint.X - centerPoint.X) * scales.X + centerPoint.X,
                (node.LeftControlPoint.Y - centerPoint.Y) * scales.Y + centerPoint.Y),

                RightControlPoint = new Vector2((node.RightControlPoint.X - centerPoint.X) * scales.X + centerPoint.X,
                (node.RightControlPoint.Y - centerPoint.Y) * scales.Y + centerPoint.Y),
            };
        }

        public static Node Rotate(Node node, Rotation2x2 rotation)
        {
            return new Node
            {
                Point = new Vector2(node.Point.X * rotation.C - node.Point.Y * rotation.S,
                node.Point.X * rotation.S + node.Point.Y * rotation.C),

                LeftControlPoint = new Vector2(node.LeftControlPoint.X * rotation.C - node.LeftControlPoint.Y * rotation.S,
                node.LeftControlPoint.X * rotation.S + node.LeftControlPoint.Y * rotation.C),

                RightControlPoint = new Vector2(node.RightControlPoint.X * rotation.C - node.RightControlPoint.Y * rotation.S,
                node.RightControlPoint.X * rotation.S + node.RightControlPoint.Y * rotation.C),
            };
        }

        public static Node Transform(Node node, Matrix3x2 matrix) => node * matrix;

        public static Node Transform(Node node, Vector2 translate, float scale)
        {
            return new Node
            {
                Point = new Vector2(node.Point.X * scale + translate.X,
                node.Point.Y * scale + translate.Y),

                LeftControlPoint = new Vector2(node.LeftControlPoint.X * scale + translate.X,
                node.LeftControlPoint.Y * scale + translate.Y),

                RightControlPoint = new Vector2(node.RightControlPoint.X * scale + translate.X,
                node.RightControlPoint.Y * scale + translate.Y),
            };
        }
        public static Node Transform(Node node, Vector2 translate, float scale, Vector2 centerPoint)
        {
            return new Node
            {
                Point = new Vector2((node.Point.X - centerPoint.X) * scale + centerPoint.X + translate.X,
                (node.Point.Y - centerPoint.Y) * scale + centerPoint.Y + translate.Y),

                LeftControlPoint = new Vector2((node.LeftControlPoint.X - centerPoint.X) * scale + centerPoint.X + translate.X,
                (node.LeftControlPoint.Y - centerPoint.Y) * scale + centerPoint.Y + translate.Y),

                RightControlPoint = new Vector2((node.RightControlPoint.X - centerPoint.X) * scale + centerPoint.X + translate.X,
                (node.RightControlPoint.Y - centerPoint.Y) * scale + centerPoint.Y + translate.Y),
            };
        }

        public static Node Transform(Node node, Vector2 translate, Vector2 scales)
        {
            return new Node
            {
                Point = new Vector2(node.Point.X * scales.X + translate.X,
                node.Point.Y * scales.Y + translate.Y),

                LeftControlPoint = new Vector2(node.LeftControlPoint.X * scales.X + translate.X,
                node.LeftControlPoint.Y * scales.Y + translate.Y),

                RightControlPoint = new Vector2(node.RightControlPoint.X * scales.X + translate.X,
                node.RightControlPoint.Y * scales.Y + translate.Y),
            };
        }
        public static Node Transform(Node node, Vector2 translate, Vector2 scales, Vector2 centerPoint)
        {
            return new Node
            {
                Point = new Vector2((node.Point.X - centerPoint.X) * scales.X + centerPoint.X + translate.X,
                (node.Point.Y - centerPoint.Y) * scales.Y + centerPoint.Y + translate.Y),

                LeftControlPoint = new Vector2((node.LeftControlPoint.X - centerPoint.X) * scales.X + centerPoint.X + translate.X,
                (node.LeftControlPoint.Y - centerPoint.Y) * scales.Y + centerPoint.Y + translate.Y),

                RightControlPoint = new Vector2((node.RightControlPoint.X - centerPoint.X) * scales.X + centerPoint.X + translate.X,
                (node.RightControlPoint.Y - centerPoint.Y) * scales.Y + centerPoint.Y + translate.Y),
            };
        }
        #endregion Public Static Methods

        #region Public operator methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Node Add(Node left, Vector2 right)
        {
            return left + right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Node Add(Node left, Node right)
        {
            return left + right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Node Subtract(Node left, Vector2 right)
        {
            return left - right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Node Subtract(Vector2 left, Node right)
        {
            return left - right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Node Subtract(Node left, Node right)
        {
            return left - right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Node Multiply(Node left, Vector2 right)
        {
            return left * right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Node Multiply(Node left, Single right)
        {
            return left * right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Node Multiply(Single left, Node right)
        {
            return left * right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Node Divide(Node left, Vector2 right)
        {
            return left / right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Node Divide(Node left, Single divisor)
        {
            return left / divisor;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Node Negate(Node value)
        {
            return -value;
        }
        #endregion Public operator methods
    }
}