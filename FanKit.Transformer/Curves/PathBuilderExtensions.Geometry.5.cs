using FanKit.Transformer.Cache;
using System.Numerics;

namespace FanKit.Transformer.Curves
{
    partial class PathBuilderExtensions
    {
        // Radians Quadrant
        const float RQ = R90;

        // Radians Positive
        const float RP360 = R360;
        const float RP270 = R270;
        const float RP180 = R180;
        const float RP90 = R90;

        // Radians Zero
        const float RZ0 = R0;

        // Radians Negative
        const float RN90 = -R90;
        const float RN180 = -R180;
        const float RN270 = -R270;
        const float RN360 = -R360;

        // Arc Positive
        const byte P360 = 0;
        const byte P270T360 = 1;
        const byte P270 = 2;
        const byte P180T270 = 3;
        const byte P180 = 4;
        const byte P90T180 = 5;
        const byte P90 = 6;
        const byte P0T90 = 7;

        // Arc Zero
        const byte Z0 = 8;

        // Arc Negative
        const byte N0T90 = 9;
        const byte N90 = 10;
        const byte N90T180 = 11;
        const byte N180 = 12;
        const byte N180T270 = 13;
        const byte N270 = 14;
        const byte N270T360 = 15;
        const byte N360 = 16;

        #region Arc
        public static void CreateArc(this IPathBuilder pathBuilder, Triangle bounds, float startAngle = R0, float sweepAngle = R270)
        {
            Matrix3x2 oneMatrix = bounds.Normalize();

            CreateArcCore(pathBuilder, oneMatrix, startAngle, sweepAngle, true, true);
            pathBuilder.EndFigure(Open);
        }

        public static void CreateArc(this IPathBuilder pathBuilder, Triangle bounds, Matrix3x2 matrix, float startAngle = R0, float sweepAngle = R270)
        {
            Matrix3x2 oneMatrix = bounds.Normalize();
            Matrix3x2 oneMatrix2 = oneMatrix * matrix;

            CreateArcCore(pathBuilder, oneMatrix2, startAngle, sweepAngle, true, true);
            pathBuilder.EndFigure(Open);
        }

        private static void CreateArcCore(IPathBuilder pathBuilder, Matrix3x2 oneMatrix, float startAngle, float sweepAngle, bool isBegin, bool isClosed = false)
        {
            float start = startAngle + R90;
            Rotation2x2 r = new Rotation2x2(start);

            Vector2 centerRight = new Vector2(1, 0);
            Vector2 centerLeft = new Vector2(-1, 0);
            Vector2 centerBottom = new Vector2(0, 1);
            Vector2 centerTop = new Vector2(0, -1);

            // A Ellipse has left, top, right, bottom four nodes.
            // 
            // Control points on the left and right sides of the node.
            // 
            // The distance of the control point 
            // is 0.552f times
            // the length of the square edge.

            // HV
            Vector2 horizontal = (centerRight - centerLeft);
            Vector2 horizontal276 = horizontal * Z276; // vector * Z552 / 2

            Vector2 vertical = (centerBottom - centerTop);
            Vector2 vertical276 = vertical * Z276; // vector * Z552 / 2

            // Control
            Vector2 left1 = centerLeft + vertical276;
            Vector2 left2 = centerLeft - vertical276;
            Vector2 top1 = centerTop - horizontal276;
            Vector2 top2 = centerTop + horizontal276;
            Vector2 right1 = centerRight - vertical276;
            Vector2 right2 = centerRight + vertical276;
            Vector2 bottom1 = centerBottom + horizontal276;
            Vector2 bottom2 = centerBottom - horizontal276;

            switch (GetArcMode(sweepAngle))
            {
                case P360:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(bottom2), oneMatrix), Vector2.Transform(r.T2(left1), oneMatrix), Vector2.Transform(r.T2(centerLeft), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(left2), oneMatrix), Vector2.Transform(r.T2(top1), oneMatrix), Vector2.Transform(r.T2(centerTop), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(top2), oneMatrix), Vector2.Transform(r.T2(right1), oneMatrix), Vector2.Transform(r.T2(centerRight), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(right2), oneMatrix), Vector2.Transform(r.T2(bottom1), oneMatrix), Vector2.Transform(r.T2(centerBottom), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                    }
                    break;
                case P270T360:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(bottom2), oneMatrix), Vector2.Transform(r.T2(left1), oneMatrix), Vector2.Transform(r.T2(centerLeft), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(left2), oneMatrix), Vector2.Transform(r.T2(top1), oneMatrix), Vector2.Transform(r.T2(centerTop), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(top2), oneMatrix), Vector2.Transform(r.T2(right1), oneMatrix), Vector2.Transform(r.T2(centerRight), oneMatrix));

