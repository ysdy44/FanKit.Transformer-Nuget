using System.Numerics;

namespace FanKit.Transformer
{
    public interface ICanvasInverseMatrix
    {
        Matrix3x2 InverseMatrix { get; }

        float InverseScale(float value);
        Vector2 InverseScale(Vector2 value);

        Vector2 InverseTransform(Vector2 position);
        Vector2 InverseTransform(float xPosition, float yPosition);
        Node InverseTransform(Node node);
        Triangle InverseTransform(Triangle triangle);
        Quadrilateral InverseTransform(Quadrilateral quad);
    }
}