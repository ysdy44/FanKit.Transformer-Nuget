using FanKit.Transformer.Controllers;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Curves
{
    public readonly struct SegmentInserter
    {
        readonly static SegmentInserter empty = new SegmentInserter(SegmentInserterMode.None, -1);

        public static SegmentInserter Empty
        {
            get { return empty; }
        }

        public readonly SegmentInserterMode Mode;
        public readonly int Previous;
        public readonly int Current;
        public readonly int Next;

        public SegmentInserter(SegmentInserterMode mode, int current) : this()
        {
            Mode = mode;
            Previous = -1;
            Current = current;
            Next = -1;
        }

        public SegmentInserter(SegmentInserterMode mode, int previous, int current, int next)
        {
            Mode = mode;
            Previous = previous;
            Current = current;
            Next = next;
        }

        public SegmentInserter(ref ClosestPointer closest, IReadOnlyList<Segment0> items, bool isClosed, Vector2 point, float minLengthSquared = 144f)
        {
            int count = items.Count;

            if (isClosed)
            {
                Segment0 last = items[count - 1];
                Segment0 first = items[0];

                closest = new ClosestPointer(point, last.Point, first.Point, last.IsSmooth, first.IsSmooth, minLengthSquared);
                if (closest.Contains)
                {
                    // Append
                    this = closest.Append(count);
                    return;
                }
            }

            for (int i = 1; i < count; i++)
            {
                Segment0 previous = items[i - 1];
                Segment0 next = items[i];

                closest = new ClosestPointer(point, previous.Point, next.Point, previous.IsSmooth, next.IsSmooth, minLengthSquared);
                if (closest.Contains)
                {
                    // InsertAt
                    this = closest.InsertAt(i);
                    return;
                }
            }

            this = empty;
        }

        public SegmentInserter(ref ClosestPointer closest, NodePointUnits unit, IReadOnlyList<Segment1> items, bool isClosed, Vector2 point, float minLengthSquared = 144f)
        {
            int count = items.Count;

            switch (unit)
            {
                case NodePointUnits.Normal:
                    if (isClosed)
                    {
                        Segment1 last = items[count - 1];
                        Segment1 first = items[0];

                        closest = new ClosestPointer(point, last.Point, first.Point, last.IsSmooth, first.IsSmooth, minLengthSquared);
                        if (closest.Contains)
                        {
                            // Append
                            this = closest.Append(count);
                            return;
                        }
                    }

                    for (int i = 1; i < count; i++)
                    {
                        Segment1 previous = items[i - 1];
                        Segment1 next = items[i];

                        closest = new ClosestPointer(point, previous.Point, next.Point, previous.IsSmooth, next.IsSmooth, minLengthSquared);
                        if (closest.Contains)
                        {
                            // InsertAt
                            this = closest.InsertAt(i);
                            return;
                        }
                    }
                    break;
                case NodePointUnits.Actual:
                    if (isClosed)
                    {
                        Segment1 last = items[count - 1];
                        Segment1 first = items[0];

                        closest = new ClosestPointer(point, last.Actual, first.Actual, last.IsSmooth, first.IsSmooth, minLengthSquared);
                        if (closest.Contains)
                        {
                            // Append
                            this = closest.Append(count);
                            return;
                        }
                    }

                    for (int i = 1; i < count; i++)
                    {
                        Segment1 previous = items[i - 1];
                        Segment1 next = items[i];

                        closest = new ClosestPointer(point, previous.Actual, next.Actual, previous.IsSmooth, next.IsSmooth, minLengthSquared);
                        if (closest.Contains)
                        {
                            // InsertAt
                            this = closest.InsertAt(i);
                            return;
                        }
                    }
                    break;
                default:
                    break;
            }

            this = empty;
        }

        public SegmentInserter(ref ClosestPointer closest, IReadOnlyList<Segment2> items, bool isClosed, Vector2 point, float minLengthSquared = 144f)
        {
            int count = items.Count;

            if (isClosed)
            {
                Segment2 last = items[count - 1];
                Segment2 first = items[0];

                closest = new ClosestPointer(point, last.Map, first.Map, last.IsSmooth, first.IsSmooth, minLengthSquared);
                if (closest.Contains)
                {
                    // Append
                    this = closest.Append(count);
                    return;
                }
            }

            for (int i = 1; i < count; i++)
            {
                Segment2 previous = items[i - 1];
                Segment2 next = items[i];

                closest = new ClosestPointer(point, previous.Map, next.Map, previous.IsSmooth, next.IsSmooth, minLengthSquared);
                if (closest.Contains)
                {
                    // InsertAt
                    this = closest.InsertAt(i);
                    return;
                }
            }

            this = empty;
        }

        public SegmentInserter(ref ClosestPointer closest, NodePointUnits unit, IReadOnlyList<Segment3> items, bool isClosed, Vector2 point, float minLengthSquared = 144f)
        {
            int count = items.Count;

            switch (unit)
            {
                case NodePointUnits.Normal:
                    if (isClosed)
                    {
                        Segment3 last = items[count - 1];
                        Segment3 first = items[0];

                        closest = new ClosestPointer(point, last.Map, first.Map, last.IsSmooth, first.IsSmooth, minLengthSquared);
                        if (closest.Contains)
                        {
                            // Append
                            this = closest.Append(count);
                            return;
                        }
                    }

                    for (int i = 1; i < count; i++)
                    {
                        Segment3 previous = items[i - 1];
                        Segment3 next = items[i];

                        closest = new ClosestPointer(point, previous.Map, next.Map, previous.IsSmooth, next.IsSmooth, minLengthSquared);
                        if (closest.Contains)
                        {
                            // InsertAt
                            this = closest.InsertAt(i);
                            return;
                        }
                    }
                    break;
                case NodePointUnits.Actual:
                    if (isClosed)
                    {
                        Segment3 last = items[count - 1];
                        Segment3 first = items[0];

                        closest = new ClosestPointer(point, last.Actual, first.Actual, last.IsSmooth, first.IsSmooth, minLengthSquared);
                        if (closest.Contains)
                        {
                            // Append
                            this = closest.Append(count);
                            return;
                        }
                    }

                    for (int i = 1; i < count; i++)
                    {
                        Segment3 previous = items[i - 1];
                        Segment3 next = items[i];

                        closest = new ClosestPointer(point, previous.Actual, next.Actual, previous.IsSmooth, next.IsSmooth, minLengthSquared);
                        if (closest.Contains)
                        {
                            // InsertAt
                            this = closest.InsertAt(i);
                            return;
                        }
                    }
                    break;
                default:
                    break;
            }

            this = empty;
        }
    }
}