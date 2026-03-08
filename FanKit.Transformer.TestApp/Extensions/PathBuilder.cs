using FanKit.Transformer.Curves;
using FanKit.Transformer.Demos;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Transforms;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Xaml.Media;

namespace FanKit.Transformer.TestApp
{
    public class PathBuilder : IPathBuilder, IDisposable
    {
        public readonly CanvasPathBuilder Builder;
        public PathBuilder(ICanvasResourceCreator resourceCreator) => Builder = new CanvasPathBuilder(resourceCreator);
        public void BeginFigure(Vector2 startPoint) => Builder.BeginFigure(startPoint);
        public void AddCubicBezier(Vector2 controlPoint1, Vector2 controlPoint2, Vector2 endPoint) => Builder.AddCubicBezier(controlPoint1, controlPoint2, endPoint);
        public void AddQuadraticBezier(Vector2 controlPoint, Vector2 endPoint) => Builder.AddQuadraticBezier(controlPoint, endPoint);
        public void AddLine(Vector2 endPoint) => Builder.AddLine(endPoint);
        public void EndFigure(bool isClosed) => Builder.EndFigure(isClosed ? CanvasFigureLoop.Closed : CanvasFigureLoop.Open);
        public void Dispose() => Builder.Dispose();
    }
}