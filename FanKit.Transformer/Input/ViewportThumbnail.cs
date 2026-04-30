using FanKit.Transformer.Mathematics;
using System.Numerics;

namespace FanKit.Transformer.Input
{
    public readonly struct ViewportThumbnail
    {
        readonly Quadrilateral Viewport;

        public readonly Quadrilateral Quadrilateral;
        public readonly Matrix3x2 Matrix;

        public ViewportThumbnail(float viewportWidth, float viewportHeight, ICanvasInverseMatrix inverseMatrix, Coordinate thumbCoord)
        {
            this.Viewport = new Quadrilateral(viewportWidth, viewportHeight, inverseMatrix.InverseMatrix);

            this.Quadrilateral = thumbCoord.Transform(this.Viewport);
            this.Matrix = thumbCoord.Transform(inverseMatrix.InverseMatrix);
        }

        public ViewportThumbnail(Viewport viewport, ICanvasInverseMatrix inverseMatrix, Coordinate thumbCoord)
        {
            this.Viewport = new Quadrilateral(viewport.ViewportWidth, viewport.ViewportHeight, inverseMatrix.InverseMatrix);

            this.Quadrilateral = thumbCoord.Transform(this.Viewport);
            this.Matrix = thumbCoord.Transform(inverseMatrix.InverseMatrix);
        }
    }
}