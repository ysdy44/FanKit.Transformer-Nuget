using FanKit.Transformer.Curves;
using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Curves
{
    public struct SegmentIndexer
    {
        readonly static SegmentIndexer empty = new SegmentIndexer
        {
            Mode = SegmentMode.None,
            Index = -1,
        };

        public static SegmentIndexer Empty
        {
            get { return empty; }
        }

        public int Index;
        public SegmentMode Mode;

        public SegmentIndexer(IReadOnlyList<Node> items, Vector2 point, float minLengthSquared = 144f, float minControlLengthSquared = 100f)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Node item = items[i];
                {
                    if (item.RightControlPoint.ContainsNode(point, minControlLengthSquared))
                    {
                        this.Index = i;
                        this.Mode = SegmentMode.RightControlPoint;
                        return;
                    }

                    if (item.LeftControlPoint.ContainsNode(point, minControlLengthSquared))
                    {
                        this.Index = i;
                        this.Mode = SegmentMode.LeftControlPoint;
                        return;
                    }
                }
            }

            for (int i = 0; i < items.Count; i++)
            {
                Node item = items[i];
                if (item.Point.ContainsNode(point, minLengthSquared))
                {
                    this.Index = i;
                    this.Mode = SegmentMode.PointWithChecked;
                    return;
                }
            }

            this.Index = -1;
            this.Mode = SegmentMode.None;
        }

        public SegmentIndexer(IReadOnlyList<Segment0> items, Vector2 point, float minLengthSquared = 144f, float minControlLengthSquared = 100f)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Segment0 item = items[i];
                if (item.IsChecked && item.IsSmooth)
                {
                    if (item.Point.RightControlPoint.ContainsNode(point, minControlLengthSquared))
                    {
                        this.Index = i;
                        this.Mode = SegmentMode.RightControlPoint;
                        return;
                    }

                    if (item.Point.LeftControlPoint.ContainsNode(point, minControlLengthSquared))
                    {
                        this.Index = i;
                        this.Mode = SegmentMode.LeftControlPoint;
                        return;
                    }
                }
            }

            for (int i = 0; i < items.Count; i++)
            {
                Segment0 item = items[i];
                if (item.Point.Point.ContainsNode(point, minLengthSquared))
                {
                    this.Index = i;
                    if (item.IsChecked)
                    {
                        this.Mode = SegmentMode.PointWithChecked;
                    }
                    else
                    {
                        this.Mode = SegmentMode.PointWithoutChecked;
                    }
                    return;
                }
            }

            this.Index = -1;
            this.Mode = SegmentMode.None;
        }

        public SegmentIndexer(IReadOnlyList<Segment1> items, Vector2 point, float minLengthSquared = 144f, float minControlLengthSquared = 100f)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Segment1 item = items[i];
                if (item.IsChecked && item.IsSmooth)
                {
                    if (item.Actual.RightControlPoint.ContainsNode(point, minControlLengthSquared))
                    {
                        this.Index = i;
                        this.Mode = SegmentMode.RightControlPoint;
                        return;
                    }

                    if (item.Actual.LeftControlPoint.ContainsNode(point, minControlLengthSquared))
                    {
                        this.Index = i;
                        this.Mode = SegmentMode.LeftControlPoint;
                        return;
                    }
                }
            }

            for (int i = 0; i < items.Count; i++)
            {
                Segment1 item = items[i];
                if (item.Actual.Point.ContainsNode(point, minLengthSquared))
                {
                    this.Index = i;
                    if (item.IsChecked)
                    {
                        this.Mode = SegmentMode.PointWithChecked;
                    }
                    else
                    {
                        this.Mode = SegmentMode.PointWithoutChecked;
                    }
                    return;
                }
            }

            this.Index = -1;
            this.Mode = SegmentMode.None;
        }

        public SegmentIndexer(IReadOnlyList<Segment2> items, Vector2 point, float minLengthSquared = 144f, float minControlLengthSquared = 100f)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Segment2 item = items[i];
                if (item.IsChecked && item.IsSmooth)
                {
                    if (item.Map.RightControlPoint.ContainsNode(point, minControlLengthSquared))
                    {
                        this.Index = i;
                        this.Mode = SegmentMode.RightControlPoint;
                        return;
                    }

                    if (item.Map.LeftControlPoint.ContainsNode(point, minControlLengthSquared))
                    {
                        this.Index = i;
                        this.Mode = SegmentMode.LeftControlPoint;
                        return;
                    }
                }
            }

            for (int i = 0; i < items.Count; i++)
            {
                Segment2 item = items[i];
                if (item.Map.Point.ContainsNode(point, minLengthSquared))
                {
                    this.Index = i;
                    if (item.IsChecked)
                    {
                        this.Mode = SegmentMode.PointWithChecked;
                    }
                    else
                    {
                        this.Mode = SegmentMode.PointWithoutChecked;
                    }
                    return;
                }
            }

            this.Index = -1;
            this.Mode = SegmentMode.None;
        }

        public SegmentIndexer(IReadOnlyList<Segment3> items, Vector2 point, float minLengthSquared = 144f, float minControlLengthSquared = 100f)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Segment3 item = items[i];
                if (item.IsChecked && item.IsSmooth)
                {
                    if (item.Actual.RightControlPoint.ContainsNode(point, minControlLengthSquared))
                    {
                        this.Index = i;
                        this.Mode = SegmentMode.RightControlPoint;
                        return;
                    }

                    if (item.Actual.LeftControlPoint.ContainsNode(point, minControlLengthSquared))
                    {
                        this.Index = i;
                        this.Mode = SegmentMode.LeftControlPoint;
                        return;
                    }
                }
            }

            for (int i = 0; i < items.Count; i++)
            {
                Segment3 item = items[i];
                if (item.Actual.Point.ContainsNode(point, minLengthSquared))
                {
                    this.Index = i;
                    if (item.IsChecked)
                    {
                        this.Mode = SegmentMode.PointWithChecked;
                    }
                    else
                    {
                        this.Mode = SegmentMode.PointWithoutChecked;
                    }
                    return;
                }
            }

            this.Index = -1;
            this.Mode = SegmentMode.None;
        }
    }
}