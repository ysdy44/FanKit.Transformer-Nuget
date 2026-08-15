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
    public class PathBuilder : IPathBuilder0, IPathBuilder1, IDisposable
    {
        public readonly CanvasPathBuilder Builder;
        public PathBuilder(ICanvasResourceCreator resourceCreator) => this.Builder = new CanvasPathBuilder(resourceCreator);

        #region IPathBuilder0
        public void BeginFigure(Vector2 startPoint) => this.Builder.BeginFigure(startPoint);
        public void AddCubicBezier(Vector2 controlPoint1, Vector2 controlPoint2, Vector2 endPoint) => this.Builder.AddCubicBezier(controlPoint1, controlPoint2, endPoint);
        public void AddQuadraticBezier(Vector2 controlPoint, Vector2 endPoint) => this.Builder.AddQuadraticBezier(controlPoint, endPoint);
        public void AddLine(Vector2 endPoint) => this.Builder.AddLine(endPoint);
        public void EndFigure(bool isClosed) => this.Builder.EndFigure(isClosed ? CanvasFigureLoop.Closed : CanvasFigureLoop.Open);
        #endregion

        #region IPathBuilder1
        public void BeginFigure(ICanvasMatrix canvasMatrix, Vector2 startPoint) => this.Builder.BeginFigure(canvasMatrix.Transform(startPoint));
        public void AddCubicBezier(ICanvasMatrix canvasMatrix, Vector2 controlPoint1, Vector2 controlPoint2, Vector2 endPoint) => this.Builder.AddCubicBezier(canvasMatrix.Transform(controlPoint1), canvasMatrix.Transform(controlPoint2), canvasMatrix.Transform(endPoint));
        public void AddQuadraticBezier(ICanvasMatrix canvasMatrix, Vector2 controlPoint, Vector2 endPoint) => this.Builder.AddQuadraticBezier(canvasMatrix.Transform(controlPoint), canvasMatrix.Transform(endPoint));
        public void AddLine(ICanvasMatrix canvasMatrix, Vector2 endPoint) => this.Builder.AddLine(canvasMatrix.Transform(endPoint));
        public void EndFigure(ICanvasMatrix canvasMatrix, bool isClosed) => this.Builder.EndFigure(isClosed ? CanvasFigureLoop.Closed : CanvasFigureLoop.Open);
        #endregion

        public void Dispose() => this.Builder.Dispose();
    }
}