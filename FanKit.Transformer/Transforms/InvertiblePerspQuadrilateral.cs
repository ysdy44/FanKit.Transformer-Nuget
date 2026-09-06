using FanKit.Transformer.Mathematics;
using System.Numerics;

namespace FanKit.Transformer.Transforms
{
    public class InvertiblePerspQuadrilateral
    {
        InvertiblePerspQuadrilateralMatrix3x3 Core;

        public Matrix4x4 HomographyMatrix => this.Core;
        public void FindHomography(Quadrilateral source, Quadrilateral destination) => this.Core = new InvertiblePerspQuadrilateralMatrix3x3(source, destination);
    }
}