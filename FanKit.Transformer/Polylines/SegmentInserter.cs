using FanKit.Transformer.Controllers;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Polylines
{
    public readonly struct SegmentInserter
    {
        readonly static SegmentInserter empty = new SegmentInserter(-1);

        public static SegmentInserter Empty
        {
            get { return empty; }
        }

        public readonly int Index;
        public bool Contains => Index >= 0;

        public SegmentInserter(int index)
        {
            Index = index;
        }

        public SegmentInserter(ref FootPointer closest, IReadOnlyList<Segment0> segments, bool isClosed, Vector2 point, float minLengthSquared = 144f)
        {
            int count = segments.Count;

            if (isClosed)
            {
                Segment0 last = segments[count - 1];
                Segment0 first = segments[0];

                closest = new FootPointer(point, last.Point, first.Point, minLengthSquared);
                if (closest.Contains)
                {
                    // Append
                    Index = count;
                    return;
                }
            }

            for (int i = 1; i < count; i++)
            {
                Segment0 previous = segments[i - 1];
                Segment0 next = segments[i];

                closest = new FootPointer(point, previous.Point, next.Point, minLengthSquared);
                if (closest.Contains)
                {
                    // InsertAt
                    Index = i;
                    return;
                }
            }

            Index = -1;
        }

        public SegmentInserter(ref FootPointer closest, NodePointUnits unit, IReadOnlyList<Segment1> segments, bool isClosed, Vector2 point, float minLengthSquared = 144f)
        {
            int count = segments.Count;

            switch (unit)
            {
                case NodePointUnits.Normal:
                    if (isClosed)
                    {
                        Segment1 last = segments[count - 1];
                        Segment1 first = segments[0];

                        closest = new FootPointer(point, last.Point, first.Point, minLengthSquared);
                        if (closest.Contains)
                        {
                            // Append
                            Index = count;
                            return;
                        }
                    }

                    for (int i = 1; i < count; i++)
                    {
                        Segment1 previous = segments[i - 1];
                        Segment1 next = segments[i];

                        closest = new FootPointer(point, previous.Point, next.Point, minLengthSquared);
                        if (closest.Contains)
                        {
                            // InsertAt
                            Index = i;
                            return;
                        }
                    }
                    break;
                case NodePointUnits.Actual:
                    if (isClosed)
                    {
                        Segment1 last = segments[count - 1];
                        Segment1 first = segments[0];

                        closest = new FootPointer(point, last.Actual, first.Actual, minLengthSquared);
                        if (closest.Contains)
                        {
                            // Append
                            Index = count;
                            return;
                        }
                    }

                    for (int i = 1; i < count; i++)
                    {
                        Segment1 previous = segments[i - 1];
                        Segment1 next = segments[i];

                        closest = new FootPointer(point, previous.Actual, next.Actual, minLengthSquared);
                        if (closest.Contains)
                        {
                            // InsertAt
                            Index = i;
                            return;
                        }
                    }
                    break;
                default:
                    break;
            }

            Index = -1;
        }

        public SegmentInserter(ref FootPointer closest, IReadOnlyList<Segment2> segments, bool isClosed, Vector2 point, float minLengthSquared = 144f)
        {
            int count = segments.Count;

            if (isClosed)
            {
                Segment2 last = segments[count - 1];
                Segment2 first = segments[0];

                closest = new FootPointer(point, last.Map, first.Map, minLengthSquared);
                if (closest.Contains)
                {
                    // Append
                    Index = count;
                    return;
                }
            }

            for (int i = 1; i < count; i++)
            {
                Segment2 previous = segments[i - 1];
                Segment2 next = segments[i];

                closest = new FootPointer(point, previous.Map, next.Map, minLengthSquared);
                if (closest.Contains)
                {
                    // InsertAt
                    Index = i;
                    return;
                }
            }

            Index = -1;
        }

        public SegmentInserter(ref FootPointer closest, NodePointUnits unit, IReadOnlyList<Segment3> segments, bool isClosed, Vector2 point, float minLengthSquared = 144f)
        {
            int count = segments.Count;

            switch (unit)
            {
                case NodePointUnits.Normal:
                    if (isClosed)
                    {
                        Segment3 last = segments[count - 1];
                        Segment3 first = segments[0];

                        closest = new FootPointer(point, last.Map, first.Map, minLengthSquared);
                        if (closest.Contains)
                        {
                            // Append
                            Index = count;
                            return;
                        }
                    }

                    for (int i = 1; i < count; i++)
                    {
                        Segment3 previous = segments[i - 1];
                        Segment3 next = segments[i];

                        closest = new FootPointer(point, previous.Map, next.Map, minLengthSquared);
                        if (closest.Contains)
                        {
                            // InsertAt
                            Index = i;
                            return;
                        }
                    }
                    break;
                case NodePointUnits.Actual:
                    if (isClosed)
                    {
                        Segment3 last = segments[count - 1];
                        Segment3 first = segments[0];

                        closest = new FootPointer(point, last.Actual, first.Actual, minLengthSquared);
                        if (closest.Contains)
                        {
                            // Append
                            Index = count;
                            return;
                        }
                    }

                    for (int i = 1; i < count; i++)
                    {
                        Segment3 previous = segments[i - 1];
                        Segment3 next = segments[i];

                        closest = new FootPointer(point, previous.Actual, next.Actual, minLengthSquared);
                        if (closest.Contains)
                        {
                            // InsertAt
                            Index = i;
                            return;
                        }
                    }
                    break;
                default:
                    break;
            }

            Index = -1;
        }
    }
}