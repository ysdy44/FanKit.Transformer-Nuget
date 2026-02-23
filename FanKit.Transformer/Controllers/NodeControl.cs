using System.Numerics;

namespace FanKit.Transformer.Controllers
{
    internal readonly struct NodeControl
    {
        public readonly ControlPointMode m;

        public readonly Vector2 x;
        public readonly Vector2 y;
        public readonly Vector2 z;

        public readonly float a;
        public readonly float b;

        public NodeControl(Vector2 p, Vector2 cp1, Vector2 cp2, bool isl, SelfControlPointMode m1, EachControlPointLengthMode m2) : this()
        {
            switch (m1)
            {
                case SelfControlPointMode.None:
                    switch (m2)
                    {
                        case EachControlPointLengthMode.Equal:
                            if (isl)
                                m = ControlPointMode.LeftEqual;
                            else
                                m = ControlPointMode.RightEqual;
                            break;
                        case EachControlPointLengthMode.Ratio:
                            if (isl)
                            {
                                m = ControlPointMode.LeftRatio;
                                z = cp2 - p;
                            }
                            else
                            {
                                m = ControlPointMode.RightRatio;
                                z = cp1 - p;
                            }

                            b = z.Length();
                            break;
                        default:
                            goto case EachControlPointLengthMode.Equal;
                    }
                    break;
                case SelfControlPointMode.Angle:
                    switch (m2)
                    {
                        case EachControlPointLengthMode.Equal:
                            if (isl)
                            {
                                m = ControlPointMode.LeftAngleEqual;
                                y = cp1;
                            }
                            else
                            {
                                m = ControlPointMode.RightAngleEqual;
                                y = cp2;
                            }

                            x = y - p;
                            b = x.LengthSquared();
                            break;
                        case EachControlPointLengthMode.Ratio:
                            if (isl)
                            {
                                m = ControlPointMode.LeftAngleRatio;
                                y = cp1;
                                z = cp2 - p;
                            }
                            else
                            {
                                m = ControlPointMode.RightAngleRatio;
                                y = cp2;
                                z = cp1 - p;
                            }

                            x = y - p;
                            a = x.LengthSquared();
                            b = z.Length();
                            break;
                        default:
                            goto case EachControlPointLengthMode.Equal;
                    }
                    break;
                case SelfControlPointMode.Length:
                    if (isl)
                    {
                        m = ControlPointMode.LeftLength;
                        y = cp1 - p;
                    }
                    else
                    {
                        m = ControlPointMode.RightLength;
                        y = cp2 - p;
                    }

                    a = y.Length();
                    break;
                default:
                    break;
            }
        }
    }
}