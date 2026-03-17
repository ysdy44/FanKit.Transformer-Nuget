using FanKit.Transformer.Curves;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Sample;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class SvgPathPage : Page
    {
        readonly static Color StrokeColor = Colors.DeepSkyBlue;
        readonly static Color FillColor = Color.FromArgb(51, Colors.DeepSkyBlue.R, Colors.DeepSkyBlue.G, Colors.DeepSkyBlue.B);

        CanvasGeometry Curve;

        readonly CanvasPath Path = new CanvasPath();

        readonly Matrix3x2 Offset = Matrix3x2.CreateTranslation(412f, 0f);

        public SvgPathPage()
        {
            this.InitializeComponent();

            this.CanvasControl.CreateResources += (s, args) =>
            {
                this.Path.ResetByXaml((Windows.UI.Xaml.Media.PathGeometry)this.SvgPath.Data);

                this.Curve = this.Path.ToGeometry(s);

                this.Curve.SendPathTo(this.Path);
            };
            this.CanvasControl.Draw += (s, args) =>
            {
                args.DrawingSession.Transform = this.Offset;
                args.DrawingSession.FillGeometry(this.Curve, FillColor);
                args.DrawingSession.Transform = Matrix3x2.Identity;

                args.DrawingSession.Transform = this.Offset;
                args.DrawingSession.DrawGeometry(this.Curve, StrokeColor, 2f);
                args.DrawingSession.Transform = Matrix3x2.Identity;

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

                args.DrawingSession.Transform = this.Offset;
                foreach (List<Node> figure in this.Path)
                {
                    foreach (Node item in figure)
                    {
                        args.DrawingSession.DrawNode(item);
                    }
                }
                //args.DrawingSession.Transform = Matrix3x2.Identity;
            };
        }
    }
}