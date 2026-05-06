using System.Numerics;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace FanKit.Transformer.Curves
{
    public struct Segment1
    {
        public bool IsChecked;
        public bool IsSmooth;

        public Node Starting;

        public Node Point;
        public Node Actual;

        #region Constructors
        public Segment1(bool isChecked, Vector2 point, ICanvasMatrix canvasMatrix)
        {
            this.IsChecked = isChecked;
            this.IsSmooth = true;

            this.Starting = default;
            this.Point = new Node(point);
            this.Actual = new Node(canvasMatrix.Transform(this.Point.Point));
        }

        public Segment1(bool isChecked, Node node, ICanvasMatrix canvasMatrix)
        {
            this.IsChecked = isChecked;
            this.IsSmooth = true;

            this.Starting = default;
            this.Point = node;
            this.Actual = canvasMatrix.Transform(this.Point);
        }

        private Segment1(Segment1 center, Segment1 control, bool atFirst)
        {
            this.IsChecked = true;
            this.IsSmooth = true;
            this.Starting = default;

            if (atFirst)
            {
                Vector2 point = (control.Point.Point - center.Point.Point) / 3f;
                Vector2 actual = (control.Actual.Point - center.Actual.Point) / 3f;

                this.Point = new Node
                {
                    Point = center.Point.Point,
                    LeftControlPoint = center.Point.Point - point,
                    RightControlPoint = center.Point.Point + point,
                };
                this.Actual = new Node
                {
                    Point = center.Actual.Point,
                    LeftControlPoint = center.Actual.Point - actual,
                    RightControlPoint = center.Actual.Point + actual,
                };
            }
            else
            {
                Vector2 point = (center.Point.Point - control.Point.Point) / 3f;
                Vector2 actual = (center.Actual.Point - control.Actual.Point) / 3f;

                this.Point = new Node
                {
                    Point = center.Point.Point,
                    LeftControlPoint = center.Point.Point - point,
                    RightControlPoint = center.Point.Point + point,
                };
                this.Actual = new Node
                {
                    Point = center.Actual.Point,
                    LeftControlPoint = center.Actual.Point - actual,
                    RightControlPoint = center.Actual.Point + actual,
                };
            }
        }

        private Segment1(Segment1 center, Segment1 left, Segment1 right)
        {
            this.IsChecked = true;
            this.IsSmooth = true;
            this.Starting = default;

            Vector2 point = (right.Point.Point - left.Point.Point) / 3f;
            Vector2 actual = (right.Actual.Point - left.Actual.Point) / 3f;

            this.Point = new Node
            {
                Point = center.Point.Point,
                LeftControlPoint = center.Point.Point - point,
                RightControlPoint = center.Point.Point + point,
            };
            this.Actual = new Node
            {
                Point = center.Actual.Point,
                LeftControlPoint = center.Actual.Point - actual,
                RightControlPoint = center.Actual.Point + actual,
            };
        }
        #endregion Constructors

        #region Public Instance Methods
        public Segment1 Smooth(Segment1 left, Segment1 right) => new Segment1(this, left, right);
        public Segment1 SmoothFirst(Segment1 second) => new Segment1(this, second, true);
        public Segment1 SmoothLast(Segment1 second) => new Segment1(this, second, false); // Second to last
        #endregion Public Instance Methods

        #region Public Static Methods
        public static Bounds Extend(Segment1 previous, Segment1 next)
        {
            if (next.IsSmooth)
            {
                if (previous.IsSmooth)
                    return new Bounds(previous.Point.Point,
                        previous.Point.RightControlPoint,
                        next.Point.LeftControlPoint,
                        next.Point.Point);
                else
                    return new Bounds(previous.Point.Point,
                        next.Point.LeftControlPoint,
                        next.Point.Point);
            }
            else
            {
                if (previous.IsSmooth)
                    return new Bounds(previous.Point.Point,
                        previous.Point.RightControlPoint,
                        next.Point.Point);
                else
                    return new Bounds(previous.Point.Point,
                        next.Point.Point);
            }
        }
        #endregion Public Static Methods
    }
}