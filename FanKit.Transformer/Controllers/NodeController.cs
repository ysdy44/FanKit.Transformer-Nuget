using System.Numerics;

namespace FanKit.Transformer.Controllers
{
    public readonly struct NodeController
    {
        internal readonly Vector2 p0;

        internal readonly Vector2 cp0;

        internal readonly NodeControl c;

        public NodeController(Node node, bool isLeft, SelfControlPointMode mode1 = SelfControlPointMode.None, EachControlPointLengthMode mode2 = EachControlPointLengthMode.Equal)
        {
            p0 = node.Point;

            if (isLeft)
            {
                cp0 = node.RightControlPoint;
            }
            else
            {
                cp0 = node.LeftControlPoint;
            }

            c = new NodeControl(node.Point, node.LeftControlPoint, node.RightControlPoint, isLeft, mode1, mode2);
        }

        public Node ToNode(Vector2 point, bool disconnected)
        {
            switch (this.c.m)
            {
                case ControlPointMode.LeftEqual:
                    return new Node
                    {
                        Point = this.p0,
                        LeftControlPoint = point,
                        RightControlPoint = disconnected ? this.cp0 : new Vector2(
                            this.p0.X + this.p0.X - point.X,
                            this.p0.Y + this.p0.Y - point.Y)
                    };
                case ControlPointMode.RightEqual:
                    return new Node
                    {
                        Point = this.p0,
                        RightControlPoint = point,
                        LeftControlPoint = disconnected ? this.cp0 : new Vector2(
                            this.p0.X + this.p0.X - point.X,
                            this.p0.Y + this.p0.Y - point.Y)
                    };
                case ControlPointMode.LeftRatio:
                    if (disconnected)
                        return new Node
                        {
                            Point = this.p0,
                            LeftControlPoint = point,
                            RightControlPoint = this.cp0
                        };
                    else
                    {
                        Vector2 p = point - this.p0;

                        float s = this.c.b / p.Length();
                        Vector2 v = s * p;

                        return new Node
                        {
                            Point = this.p0,
                            LeftControlPoint = point,
                            RightControlPoint = this.p0 - v
                        };
                    }
                case ControlPointMode.RightRatio:
                    if (disconnected)
                        return new Node
                        {
                            Point = this.p0,
                            RightControlPoint = point,
                            LeftControlPoint = this.cp0
                        };
                    else
                    {
                        Vector2 p = point - this.p0;

                        float s = this.c.b / p.Length();
                        Vector2 v = s * p;

                        return new Node
                        {
                            Point = this.p0,
                            RightControlPoint = point,
                            LeftControlPoint = this.p0 - v
                        };
                    }
                case ControlPointMode.LeftAngleEqual:
                    {
                        Vector2 p = point - this.p0;
                        float l = p.X * this.c.x.X + p.Y * this.c.x.Y;

                        float s = l / this.c.b;
                        Vector2 v = this.c.x * s;

                        return new Node
                        {
                            Point = this.p0,
                            LeftControlPoint = this.p0 + v,
                            RightControlPoint = disconnected ? this.cp0 : this.p0 - v
                        };
                    }
                case ControlPointMode.RightAngleEqual:
                    {
                        Vector2 p = point - this.p0;
                        float l = p.X * this.c.x.X + p.Y * this.c.x.Y;

                        float s = l / this.c.b;
                        Vector2 v = this.c.x * s;

                        return new Node
                        {
                            Point = this.p0,
                            RightControlPoint = this.p0 + v,
                            LeftControlPoint = disconnected ? this.cp0 : this.p0 - v
                        };
                    }
                case ControlPointMode.LeftAngleRatio:
                    {
                        Vector2 p = point - this.p0;
                        float l1 = p.X * this.c.x.X + p.Y * this.c.x.Y;

                        float s1 = l1 / this.c.a;
                        Vector2 v1 = s1 * this.c.x;

                        if (disconnected)
                            return new Node
                            {
                                Point = this.p0,
                                LeftControlPoint = this.p0 + v1,
                                RightControlPoint = this.cp0
                            };
                        else
                        {
                            float l2 = v1.Length();

                            float s2 = this.c.b / l2;
                            Vector2 v2 = s2 * v1;

                            return new Node
                            {
                                Point = this.p0,
                                LeftControlPoint = this.p0 + v1,
                                RightControlPoint = this.p0 - v2
                            };
                        }
                    }
                case ControlPointMode.RightAngleRatio:
                    {
                        Vector2 p = point - this.p0;
                        float l1 = p.X * this.c.x.X + p.Y * this.c.x.Y;

                        float s1 = l1 / this.c.a;
                        Vector2 v1 = s1 * this.c.x;

                        if (disconnected)
                            return new Node
                            {
                                Point = this.p0,
                                RightControlPoint = this.p0 + v1,
                                LeftControlPoint = this.cp0
                            };
                        else
                        {
                            float l2 = v1.Length();

                            float s2 = this.c.b / l2;
                            Vector2 v2 = s2 * v1;

                            return new Node
                            {
                                Point = this.p0,
                                RightControlPoint = this.p0 + v1,
                                LeftControlPoint = this.p0 - v2
                            };
                        }
                    }
                case ControlPointMode.LeftLength:
                    {
                        Vector2 p = point - this.p0;
                        float l = p.Length();

                        float s = this.c.a / l;
                        Vector2 b = s * p;

                        return new Node
                        {
                            Point = this.p0,
                            LeftControlPoint = this.p0 + b,
                            RightControlPoint = disconnected ? this.cp0 : this.p0 - b
                        };
                    }
                case ControlPointMode.RightLength:
                    {
                        Vector2 p = point - this.p0;
                        float l = p.Length();

                        float s = this.c.a / l;
                        Vector2 v = s * p;

                        return new Node
                        {
                            Point = this.p0,
                            RightControlPoint = this.p0 + v,
                            LeftControlPoint = disconnected ? this.cp0 : this.p0 - v
                        };
                    }
                default:
                    return new Node
                    {
                        Point = this.p0,
                        LeftControlPoint = point,
                        RightControlPoint = this.cp0
                    };
            }
        }
    }
}