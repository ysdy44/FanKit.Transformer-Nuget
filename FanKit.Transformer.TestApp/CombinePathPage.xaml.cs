using FanKit.Transformer.Curves;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Sample;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public class CombinePath : List<List<Node>>, ICanvasPathReceiver
    {
        PathReceiver Receiver;

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
    }

    public sealed partial class CombinePathPage : Page
    {
        // Source
        readonly static Color LeftStrokeColor = Colors.OrangeRed;
        readonly static Color LeftFillColor = Color.FromArgb(51, Colors.OrangeRed.R, Colors.OrangeRed.G, Colors.OrangeRed.B);

        // Destination
        readonly static Color RightStrokeColor = Colors.DeepSkyBlue;
        readonly static Color RightFillColor = Color.FromArgb(51, Colors.DeepSkyBlue.R, Colors.DeepSkyBlue.G, Colors.DeepSkyBlue.B);

        CanvasGeometryCombine Combine = CanvasGeometryCombine.Union;

        CanvasGeometry Curve0;
        CanvasGeometry Curve1;
        CanvasGeometry Curve;

        readonly CombinePath Path0 = new CombinePath();
        readonly CombinePath Path1 = new CombinePath();
        readonly CombinePath Path = new CombinePath();

        readonly Matrix3x2 Offset = Matrix3x2.CreateTranslation(412f, 0f);

        public CombinePathPage()
        {
            this.InitializeComponent();
            this.ComboBox.SelectionChanged += delegate
            {
                switch (this.ComboBox.SelectedIndex)
                {
                    case 0: this.Combine = CanvasGeometryCombine.Union; break;
                    case 1: this.Combine = CanvasGeometryCombine.Intersect; break;
                    case 2: this.Combine = CanvasGeometryCombine.Xor; break;
                    case 3: this.Combine = CanvasGeometryCombine.Exclude; break;
                    default: break;
                }

                this.Path.Clear();
                this.Curve = this.Curve0.CombineWith(this.Curve1, Matrix3x2.Identity, this.Combine);
                this.Curve.SendPathTo(this.Path);

                this.CanvasControl.Invalidate();
            };

            this.ToggleSwitch.Toggled += delegate { this.CanvasControl.Invalidate(); };
            this.CanvasControl.CreateResources += (s, args) =>
            {
                this.Curve0 = CanvasGeometry.CreateCircle(s, 130f, 200f, 108f);
                this.Curve1 = CanvasGeometry.CreateCircle(s, 270f, 200f, 108f);
                this.Curve = this.Curve0.CombineWith(this.Curve1, Matrix3x2.Identity, this.Combine);

                this.Curve0.SendPathTo(this.Path0);
                this.Curve1.SendPathTo(this.Path1);
                this.Curve.SendPathTo(this.Path);
            };
            this.CanvasControl.Draw += (s, args) =>
            {
                args.DrawingSession.FillGeometry(this.Curve0, LeftFillColor);
                args.DrawingSession.FillGeometry(this.Curve1, LeftFillColor);
                args.DrawingSession.Transform = this.Offset;
                args.DrawingSession.FillGeometry(this.Curve, RightFillColor);
                args.DrawingSession.Transform = Matrix3x2.Identity;

                args.DrawingSession.DrawGeometry(this.Curve0, LeftStrokeColor, 2f);
                args.DrawingSession.DrawGeometry(this.Curve1, LeftStrokeColor, 2f);
                args.DrawingSession.Transform = this.Offset;
                args.DrawingSession.DrawGeometry(this.Curve, RightStrokeColor, 2f);
                args.DrawingSession.Transform = Matrix3x2.Identity;

                if (this.ToggleSwitch.IsOn)
                {
                    foreach (List<Node> figure in this.Path0)
                    {
                        foreach (Node item in figure)
                        {
                            args.DrawingSession.DrawLine(item.Point, item.LeftControlPoint);
                            args.DrawingSession.DrawLine(item.Point, item.RightControlPoint);
                        }
                    }
                    foreach (List<Node> figure in this.Path1)
                    {
                        foreach (Node item in figure)
                        {
                            args.DrawingSession.DrawLine(item.Point, item.LeftControlPoint);
                            args.DrawingSession.DrawLine(item.Point, item.RightControlPoint);
                        }
                    }
                    args.DrawingSession.Transform = this.Offset;
                    foreach (List<Node> figure in this.Path)
                    {
                        foreach (Node item in figure)
                        {
                            args.DrawingSession.DrawLine(item.Point, item.LeftControlPoint);
                            args.DrawingSession.DrawLine(item.Point, item.RightControlPoint);
                        }
                    }
                    args.DrawingSession.Transform = Matrix3x2.Identity;

                    foreach (List<Node> figure in this.Path0)
                    {
                        foreach (Node item in figure)
                        {
                            args.DrawingSession.DrawNode(item);
                        }
                    }
                    foreach (List<Node> figure in this.Path1)
                    {
                        foreach (Node item in figure)
                        {
                            args.DrawingSession.DrawNode(item);
                        }
                    }
                    args.DrawingSession.Transform = this.Offset;
                    foreach (List<Node> figure in this.Path)
                    {
                        foreach (Node item in figure)
                        {
                            args.DrawingSession.DrawNode(item);
                        }
                    }
                    //args.DrawingSession.Transform = Matrix3x2.Identity;
                }
                else
                {
                    foreach (List<Node> figure in this.Path0)
                    {
                        foreach (Node item in figure)
                        {
                            args.DrawingSession.DrawNode3(item.Point);
                        }
                    }
                    foreach (List<Node> figure in this.Path1)
                    {
                        foreach (Node item in figure)
                        {
                            args.DrawingSession.DrawNode3(item.Point);
                        }
                    }
                    args.DrawingSession.Transform = this.Offset;
                    foreach (List<Node> figure in this.Path)
                    {
                        foreach (Node item in figure)
                        {
                            args.DrawingSession.DrawNode3(item.Point);
                        }
                    }
                    //args.DrawingSession.Transform = Matrix3x2.Identity;
                }
            };
        }
    }
}