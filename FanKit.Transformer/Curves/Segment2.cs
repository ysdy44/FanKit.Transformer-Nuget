using System.Numerics;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace FanKit.Transformer.Curves
{
    public struct Segment2
    {
        public bool IsChecked;
        public bool IsSmooth;

        public Node Starting;

        public Node Raw;
        public Node Map;

        #region Constructors
        public Segment2(bool isChecked, Vector2 pointRaw, Matrix3x2 homographyMatrix)
        {
            this.IsChecked = isChecked;
            this.IsSmooth = true;

            this.Starting = default;
            this.Raw = new Node(pointRaw);
            this.Map = new Node(Vector2.Transform(this.Raw.Point, homographyMatrix));
        }

        public Segment2(bool isChecked, Node nodeRaw, Matrix3x2 homographyMatrix)
        {
            this.IsChecked = isChecked;
            this.IsSmooth = true;

            this.Starting = default;
            this.Raw = nodeRaw;
            this.Map = Node.Transform(this.Raw, homographyMatrix);
        }

        private Segment2(Segment2 center, Segment2 control, bool atFirst)
        {
            this.IsChecked = true;
            this.IsSmooth = true;
            this.Starting = default;

            if (atFirst)
            {
                Vector2 raw = (control.Raw.Point - center.Raw.Point) / 3f;
                Vector2 map = (control.Map.Point - center.Map.Point) / 3f;

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
            }
            else
            {
                Vector2 raw = (center.Raw.Point - control.Raw.Point) / 3f;
                Vector2 map = (center.Map.Point - control.Map.Point) / 3f;

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
            }
        }

        private Segment2(Segment2 center, Segment2 left, Segment2 right)
        {
            this.IsChecked = true;
            this.IsSmooth = true;
            this.Starting = default;

            Vector2 raw = (right.Raw.Point - left.Raw.Point) / 3f;
            Vector2 map = (right.Map.Point - left.Map.Point) / 3f;

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
        }
        #endregion Constructors

        #region Public Instance Methods
        public Segment2 Smooth(Segment2 left, Segment2 right) => new Segment2(this, left, right);
        public Segment2 SmoothFirst(Segment2 second) => new Segment2(this, second, true);
        public Segment2 SmoothLast(Segment2 second) => new Segment2(this, second, false); // Second to last
        #endregion Public Instance Methods

        #region Public Static Methods
        public static Bounds Extend(Segment2 previous, Segment2 next)
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