                        float scale = Z552 * (sweepAngle - R270) / RQ;
                        float sweep = sweepAngle - R90;
                        Rotation2x2 arc = new Rotation2x2(sweep);
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(centerBottom * scale + centerRight), oneMatrix), Vector2.Transform(r.T2(arc.NCP(scale)), oneMatrix), Vector2.Transform(r.T2(arc.NP()), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T2(arc.NP()), oneMatrix));
                    }
                    break;
                case P270:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(bottom2), oneMatrix), Vector2.Transform(r.T2(left1), oneMatrix), Vector2.Transform(r.T2(centerLeft), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(left2), oneMatrix), Vector2.Transform(r.T2(top1), oneMatrix), Vector2.Transform(r.T2(centerTop), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(top2), oneMatrix), Vector2.Transform(r.T2(right1), oneMatrix), Vector2.Transform(r.T2(centerRight), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T2(centerRight), oneMatrix));
                    }
                    break;
                case P180T270:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(bottom2), oneMatrix), Vector2.Transform(r.T2(left1), oneMatrix), Vector2.Transform(r.T2(centerLeft), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(left2), oneMatrix), Vector2.Transform(r.T2(top1), oneMatrix), Vector2.Transform(r.T2(centerTop), oneMatrix));

                        float scale = Z552 * (sweepAngle - R180) / RQ;
                        float sweep = sweepAngle - R90;
                        Rotation2x2 arc = new Rotation2x2(sweep);
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(centerRight * scale + centerTop), oneMatrix), Vector2.Transform(r.T2(arc.NCP(scale)), oneMatrix), Vector2.Transform(r.T2(arc.NP()), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T2(arc.NP()), oneMatrix));
                    }
                    break;
                case P180:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(bottom2), oneMatrix), Vector2.Transform(r.T2(left1), oneMatrix), Vector2.Transform(r.T2(centerLeft), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(left2), oneMatrix), Vector2.Transform(r.T2(top1), oneMatrix), Vector2.Transform(r.T2(centerTop), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T2(centerTop), oneMatrix));
                    }
                    break;
                case P90T180:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(bottom2), oneMatrix), Vector2.Transform(r.T2(left1), oneMatrix), Vector2.Transform(r.T2(centerLeft), oneMatrix));

                        float scale = Z552 * (sweepAngle - R90) / RQ;
                        float sweep = sweepAngle - R90;
                        Rotation2x2 arc = new Rotation2x2(sweep);
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(centerTop * scale + centerLeft), oneMatrix), Vector2.Transform(r.T2(arc.NCP(scale)), oneMatrix), Vector2.Transform(r.T2(arc.NP()), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T2(arc.NP()), oneMatrix));
                    }
                    break;
                case P90:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(bottom2), oneMatrix), Vector2.Transform(r.T2(left1), oneMatrix), Vector2.Transform(r.T2(centerLeft), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T2(centerLeft), oneMatrix));
                    }
                    break;
                case P0T90:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T2(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T2(centerBottom), oneMatrix));

                        float scale = Z552 * (sweepAngle - R0) / RQ;
                        float sweep = sweepAngle - R90;
                        Rotation2x2 arc = new Rotation2x2(sweep);
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T2(centerLeft * scale + centerBottom), oneMatrix), Vector2.Transform(r.T2(arc.NCP(scale)), oneMatrix), Vector2.Transform(r.T2(arc.NP()), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T2(arc.NP()), oneMatrix));
                    }
                    break;
                case Z0:
                    break;
                case N0T90:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T3(centerBottom), oneMatrix));

                        float scale = Z552 * (-sweepAngle - R0) / RQ;
                        float sweep = -sweepAngle - R90;
                        Rotation2x2 arc = new Rotation2x2(sweep);
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(centerLeft * scale + centerBottom), oneMatrix), Vector2.Transform(r.T3(arc.NCP(scale)), oneMatrix), Vector2.Transform(r.T3(arc.NP()), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T3(arc.NP()), oneMatrix));
                    }
                    break;
                case N90:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(bottom2), oneMatrix), Vector2.Transform(r.T3(left1), oneMatrix), Vector2.Transform(r.T3(centerLeft), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T3(centerLeft), oneMatrix));
                    }
                    break;
                case N90T180:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(bottom2), oneMatrix), Vector2.Transform(r.T3(left1), oneMatrix), Vector2.Transform(r.T3(centerLeft), oneMatrix));

                        float scale = Z552 * (-sweepAngle - R90) / RQ;
                        float sweep = -sweepAngle - R90;
                        Rotation2x2 arc = new Rotation2x2(sweep);
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(centerTop * scale + centerLeft), oneMatrix), Vector2.Transform(r.T3(arc.NCP(scale)), oneMatrix), Vector2.Transform(r.T3(arc.NP()), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T3(arc.NP()), oneMatrix));
                    }
                    break;
                case N180:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(bottom2), oneMatrix), Vector2.Transform(r.T3(left1), oneMatrix), Vector2.Transform(r.T3(centerLeft), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(left2), oneMatrix), Vector2.Transform(r.T3(top1), oneMatrix), Vector2.Transform(r.T3(centerTop), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T3(centerTop), oneMatrix));
                    }
                    break;
                case N180T270:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(bottom2), oneMatrix), Vector2.Transform(r.T3(left1), oneMatrix), Vector2.Transform(r.T3(centerLeft), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(left2), oneMatrix), Vector2.Transform(r.T3(top1), oneMatrix), Vector2.Transform(r.T3(centerTop), oneMatrix));

                        float scale = Z552 * (-sweepAngle - R180) / RQ;
                        float sweep = -sweepAngle - R90;
                        Rotation2x2 arc = new Rotation2x2(sweep);
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(centerRight * scale + centerTop), oneMatrix), Vector2.Transform(r.T3(arc.NCP(scale)), oneMatrix), Vector2.Transform(r.T3(arc.NP()), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T3(arc.NP()), oneMatrix));
                    }
                    break;
                case N270:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(bottom2), oneMatrix), Vector2.Transform(r.T3(left1), oneMatrix), Vector2.Transform(r.T3(centerLeft), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(left2), oneMatrix), Vector2.Transform(r.T3(top1), oneMatrix), Vector2.Transform(r.T3(centerTop), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(top2), oneMatrix), Vector2.Transform(r.T3(right1), oneMatrix), Vector2.Transform(r.T3(centerRight), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T3(centerRight), oneMatrix));
                    }
                    break;
                case N270T360:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(bottom2), oneMatrix), Vector2.Transform(r.T3(left1), oneMatrix), Vector2.Transform(r.T3(centerLeft), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(left2), oneMatrix), Vector2.Transform(r.T3(top1), oneMatrix), Vector2.Transform(r.T3(centerTop), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(top2), oneMatrix), Vector2.Transform(r.T3(right1), oneMatrix), Vector2.Transform(r.T3(centerRight), oneMatrix));

                        float scale = Z552 * (-sweepAngle - R270) / RQ;
                        float sweep = -sweepAngle - R90;
                        Rotation2x2 arc = new Rotation2x2(sweep);
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(centerBottom * scale + centerRight), oneMatrix), Vector2.Transform(r.T3(arc.NCP(scale)), oneMatrix), Vector2.Transform(r.T3(arc.NP()), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T3(arc.NP()), oneMatrix));
                    }
                    break;
                case N360:
                    {
                        // Path
                        if (isBegin)
                            pathBuilder.BeginFigure(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        else
                            pathBuilder.AddLine(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(bottom2), oneMatrix), Vector2.Transform(r.T3(left1), oneMatrix), Vector2.Transform(r.T3(centerLeft), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(left2), oneMatrix), Vector2.Transform(r.T3(top1), oneMatrix), Vector2.Transform(r.T3(centerTop), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(top2), oneMatrix), Vector2.Transform(r.T3(right1), oneMatrix), Vector2.Transform(r.T3(centerRight), oneMatrix));
                        pathBuilder.AddCubicBezier(Vector2.Transform(r.T3(right2), oneMatrix), Vector2.Transform(r.T3(bottom1), oneMatrix), Vector2.Transform(r.T3(centerBottom), oneMatrix));

                        // Closed
                        if (isClosed)
                            pathBuilder.AddLine(Vector2.Transform(r.T3(centerBottom), oneMatrix));
                    }
                    break;
                default:
                    break;
            }
        }

        private static byte GetArcMode(float sweepAngle)
        {
            switch (sweepAngle.CompareTo(RZ0))
            {
                case 1:
                    switch (sweepAngle.CompareTo(RP90))
                    {
                        case 1:
                            switch (sweepAngle.CompareTo(RP180))
                            {
                                case 1:
                                    switch (sweepAngle.CompareTo(RP270))
                                    {
                                        case 1:
                                            switch (sweepAngle.CompareTo(RP360))
                                            {
                                                case 1: case 0: return P360;
                                                case -1: return P270T360;
                                                default: return Z0;
                                            }
                                        case 0: return P270;
                                        case -1: return P180T270;
                                        default: return Z0;
                                    }
                                case 0: return P180;
                                case -1: return P90T180;
                                default: return Z0;
                            }
                        case 0: return P90;
                        case -1: return P0T90;
                        default: return Z0;
                    }
                case 0: return Z0;
                case -1:
                    switch (sweepAngle.CompareTo(RN90))
                    {
                        case 1: return N0T90;
                        case 0: return N90;
                        case -1:
                            switch (sweepAngle.CompareTo(RN180))
                            {
                                case 1: return N90T180;
                                case 0: return N180;
                                case -1:
                                    switch (sweepAngle.CompareTo(RN270))
                                    {
                                        case 1: return N180T270;
                                        case 0: return N270;
                                        case -1:
                                            switch (sweepAngle.CompareTo(RN360))
                                            {
                                                case 1: return N270T360;
                                                case 0: case -1: return N360;
                                                default: return Z0;
                                            }
                                        default: return Z0;
                                    }
                                default: return Z0;
                            }
                        default: return Z0;
                    }
                default: return Z0;
            }
        }
        #endregion
    }
}