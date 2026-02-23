using System.Numerics;

namespace FanKit.Transformer.Curves
{
    public interface IPathBuilder
    {
        void BeginFigure(Vector2 startPoint);
        void AddCubicBezier(Vector2 controlPoint1, Vector2 controlPoint2, Vector2 endPoint);
        void AddQuadraticBezier(Vector2 controlPoint, Vector2 endPoint);
        void AddLine(Vector2 endPoint);
        void EndFigure(bool isClosed);
    }
}