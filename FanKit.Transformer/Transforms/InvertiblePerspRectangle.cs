using FanKit.Transformer.Mathematics;
using System.Numerics;

namespace FanKit.Transformer.Transforms
{
    public class InvertiblePerspectiveRect
    {
        InvertiblePerspRectMatrix3x3 Core;

        public Matrix4x4 HomographyMatrix => this.Core;
        public void FindHomography(Quadrilateral source, float destinationWidth, float destinationHeight) => this.Core = new InvertiblePerspRectMatrix3x3(source, destinationWidth, destinationHeight);
        public void FindHomography(Quadrilateral source, Rectangle destination) => this.Core = new InvertiblePerspRectMatrix3x3(source, destination);
    }
}