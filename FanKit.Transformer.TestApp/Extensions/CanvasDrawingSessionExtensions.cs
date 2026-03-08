using FanKit.Transformer.Cache;
using FanKit.Transformer.Polylines;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.TestApp
{
    public static class CanvasDrawingSessionExtensions
    {
        public static readonly Windows.UI.Color CanvasStroke = Windows.UI.Colors.DeepSkyBlue;
        public static readonly Windows.UI.Color CanvasFill = Windows.UI.Color.FromArgb(255, 238, 238, 238);

        public static readonly Windows.UI.Color ViewportStroke = Windows.UI.Color.FromArgb(255, 127, 127, 255);
        public static readonly Windows.UI.Color ViewportFill = Windows.UI.Color.FromArgb(102, 127, 127, 255);

        public static readonly Windows.UI.Color RectChooseStroke = Windows.UI.Colors.DeepSkyBlue;
        public static readonly Windows.UI.Color RectChooseFill = Windows.UI.Color.FromArgb(102, 0, 191, 255);

        public static readonly Windows.UI.Color ShadowFill = Windows.UI.Color.FromArgb(70, 127, 127, 127);
        public static readonly Windows.UI.Color PreviousStroke = Windows.UI.Colors.Fuchsia;
        public static readonly Windows.UI.Color NextStroke = Windows.UI.Colors.Red;

        public static readonly Windows.UI.Color AccentStroke = Windows.UI.Colors.DeepSkyBlue;
        public static readonly Windows.UI.Color AccentFill = Windows.UI.Colors.DeepSkyBlue;
        public static readonly CanvasStrokeStyle StrokeStyle = new CanvasStrokeStyle
        {
            CustomDashStyle = new float[] { 12f, 4f, }
        };

        public static void DrawPreviousLine(this CanvasDrawingSession drawingSession, Vector2 point0, Vector2 point1)
        {
            drawingSession.DrawLine(point0, point1, PreviousStroke, 2f);
        }
        public static void DrawNextLine(this CanvasDrawingSession drawingSession, Vector2 point0, Vector2 point1)
        {
            drawingSession.DrawLine(point0, point1, NextStroke, 2f);
        }

        public static void DrawLine(this CanvasDrawingSession drawingSession, Vector2 point0, Vector2 point1)
        {
            drawingSession.DrawLine(point0, point1, AccentStroke, 2f);
        }
        public static void DrawDashLine(this CanvasDrawingSession drawingSession, Vector2 point0, Vector2 point1)
        {
            drawingSession.DrawLine(point0, point1, AccentStroke, 2f, StrokeStyle);
        }
        public static void DrawDashLine(this CanvasDrawingSession drawingSession, Vector2 point0, Vector2 point1, float strokeWidth)
        {
            drawingSession.DrawLine(point0, point1, AccentStroke, strokeWidth, StrokeStyle);
        }

        public static void DrawDashPointPolyline(this CanvasDrawingSession drawingSession, List<Segment0> items, bool isClosed, float strokeWidth)
        {
            for (int i = 1; i < items.Count; i++)
            {
                Segment0 previous = items[i - 1];
                Segment0 next = items[i];
                drawingSession.DrawDashLine(previous.Point, next.Point, strokeWidth);
            }

            if (isClosed)
            {
                Segment0 last = items[items.Count - 1];
                Segment0 first = items[0];
                drawingSession.DrawDashLine(last.Point, first.Point, strokeWidth);
            }
        }

        public static void DrawDashPointPolyline(this CanvasDrawingSession drawingSession, List<Segment1> items, bool isClosed, float strokeWidth)
        {
            for (int i = 1; i < items.Count; i++)
            {
                Segment1 previous = items[i - 1];
                Segment1 next = items[i];
                drawingSession.DrawDashLine(previous.Point, next.Point, strokeWidth);
            }

            if (isClosed)
            {
                Segment1 last = items[items.Count - 1];
                Segment1 first = items[0];
                drawingSession.DrawDashLine(last.Point, first.Point, strokeWidth);
            }
        }
        public static void DrawDashActualPolyline(this CanvasDrawingSession drawingSession, List<Segment1> items, bool isClosed, float strokeWidth)
        {
            for (int i = 1; i < items.Count; i++)
            {
                Segment1 previous = items[i - 1];
                Segment1 next = items[i];
                drawingSession.DrawDashLine(previous.Actual, next.Actual, strokeWidth);
            }

            if (isClosed)
            {
                Segment1 last = items[items.Count - 1];
                Segment1 first = items[0];
                drawingSession.DrawDashLine(last.Actual, first.Actual, strokeWidth);
            }
        }

        public static void DrawDashRawPolyline(this CanvasDrawingSession drawingSession, List<Segment2> items, bool isClosed, float strokeWidth)
        {
            for (int i = 1; i < items.Count; i++)
            {
                Segment2 previous = items[i - 1];
                Segment2 next = items[i];
                drawingSession.DrawDashLine(previous.Raw, next.Raw, strokeWidth);
            }

            if (isClosed)
            {
                Segment2 last = items[items.Count - 1];
                Segment2 first = items[0];
                drawingSession.DrawDashLine(last.Raw, first.Raw, strokeWidth);
            }
        }
        public static void DrawDashMapPolyline(this CanvasDrawingSession drawingSession, List<Segment2> items, bool isClosed, float strokeWidth)
        {
            for (int i = 1; i < items.Count; i++)
            {
                Segment2 previous = items[i - 1];
                Segment2 next = items[i];
                drawingSession.DrawDashLine(previous.Map, next.Map, strokeWidth);
            }

            if (isClosed)
            {
                Segment2 last = items[items.Count - 1];
                Segment2 first = items[0];
                drawingSession.DrawDashLine(last.Map, first.Map, strokeWidth);
            }
        }

        public static void DrawDashRawPolyline(this CanvasDrawingSession drawingSession, List<Segment3> items, bool isClosed, float strokeWidth)
        {
            for (int i = 1; i < items.Count; i++)
            {
                Segment3 previous = items[i - 1];
                Segment3 next = items[i];
                drawingSession.DrawDashLine(previous.Raw, next.Raw, strokeWidth);
            }

            if (isClosed)
            {
                Segment3 last = items[items.Count - 1];
                Segment3 first = items[0];
                drawingSession.DrawDashLine(last.Raw, first.Raw, strokeWidth);
            }
        }
        public static void DrawDashMapPolyline(this CanvasDrawingSession drawingSession, List<Segment3> items, bool isClosed, float strokeWidth)
        {
            for (int i = 1; i < items.Count; i++)
            {
                Segment3 previous = items[i - 1];
                Segment3 next = items[i];
                drawingSession.DrawDashLine(previous.Map, next.Map, strokeWidth);
            }

            if (isClosed)
            {
                Segment3 last = items[items.Count - 1];
                Segment3 first = items[0];
                drawingSession.DrawDashLine(last.Map, first.Map, strokeWidth);
            }
        }
        public static void DrawDashActualPolyline(this CanvasDrawingSession drawingSession, List<Segment3> items, bool isClosed, float strokeWidth)
        {
            for (int i = 1; i < items.Count; i++)
            {
                Segment3 previous = items[i - 1];
                Segment3 next = items[i];
                drawingSession.DrawDashLine(previous.Actual, next.Actual, strokeWidth);
            }

            if (isClosed)
            {
                Segment3 last = items[items.Count - 1];
                Segment3 first = items[0];
                drawingSession.DrawDashLine(last.Actual, first.Actual, strokeWidth);
            }
        }

        public static void DrawPreviousCurve(this CanvasDrawingSession drawingSession, CanvasGeometry curve)
        {
            drawingSession.DrawGeometry(curve, PreviousStroke, 2f);
        }
        public static void DrawNextCurve(this CanvasDrawingSession drawingSession, CanvasGeometry curve)
        {
            drawingSession.DrawGeometry(curve, NextStroke, 2f);
        }

        public static void DrawCurve(this CanvasDrawingSession drawingSession, CanvasGeometry curve)
        {
            drawingSession.DrawGeometry(curve, AccentStroke, 2f);
        }
        public static void DrawDashCurve(this CanvasDrawingSession drawingSession, CanvasGeometry curve)
        {
            drawingSession.DrawGeometry(curve, AccentStroke, 2f, StrokeStyle);
        }
        public static void DrawDashCurve(this CanvasDrawingSession drawingSession, CanvasGeometry curve, float strokeWidth)
        {
            drawingSession.DrawGeometry(curve, AccentStroke, strokeWidth, StrokeStyle);
        }

        // Node
        public static void DrawNode(this CanvasDrawingSession drawingSession, Node node)
        {
            // Right
            drawingSession.FillCircle(node.RightControlPoint, 7f, AccentFill);
            drawingSession.FillCircle(node.RightControlPoint, 5f, Windows.UI.Colors.White);

            // Left
            drawingSession.FillCircle(node.LeftControlPoint, 7f, AccentFill);
            drawingSession.FillCircle(node.LeftControlPoint, 5f, Windows.UI.Colors.White);

            // Center
            drawingSession.FillCircle(node.Point, 10f, ShadowFill);
            drawingSession.FillCircle(node.Point, 8f, Windows.UI.Colors.White);

            drawingSession.FillCircle(node.Point, 6f, AccentFill);
            drawingSession.DrawCircle(node.Point, 5f, Windows.UI.Colors.DodgerBlue);
        }

        public static void DrawNode(this CanvasDrawingSession drawingSession, Vector2 position)
        {
            drawingSession.FillCircle(position, 10f, ShadowFill);
            drawingSession.FillCircle(position, 8f, Windows.UI.Colors.White);

            drawingSession.FillCircle(position, 6f, AccentFill);
            drawingSession.DrawCircle(position, 5f, Windows.UI.Colors.DodgerBlue);
        }

        public static void DrawNode2(this CanvasDrawingSession drawingSession, Vector2 position)
        {
            drawingSession.FillCircle(position, 7f, AccentFill);
            drawingSession.FillCircle(position, 5f, Windows.UI.Colors.White);
        }

        public static void DrawNode3(this CanvasDrawingSession drawingSession, Node node)
        {
            drawingSession.FillCircle(node.RightControlPoint, 5f, AccentFill);
            drawingSession.FillCircle(node.RightControlPoint, 3f, Windows.UI.Colors.White);

            drawingSession.FillCircle(node.LeftControlPoint, 5f, AccentFill);
            drawingSession.FillCircle(node.LeftControlPoint, 3f, Windows.UI.Colors.White);

            drawingSession.FillCircle(node.Point, 5f, AccentFill);
            drawingSession.FillCircle(node.Point, 3f, Windows.UI.Colors.White);
        }

        public static void DrawNode3(this CanvasDrawingSession drawingSession, Vector2 position)
        {
            drawingSession.FillCircle(position, 5f, AccentFill);
            drawingSession.FillCircle(position, 3f, Windows.UI.Colors.White);
        }

        // Canvas
        public static void FillCanvas(this CanvasDrawingSession drawingSession, float w, float h)
        {
            drawingSession.FillRectangle(0f, 0f, w, h, CanvasFill);
        }
        public static void FillCanvas(this CanvasDrawingSession drawingSession, Rectangle rect)
        {
            drawingSession.FillRectangle(rect.X, rect.Y, rect.Width, rect.Height, CanvasFill);
        }

        public static void DrawCanvas(this CanvasDrawingSession drawingSession, Rectangle rect)
        {
            drawingSession.DrawRectangle(rect.X, rect.Y, rect.Width, rect.Height, CanvasStroke);
        }
        public static void DrawCanvas(this CanvasDrawingSession drawingSession, Bounds t)
        {
            drawingSession.DrawLine(t.Left, t.Top, t.Right, t.Top, CanvasStroke);
            drawingSession.DrawLine(t.Right, t.Top, t.Right, t.Bottom, CanvasStroke);
            drawingSession.DrawLine(t.Right, t.Bottom, t.Left, t.Bottom, CanvasStroke);
            drawingSession.DrawLine(t.Left, t.Bottom, t.Left, t.Top, CanvasStroke);
        }
        public static void DrawCanvas(this CanvasDrawingSession drawingSession, Quadrilateral t)
        {
            drawingSession.DrawLine(t.LeftTop, t.RightTop, CanvasStroke);
            drawingSession.DrawLine(t.RightTop, t.RightBottom, CanvasStroke);
            drawingSession.DrawLine(t.RightBottom, t.LeftBottom, CanvasStroke);
            drawingSession.DrawLine(t.LeftBottom, t.LeftTop, CanvasStroke);
        }

        // Viewport
        public static void FillViewport(this CanvasDrawingSession drawingSession, float width, float height)
        {
            drawingSession.FillRectangle(0f, 0f, width, height, ViewportFill);
        }

        public static void DrawViewport(this CanvasDrawingSession drawingSession, Quadrilateral t)
        {
            drawingSession.DrawLine(t.LeftTop, t.RightTop, ViewportStroke);
            drawingSession.DrawLine(t.RightTop, t.RightBottom, ViewportStroke);
            drawingSession.DrawLine(t.RightBottom, t.LeftBottom, ViewportStroke);
            drawingSession.DrawLine(t.LeftBottom, t.LeftTop, ViewportStroke);
        }

        // RectChoose
        public static void FillRectChoose(this CanvasDrawingSession drawingSession, Rectangle rect)
        {
            drawingSession.FillRectangle(rect.X, rect.Y, rect.Width, rect.Height, RectChooseFill);
        }

        public static void DrawRectChoose(this CanvasDrawingSession drawingSession, Quadrilateral quad)
        {
            drawingSession.DrawLine(quad.LeftTop, quad.RightTop, RectChooseStroke);
            drawingSession.DrawLine(quad.RightTop, quad.RightBottom, RectChooseStroke);
            drawingSession.DrawLine(quad.RightBottom, quad.LeftBottom, RectChooseStroke);
            drawingSession.DrawLine(quad.LeftBottom, quad.LeftTop, RectChooseStroke);
        }
        public static void DrawRectChoose(this CanvasDrawingSession drawingSession, TransformedBounds bounds)
        {
            drawingSession.DrawLine(bounds.LeftTop, bounds.RightTop, RectChooseStroke);
            drawingSession.DrawLine(bounds.RightTop, bounds.RightBottom, RectChooseStroke);
            drawingSession.DrawLine(bounds.RightBottom, bounds.LeftBottom, RectChooseStroke);
            drawingSession.DrawLine(bounds.LeftBottom, bounds.LeftTop, RectChooseStroke);
        }

        // Line
        public static void DrawLine(this CanvasDrawingSession drawingSession, Line0 line)
        {
            // Handle Sides
            /*
            drawingSession.DrawLine(line.Center, line.Handle, AccentStroke);
             */

            // Lines
            drawingSession.DrawLine(line.Point0, line.Point1, AccentStroke);

            // Handle Corners
            /*
            DrawNode2(drawingSession, line.Handle0);
            DrawNode2(drawingSession, line.Handle1);
             */

            // Handle Sides
            /*
            DrawNode2(drawingSession, line.Handle);
             */

            // Sides
            /*
            const float d = 12 * 2;
            const float ds = d * d;

            if (line.LengthSquared > ds)
            {
                DrawNode(drawingSession, line.Center);
            }
             */

            // Line
            DrawNode(drawingSession, line.Point0);
            DrawNode(drawingSession, line.Point1);
        }
        public static void DrawLine(this CanvasDrawingSession drawingSession, Line1 line)
        {
            // Handle Sides
            /*
            drawingSession.DrawLine(line.Center, line.Handle, AccentStroke);
             */

            // Lines
            drawingSession.DrawLine(line.Point0, line.Point1, AccentStroke);

            // Handle Corners
            /*
            DrawNode2(drawingSession, line.Handle0);
            DrawNode2(drawingSession, line.Handle1);
             */

            // Handle Sides
            /*
            DrawNode2(drawingSession, line.Handle);
             */

            // Sides
            const float d = 12 * 2;
            const float ds = d * d;

            if (line.LengthSquared > ds)
            {
                DrawNode(drawingSession, line.Center);
            }

            // Line
            DrawNode(drawingSession, line.Point0);
            DrawNode(drawingSession, line.Point1);
        }
        public static void DrawLine(this CanvasDrawingSession drawingSession, Line2 line)
        {
            // Handle Sides
            drawingSession.DrawLine(line.Center, line.Handle, AccentStroke);

            // Lines
            drawingSession.DrawLine(line.Point0, line.Point1, AccentStroke);

            // Handle Corners
            /*
            DrawNode2(drawingSession, line.Handle0);
            DrawNode2(drawingSession, line.Handle1);
             */

            // Handle Sides
            DrawNode2(drawingSession, line.Handle);

            // Sides
            const float d = 12 * 2;
            const float ds = d * d;

            if (line.LengthSquared > ds)
            {
                DrawNode(drawingSession, line.Center);
            }

            // Line
            DrawNode(drawingSession, line.Point0);
            DrawNode(drawingSession, line.Point1);
        }
        public static void DrawLine(this CanvasDrawingSession drawingSession, Line3 line)
        {
            // Handle Sides
            drawingSession.DrawLine(line.Center, line.Handle, AccentStroke);

            // Lines
            drawingSession.DrawLine(line.Point0, line.Point1, AccentStroke);

            // Handle Corners
            DrawNode2(drawingSession, line.Handle0);
            DrawNode2(drawingSession, line.Handle1);

            // Handle Sides
            DrawNode2(drawingSession, line.Handle);

            // Sides
            const float d = 12 * 2;
            const float ds = d * d;

            if (line.LengthSquared > ds)
            {
                DrawNode(drawingSession, line.Center);
            }

            // Line
            DrawNode(drawingSession, line.Point0);
            DrawNode(drawingSession, line.Point1);
        }

        // Box
        public static void DrawBounds(this CanvasDrawingSession drawingSession, Quadrilateral quad)
        {
            drawingSession.DrawLine(quad.LeftTop, quad.RightTop, AccentStroke);
            drawingSession.DrawLine(quad.RightTop, quad.RightBottom, AccentStroke);
            drawingSession.DrawLine(quad.RightBottom, quad.LeftBottom, AccentStroke);
            drawingSession.DrawLine(quad.LeftBottom, quad.LeftTop, AccentStroke);
        }

        public static void DrawBounds(this CanvasDrawingSession drawingSession, Box0 quad)
        {
            drawingSession.DrawLine(quad.LeftTop, quad.RightTop, AccentStroke);
            drawingSession.DrawLine(quad.RightTop, quad.RightBottom, AccentStroke);
            drawingSession.DrawLine(quad.RightBottom, quad.LeftBottom, AccentStroke);
            drawingSession.DrawLine(quad.LeftBottom, quad.LeftTop, AccentStroke);
        }

        public static void DrawBox(this CanvasDrawingSession drawingSession, Box0 box)
        {
            // Lines
            drawingSession.DrawLine(box.LeftTop, box.RightTop, AccentStroke);
            drawingSession.DrawLine(box.RightTop, box.RightBottom, AccentStroke);
            drawingSession.DrawLine(box.RightBottom, box.LeftBottom, AccentStroke);
            drawingSession.DrawLine(box.LeftBottom, box.LeftTop, AccentStroke);

            // Corners
            DrawNode(drawingSession, box.LeftTop);
            DrawNode(drawingSession, box.RightTop);
            DrawNode(drawingSession, box.LeftBottom);
            DrawNode(drawingSession, box.RightBottom);
        }

        public static void DrawBox(this CanvasDrawingSession drawingSession, Box1 box)
        {
            // Lines
            drawingSession.DrawLine(box.LeftTop, box.RightTop, AccentStroke);
            drawingSession.DrawLine(box.RightTop, box.RightBottom, AccentStroke);
            drawingSession.DrawLine(box.RightBottom, box.LeftBottom, AccentStroke);
            drawingSession.DrawLine(box.LeftBottom, box.LeftTop, AccentStroke);

            const float d = 12 * 2;
            const float ds = d * d;

            // Sides
            if (box.SideLeftLengthSquared > ds)
            {
                DrawNode(drawingSession, box.CenterLeft);
            }

            if (box.SideTopLengthSquared > ds)
            {
                DrawNode(drawingSession, box.CenterTop);
            }

            if (box.SideRightLengthSquared > ds)
            {
                DrawNode(drawingSession, box.CenterRight);
            }

            if (box.SideBottomLengthSquared > ds)
            {
                DrawNode(drawingSession, box.CenterBottom);
            }

            // Corners
            DrawNode(drawingSession, box.LeftTop);
            DrawNode(drawingSession, box.RightTop);
            DrawNode(drawingSession, box.LeftBottom);
            DrawNode(drawingSession, box.RightBottom);
        }

        public static void DrawBox(this CanvasDrawingSession drawingSession, Box2 box)
        {
            // Handle Sides
            //drawingSession.DrawLine(box.CenterBottom, box.HandleBottom, AccentStroke);
            //drawingSession.DrawLine(box.CenterRight, box.HandleRight, AccentStroke);
            drawingSession.DrawLine(box.CenterTop, box.HandleTop, AccentStroke);
            //drawingSession.DrawLine(box.CenterLeft, box.HandleLeft, AccentStroke);

            // Lines
            drawingSession.DrawLine(box.LeftTop, box.RightTop, AccentStroke);
            drawingSession.DrawLine(box.RightTop, box.RightBottom, AccentStroke);
            drawingSession.DrawLine(box.RightBottom, box.LeftBottom, AccentStroke);
            drawingSession.DrawLine(box.LeftBottom, box.LeftTop, AccentStroke);

            // Handle Sides
            DrawNode2(drawingSession, box.HandleBottom);
            DrawNode2(drawingSession, box.HandleRight);
            DrawNode2(drawingSession, box.HandleTop);
            //DrawNode2(drawingSession, box.HandleLeft);

            const float d = 12 * 2;
            const float ds = d * d;

            // Sides
            if (box.SideLeftLengthSquared > ds)
            {
                DrawNode(drawingSession, box.CenterLeft);
            }

            if (box.SideTopLengthSquared > ds)
            {
                DrawNode(drawingSession, box.CenterTop);
            }

            if (box.SideRightLengthSquared > ds)
            {
                DrawNode(drawingSession, box.CenterRight);
            }

            if (box.SideBottomLengthSquared > ds)
            {
                DrawNode(drawingSession, box.CenterBottom);
            }

            // Corners
            DrawNode(drawingSession, box.LeftTop);
            DrawNode(drawingSession, box.RightTop);
            DrawNode(drawingSession, box.LeftBottom);
            DrawNode(drawingSession, box.RightBottom);
        }

        public static void DrawBox(this CanvasDrawingSession drawingSession, Box3 box)
        {
            // Handle Corners
            drawingSession.DrawLine(box.LeftTop, box.HandleLeftTop, AccentStroke);
            drawingSession.DrawLine(box.RightTop, box.HandleRightTop, AccentStroke);
            drawingSession.DrawLine(box.RightBottom, box.HandleRightBottom, AccentStroke);
            drawingSession.DrawLine(box.LeftBottom, box.HandleLeftBottom, AccentStroke);

            // Handle Sides
            drawingSession.DrawLine(box.CenterBottom, box.HandleBottom, AccentStroke);
            drawingSession.DrawLine(box.CenterRight, box.HandleRight, AccentStroke);
            drawingSession.DrawLine(box.CenterTop, box.HandleTop, AccentStroke);
            drawingSession.DrawLine(box.CenterLeft, box.HandleLeft, AccentStroke);

            // Lines
            drawingSession.DrawLine(box.LeftTop, box.RightTop, AccentStroke);
            drawingSession.DrawLine(box.RightTop, box.RightBottom, AccentStroke);
            drawingSession.DrawLine(box.RightBottom, box.LeftBottom, AccentStroke);
            drawingSession.DrawLine(box.LeftBottom, box.LeftTop, AccentStroke);

            // Handle Corners
            DrawNode2(drawingSession, box.HandleLeftTop);
            DrawNode2(drawingSession, box.HandleRightTop);
            DrawNode2(drawingSession, box.HandleLeftBottom);
            DrawNode2(drawingSession, box.HandleRightBottom);

            // Handle Sides
            DrawNode2(drawingSession, box.HandleBottom);
            DrawNode2(drawingSession, box.HandleRight);
            DrawNode2(drawingSession, box.HandleTop);
            DrawNode2(drawingSession, box.HandleLeft);

            const float d = 12 * 2;
            const float ds = d * d;

            // Sides
            if (box.SideLeftLengthSquared > ds)
            {
                DrawNode(drawingSession, box.CenterLeft);
            }

            if (box.SideTopLengthSquared > ds)
            {
                DrawNode(drawingSession, box.CenterTop);
            }

            if (box.SideRightLengthSquared > ds)
            {
                DrawNode(drawingSession, box.CenterRight);
            }

            if (box.SideBottomLengthSquared > ds)
            {
                DrawNode(drawingSession, box.CenterBottom);
            }

            // Corners
            DrawNode(drawingSession, box.LeftTop);
            DrawNode(drawingSession, box.RightTop);
            DrawNode(drawingSession, box.LeftBottom);
            DrawNode(drawingSession, box.RightBottom);
        }
    }
}