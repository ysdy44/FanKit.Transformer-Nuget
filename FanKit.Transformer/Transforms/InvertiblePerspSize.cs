using FanKit.Transformer.Mathematics;
using System.Numerics;

namespace FanKit.Transformer.Transforms
{
    public class InvertiblePerspSize
    {
        InvertiblePerspSizeMatrix3x3 Core;

        public Matrix4x4 HomographyMatrix => this.Core;
        public void FindHomography(Quadrilateral source, float destinationWidth, float destinationHeight) => this.Core = new InvertiblePerspSizeMatrix3x3(source, destinationWidth, destinationHeight);
    }
}