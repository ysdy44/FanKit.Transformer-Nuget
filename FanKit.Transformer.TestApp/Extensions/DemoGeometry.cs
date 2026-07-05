using FanKit.Transformer.Curves;
using Microsoft.Graphics.Canvas.Geometry;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace FanKit.Transformer.TestApp
{
    public class DemoGeometry : List<List<Segment0>>, IPathBuilder
    {
        PathReceiver Receiver;

        public CanvasPathBuilder Builder;

        #region IPathBuilder
        public void BeginFigure(Vector2 startPoint)
        {
            this.Add(new List<Segment0>());
            this.Receiver = new PathReceiver(startPoint);

            this.Builder.BeginFigure(startPoint);
        }

        public void AddCubicBezier(Vector2 controlPoint1, Vector2 controlPoint2, Vector2 endPoint)
        {
            this.Last().Add(this.Receiver.AddCubicBezier0(controlPoint1, controlPoint2, endPoint, out this.Receiver));

            this.Builder.AddCubicBezier(controlPoint1, controlPoint2, endPoint);
        }

        public void AddQuadraticBezier(Vector2 controlPoint, Vector2 endPoint)
        {
            this.Last().Add(this.Receiver.AddQuadraticBezier0(controlPoint, endPoint, out this.Receiver));

            this.Builder.AddQuadraticBezier(controlPoint, endPoint);
        }

        public void AddLine(Vector2 endPoint)
        {
            this.Last().Add(this.Receiver.AddLine0(endPoint, out this.Receiver));

            this.Builder.AddLine(endPoint);
        }

        public void EndFigure(bool isClosed)
        {
            if (isClosed)
            {
                this.Last().Add(this.Receiver.EndFigure0());

                this.Builder.EndFigure(CanvasFigureLoop.Closed);
            }
            else
            {
                this.Builder.EndFigure(CanvasFigureLoop.Open);
            }
        }
        #endregion
    }
}