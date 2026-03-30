using FanKit.Transformer.Mathematics;
using System.Numerics;

namespace FanKit.Transformer.Transforms
{
    public class InvertiblePerspQuadrilateral
    {
        readonly PerspQuadrilateral Core = new PerspQuadrilateral();

        public Matrix4x4 HomographyMatrix => this.Core.m;
        public void FindHomography(Quadrilateral source, Quadrilateral destination) => this.Core.FindHomography(source, destination);
    }
}