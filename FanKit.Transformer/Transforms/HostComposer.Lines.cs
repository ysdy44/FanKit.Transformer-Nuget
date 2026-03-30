using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace FanKit.Transformer.Transforms
{
    partial class HostComposer
    {
        public Vector2 LinePoint0 => this.Line.Point0;
        public Vector2 LinePoint1 => this.Line.Point1;

        #region Lines.Set
        public void LineSetTranslation(Vector2 translate)
        {
            this.Line.ST0(translate);
        }
        public void LineSetTranslation(IIndicator indicator, RowLineMode mode, Vector2 translate)
        {
            this.Line.ST1(indicator, mode, translate);
        }
        public void LineSetTranslation(IIndicator indicator, ColumnLineMode mode, Vector2 translate)
        {
            this.Line.ST2(indicator, mode, translate);
        }

        public void LineSetTranslationX(float translateX)
        {
            this.Line.STX0(translateX);
        }
        public void LineSetTranslationX(IIndicator indicator, RowLineMode mode, float translateX)
        {
            this.Line.STX1(indicator, mode, translateX);
        }
        public void LineSetTranslationX(IIndicator indicator, ColumnLineMode mode, float translateX)
        {
            this.Line.STX2(indicator, mode, translateX);
        }

        public void LineSetTranslationY(float translateY)
        {
            this.Line.STY0(translateY);
        }
        public void LineSetTranslationY(IIndicator indicator, RowLineMode mode, float translateY)
        {
            this.Line.STY1(indicator, mode, translateY);
        }
        public void LineSetTranslationY(IIndicator indicator, ColumnLineMode mode, float translateY)
        {
            this.Line.STY2(indicator, mode, translateY);
        }

        public void LineSetWidth(IIndicator indicator, RowLineMode mode, float value)
        {
            this.Line.SW(indicator, mode, value);
        }
        public void LineSetHeight(IIndicator indicator, ColumnLineMode mode, float value)
        {
            this.Line.SH(indicator, mode, value);
        }

        public void LineSetRotation(IIndicator indicator, float rotationAngleInDegrees)
        {
            this.Line.SR0(indicator, rotationAngleInDegrees);
        }
        public void LineSetRotation(IIndicator indicator, RowLineMode mode, float rotationAngleInDegrees)
        {
            this.Line.SR1(indicator, mode, rotationAngleInDegrees);
        }
        public void LineSetRotation(IIndicator indicator, ColumnLineMode mode, float rotationAngleInDegrees)
        {
            this.Line.SR2(indicator, mode, rotationAngleInDegrees);
        }
        #endregion

        #region Lines.Transform
        public void LineCacheTranslation()
        {
            this.Line.CT();
        }

        public void LineTranslate(Vector2 startingPoint, Vector2 point)
        {
            this.Line.TD0(startingPoint, point);
        }
        public void LineTranslate(IIndicator indicator, RowLineMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.Line.TD1(indicator, mode, startingPoint, point);
        }
        public void LineTranslate(IIndicator indicator, ColumnLineMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.Line.TD2(indicator, mode, startingPoint, point);
        }

        public void LineTranslate(Vector2 translate)
        {
            this.Line.T0(translate);
        }
        public void LineTranslate(IIndicator indicator, RowLineMode mode, Vector2 translate)
        {
            this.Line.T1(indicator, mode, translate);
        }
        public void LineTranslate(IIndicator indicator, ColumnLineMode mode, Vector2 translate)
        {
            this.Line.T2(indicator, mode, translate);
        }

        public void LineTranslate(float translateX, float translateY)
        {
            this.Line.TXY0(translateX, translateY);
        }
        public void LineTranslate(IIndicator indicator, RowLineMode mode, float translateX, float translateY)
        {
            this.Line.TXY1(indicator, mode, translateX, translateY);
        }
        public void LineTranslate(IIndicator indicator, ColumnLineMode mode, float translateX, float translateY)
        {
            this.Line.TXY2(indicator, mode, translateX, translateY);
        }

        public void LineTranslateX(float translateX)
        {
            this.Line.TX0(translateX);
        }
        public void LineTranslateX(IIndicator indicator, RowLineMode mode, float translateX)
        {
            this.Line.TX1(indicator, mode, translateX);
        }
        public void LineTranslateX(IIndicator indicator, ColumnLineMode mode, float translateX)
        {
            this.Line.TX2(indicator, mode, translateX);
        }

        public void LineTranslateY(float translateY)
        {
            this.Line.TY0(translateY);
        }
        public void LineTranslateY(IIndicator indicator, RowLineMode mode, float translateY)
        {
            this.Line.TY1(indicator, mode, translateY);
        }
        public void LineTranslateY(IIndicator indicator, ColumnLineMode mode, float translateY)
        {
            this.Line.TY2(indicator, mode, translateY);
        }
        #endregion

        #region Lines.Transform2
        public void LineCacheRotation(Vector2 point)
        {
            this.Line.CR(point);
        }

        public void LineCacheElongation0()
        {
            this.Line.CE0();
        }

        public void LineCacheElongation1()
        {
            this.Line.CE1();
        }

        public void LineCacheMovement()
        {
            this.Line.CM();
        }

        public void LineRotate(Vector2 point, float stepFrequency = float.NaN)
        {
            this.Line.R0(point, stepFrequency);
        }
        public void LineRotate(IIndicator indicator, RowLineMode mode, Vector2 point, float stepFrequency = float.NaN)
        {
            this.Line.R1(indicator, mode, point, stepFrequency);
        }
        public void LineRotate(IIndicator indicator, ColumnLineMode mode, Vector2 point, float stepFrequency = float.NaN)
        {
            this.Line.R2(indicator, mode, point, stepFrequency);
        }

        public void LineElongatePoint0(Vector2 startingPoint, Vector2 point)
        {
            this.Line.E00(startingPoint, point);
        }
        public void LineElongatePoint0(IIndicator indicator, RowLineMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.Line.E10(indicator, mode, startingPoint, point);
        }
        public void LineElongatePoint0(IIndicator indicator, ColumnLineMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.Line.E20(indicator, mode, startingPoint, point);
        }

        public void LineElongatePoint1(Vector2 startingPoint, Vector2 point)
        {
            this.Line.E01(startingPoint, point);
        }
        public void LineElongatePoint1(IIndicator indicator, RowLineMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.Line.E11(indicator, mode, startingPoint, point);
        }
        public void LineElongatePoint1(IIndicator indicator, ColumnLineMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.Line.E21(indicator, mode, startingPoint, point);
        }

        public void LineMovePoint0(Vector2 point)
        {
            this.Line.M00(point);
        }
        public void LineMovePoint0(IIndicator indicator, RowLineMode mode, Vector2 point)
        {
            this.Line.M10(indicator, mode, point);
        }
        public void LineMovePoint0(IIndicator indicator, ColumnLineMode mode, Vector2 point)
        {
            this.Line.M20(indicator, mode, point);
        }

        public void LineMovePoint1(Vector2 point)
        {
            this.Line.M01(point);
        }
        public void LineMovePoint1(IIndicator indicator, RowLineMode mode, Vector2 point)
        {
            this.Line.M11(indicator, mode, point);
        }
        public void LineMovePoint1(IIndicator indicator, ColumnLineMode mode, Vector2 point)
        {
            this.Line.M21(indicator, mode, point);
        }
        #endregion
    }
}