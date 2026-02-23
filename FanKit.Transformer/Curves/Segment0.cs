using System.Numerics;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace FanKit.Transformer.Curves
{
    public struct Segment0
    {
        public bool IsChecked;
        public bool IsSmooth;

        public Node Starting;

        public Node Point;

        #region Constructors
        public Segment0(bool isChecked, Vector2 point)
        {
            this.IsChecked = isChecked;
            this.IsSmooth = true;

            this.Starting = default;
            this.Point = new Node(point);
        }

        public Segment0(bool isChecked, Node node)
        {
            this.IsChecked = isChecked;
            this.IsSmooth = true;

            this.Starting = default;
            this.Point = node;
        }

        private Segment0(Segment0 center, Segment0 control, bool atFirst)
        {
            this.IsChecked = true;
            this.IsSmooth = true;
            this.Starting = default;

            if (atFirst)
            {
                Vector2 point = (control.Point.Point - center.Point.Point) / 3f;

                this.Point = new Node
                {
                    Point = center.Point.Point,
                    LeftControlPoint = center.Point.Point - point,
                    RightControlPoint = center.Point.Point + point,
                };
            }
            else
            {
                Vector2 point = (center.Point.Point - control.Point.Point) / 3f;

                this.Point = new Node
                {
                    Point = center.Point.Point,
                    LeftControlPoint = center.Point.Point - point,
                    RightControlPoint = center.Point.Point + point,
                };
            }
        }

        private Segment0(Segment0 center, Segment0 left, Segment0 right)
        {
            this.IsChecked = true;
            this.IsSmooth = true;
            this.Starting = default;

            Vector2 point = (right.Point.Point - left.Point.Point) / 3f;

            this.Point = new Node
            {
                Point = center.Point.Point,
                LeftControlPoint = center.Point.Point - point,
                RightControlPoint = center.Point.Point + point,
            };
        }
        #endregion Constructors

        #region Public Instance Methods
        public Segment0 Smooth(Segment0 left, Segment0 right) => new Segment0(this, left, right);
        public Segment0 SmoothFirst(Segment0 second) => new Segment0(this, second, true);
        public Segment0 SmoothLast(Segment0 second) => new Segment0(this, second, false); // Second to last
        #endregion Public Instance Methods

        #region Public Static Methods
        public static Bounds Extend(Segment0 previous, Segment0 next)
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