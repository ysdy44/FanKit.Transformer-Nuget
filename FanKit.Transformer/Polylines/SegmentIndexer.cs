using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Polylines
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

        public SegmentIndexer(IReadOnlyList<Vector2> items, Vector2 point, float minLengthSquared = 144f)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Vector2 item = items[i];
                if (item.ContainsNode(point, minLengthSquared))
                {
                    this.Index = i;
                    this.Mode = SegmentMode.PointWithChecked;
                    return;
                }
            }

            this.Index = -1;
            this.Mode = SegmentMode.None;
        }

        public SegmentIndexer(IReadOnlyList<Segment0> items, Vector2 point, float minLengthSquared = 144f)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Segment0 item = items[i];
                if (item.Point.ContainsNode(point, minLengthSquared))
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

        public SegmentIndexer(IReadOnlyList<Segment1> items, Vector2 point, float minLengthSquared = 144f)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Segment1 item = items[i];
                if (item.Actual.ContainsNode(point, minLengthSquared))
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

        public SegmentIndexer(IReadOnlyList<Segment2> items, Vector2 point, float minLengthSquared = 144f)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Segment2 item = items[i];
                if (item.Map.ContainsNode(point, minLengthSquared))
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

        public SegmentIndexer(IReadOnlyList<Segment3> items, Vector2 point, float minLengthSquared = 144f)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Segment3 item = items[i];
                if (item.Actual.ContainsNode(point, minLengthSquared))
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