using FanKit.Transformer.Mathematics;
using System.Numerics;

namespace FanKit.Transformer.Input
{
    public readonly struct ViewportThumbnail
    {
        readonly Quadrilateral Viewport;

        public readonly Quadrilateral Quadrilateral;
        public readonly Matrix3x2 Matrix;

        public ViewportThumbnail(float viewportWidth, float viewportHeight, ICanvasInverseMatrix matrix, Coordinate thumbCoord)
        {
            this.Viewport = new Quadrilateral(viewportWidth, viewportHeight, matrix.InverseMatrix);

            this.Quadrilateral = thumbCoord.Transform(this.Viewport);
            this.Matrix = thumbCoord.Transform(matrix.InverseMatrix);
        }

        public ViewportThumbnail(Viewport viewport, ICanvasInverseMatrix matrix, Coordinate thumbCoord)
        {
            this.Viewport = new Quadrilateral(viewport.ViewportWidth, viewport.ViewportHeight, matrix.InverseMatrix);

            this.Quadrilateral = thumbCoord.Transform(this.Viewport);
            this.Matrix = thumbCoord.Transform(matrix.InverseMatrix);
        }
    }
}