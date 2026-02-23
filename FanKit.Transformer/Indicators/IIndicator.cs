using System;
using System.Numerics;

namespace FanKit.Transformer.Indicators
{
    public partial interface IIndicator
    {
        event EventHandler<IndicatorSizeType> SizeTypeChanged;

        event EventHandler<float> XChanged;
        event EventHandler<float> YChanged;

        event EventHandler<float> WidthChanged;
        event EventHandler<float> HeightChanged;

        event EventHandler<float> RotationChanged;
        event EventHandler<float> SkewChanged;

        IndicatorSizeType SizeType { get; }

        float X { get; }
        float Y { get; }
        float Width { get; }
        float Height { get; }
        float Rotation { get; }
        float Skew { get; }

        Matrix3x2 CreateRotation(float rotationAngleInDegrees);

        void ClearAll();
        void ChangeAll(Vector2 point);

        void ChangeAll(Vector2 point0, Vector2 point1, RowLineMode mode);
        void ChangeXY(Vector2 point0, Vector2 point1, RowLineMode mode);

        void ChangeAll(Vector2 point0, Vector2 point1, ColumnLineMode mode);
        void ChangeXY(Vector2 point0, Vector2 point1, ColumnLineMode mode);

        #region Bounds
        void ChangeX(Bounds bounds, BoxMode mode);
        void ChangeY(Bounds bounds, BoxMode mode);
        void ChangeXY(Bounds bounds, BoxMode mode);
        void ChangeXYWH(Bounds bounds, BoxMode mode);
        //void ChangeXYWHRS(Bounds bounds, BoxMode mode);
        void ChangeAll(Bounds bounds, BoxMode mode);

        Bounds CreateWidth(Bounds bounds, BoxMode mode, float value, bool keepRatio);
        Bounds CreateHeight(Bounds bounds, BoxMode mode, float value, bool keepRatio);
        //Bounds CreateSkew(Bounds bounds, BoxMode mode, float skewAngleInDegrees, float minimum, float maximum);
        #endregion

        #region Triangles
        void ChangeX(Triangle triangle, BoxMode mode);
        void ChangeY(Triangle triangle, BoxMode mode);
        void ChangeXY(Triangle triangle, BoxMode mode);
        void ChangeXYWH(Triangle triangle, BoxMode mode);
        void ChangeXYWHRS(Triangle triangle, BoxMode mode);
        void ChangeAll(Triangle triangle, BoxMode mode);

        Triangle CreateWidth(Triangle triangle, BoxMode mode, float value, bool keepRatio);
        Triangle CreateHeight(Triangle triangle, BoxMode mode, float value, bool keepRatio);
        Triangle CreateSkew(Triangle triangle, BoxMode mode, float skewAngleInDegrees, float minimum, float maximum);
        #endregion

        #region Quadrilaterals
        void ChangeX(Quadrilateral quad, BoxMode mode);
        void ChangeY(Quadrilateral quad, BoxMode mode);
        void ChangeXY(Quadrilateral quad, BoxMode mode);
        void ChangeXYWH(Quadrilateral quad, BoxMode mode);
        void ChangeXYWHRS(Quadrilateral quad, BoxMode mode);
        void ChangeAll(Quadrilateral quad, BoxMode mode);

        Quadrilateral CreateWidth(Quadrilateral quad, BoxMode mode, float value, bool keepRatio);
        Quadrilateral CreateHeight(Quadrilateral quad, BoxMode mode, float value, bool keepRatio);
        Quadrilateral CreateSkew(Quadrilateral quad, BoxMode mode, float skewAngleInDegrees, float minimum, float maximum);
        #endregion
    }
}