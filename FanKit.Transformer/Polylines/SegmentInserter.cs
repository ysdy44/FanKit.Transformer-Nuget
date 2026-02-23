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

        public SegmentInserter(ref FootPointer closest, IReadOnlyList<Segment0> items, bool isClosed, Vector2 point, float minLengthSquared = 144f)
        {
            int count = items.Count;

            if (isClosed)
            {
                Segment0 last = items[count - 1];
                Segment0 first = items[0];

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
                Segment0 previous = items[i - 1];
                Segment0 next = items[i];

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

        public SegmentInserter(ref FootPointer closest, NodePointUnits unit, IReadOnlyList<Segment1> items, bool isClosed, Vector2 point, float minLengthSquared = 144f)
        {
            int count = items.Count;

            switch (unit)
            {
                case NodePointUnits.Normal:
                    if (isClosed)
                    {
                        Segment1 last = items[count - 1];
                        Segment1 first = items[0];

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
                        Segment1 previous = items[i - 1];
                        Segment1 next = items[i];

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
                        Segment1 last = items[count - 1];
                        Segment1 first = items[0];

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
                        Segment1 previous = items[i - 1];
                        Segment1 next = items[i];

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

        public SegmentInserter(ref FootPointer closest, IReadOnlyList<Segment2> items, bool isClosed, Vector2 point, float minLengthSquared = 144f)
        {
            int count = items.Count;

            if (isClosed)
            {
                Segment2 last = items[count - 1];
                Segment2 first = items[0];

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
                Segment2 previous = items[i - 1];
                Segment2 next = items[i];

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

        public SegmentInserter(ref FootPointer closest, NodePointUnits unit, IReadOnlyList<Segment3> items, bool isClosed, Vector2 point, float minLengthSquared = 144f)
        {
            int count = items.Count;

            switch (unit)
            {
                case NodePointUnits.Normal:
                    if (isClosed)
                    {
                        Segment3 last = items[count - 1];
                        Segment3 first = items[0];

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
                        Segment3 previous = items[i - 1];
                        Segment3 next = items[i];

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
                        Segment3 last = items[count - 1];
                        Segment3 first = items[0];

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
                        Segment3 previous = items[i - 1];
                        Segment3 next = items[i];

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