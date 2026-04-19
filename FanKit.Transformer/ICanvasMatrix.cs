using System.Numerics;

namespace FanKit.Transformer
{
    public interface ICanvasMatrix
    {
        Matrix3x2 Matrix { get; }

        float Scale(float value);
        Vector2 Scale(Vector2 value);

        Vector2 Transform(Vector2 position);
        Vector2 Transform(float xPosition, float yPosition);
        Node Transform(Node node);
        Triangle Transform(Triangle triangle);
        Quadrilateral Transform(Quadrilateral quad);

        Matrix3x2 TransformTranslation(Vector2 translate);
        Matrix3x2 TransformTranslation(float translateX, float translateY);
    }
}