using FanKit.Transformer.Controllers;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Polylines
{
    public readonly struct FootPointer
    {
        public readonly Vector2 Point;
        public readonly Vector2 LinePoint0;
        public readonly Vector2 LinePoint1;

        readonly float x;
        readonly float y;
        readonly float s;

        readonly float a;
        readonly float b;
        readonly float p;

        public readonly float Value;
        public readonly Vector2 Foot;

        readonly float n;
        readonly float m;
        readonly float v;

        public readonly bool Contains;

        public FootPointer(Vector2 point)
        {
            Point = point;
            LinePoint0 = point;
            LinePoint1 = point;

            x = 0;
            y = 0;
            s = 0;

            a = 0;
            b = 0;
            p = 0;

            Value = 1;
            Foot = point;

            n = 0;
            m = 0;
            v = 0;

            Contains = true;
        }

        public FootPointer(Vector2 point, Vector2 linePoint0, Vector2 linePoint1, bool contains)
        {
            Point = point;
            LinePoint0 = linePoint0;
            LinePoint1 = linePoint1;

            x = LinePoint0.X - LinePoint1.X;
            y = LinePoint0.Y - LinePoint1.Y;
            s = x * x + y * y;

            a = LinePoint0.X - Point.X;
            b = LinePoint0.Y - Point.Y;
            p = a * x + b * y;

            Value = p / s;
            Foot = new Vector2
            {
                X = LinePoint0.X - x * Value,
                Y = LinePoint0.Y - y * Value,
            };

            n = 0f;
            m = 0f;
            v = 0f;

            Contains = contains;
        }

        public FootPointer(Vector2 point, Vector2 linePoint0, Vector2 linePoint1, float strokeWidthSquared)
        {
            Point = point;
            LinePoint0 = linePoint0;
            LinePoint1 = linePoint1;

            x = LinePoint0.X - LinePoint1.X;
            y = LinePoint0.Y - LinePoint1.Y;
            s = x * x + y * y;

            a = LinePoint0.X - Point.X;
            b = LinePoint0.Y - Point.Y;
            p = a * x + b * y;

            Value = p / s;
            Foot = new Vector2
            {
                X = LinePoint0.X - x * Value,
                Y = LinePoint0.Y - y * Value,
            };

            n = Point.X - Foot.X;
            m = Point.Y - Foot.Y;
            v = n * n + m * m;

            if (v < strokeWidthSquared)
            {
                Contains = Value >= 0f && Value <= 1f;
            }
            else
            {
                Contains = false;
            }
        }

        public FootPointer(Vector2 point, Segment0 previous, Segment0 next, float minLengthSquared = 144f) => this = new FootPointer(point,
            previous.Point, next.Point,
            minLengthSquared);

        public FootPointer(NodePointUnits unit, Vector2 point, Segment1 previous, Segment1 next, float minLengthSquared = 144f) => this = unit == NodePointUnits.Normal ? new FootPointer(point,
            previous.Point, next.Point,
            minLengthSquared) : new FootPointer(point,
            previous.Actual, next.Actual,
            minLengthSquared);

        public FootPointer(Vector2 point, Segment2 previous, Segment2 next, float minLengthSquared = 144f) => this = new FootPointer(point,
            previous.Map, next.Map,
            minLengthSquared);

        public FootPointer(NodePointUnits unit, Vector2 point, Segment3 previous, Segment3 next, float minLengthSquared = 144f) => this = unit == NodePointUnits.Normal ? new FootPointer(point,
            previous.Map, next.Map,
            minLengthSquared) : new FootPointer(point,
            previous.Actual, next.Actual,
            minLengthSquared);

        public FootPointer(IReadOnlyList<Segment0> items, bool isClosed, Vector2 point, float minLengthSquared = 144f)
        {
            int count = items.Count;

            if (isClosed)
            {
                Segment0 last = items[count - 1];
                Segment0 first = items[0];

                this = new FootPointer(point, last.Point, first.Point, minLengthSquared);
                if (this.Contains)
                {
                    // Append
                    return;
                }
            }

            for (int i = 1; i < count; i++)
            {
                Segment0 previous = items[i - 1];
                Segment0 next = items[i];

                this = new FootPointer(point, previous.Point, next.Point, minLengthSquared);
                if (this.Contains)
                {
                    // InsertAt
                    return;
                }
            }

            this = default;
        }

        public FootPointer(NodePointUnits unit, IReadOnlyList<Segment1> items, bool isClosed, Vector2 point, float minLengthSquared = 144f)
        {
            int count = items.Count;

            switch (unit)
            {
                case NodePointUnits.Normal:
                    if (isClosed)
                    {
                        Segment1 last = items[count - 1];
                        Segment1 first = items[0];

                        this = new FootPointer(point, last.Point, first.Point, minLengthSquared);
                        if (this.Contains)
                        {
                            // Append
                            return;
                        }
                    }

                    for (int i = 1; i < count; i++)
                    {
                        Segment1 previous = items[i - 1];
                        Segment1 next = items[i];

                        this = new FootPointer(point, previous.Point, next.Point, minLengthSquared);
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
                        Segment1 last = items[count - 1];
                        Segment1 first = items[0];

                        this = new FootPointer(point, last.Actual, first.Actual, minLengthSquared);
                        if (this.Contains)
                        {
                            // Append
                            return;
                        }
                    }

                    for (int i = 1; i < count; i++)
                    {
                        Segment1 previous = items[i - 1];
                        Segment1 next = items[i];

                        this = new FootPointer(point, previous.Actual, next.Actual, minLengthSquared);
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

        public FootPointer(IReadOnlyList<Segment2> items, bool isClosed, Vector2 point, float minLengthSquared = 144f)
        {
            int count = items.Count;

            if (isClosed)
            {
                Segment2 last = items[count - 1];
                Segment2 first = items[0];

                this = new FootPointer(point, last.Map, first.Map, minLengthSquared);
                if (this.Contains)
                {
                    // Append
                    return;
                }
            }

            for (int i = 1; i < count; i++)
            {
                Segment2 previous = items[i - 1];
                Segment2 next = items[i];

                this = new FootPointer(point, previous.Map, next.Map, minLengthSquared);
                if (this.Contains)
                {
                    // InsertAt
                    return;
                }
            }

            this = default;
        }

        public FootPointer(NodePointUnits unit, IReadOnlyList<Segment3> items, bool isClosed, Vector2 point, float minLengthSquared = 144f)
        {
            int count = items.Count;

            switch (unit)
            {
                case NodePointUnits.Normal:
                    if (isClosed)
                    {
                        Segment3 last = items[count - 1];
                        Segment3 first = items[0];

                        this = new FootPointer(point, last.Map, first.Map, minLengthSquared);
                        if (this.Contains)
                        {
                            // Append
                            return;
                        }
                    }

                    for (int i = 1; i < count; i++)
                    {
                        Segment3 previous = items[i - 1];
                        Segment3 next = items[i];

                        this = new FootPointer(point, previous.Map, next.Map, minLengthSquared);
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
                        Segment3 last = items[count - 1];
                        Segment3 first = items[0];

                        this = new FootPointer(point, last.Actual, first.Actual, minLengthSquared);
                        if (this.Contains)
                        {
                            // Append
                            return;
                        }
                    }

                    for (int i = 1; i < count; i++)
                    {
                        Segment3 previous = items[i - 1];
                        Segment3 next = items[i];

                        this = new FootPointer(point, previous.Actual, next.Actual, minLengthSquared);
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
    }
}