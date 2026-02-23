using System.Numerics;

namespace FanKit.Transformer.Curves
{
    /*
    System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
    path.AddEllipse(0,0,100,100);
    CombinePath p2 = new CombinePath(path);
  
    public CombinePath(GraphicsPath path)
    {
        int Length = path.PathPoints.Length;
        byte[] types = path.PathTypes;
        Vector2[] points = path.PathPoints.Select(To).ToArray();
        this.SendPathTo(Length, types, points);
    }
    */
    public static class GraphicsPathReceiver
    {
        public static void SendPathTo(this IPathBuilder builder, int length, byte[] types, Vector2[] points)
        {
            const byte Start = 0;
            const byte Line = 1;
            const byte Bezier = 3;
            const byte Bezier3 = 16;

            int Index = 0;
            bool? isClosed = null;

            while (Index < length)
            {
                byte type = types[Index];
                bool closed = (type & 0x80) != 0;
                byte pointType = (byte)(type & 0x7F); // 清除闭合标志

                switch (pointType)
                {
                    case Start:
                        Index++;

                        // Start
                        if (isClosed.HasValue)
                            builder.EndFigure(isClosed.Value);

                        builder.BeginFigure(points[Index - 1]);
                        break;

                    case Line:
                        Index++;

                        // Line
                        builder.AddLine(points[Index - 1]);
                        break;

                    case Bezier:
                        if (Index + 2 >= length)
                            goto case Line;

                        Index += 3;

                        // Bezier
                        Vector2 cp1 = points[Index - 3];
                        Vector2 cp2 = points[Index - 2];
                        Vector2 endPoint = points[Index - 1];
                        builder.AddCubicBezier(cp1, cp2, endPoint);
                        break;

                    case Bezier3:
                        goto case Line;

                    default:
                        goto case Line;
                }

                isClosed = closed;
            }

            if (isClosed.HasValue)
                builder.EndFigure(isClosed.Value);
        }
    }
}