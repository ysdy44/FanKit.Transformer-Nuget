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

        void ChangeAll(Vector2 point0, Vector2 point1, RowLineAnchorMode anchorMode);
        void ChangeXY(Vector2 point0, Vector2 point1, RowLineAnchorMode anchorMode);

        void ChangeAll(Vector2 point0, Vector2 point1, ColumnLineAnchorMode anchorMode);
        void ChangeXY(Vector2 point0, Vector2 point1, ColumnLineAnchorMode anchorMode);

        #region Bounds
        void ChangeX(Bounds bounds, PanelAnchorMode anchorMode);
        void ChangeY(Bounds bounds, PanelAnchorMode anchorMode);
        void ChangeXY(Bounds bounds, PanelAnchorMode anchorMode);
        void ChangeXYWH(Bounds bounds, PanelAnchorMode anchorMode);
        //void ChangeXYWHRS(Bounds bounds, PanelAnchorMode anchorMode);
        void ChangeAll(Bounds bounds, PanelAnchorMode anchorMode);

        Bounds CreateWidth(Bounds bounds, PanelAnchorMode anchorMode, float value, bool keepRatio);
        Bounds CreateHeight(Bounds bounds, PanelAnchorMode anchorMode, float value, bool keepRatio);
        //Bounds CreateSkew(Bounds bounds, PanelAnchorMode anchorMode, float skewAngleInDegrees, float minimum, float maximum);
        #endregion

        #region Triangles
        void ChangeX(Triangle triangle, PanelAnchorMode anchorMode);
        void ChangeY(Triangle triangle, PanelAnchorMode anchorMode);
        void ChangeXY(Triangle triangle, PanelAnchorMode anchorMode);
        void ChangeXYWH(Triangle triangle, PanelAnchorMode anchorMode);
        void ChangeXYWHRS(Triangle triangle, PanelAnchorMode anchorMode);
        void ChangeAll(Triangle triangle, PanelAnchorMode anchorMode);

        Triangle CreateWidth(Triangle triangle, PanelAnchorMode anchorMode, float value, bool keepRatio);
        Triangle CreateHeight(Triangle triangle, PanelAnchorMode anchorMode, float value, bool keepRatio);
        Triangle CreateSkew(Triangle triangle, PanelAnchorMode anchorMode, float skewAngleInDegrees, float minimum, float maximum);
        #endregion

        #region Quadrilaterals
        void ChangeX(Quadrilateral quad, PanelAnchorMode anchorMode);
        void ChangeY(Quadrilateral quad, PanelAnchorMode anchorMode);
        void ChangeXY(Quadrilateral quad, PanelAnchorMode anchorMode);
        void ChangeXYWH(Quadrilateral quad, PanelAnchorMode anchorMode);
        void ChangeXYWHRS(Quadrilateral quad, PanelAnchorMode anchorMode);
        void ChangeAll(Quadrilateral quad, PanelAnchorMode anchorMode);

        Quadrilateral CreateWidth(Quadrilateral quad, PanelAnchorMode anchorMode, float value, bool keepRatio);
        Quadrilateral CreateHeight(Quadrilateral quad, PanelAnchorMode anchorMode, float value, bool keepRatio);
        Quadrilateral CreateSkew(Quadrilateral quad, PanelAnchorMode anchorMode, float skewAngleInDegrees, float minimum, float maximum);
        #endregion
    }
}