using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace FanKit.Transformer.TestApp
{
    public static class MatrixHelper
    {
        public static void ToLineX(this Line line, float y, double width)
        {
            line.X1 = 0d;
            line.Y1 = y;
            line.X1 = width;
            line.Y2 = y;
        }

        public static void ToLineY(this Line line, float x, double height)
        {
            line.X1 = x;
            line.Y1 = 0d;
            line.X2 = x;
            line.Y1 = height;
        }

        public static void Rect(this FrameworkElement rectangle, Bounds t)
        {
            Canvas.SetLeft(rectangle, System.Math.Min(t.Right, t.Left));
            Canvas.SetTop(rectangle, System.Math.Min(t.Bottom, t.Top));
            rectangle.Width = System.Math.Abs(t.Right - t.Left);
            rectangle.Height = System.Math.Abs(t.Bottom - t.Top);
        }

        public static void Rect(this FrameworkElement rectangle, Rectangle t)
        {
            Canvas.SetLeft(rectangle, t.X);
            Canvas.SetTop(rectangle, t.Y);
            rectangle.Width = t.Width;
            rectangle.Height = t.Height;
        }

        public static void Points3(this Polygon polygon, Triangle t)
        {
            polygon.Points[0] = t.LeftTop.ToPoint();
            polygon.Points[1] = t.RightTop.ToPoint();
            polygon.Points[2] = t.LeftBottom.ToPoint();
        }

        public static void Points4(this Polygon polygon, Quadrilateral t)
        {
            polygon.Points[0] = t.LeftTop.ToPoint();
            polygon.Points[1] = t.RightTop.ToPoint();
            polygon.Points[3] = t.LeftBottom.ToPoint();
            polygon.Points[2] = t.RightBottom.ToPoint();
        }

        public static void Points4(this Polygon polygon, TransformedBounds t)
        {
            polygon.Points[0] = t.LeftTop.ToPoint();
            polygon.Points[1] = t.RightTop.ToPoint();
            polygon.Points[3] = t.LeftBottom.ToPoint();
            polygon.Points[2] = t.RightBottom.ToPoint();
        }

        public static void SetPosition(this Ellipse ellipse, float x, float y)
        {
            Canvas.SetLeft(ellipse, x - 12d);
            Canvas.SetTop(ellipse, y - 12d);
        }

        public static void SetPosition(this Ellipse ellipse, Vector2 point)
        {
            Canvas.SetLeft(ellipse, point.X - 12d);
            Canvas.SetTop(ellipse, point.Y - 12d);
        }

        public static Matrix ToMatrix(this Matrix3x2 matrix)
        {
            return new Matrix(
                matrix.M11,
                matrix.M12,
                matrix.M21,
                matrix.M22,
                matrix.M31,
                matrix.M32);
        }
    }
}