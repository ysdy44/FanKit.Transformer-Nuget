using FanKit.Transformer.Controllers;
using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Curves
{
    public readonly struct ClosestPointer
    {
        public readonly Vector2 Point;
        public readonly bool PreviousIsSmooth;
        public readonly bool NextIsSmooth;
        public bool CurrentIsSmooth
        {
            get
            {
                return PreviousIsSmooth || NextIsSmooth;
            }
        }

        public readonly float Time; // Value
        readonly float inv;
        readonly Vector2 P;
        readonly Linear L;
        readonly Quadratic Q;
        readonly Cubic C;

        public readonly Node Previous;
        public readonly Node Current;
        public readonly Node Next;

        public readonly bool Contains;

        public ClosestPointer(Vector2 point)
        {
            Point = point;
            NextIsSmooth = false;
            PreviousIsSmooth = false;

            Time = 1f;
            inv = 0f;
            P = Vector2.Zero;
            L = default;
            Q = default;
            C = default;

            Previous = new Node(point);
            Current = new Node(point);
            Next = new Node(point);

            Contains = true;
        }

        public ClosestPointer(Vector2 point, Node previous, Node next, bool previousIsSmooth, bool nextIsSmooth, float strokeWidthSquared)
        {
            Point = point;

            PreviousIsSmooth = previousIsSmooth;
            NextIsSmooth = nextIsSmooth;

            if (NextIsSmooth)
            {
                if (PreviousIsSmooth)
                {
                    // 1. Root
                    C = new Cubic
                    {
                        C0 = previous.Point,
                        C1 = previous.RightControlPoint,
                        C2 = next.LeftControlPoint,
                        C3 = next.Point,
                    };

                    // 2. Closet
                    Time = C.FindClosest(point);
                    inv = 1f - Time;

                    // 3. Point
                    Q = C.Quadratic(Time, inv);
                    L = Q.Linear(Time, inv);
                    P = L.Lerp(Time, inv);

                    // 4. Result
                    Previous = new Node
                    {
                        LeftControlPoint = previous.LeftControlPoint,
                        Point = C.C0,
                        RightControlPoint = Q.Q0,
                    };
                    Current = new Node
                    {
                        LeftControlPoint = L.L0,
                        Point = P,
                        RightControlPoint = L.L1,
                    };
                    Next = new Node
                    {
                        LeftControlPoint = Q.Q2,
                        Point = C.C3,
                        RightControlPoint = next.RightControlPoint,
                    };
                }
                else
                {
                    // 1. Root
                    Q = new Quadratic
                    {
                        Q0 = previous.Point,
                        Q1 = next.LeftControlPoint,
                        Q2 = next.Point
                    };

                    // 2. Closet
                    Time = Q.FindClosest(point);
                    inv = 1f - Time;

                    // 3. Point
                    L = Q.Linear(Time, inv);
                    P = L.Lerp(Time, inv);

                    // 4. Result
                    C = Q.Cubic();
                    Previous = new Node(Q.Q0);
                    Current = new Node
                    {
                        LeftControlPoint = L.L0,
                        Point = P,
                        RightControlPoint = L.L1,
                    };
                    Next = new Node
                    {
                        LeftControlPoint = L.L1,
                        Point = Q.Q2,
                        RightControlPoint = next.RightControlPoint,
                    };
                }
            }
            else
            {
                if (PreviousIsSmooth)
                {
                    // 1. Root
                    Q = new Quadratic
                    {
                        Q0 = previous.Point,
                        Q1 = previous.RightControlPoint,
                        Q2 = next.Point,
                    };

                    // 2. Closet
                    Time = Q.FindClosest(point);
                    inv = 1f - Time;

                    // 3. Point
                    L = Q.Linear(Time, inv);
                    P = L.Lerp(Time, inv);

                    // 4. Result
                    C = Q.Cubic();
                    Previous = new Node
                    {
                        LeftControlPoint = previous.LeftControlPoint,
                        Point = Q.Q0,
                        RightControlPoint = L.L0,
                    };
                    Current = new Node
                    {
                        LeftControlPoint = L.L0,
                        Point = P,
                        RightControlPoint = L.L1,
                    };
                    Next = new Node(Q.Q2);
                }
                else
                {
                    // 1. Root
                    L = new Linear
                    {
                        L0 = previous.Point,
                        L1 = next.Point,
                    };

                    // 2. Closet
                    Time = L.Foot(point);
                    inv = 1 - Time;

                    // 3. Point
                    P = L.Lerp(Time, inv);

                    // 4. Result
                    C = L.Cubic();
                    Q = L.Quadratic();
                    Previous = new Node(L.L0);
                    Current = new Node(P);
                    Next = new Node(L.L1);
                }
            }

            Contains = Vector2.DistanceSquared(point, Current.Point) < strokeWidthSquared;
        }

        public ClosestPointer(Vector2 point, Node previous, Node next, float minLengthSquared = 144f) => this = new ClosestPointer(point,
            previous, next,
            true, true,
            minLengthSquared);

        public ClosestPointer(Vector2 point, Segment0 previous, Segment0 next, float minLengthSquared) => this = new ClosestPointer(point,
            previous.Point, next.Point,
            previous.IsSmooth, next.IsSmooth,
            minLengthSquared);

        public ClosestPointer(NodePointUnits unit, Vector2 point, Segment1 previous, Segment1 next, float minLengthSquared) => this = unit == NodePointUnits.Normal ? new ClosestPointer(point,
            previous.Point, next.Point,
            previous.IsSmooth, next.IsSmooth,
            minLengthSquared) : new ClosestPointer(point,
            previous.Actual, next.Actual,
            previous.IsSmooth, next.IsSmooth,
            minLengthSquared);

        public ClosestPointer(Vector2 point, Segment2 previous, Segment2 next, float minLengthSquared) => this = new ClosestPointer(point,
            previous.Map, next.Map,
            previous.IsSmooth, next.IsSmooth,
            minLengthSquared);

        public ClosestPointer(NodePointUnits unit, Vector2 point, Segment3 previous, Segment3 next, float minLengthSquared) => this = unit == NodePointUnits.Normal ? new ClosestPointer(point,
            previous.Map, next.Map,
            previous.IsSmooth, next.IsSmooth,
            minLengthSquared) : new ClosestPointer(point,
            previous.Actual, next.Actual,
            previous.IsSmooth, next.IsSmooth,
            minLengthSquared);

        public ClosestPointer(IReadOnlyList<Segment0> segments, bool isClosed, Vector2 point, float minLengthSquared = 144f)
        {
            int count = segments.Count;

            if (isClosed)
            {
                Segment0 last = segments[count - 1];
                Segment0 first = segments[0];

                this = new ClosestPointer(point, last.Point, first.Point, last.IsSmooth, first.IsSmooth, minLengthSquared);
                if (this.Contains)
                {
                    // Append
                    return;
                }
            }

            for (int i = 1; i < count; i++)
            {
                Segment0 previous = segments[i - 1];
                Segment0 next = segments[i];

                this = new ClosestPointer(point, previous.Point, next.Point, previous.IsSmooth, next.IsSmooth, minLengthSquared);
                if (this.Contains)
                {
                    // InsertAt
                    return;
                }
            }

            this = default;
        }

        public ClosestPointer(NodePointUnits unit, IReadOnlyList<Segment1> segments, bool isClosed, Vector2 point, float minLengthSquared = 144f)
        {
            int count = segments.Count;

            switch (unit)
            {
                case NodePointUnits.Normal:
                    if (isClosed)
                    {
                        Segment1 last = segments[count - 1];
                        Segment1 first = segments[0];

                        this = new ClosestPointer(point, last.Point, first.Point, last.IsSmooth, first.IsSmooth, minLengthSquared);
                        if (this.Contains)
                        {
                            // Append
                            return;
                        }
                    }

                    for (int i = 1; i < count; i++)
                    {
                        Segment1 previous = segments[i - 1];
                        Segment1 next = segments[i];

                        this = new ClosestPointer(point, previous.Point, next.Point, previous.IsSmooth, next.IsSmooth, minLengthSquared);
                        if (this.Contains)
                        {
                            // InsertAt
                            return;
                        }
                    }
                    break;
                case NodePointUnits.Actual:
                    if (isClosed)
                    {
                        Segment1 last = segments[count - 1];
                        Segment1 first = segments[0];

                        this = new ClosestPointer(point, last.Actual, first.Actual, last.IsSmooth, first.IsSmooth, minLengthSquared);
                        if (this.Contains)
                        {
                            // Append
                            return;
                        }
                    }

                    for (int i = 1; i < count; i++)
                    {
                        Segment1 previous = segments[i - 1];
                        Segment1 next = segments[i];

                        this = new ClosestPointer(point, previous.Actual, next.Actual, previous.IsSmooth, next.IsSmooth, minLengthSquared);
                        if (this.Contains)
                        {
                            // InsertAt
                            return;
                        }
                    }
                    break;
                default:
                    break;
            }

            this = default;
        }

        public ClosestPointer(IReadOnlyList<Segment2> segments, bool isClosed, Vector2 point, float minLengthSquared = 144f)
        {
            int count = segments.Count;

            if (isClosed)
            {
                Segment2 last = segments[count - 1];
                Segment2 first = segments[0];

                this = new ClosestPointer(point, last.Map, first.Map, last.IsSmooth, first.IsSmooth, minLengthSquared);
                if (this.Contains)
                {
                    // Append
                    return;
                }
            }

            for (int i = 1; i < count; i++)
            {
                Segment2 previous = segments[i - 1];
                Segment2 next = segments[i];

                this = new ClosestPointer(point, previous.Map, next.Map, previous.IsSmooth, next.IsSmooth, minLengthSquared);
                if (this.Contains)
                {
                    // InsertAt
                    return;
                }
            }

            this = default;
        }

        public ClosestPointer(NodePointUnits unit, IReadOnlyList<Segment3> segments, bool isClosed, Vector2 point, float minLengthSquared = 144f)
        {
            int count = segments.Count;

            switch (unit)
            {
                case NodePointUnits.Normal:
                    if (isClosed)
                    {
                        Segment3 last = segments[count - 1];
                        Segment3 first = segments[0];

                        this = new ClosestPointer(point, last.Map, first.Map, last.IsSmooth, first.IsSmooth, minLengthSquared);
                        if (this.Contains)
                        {
                            // Append
                            return;
                        }
                    }

                    for (int i = 1; i < count; i++)
                    {
                        Segment3 previous = segments[i - 1];
                        Segment3 next = segments[i];

                        this = new ClosestPointer(point, previous.Map, next.Map, previous.IsSmooth, next.IsSmooth, minLengthSquared);
                        if (this.Contains)
                        {
                            // InsertAt
                            return;
                        }
                    }
                    break;
                case NodePointUnits.Actual:
                    if (isClosed)
                    {
                        Segment3 last = segments[count - 1];
                        Segment3 first = segments[0];

                        this = new ClosestPointer(point, last.Actual, first.Actual, last.IsSmooth, first.IsSmooth, minLengthSquared);
                        if (this.Contains)
                        {
                            // Append
                            return;
                        }
                    }

                    for (int i = 1; i < count; i++)
                    {
                        Segment3 previous = segments[i - 1];
                        Segment3 next = segments[i];

                        this = new ClosestPointer(point, previous.Actual, next.Actual, previous.IsSmooth, next.IsSmooth, minLengthSquared);
                        if (this.Contains)
                        {
                            // InsertAt
                            return;
                        }
                    }
                    break;
                default:
                    break;
            }

            this = default;
        }

        internal SegmentInserter InsertAt(int index) => this.CurrentIsSmooth ? new SegmentInserter
        (
            mode: SegmentInserterMode.Smooth,
            previous: index - 1,
            current: index,
            next: index
        ) : new SegmentInserter
        (
            mode: SegmentInserterMode.Sharp,
            current: index
        );

        internal SegmentInserter Append(int count) => this.CurrentIsSmooth ? new SegmentInserter
        (
            mode: SegmentInserterMode.Smooth,
            previous: count - 1,
            current: count,
            next: 0
        ) : new SegmentInserter
        (
            mode: SegmentInserterMode.Sharp,
            current: count
        );
    }
}