using FanKit.Transformer.Curves;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.Foundation;

namespace FanKit.Transformer.TestApp
{
    public class CanvasPath : List<List<Node>>, ICanvasPathReceiver
    {
        PathReceiver Receiver;

        #region ICanvasPathReceiver
        public void BeginFigure(Vector2 startPoint, CanvasFigureFill figureFill)
        {
            this.Add(new List<Node>());
            this.Receiver = new PathReceiver(startPoint);
        }

        public void AddArc(Vector2 endPoint, float radiusX, float radiusY, float rotationAngle, CanvasSweepDirection sweepDirection, CanvasArcSize arcSize) { }

        public void AddCubicBezier(Vector2 controlPoint1, Vector2 controlPoint2, Vector2 endPoint)
        {
            this.Last().Add(this.Receiver.CubicBezier(controlPoint1, controlPoint2, endPoint));
            this.Receiver = this.Receiver.ToCubicBezier(controlPoint1, controlPoint2, endPoint);
        }

        public void AddQuadraticBezier(Vector2 controlPoint, Vector2 endPoint)
        {
            this.Last().Add(this.Receiver.QuadraticBezier(controlPoint, endPoint));
            this.Receiver = this.Receiver.ToQuadraticBezier(controlPoint, endPoint);
        }

        public void AddLine(Vector2 endPoint)
        {
            this.Last().Add(this.Receiver.Line(endPoint));
            this.Receiver = this.Receiver.ToLine(endPoint);
        }

        public void SetFilledRegionDetermination(CanvasFilledRegionDetermination filledRegionDetermination) { }

        public void SetSegmentOptions(CanvasFigureSegmentOptions figureSegmentOptions) { }

        public void EndFigure(CanvasFigureLoop figureLoop)
        {
            switch (figureLoop)
            {
                case CanvasFigureLoop.Open:
                    break;
                case CanvasFigureLoop.Closed:
                    this.Last().Add(this.Receiver.EndFigure());
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region CanvasGeometry
        public CanvasGeometry ToGeometry(ICanvasResourceCreator resourceCreator)
        {
            CanvasGeometry[] paths = new CanvasGeometry[Count];

            for (int i = 0; i < Count; i++)
            {
                List<Node> item = this[i];
                using (PathBuilder path = new PathBuilder(resourceCreator))
                {
                    path.CreatePointPath(item, true);
                    paths[i] = CanvasGeometry.CreatePath(path.Builder);
                }
            }

            return CanvasGeometry.CreateGroup(resourceCreator, paths);
        }
        #endregion

        #region Xaml
        public void ResetByXaml(Windows.UI.Xaml.Media.PathGeometry path)
        {
            foreach (Windows.UI.Xaml.Media.PathFigure figure in path.Figures)
            {
                BeginFigure(ToVector2(figure.StartPoint), CanvasFigureFill.Default);

                foreach (Windows.UI.Xaml.Media.PathSegment segment in figure.Segments)
                {
                    if (segment is Windows.UI.Xaml.Media.BezierSegment cubic)
                    {
                        AddCubicBezier(
                            ToVector2(cubic.Point1),
                            ToVector2(cubic.Point2),
                            ToVector2(cubic.Point3));
                    }
                    else if (segment is Windows.UI.Xaml.Media.QuadraticBezierSegment quadratic)
                    {
                        AddQuadraticBezier(
                            ToVector2(quadratic.Point1),
                            ToVector2(quadratic.Point2));
                    }
                    else if (segment is Windows.UI.Xaml.Media.LineSegment line)
                    {
                        this.AddLine(ToVector2(line.Point));
                    }
                    else if (segment is Windows.UI.Xaml.Media.PolyBezierSegment cubics)
                    {
                        const int Cubic = 3;

                        int count = cubics.Points.Count / Cubic;
                        for (int i = 0; i < count; i++)
                        {
                            AddCubicBezier(
                                ToVector2(cubics.Points[i * Cubic]),
                                ToVector2(cubics.Points[i * Cubic + 1]),
                                ToVector2(cubics.Points[i * Cubic + 2]));
                        }
                    }
                    else if (segment is Windows.UI.Xaml.Media.PolyQuadraticBezierSegment quadratics)
                    {
                        const int Quadratic = 2;

                        int count = quadratics.Points.Count / Quadratic;
                        for (int i = 0; i < count; i++)
                        {
                            AddQuadraticBezier(
                                ToVector2(quadratics.Points[i * Quadratic]),
                                ToVector2(quadratics.Points[i * Quadratic + 1]));
                        }
                    }
                    else if (segment is Windows.UI.Xaml.Media.PolyLineSegment lines)
                    {
                        foreach (Point item in lines.Points)
                        {
                            AddLine(ToVector2(item));
                        }
                    }
                }

                EndFigure(figure.IsClosed ? CanvasFigureLoop.Closed : CanvasFigureLoop.Open);
            }
        }

        private static Vector2 ToVector2(Point point, float scale = 0.5f)
        {
            return new Vector2
            {
                X = (float)(point.X * scale),
                Y = (float)(point.Y * scale),
            };
        }
        #endregion
    }
}