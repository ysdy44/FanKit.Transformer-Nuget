using FanKit.Transformer;
using FanKit.Transformer.Cache;
using FanKit.Transformer.Curves;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class CombinePathPage : Page
    {
        static readonly Bounds B0 = new Bounds(40, 40, 140, 140);
        static readonly Bounds B1 = new Bounds(60, 110, 160, 210);

        DemoGeometry Source0;
        DemoGeometry Source1;
        CanvasGeometry SourceCanvasGeometry0;
        CanvasGeometry SourceCanvasGeometry1;

        DemoGeometry Union0;
        DemoGeometry Union1;
        DemoPath Union2;
        CanvasGeometry UnionCanvasGeometry;

        DemoGeometry Intersect0;
        DemoGeometry Intersect1;
        DemoPath Intersect2;
        CanvasGeometry IntersectCanvasGeometry;

        DemoGeometry Xor0;
        DemoGeometry Xor1;
        DemoPath Xor2;
        CanvasGeometry XorCanvasGeometry;

        DemoGeometry Exclude0;
        DemoGeometry Exclude1;
        DemoPath Exclude2;
        CanvasGeometry ExcludeCanvasGeometry;

        public CombinePathPage()
        {
            this.InitializeComponent();

            this.CanvasControl00.CreateResources += (s, e) =>
            {
                this.Source0 = new DemoGeometry { Builder = new CanvasPathBuilder(s) };
                this.Source0.CreateEllipse(new Box1(B0));

                this.Source1 = new DemoGeometry { Builder = new CanvasPathBuilder(s) };
                this.Source1.CreateEllipse(new Box1(B1));

                this.SourceCanvasGeometry0 = CanvasGeometry.CreatePath(this.Source0.Builder);
                this.SourceCanvasGeometry1 = CanvasGeometry.CreatePath(this.Source1.Builder);
            };
            this.CanvasControl01.CreateResources += (s, e) =>
            {
                this.Union0 = new DemoGeometry { Builder = new CanvasPathBuilder(s) };
                this.Union0.CreateEllipse(new Box1(B0));

                this.Union1 = new DemoGeometry { Builder = new CanvasPathBuilder(s) };
                this.Union1.CreateEllipse(new Box1(B1));

                this.Union2 = new DemoPath();

                using (var left = CanvasGeometry.CreatePath(this.Union0.Builder))
                using (var right = CanvasGeometry.CreatePath(this.Union1.Builder))
                {
                    const CanvasGeometryCombine combine = CanvasGeometryCombine.Union;
                    this.UnionCanvasGeometry = left.CombineWith(right, Matrix3x2.Identity, combine);
                    this.UnionCanvasGeometry.SendPathTo(this.Union2);
                }
            };
            this.CanvasControl02.CreateResources += (s, e) =>
            {
                this.Intersect0 = new DemoGeometry { Builder = new CanvasPathBuilder(s) };
                this.Intersect0.CreateEllipse(new Box1(B0));

                this.Intersect1 = new DemoGeometry { Builder = new CanvasPathBuilder(s) };
                this.Intersect1.CreateEllipse(new Box1(B1));

                this.Intersect2 = new DemoPath();

                using (var left = CanvasGeometry.CreatePath(this.Intersect0.Builder))
                using (var right = CanvasGeometry.CreatePath(this.Intersect1.Builder))
                {
                    const CanvasGeometryCombine combine = CanvasGeometryCombine.Intersect;
                    this.IntersectCanvasGeometry = left.CombineWith(right, Matrix3x2.Identity, combine);
                    this.IntersectCanvasGeometry.SendPathTo(this.Intersect2);
                }
            };
            this.CanvasControl03.CreateResources += (s, e) =>
            {
                this.Xor0 = new DemoGeometry { Builder = new CanvasPathBuilder(s) };
                this.Xor0.CreateEllipse(new Box1(B0));

                this.Xor1 = new DemoGeometry { Builder = new CanvasPathBuilder(s) };
                this.Xor1.CreateEllipse(new Box1(B1));

                this.Xor2 = new DemoPath();

                using (var left = CanvasGeometry.CreatePath(this.Xor0.Builder))
                using (var right = CanvasGeometry.CreatePath(this.Xor1.Builder))
                {
                    const CanvasGeometryCombine combine = CanvasGeometryCombine.Xor;
                    this.XorCanvasGeometry = left.CombineWith(right, Matrix3x2.Identity, combine);
                    this.XorCanvasGeometry.SendPathTo(this.Xor2);
                }
            };
            this.CanvasControl04.CreateResources += (s, e) =>
            {
                this.Exclude0 = new DemoGeometry { Builder = new CanvasPathBuilder(s) };
                this.Exclude0.CreateEllipse(new Box1(B0));

                this.Exclude1 = new DemoGeometry { Builder = new CanvasPathBuilder(s) };
                this.Exclude1.CreateEllipse(new Box1(B1));

                this.Exclude2 = new DemoPath();

                using (var left = CanvasGeometry.CreatePath(this.Exclude0.Builder))
                using (var right = CanvasGeometry.CreatePath(this.Exclude1.Builder))
                {
                    const CanvasGeometryCombine combine = CanvasGeometryCombine.Exclude;
                    this.ExcludeCanvasGeometry = left.CombineWith(right, Matrix3x2.Identity, combine);
                    this.ExcludeCanvasGeometry.SendPathTo(this.Exclude2);
                }
            };

            this.CanvasControl00.Draw += (s, e) =>
            {
                Draw(e.DrawingSession, this.SourceCanvasGeometry0, this.Source0);
                Draw(e.DrawingSession, this.SourceCanvasGeometry1, this.Source1);
            };
            this.CanvasControl01.Draw += (s, e) => Draw(e.DrawingSession, this.UnionCanvasGeometry, this.Union2);
            this.CanvasControl02.Draw += (s, e) => Draw(e.DrawingSession, this.IntersectCanvasGeometry, this.Intersect2);
            this.CanvasControl03.Draw += (s, e) => Draw(e.DrawingSession, this.XorCanvasGeometry, this.Xor2);
            this.CanvasControl04.Draw += (s, e) => Draw(e.DrawingSession, this.ExcludeCanvasGeometry, this.Exclude2);
        }

        private static void Draw(CanvasDrawingSession drawingSession, CanvasGeometry canvasGeometry, DemoPath path)
        {
            drawingSession.FillGeometry(canvasGeometry, Vector2.Zero, Windows.UI.Colors.Gray);
            drawingSession.DrawGeometry(canvasGeometry, Vector2.Zero, Windows.UI.Colors.DeepSkyBlue, 2f);

            DrawNodes(drawingSession, path);
        }

        private static void Draw(CanvasDrawingSession drawingSession, CanvasGeometry canvasGeometry, DemoGeometry geometry)
        {
            drawingSession.DrawGeometry(canvasGeometry, Vector2.Zero, Windows.UI.Colors.DeepSkyBlue, 2f);

            DrawNodes(drawingSession, geometry);
        }

        private static void DrawNodes(CanvasDrawingSession drawingSession, List<List<Segment0>> path)
        {
            foreach (List<Segment0> figure in path)
            {
                foreach (Segment0 segment in figure)
                {
                    if (segment.IsSmooth)
                    {
                        drawingSession.DrawLine(segment.Point.Point, segment.Point.LeftControlPoint, Windows.UI.Colors.DeepSkyBlue, 1f);
                        drawingSession.DrawLine(segment.Point.Point, segment.Point.RightControlPoint, Windows.UI.Colors.DeepSkyBlue, 1f);
                    }
                }
            }

            foreach (List<Segment0> figure in path)
            {
                foreach (Segment0 segment in figure)
                {
                    if (segment.IsSmooth)
                    {
                        drawingSession.FillCircle(segment.Point.LeftControlPoint, 2f, Windows.UI.Colors.DeepSkyBlue);
                        drawingSession.FillCircle(segment.Point.RightControlPoint, 2f, Windows.UI.Colors.DeepSkyBlue);
                    }

                    drawingSession.FillCircle(segment.Point.Point, 3f, Windows.UI.Colors.DeepSkyBlue);
                    drawingSession.FillCircle(segment.Point.Point, 2f, Windows.UI.Colors.White);
                }
            }
        }
    }
}