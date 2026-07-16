using FanKit.Transformer.Compute;
using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace FanKit.Transformer.Transforms
{
    public partial class HostLine
    {
        public Vector2 Point0
        {
            get => this.Line.Point0;
            set => this.Line.Point0 = value;
        }
        public Vector2 Point1
        {
            get => this.Line.Point1;
            set => this.Line.Point1 = value;
        }

        public float TranslationX => this.Host.Matrix.M31;
        public float TranslationY => this.Host.Matrix.M32;
        public Matrix3x2 TransformMatrix => this.Host.Matrix;

        readonly M3x2 Host;
        readonly ComposerLine Line;

        public HostLine()
        {
            this.Host = new M3x2();
            this.Line = new ComposerLine(this.Host);
        }

        #region Lines.Set
        public void SetTranslation(Vector2 translate) => this.Line.ST0(translate);
        public void SetTranslation(IIndicator indicator, RowLineAnchorMode anchorMode, Vector2 translate) => this.Line.ST1(indicator, anchorMode, translate);
        public void SetTranslation(IIndicator indicator, ColumnLineAnchorMode anchorMode, Vector2 translate) => this.Line.ST2(indicator, anchorMode, translate);

        public void SetTranslationX(float translateX) => this.Line.STX0(translateX);
        public void SetTranslationX(IIndicator indicator, RowLineAnchorMode anchorMode, float translateX) => this.Line.STX1(indicator, anchorMode, translateX);
        public void SetTranslationX(IIndicator indicator, ColumnLineAnchorMode anchorMode, float translateX) => this.Line.STX2(indicator, anchorMode, translateX);

        public void SetTranslationY(float translateY) => this.Line.STY0(translateY);
        public void SetTranslationY(IIndicator indicator, RowLineAnchorMode anchorMode, float translateY) => this.Line.STY1(indicator, anchorMode, translateY);
        public void SetTranslationY(IIndicator indicator, ColumnLineAnchorMode anchorMode, float translateY) => this.Line.STY2(indicator, anchorMode, translateY);

        public void SetWidth(IIndicator indicator, RowLineAnchorMode anchorMode, float value) => this.Line.SW(indicator, anchorMode, value);
        public void SetHeight(IIndicator indicator, ColumnLineAnchorMode anchorMode, float value) => this.Line.SH(indicator, anchorMode, value);

        public void SetRotation(IIndicator indicator, float rotationAngleInDegrees) => this.Line.SR0(indicator, rotationAngleInDegrees);
        public void SetRotation(IIndicator indicator, RowLineAnchorMode anchorMode, float rotationAngleInDegrees) => this.Line.SR1(indicator, anchorMode, rotationAngleInDegrees);
        public void SetRotation(IIndicator indicator, ColumnLineAnchorMode anchorMode, float rotationAngleInDegrees) => this.Line.SR2(indicator, anchorMode, rotationAngleInDegrees);
        #endregion

        #region Lines.Transform
        public void CacheTranslation() => this.Line.CT();

        public void Translate(Vector2 startingPoint, Vector2 point) => this.Line.TD0(startingPoint, point);
        public void Translate(IIndicator indicator, RowLineAnchorMode anchorMode, Vector2 startingPoint, Vector2 point) => this.Line.TD1(indicator, anchorMode, startingPoint, point);
        public void Translate(IIndicator indicator, ColumnLineAnchorMode anchorMode, Vector2 startingPoint, Vector2 point) => this.Line.TD2(indicator, anchorMode, startingPoint, point);

        public void Translate(Vector2 translate) => this.Line.T0(translate);
        public void Translate(IIndicator indicator, RowLineAnchorMode anchorMode, Vector2 translate) => this.Line.T1(indicator, anchorMode, translate);
        public void Translate(IIndicator indicator, ColumnLineAnchorMode anchorMode, Vector2 translate) => this.Line.T2(indicator, anchorMode, translate);

        public void Translate(float translateX, float translateY) => this.Line.TXY0(translateX, translateY);
        public void Translate(IIndicator indicator, RowLineAnchorMode anchorMode, float translateX, float translateY) => this.Line.TXY1(indicator, anchorMode, translateX, translateY);
        public void Translate(IIndicator indicator, ColumnLineAnchorMode anchorMode, float translateX, float translateY) => this.Line.TXY2(indicator, anchorMode, translateX, translateY);

        public void TranslateX(float translateX) => this.Line.TX0(translateX);
        public void TranslateX(IIndicator indicator, RowLineAnchorMode anchorMode, float translateX) => this.Line.TX1(indicator, anchorMode, translateX);
        public void TranslateX(IIndicator indicator, ColumnLineAnchorMode anchorMode, float translateX) => this.Line.TX2(indicator, anchorMode, translateX);

        public void TranslateY(float translateY) => this.Line.TY0(translateY);
        public void TranslateY(IIndicator indicator, RowLineAnchorMode anchorMode, float translateY) => this.Line.TY1(indicator, anchorMode, translateY);
        public void TranslateY(IIndicator indicator, ColumnLineAnchorMode anchorMode, float translateY) => this.Line.TY2(indicator, anchorMode, translateY);
        #endregion

        #region Lines.Transform2
        public void CacheRotation(Vector2 point) => this.Line.CR(point);

        public void CacheElongation0() => this.Line.CE0();

        public void CacheElongation1() => this.Line.CE1();

        public void CacheMovement() => this.Line.CM();

        public void Rotate(Vector2 point, float stepFrequency = float.NaN) => this.Line.R0(point, stepFrequency);
        public void Rotate(IIndicator indicator, RowLineAnchorMode anchorMode, Vector2 point, float stepFrequency = float.NaN) => this.Line.R1(indicator, anchorMode, point, stepFrequency);
        public void Rotate(IIndicator indicator, ColumnLineAnchorMode anchorMode, Vector2 point, float stepFrequency = float.NaN) => this.Line.R2(indicator, anchorMode, point, stepFrequency);

        public void ElongatePoint0(Vector2 startingPoint, Vector2 point) => this.Line.E00(startingPoint, point);
        public void ElongatePoint0(IIndicator indicator, RowLineAnchorMode anchorMode, Vector2 startingPoint, Vector2 point) => this.Line.E10(indicator, anchorMode, startingPoint, point);
        public void ElongatePoint0(IIndicator indicator, ColumnLineAnchorMode anchorMode, Vector2 startingPoint, Vector2 point) => this.Line.E20(indicator, anchorMode, startingPoint, point);

        public void ElongatePoint1(Vector2 startingPoint, Vector2 point) => this.Line.E01(startingPoint, point);
        public void ElongatePoint1(IIndicator indicator, RowLineAnchorMode anchorMode, Vector2 startingPoint, Vector2 point) => this.Line.E11(indicator, anchorMode, startingPoint, point);
        public void ElongatePoint1(IIndicator indicator, ColumnLineAnchorMode anchorMode, Vector2 startingPoint, Vector2 point) => this.Line.E21(indicator, anchorMode, startingPoint, point);

        public void MovePoint0(Vector2 point) => this.Line.M00(point);
        public void MovePoint0(IIndicator indicator, RowLineAnchorMode anchorMode, Vector2 point) => this.Line.M10(indicator, anchorMode, point);
        public void MovePoint0(IIndicator indicator, ColumnLineAnchorMode anchorMode, Vector2 point) => this.Line.M20(indicator, anchorMode, point);

        public void MovePoint1(Vector2 point) => this.Line.M01(point);
        public void MovePoint1(IIndicator indicator, RowLineAnchorMode anchorMode, Vector2 point) => this.Line.M11(indicator, anchorMode, point);
        public void MovePoint1(IIndicator indicator, ColumnLineAnchorMode anchorMode, Vector2 point) => this.Line.M21(indicator, anchorMode, point);
        #endregion
    }
}