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
    public sealed partial class GeometryPage : Page
    {
        static readonly Bounds B = new Bounds(50, 50, 150, 150);
        static readonly Bounds T = new Bounds(100, 100, 150, 150);
        static readonly Bounds M = new Bounds(30, 50, 170, 150);

        // 0
        DemoGeometry G00;
        DemoGeometry G01;
        DemoGeometry G02;
        DemoGeometry G03;
        DemoGeometry G04;

        // 1
        DemoGeometry G10;
        DemoGeometry G11;
        DemoGeometry G12;
        DemoGeometry G13;
        DemoGeometry G14;

        // 2
        DemoGeometry G20;
        DemoGeometry G21;
        DemoGeometry G22;
        DemoGeometry G23;
        DemoGeometry G24;

        // 0
        CanvasGeometry CanvasGeometry00;
        CanvasGeometry CanvasGeometry01;
        CanvasGeometry CanvasGeometry02;
        CanvasGeometry CanvasGeometry03;
        CanvasGeometry CanvasGeometry04;

        // 1
        CanvasGeometry CanvasGeometry10;
        CanvasGeometry CanvasGeometry11;
        CanvasGeometry CanvasGeometry12;
        CanvasGeometry CanvasGeometry13;
        CanvasGeometry CanvasGeometry14;

        // 2
        CanvasGeometry CanvasGeometry20;
        CanvasGeometry CanvasGeometry21;
        CanvasGeometry CanvasGeometry22;
        CanvasGeometry CanvasGeometry23;
        CanvasGeometry CanvasGeometry24;

        // 0
        Quadrilateral Quadrilateral00;
        Quadrilateral Quadrilateral01;
        Quadrilateral Quadrilateral02;
        Quadrilateral Quadrilateral03;
        Quadrilateral Quadrilateral04;

        // 1
        Quadrilateral Quadrilateral10;
        Quadrilateral Quadrilateral11;
        Quadrilateral Quadrilateral12;
        Quadrilateral Quadrilateral13;
        Quadrilateral Quadrilateral14;

        // 2
        Quadrilateral Quadrilateral20;
        Quadrilateral Quadrilateral21;
        Quadrilateral Quadrilateral22;
        Quadrilateral Quadrilateral23;
        Quadrilateral Quadrilateral24;

        public GeometryPage()
        {
            this.InitializeComponent();

            // 0
            this.CanvasControl00.CreateResources += (s, e) =>
            {
                this.G00 = new DemoGeometry { Builder = new CanvasPathBuilder(s) };
                this.G00.CreateRectangle(new Box0(B));
                this.CanvasGeometry00 = CanvasGeometry.CreatePath(this.G00.Builder);
                this.Quadrilateral00 = new Quadrilateral(B);
            };
            this.CanvasControl01.CreateResources += (s, e) =>
            {
                this.G01 = new DemoGeometry { Builder = new CanvasPathBuilder(s) };
                this.G01.CreateEllipse(new Box1(B));
                this.CanvasGeometry01 = CanvasGeometry.CreatePath(this.G01.Builder);
                this.Quadrilateral01 = new Quadrilateral(B);
            };
            this.CanvasControl02.CreateResources += (s, e) =>
            {
                this.G02 = new DemoGeometry { Builder = new CanvasPathBuilder(s) };
                this.G02.CreateRoundRectangle(new Box1(B));
                this.CanvasGeometry02 = CanvasGeometry.CreatePath(this.G02.Builder);
                this.Quadrilateral02 = new Quadrilateral(B);
            };
            this.CanvasControl03.CreateResources += (s, e) =>
            {
                this.G03 = new DemoGeometry { Builder = new CanvasPathBuilder(s) };
                this.G03.CreateTriangle(new Box0(B));
                this.CanvasGeometry03 = CanvasGeometry.CreatePath(this.G03.Builder);
                this.Quadrilateral03 = new Quadrilateral(B);
            };
            this.CanvasControl04.CreateResources += (s, e) =>
            {
                this.G04 = new DemoGeometry { Builder = new CanvasPathBuilder(s) };
                this.G04.CreateDiamond(new Box1(B));
                this.CanvasGeometry04 = CanvasGeometry.CreatePath(this.G04.Builder);
                this.Quadrilateral04 = new Quadrilateral(B);
            };

            // 1
            this.CanvasControl10.CreateResources += (s, e) =>
            {
                this.G10 = new DemoGeometry { Builder = new CanvasPathBuilder(s) };
                this.G10.CreatePentagon(new Triangle(T));
                this.CanvasGeometry10 = CanvasGeometry.CreatePath(this.G10.Builder);
                this.Quadrilateral10 = new Quadrilateral(T);
            };
            this.CanvasControl11.CreateResources += (s, e) =>
            {
                this.G11 = new DemoGeometry { Builder = new CanvasPathBuilder(s) };
                this.G11.CreateStar(new Triangle(T));
                this.CanvasGeometry11 = CanvasGeometry.CreatePath(this.G11.Builder);
                this.Quadrilateral11 = new Quadrilateral(T);
            };
            this.CanvasControl12.CreateResources += (s, e) =>
            {
                this.G12 = new DemoGeometry { Builder = new CanvasPathBuilder(s) };
                this.G12.CreateCog(new Triangle(T));
                this.CanvasGeometry12 = CanvasGeometry.CreatePath(this.G12.Builder);
                this.Quadrilateral12 = new Quadrilateral(T);
            };
            this.CanvasControl13.CreateResources += (s, e) =>
            {
                this.G13 = new DemoGeometry { Builder = new CanvasPathBuilder(s) };
                this.G13.CreateDonut(new Box1(B));
                this.CanvasGeometry13 = CanvasGeometry.CreatePath(this.G13.Builder);
                this.Quadrilateral13 = new Quadrilateral(B);
            };
            this.CanvasControl14.CreateResources += (s, e) =>
            {
                this.G14 = new DemoGeometry { Builder = new CanvasPathBuilder(s) };
                this.G14.CreatePie(new Box1(T));
                this.CanvasGeometry14 = CanvasGeometry.CreatePath(this.G14.Builder);
                this.Quadrilateral14 = new Quadrilateral(T);
            };

            // 2
            this.CanvasControl20.CreateResources += (s, e) =>
            {
                this.G20 = new DemoGeometry { Builder = new CanvasPathBuilder(s) };
                this.G20.CreateCookie(new Box1(T));
                this.CanvasGeometry20 = CanvasGeometry.CreatePath(this.G20.Builder);
                this.Quadrilateral20 = new Quadrilateral(T);
            };
            this.CanvasControl21.CreateResources += (s, e) =>
            {
                this.G21 = new DemoGeometry { Builder = new CanvasPathBuilder(s) };
                this.G21.CreateArrow(new Box2(M));
                this.CanvasGeometry21 = CanvasGeometry.CreatePath(this.G21.Builder);
                this.Quadrilateral21 = new Quadrilateral(M);
            };
            this.CanvasControl22.CreateResources += (s, e) =>
            {
                this.G22 = new DemoGeometry { Builder = new CanvasPathBuilder(s) };
                this.G22.CreateCapsule(new Box2(M));
                this.CanvasGeometry22 = CanvasGeometry.CreatePath(this.G22.Builder);
                this.Quadrilateral22 = new Quadrilateral(M);
            };
            this.CanvasControl23.CreateResources += (s, e) =>
            {
                this.G23 = new DemoGeometry { Builder = new CanvasPathBuilder(s) };
                this.G23.CreateHeart(new Box2(T));
                this.CanvasGeometry23 = CanvasGeometry.CreatePath(this.G23.Builder);
                this.Quadrilateral23 = new Quadrilateral(T);
            };
            this.CanvasControl24.CreateResources += (s, e) =>
            {
                this.G24 = new DemoGeometry { Builder = new CanvasPathBuilder(s) };
                this.G24.CreateArc(new Box1(T));
                this.CanvasGeometry24 = CanvasGeometry.CreatePath(this.G24.Builder);
                this.Quadrilateral24 = new Quadrilateral(T);
            };

            // 0
            this.CanvasControl00.Draw += (s, e) => Draw(e.DrawingSession, this.Quadrilateral00, this.CanvasGeometry00, this.G00);
            this.CanvasControl01.Draw += (s, e) => Draw(e.DrawingSession, this.Quadrilateral01, this.CanvasGeometry01, this.G01);
            this.CanvasControl02.Draw += (s, e) => Draw(e.DrawingSession, this.Quadrilateral02, this.CanvasGeometry02, this.G02);
            this.CanvasControl03.Draw += (s, e) => Draw(e.DrawingSession, this.Quadrilateral03, this.CanvasGeometry03, this.G03);
            this.CanvasControl04.Draw += (s, e) => Draw(e.DrawingSession, this.Quadrilateral04, this.CanvasGeometry04, this.G04);

            // 1
            this.CanvasControl10.Draw += (s, e) => Draw(e.DrawingSession, this.Quadrilateral10, this.CanvasGeometry10, this.G10);
            this.CanvasControl11.Draw += (s, e) => Draw(e.DrawingSession, this.Quadrilateral11, this.CanvasGeometry11, this.G11);
            this.CanvasControl12.Draw += (s, e) => Draw(e.DrawingSession, this.Quadrilateral12, this.CanvasGeometry12, this.G12);
            this.CanvasControl13.Draw += (s, e) => Draw(e.DrawingSession, this.Quadrilateral13, this.CanvasGeometry13, this.G13);
            this.CanvasControl14.Draw += (s, e) => Draw(e.DrawingSession, this.Quadrilateral14, this.CanvasGeometry14, this.G14);

            // 2
            this.CanvasControl20.Draw += (s, e) => Draw(e.DrawingSession, this.Quadrilateral20, this.CanvasGeometry20, this.G20);
            this.CanvasControl21.Draw += (s, e) => Draw(e.DrawingSession, this.Quadrilateral21, this.CanvasGeometry21, this.G21);
            this.CanvasControl22.Draw += (s, e) => Draw(e.DrawingSession, this.Quadrilateral22, this.CanvasGeometry22, this.G22);
            this.CanvasControl23.Draw += (s, e) => Draw(e.DrawingSession, this.Quadrilateral23, this.CanvasGeometry23, this.G23);
            this.CanvasControl24.Draw += (s, e) => Draw(e.DrawingSession, this.Quadrilateral24, this.CanvasGeometry24, this.G24);
        }

        private static void Draw(CanvasDrawingSession drawingSession, Quadrilateral quadr, CanvasGeometry canvasGeometry, DemoGeometry geometry)
        {
            drawingSession.DrawLine(quadr.LeftTop, quadr.RightTop, Windows.UI.Colors.Gray);
            drawingSession.DrawLine(quadr.RightTop, quadr.RightBottom, Windows.UI.Colors.Gray);
            drawingSession.DrawLine(quadr.RightBottom, quadr.LeftBottom, Windows.UI.Colors.Gray);
            drawingSession.DrawLine(quadr.LeftBottom, quadr.LeftTop, Windows.UI.Colors.Gray);

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