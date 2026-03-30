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
        public void SetTranslation(Vector2 translate)
        {
            this.Line.ST0(translate);
        }
        public void SetTranslation(IIndicator indicator, RowLineMode mode, Vector2 translate)
        {
            this.Line.ST1(indicator, mode, translate);
        }
        public void SetTranslation(IIndicator indicator, ColumnLineMode mode, Vector2 translate)
        {
            this.Line.ST2(indicator, mode, translate);
        }

        public void SetTranslationX(float translateX)
        {
            this.Line.STX0(translateX);
        }
        public void SetTranslationX(IIndicator indicator, RowLineMode mode, float translateX)
        {
            this.Line.STX1(indicator, mode, translateX);
        }
        public void SetTranslationX(IIndicator indicator, ColumnLineMode mode, float translateX)
        {
            this.Line.STX2(indicator, mode, translateX);
        }

        public void SetTranslationY(float translateY)
        {
            this.Line.STY0(translateY);
        }
        public void SetTranslationY(IIndicator indicator, RowLineMode mode, float translateY)
        {
            this.Line.STY1(indicator, mode, translateY);
        }
        public void SetTranslationY(IIndicator indicator, ColumnLineMode mode, float translateY)
        {
            this.Line.STY2(indicator, mode, translateY);
        }

        public void SetWidth(IIndicator indicator, RowLineMode mode, float value)
        {
            this.Line.SW(indicator, mode, value);
        }
        public void SetHeight(IIndicator indicator, ColumnLineMode mode, float value)
        {
            this.Line.SH(indicator, mode, value);
        }

        public void SetRotation(IIndicator indicator, float rotationAngleInDegrees)
        {
            this.Line.SR0(indicator, rotationAngleInDegrees);
        }
        public void SetRotation(IIndicator indicator, RowLineMode mode, float rotationAngleInDegrees)
        {
            this.Line.SR1(indicator, mode, rotationAngleInDegrees);
        }
        public void SetRotation(IIndicator indicator, ColumnLineMode mode, float rotationAngleInDegrees)
        {
            this.Line.SR2(indicator, mode, rotationAngleInDegrees);
        }
        #endregion

        #region Lines.Transform
        public void CacheTranslation()
        {
            this.Line.CT();
        }

        public void Translate(Vector2 startingPoint, Vector2 point)
        {
            this.Line.TD0(startingPoint, point);
        }
        public void Translate(IIndicator indicator, RowLineMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.Line.TD1(indicator, mode, startingPoint, point);
        }
        public void Translate(IIndicator indicator, ColumnLineMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.Line.TD2(indicator, mode, startingPoint, point);
        }

        public void Translate(Vector2 translate)
        {
            this.Line.T0(translate);
        }
        public void Translate(IIndicator indicator, RowLineMode mode, Vector2 translate)
        {
            this.Line.T1(indicator, mode, translate);
        }
        public void Translate(IIndicator indicator, ColumnLineMode mode, Vector2 translate)
        {
            this.Line.T2(indicator, mode, translate);
        }

        public void Translate(float translateX, float translateY)
        {
            this.Line.TXY0(translateX, translateY);
        }
        public void Translate(IIndicator indicator, RowLineMode mode, float translateX, float translateY)
        {
            this.Line.TXY1(indicator, mode, translateX, translateY);
        }
        public void Translate(IIndicator indicator, ColumnLineMode mode, float translateX, float translateY)
        {
            this.Line.TXY2(indicator, mode, translateX, translateY);
        }

        public void TranslateX(float translateX)
        {
            this.Line.TX0(translateX);
        }
        public void TranslateX(IIndicator indicator, RowLineMode mode, float translateX)
        {
            this.Line.TX1(indicator, mode, translateX);
        }
        public void TranslateX(IIndicator indicator, ColumnLineMode mode, float translateX)
        {
            this.Line.TX2(indicator, mode, translateX);
        }

        public void TranslateY(float translateY)
        {
            this.Line.TY0(translateY);
        }
        public void TranslateY(IIndicator indicator, RowLineMode mode, float translateY)
        {
            this.Line.TY1(indicator, mode, translateY);
        }
        public void TranslateY(IIndicator indicator, ColumnLineMode mode, float translateY)
        {
            this.Line.TY2(indicator, mode, translateY);
        }
        #endregion

        #region Lines.Transform2
        public void CacheRotation(Vector2 point)
        {
            this.Line.CR(point);
        }

        public void CacheElongation0()
        {
            this.Line.CE0();
        }

        public void CacheElongation1()
        {
            this.Line.CE1();
        }

        public void CacheMovement()
        {
            this.Line.CM();
        }

        public void Rotate(Vector2 point, float stepFrequency = float.NaN)
        {
            this.Line.R0(point, stepFrequency);
        }
        public void Rotate(IIndicator indicator, RowLineMode mode, Vector2 point, float stepFrequency = float.NaN)
        {
            this.Line.R1(indicator, mode, point, stepFrequency);
        }
        public void Rotate(IIndicator indicator, ColumnLineMode mode, Vector2 point, float stepFrequency = float.NaN)
        {
            this.Line.R2(indicator, mode, point, stepFrequency);
        }

        public void ElongatePoint0(Vector2 startingPoint, Vector2 point)
        {
            this.Line.E00(startingPoint, point);
        }
        public void ElongatePoint0(IIndicator indicator, RowLineMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.Line.E10(indicator, mode, startingPoint, point);
        }
        public void ElongatePoint0(IIndicator indicator, ColumnLineMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.Line.E20(indicator, mode, startingPoint, point);
        }

        public void ElongatePoint1(Vector2 startingPoint, Vector2 point)
        {
            this.Line.E01(startingPoint, point);
        }
        public void ElongatePoint1(IIndicator indicator, RowLineMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.Line.E11(indicator, mode, startingPoint, point);
        }
        public void ElongatePoint1(IIndicator indicator, ColumnLineMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.Line.E21(indicator, mode, startingPoint, point);
        }

        public void MovePoint0(Vector2 point)
        {
            this.Line.M00(point);
        }
        public void MovePoint0(IIndicator indicator, RowLineMode mode, Vector2 point)
        {
            this.Line.M10(indicator, mode, point);
        }
        public void MovePoint0(IIndicator indicator, ColumnLineMode mode, Vector2 point)
        {
            this.Line.M20(indicator, mode, point);
        }

        public void MovePoint1(Vector2 point)
        {
            this.Line.M01(point);
        }
        public void MovePoint1(IIndicator indicator, RowLineMode mode, Vector2 point)
        {
            this.Line.M11(indicator, mode, point);
        }
        public void MovePoint1(IIndicator indicator, ColumnLineMode mode, Vector2 point)
        {
            this.Line.M21(indicator, mode, point);
        }
        #endregion
    }
}