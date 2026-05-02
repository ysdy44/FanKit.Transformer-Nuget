using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Polylines
{
    public struct SegmentIndexer
    {
        readonly static SegmentIndexer empty = new SegmentIndexer
        {
            Mode = SegmentIndexerMode.None,
            Index = -1,
        };

        public static SegmentIndexer Empty
        {
            get { return empty; }
        }

        public int Index;
        public SegmentIndexerMode Mode;

        public SegmentIndexer(IReadOnlyList<Vector2> segments, Vector2 point, float minLengthSquared = 144f)
        {
            for (int i = 0; i < segments.Count; i++)
            {
                Vector2 item = segments[i];
                if (item.ContainsNode(point, minLengthSquared))
                {
                    this.Index = i;
                    this.Mode = SegmentIndexerMode.PointWithChecked;
                    return;
                }
            }

            this.Index = -1;
            this.Mode = SegmentIndexerMode.None;
        }

        public SegmentIndexer(IReadOnlyList<Segment0> segments, Vector2 point, float minLengthSquared = 144f)
        {
            for (int i = 0; i < segments.Count; i++)
            {
                Segment0 item = segments[i];
                if (item.Point.ContainsNode(point, minLengthSquared))
                {
                    this.Index = i;
                    if (item.IsChecked)
                    {
                        this.Mode = SegmentIndexerMode.PointWithChecked;
                    }
                    else
                    {
                        this.Mode = SegmentIndexerMode.PointWithoutChecked;
                    }
                    return;
                }
            }

            this.Index = -1;
            this.Mode = SegmentIndexerMode.None;
        }

        public SegmentIndexer(IReadOnlyList<Segment1> segments, Vector2 point, float minLengthSquared = 144f)
        {
            for (int i = 0; i < segments.Count; i++)
            {
                Segment1 item = segments[i];
                if (item.Actual.ContainsNode(point, minLengthSquared))
                {
                    this.Index = i;
                    if (item.IsChecked)
                    {
                        this.Mode = SegmentIndexerMode.PointWithChecked;
                    }
                    else
                    {
                        this.Mode = SegmentIndexerMode.PointWithoutChecked;
                    }
                    return;
                }
            }

            this.Index = -1;
            this.Mode = SegmentIndexerMode.None;
        }

        public SegmentIndexer(IReadOnlyList<Segment2> segments, Vector2 point, float minLengthSquared = 144f)
        {
            for (int i = 0; i < segments.Count; i++)
            {
                Segment2 item = segments[i];
                if (item.Map.ContainsNode(point, minLengthSquared))
                {
                    this.Index = i;
                    if (item.IsChecked)
                    {
                        this.Mode = SegmentIndexerMode.PointWithChecked;
                    }
                    else
                    {
                        this.Mode = SegmentIndexerMode.PointWithoutChecked;
                    }
                    return;
                }
            }

            this.Index = -1;
            this.Mode = SegmentIndexerMode.None;
        }

        public SegmentIndexer(IReadOnlyList<Segment3> segments, Vector2 point, float minLengthSquared = 144f)
        {
            for (int i = 0; i < segments.Count; i++)
            {
                Segment3 item = segments[i];
                if (item.Actual.ContainsNode(point, minLengthSquared))
                {
                    this.Index = i;
                    if (item.IsChecked)
                    {
                        this.Mode = SegmentIndexerMode.PointWithChecked;
                    }
                    else
                    {
                        this.Mode = SegmentIndexerMode.PointWithoutChecked;
                    }
                    return;
                }
            }

            this.Index = -1;
            this.Mode = SegmentIndexerMode.None;
        }
    }
}