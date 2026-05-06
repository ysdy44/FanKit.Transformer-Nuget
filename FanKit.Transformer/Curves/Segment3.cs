using System.Numerics;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace FanKit.Transformer.Curves
{
    public struct Segment3
    {
        public bool IsChecked;
        public bool IsSmooth;

        public Node Starting;

        public Node Raw;
        public Node Map;
        public Node Actual;

        #region Constructors
        public Segment3(bool isChecked, Vector2 pointRaw, Matrix3x2 homographyMatrix, ICanvasMatrix canvasMatrix)
        {
            this.IsChecked = isChecked;
            this.IsSmooth = true;

            this.Starting = default;
            this.Raw = new Node(pointRaw);
            this.Map = new Node(Vector2.Transform(this.Raw.Point, homographyMatrix));
            this.Actual = new Node(canvasMatrix.Transform(this.Map.Point));
        }

        public Segment3(bool isChecked, Node nodeRaw, Matrix3x2 homographyMatrix, ICanvasMatrix canvasMatrix)
        {
            this.IsChecked = isChecked;
            this.IsSmooth = true;

            this.Starting = default;
            this.Raw = nodeRaw;
            this.Map = Node.Transform(this.Raw, homographyMatrix);
            this.Actual = canvasMatrix.Transform(this.Map);
        }

        private Segment3(Segment3 center, Segment3 control, bool atFirst)
        {
            this.IsChecked = true;
            this.IsSmooth = true;
            this.Starting = default;

            if (atFirst)
            {
                Vector2 raw = (control.Raw.Point - center.Raw.Point) / 3f;
                Vector2 map = (control.Map.Point - center.Map.Point) / 3f;
                Vector2 actual = (control.Actual.Point - center.Actual.Point) / 3f;

                this.Raw = new Node
                {
                    Point = center.Raw.Point,
                    LeftControlPoint = center.Raw.Point - raw,
                    RightControlPoint = center.Raw.Point + raw,
                };
                this.Map = new Node
                {
                    Point = center.Map.Point,
                    LeftControlPoint = center.Map.Point - map,
                    RightControlPoint = center.Map.Point + map,
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
                Vector2 raw = (center.Raw.Point - control.Raw.Point) / 3f;
                Vector2 map = (center.Map.Point - control.Map.Point) / 3f;
                Vector2 actual = (center.Actual.Point - control.Actual.Point) / 3f;

                this.Raw = new Node
                {
                    Point = center.Raw.Point,
                    LeftControlPoint = center.Raw.Point - raw,
                    RightControlPoint = center.Raw.Point + raw,
                };
                this.Map = new Node
                {
                    Point = center.Map.Point,
                    LeftControlPoint = center.Map.Point - map,
                    RightControlPoint = center.Map.Point + map,
                };
                this.Actual = new Node
                {
                    Point = center.Actual.Point,
                    LeftControlPoint = center.Actual.Point - actual,
                    RightControlPoint = center.Actual.Point + actual,
                };
            }
        }

        private Segment3(Segment3 center, Segment3 left, Segment3 right)
        {
            this.IsChecked = true;
            this.IsSmooth = true;
            this.Starting = default;

            Vector2 raw = (right.Raw.Point - left.Raw.Point) / 3f;
            Vector2 map = (right.Map.Point - left.Map.Point) / 3f;
            Vector2 actual = (right.Actual.Point - left.Actual.Point) / 3f;

            this.Raw = new Node
            {
                Point = center.Raw.Point,
                LeftControlPoint = center.Raw.Point - raw,
                RightControlPoint = center.Raw.Point + raw,
            };
            this.Map = new Node
            {
                Point = center.Map.Point,
                LeftControlPoint = center.Map.Point - map,
                RightControlPoint = center.Map.Point + map,
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
        public Segment3 Smooth(Segment3 left, Segment3 right) => new Segment3(this, left, right);
        public Segment3 SmoothFirst(Segment3 second) => new Segment3(this, second, true);
        public Segment3 SmoothLast(Segment3 second) => new Segment3(this, second, false); // Second to last
        #endregion Public Instance Methods

        #region Public Static Methods
        public static Bounds Extend(Segment3 previous, Segment3 next)
        {
            if (next.IsSmooth)
            {
                if (previous.IsSmooth)
                    return new Bounds(previous.Raw.Point,
                        previous.Raw.RightControlPoint,
                        next.Raw.LeftControlPoint,
                        next.Raw.Point);
                else
                    return new Bounds(previous.Raw.Point,
                        next.Raw.LeftControlPoint,
                        next.Raw.Point);
            }
            else
            {
                if (previous.IsSmooth)
                    return new Bounds(previous.Raw.Point,
                        previous.Raw.RightControlPoint,
                        next.Raw.Point);
                else
                    return new Bounds(previous.Raw.Point,
                        next.Raw.Point);
            }
        }
        #endregion Public Static Methods
    }
}