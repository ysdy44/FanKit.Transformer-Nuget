using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace FanKit.Transformer.Compute
{
    internal partial class ComposerLine
    {
        internal Vector2 StartingPoint0;
        internal Vector2 StartingPoint1;

        internal Vector2 Point0;
        internal Vector2 Point1;

        LineMatrix HostSourceNorm;
        PinchMatrix3x2 HostDestNorm;

        Vector2 Diff;
        LineControllerFoot Foot;
        LineController Controller;

        ControllerRadians Radians;

        readonly M3x2 Host;

        internal ComposerLine(M3x2 host)
        {
            this.Host = host;
        }

        void FindHomography0()
        {
            if (this.HostSourceNorm.IsEmpty)
            {
                this.Host.Matrix = Matrix3x2.Identity;
            }
            else
            {
                this.HostDestNorm = new PinchMatrix3x2(this.HostSourceNorm, this.StartingPoint1, this.Point0);
                if (this.HostDestNorm.IsEmpty)
                {
                    this.Host.Matrix = Matrix3x2.Identity;
                }
                else
                {
                    this.Host.Matrix = this.HostDestNorm;
                }
            }
        }

        void FindHomography1()
        {
            if (this.HostSourceNorm.IsEmpty)
            {
                this.Host.Matrix = Matrix3x2.Identity;
            }
            else
            {
                this.HostDestNorm = new PinchMatrix3x2(this.HostSourceNorm, this.StartingPoint0, this.Point1);
                if (this.HostDestNorm.IsEmpty)
                {
                    this.Host.Matrix = Matrix3x2.Identity;
                }
                else
                {
                    this.Host.Matrix = this.HostDestNorm;
                }
            }
        }

        void FindHomography()
        {
            if (this.HostSourceNorm.IsEmpty)
            {
                this.Host.Matrix = Matrix3x2.Identity;
            }
            else
            {
                this.HostDestNorm = new PinchMatrix3x2(this.HostSourceNorm, this.StartingPoint0, this.Point0, this.Point1);
                if (this.HostDestNorm.IsEmpty)
                {
                    this.Host.Matrix = Matrix3x2.Identity;
                }
                else
                {
                    this.Host.Matrix = this.HostDestNorm;
                }
            }
        }

        #region Lines.Set
        internal void ST0(Vector2 translate)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translate);

            this.StartingPoint0 = this.Point0;
            this.StartingPoint1 = this.Point1;

            this.Point0 = new Vector2(this.StartingPoint0.X + this.Host.Matrix.M31, this.StartingPoint0.Y + this.Host.Matrix.M32);
            this.Point1 = new Vector2(this.StartingPoint1.X + this.Host.Matrix.M31, this.StartingPoint1.Y + this.Host.Matrix.M32);
        }
        internal void ST1(IIndicator indicator, RowLineAnchorMode anchorMode, Vector2 translate)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translate);

            this.StartingPoint0 = this.Point0;
            this.StartingPoint1 = this.Point1;

            this.Point0 = new Vector2(this.StartingPoint0.X + this.Host.Matrix.M31, this.StartingPoint0.Y + this.Host.Matrix.M32);
            this.Point1 = new Vector2(this.StartingPoint1.X + this.Host.Matrix.M31, this.StartingPoint1.Y + this.Host.Matrix.M32);

            indicator.ChangeXY(this.Point0, this.Point1, anchorMode);
        }
        internal void ST2(IIndicator indicator, ColumnLineAnchorMode anchorMode, Vector2 translate)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translate);

            this.StartingPoint0 = this.Point0;
            this.StartingPoint1 = this.Point1;

            this.Point0 = new Vector2(this.StartingPoint0.X + this.Host.Matrix.M31, this.StartingPoint0.Y + this.Host.Matrix.M32);
            this.Point1 = new Vector2(this.StartingPoint1.X + this.Host.Matrix.M31, this.StartingPoint1.Y + this.Host.Matrix.M32);

            indicator.ChangeXY(this.Point0, this.Point1, anchorMode);
        }

        internal void STX0(float translateX)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translateX, 0f);

            this.StartingPoint0 = this.Point0;
            this.StartingPoint1 = this.Point1;

            this.Point0 = new Vector2(this.StartingPoint0.X + this.Host.Matrix.M31, this.StartingPoint0.Y);
            this.Point1 = new Vector2(this.StartingPoint1.X + this.Host.Matrix.M31, this.StartingPoint1.Y);
        }
        internal void STX1(IIndicator indicator, RowLineAnchorMode anchorMode, float translateX)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translateX, 0f);

            this.StartingPoint0 = this.Point0;
            this.StartingPoint1 = this.Point1;

            this.Point0 = new Vector2(this.StartingPoint0.X + this.Host.Matrix.M31, this.StartingPoint0.Y);
            this.Point1 = new Vector2(this.StartingPoint1.X + this.Host.Matrix.M31, this.StartingPoint1.Y);

            indicator.ChangeXY(this.Point0, this.Point1, anchorMode);
        }
        internal void STX2(IIndicator indicator, ColumnLineAnchorMode anchorMode, float translateX)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translateX, 0f);

            this.StartingPoint0 = this.Point0;
            this.StartingPoint1 = this.Point1;

            this.Point0 = new Vector2(this.StartingPoint0.X + this.Host.Matrix.M31, this.StartingPoint0.Y);
            this.Point1 = new Vector2(this.StartingPoint1.X + this.Host.Matrix.M31, this.StartingPoint1.Y);

            indicator.ChangeXY(this.Point0, this.Point1, anchorMode);
        }

        internal void STY0(float translateY)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(0f, translateY);

            this.StartingPoint0 = this.Point0;
            this.StartingPoint1 = this.Point1;

            this.Point0 = new Vector2(this.StartingPoint0.X, this.StartingPoint0.Y + this.Host.Matrix.M32);
            this.Point1 = new Vector2(this.StartingPoint1.X, this.StartingPoint1.Y + this.Host.Matrix.M32);
        }
        internal void STY1(IIndicator indicator, RowLineAnchorMode anchorMode, float translateY)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(0f, translateY);

            this.StartingPoint0 = this.Point0;
            this.StartingPoint1 = this.Point1;

            this.Point0 = new Vector2(this.StartingPoint0.X, this.StartingPoint0.Y + this.Host.Matrix.M32);
            this.Point1 = new Vector2(this.StartingPoint1.X, this.StartingPoint1.Y + this.Host.Matrix.M32);

            indicator.ChangeXY(this.Point0, this.Point1, anchorMode);
        }
        internal void STY2(IIndicator indicator, ColumnLineAnchorMode anchorMode, float translateY)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(0f, translateY);

            this.StartingPoint0 = this.Point0;
            this.StartingPoint1 = this.Point1;

            this.Point0 = new Vector2(this.StartingPoint0.X, this.StartingPoint0.Y + this.Host.Matrix.M32);
            this.Point1 = new Vector2(this.StartingPoint1.X, this.StartingPoint1.Y + this.Host.Matrix.M32);

            indicator.ChangeXY(this.Point0, this.Point1, anchorMode);
        }

        internal void SW(IIndicator indicator, RowLineAnchorMode anchorMode, float value)
        {
            this.StartingPoint0 = this.Point0;
            this.StartingPoint1 = this.Point1;

            switch (anchorMode)
            {
                case RowLineAnchorMode.Left:
                    this.Controller = new LineController(value, this.StartingPoint0, this.StartingPoint1, 0);
                    this.Point1 = this.Controller.GetPoint1();
                    this.Host.Matrix = this.Controller.Multiply();
                    break;
                case RowLineAnchorMode.Center:
                    this.Controller = new LineController(value, this.StartingPoint0, this.StartingPoint1, 1);
                    this.Point0 = this.Controller.GetPoint0();
                    this.Point1 = this.Controller.GetPoint1();
                    this.Host.Matrix = this.Controller.Multiply();
                    break;
                case RowLineAnchorMode.Right:
                    this.Controller = new LineController(value, this.StartingPoint0, this.StartingPoint1, 2);
                    this.Point0 = this.Controller.GetPoint0();
                    this.Host.Matrix = this.Controller.Multiply();
                    break;
                default:
                    break;
            }

            indicator.ChangeAll(this.Point0, this.Point1, anchorMode);
        }
        internal void SH(IIndicator indicator, ColumnLineAnchorMode anchorMode, float value)
        {
            this.StartingPoint0 = this.Point0;
            this.StartingPoint1 = this.Point1;

            switch (anchorMode)
            {
                case ColumnLineAnchorMode.Top:
                    this.Controller = new LineController(value, this.StartingPoint0, this.StartingPoint1, 0);
                    this.Point1 = this.Controller.GetPoint1();
                    this.Host.Matrix = this.Controller.Multiply();
                    break;
                case ColumnLineAnchorMode.Center:
                    this.Controller = new LineController(value, this.StartingPoint0, this.StartingPoint1, 1);
                    this.Point0 = this.Controller.GetPoint0();
                    this.Point1 = this.Controller.GetPoint1();
                    this.Host.Matrix = this.Controller.Multiply();
                    break;
                case ColumnLineAnchorMode.Bottom:
                    this.Controller = new LineController(value, this.StartingPoint0, this.StartingPoint1, 2);
                    this.Point0 = this.Controller.GetPoint0();
                    this.Host.Matrix = this.Controller.Multiply();
                    break;
                default:
                    break;
            }

            indicator.ChangeAll(this.Point0, this.Point1, anchorMode);
        }

        internal void SR0(IIndicator indicator, float rotationAngleInDegrees)
        {
            this.Host.Matrix = indicator.CreateRotation(rotationAngleInDegrees);

            this.StartingPoint0 = this.Point0;
            this.StartingPoint1 = this.Point1;

            this.Point0 = Vector2.Transform(this.StartingPoint0, this.Host.Matrix);
            this.Point1 = Vector2.Transform(this.StartingPoint1, this.Host.Matrix);
        }
        internal void SR1(IIndicator indicator, RowLineAnchorMode anchorMode, float rotationAngleInDegrees)
        {
            this.Host.Matrix = indicator.CreateRotation(rotationAngleInDegrees);

            this.StartingPoint0 = this.Point0;
            this.StartingPoint1 = this.Point1;

            this.Point0 = Vector2.Transform(this.StartingPoint0, this.Host.Matrix);
            this.Point1 = Vector2.Transform(this.StartingPoint1, this.Host.Matrix);

            indicator.ChangeAll(this.Point0, this.Point1, anchorMode);
        }
        internal void SR2(IIndicator indicator, ColumnLineAnchorMode anchorMode, float rotationAngleInDegrees)
        {
            this.Host.Matrix = indicator.CreateRotation(rotationAngleInDegrees);

            this.StartingPoint0 = this.Point0;
            this.StartingPoint1 = this.Point1;

            this.Point0 = Vector2.Transform(this.StartingPoint0, this.Host.Matrix);
            this.Point1 = Vector2.Transform(this.StartingPoint1, this.Host.Matrix);

            indicator.ChangeAll(this.Point0, this.Point1, anchorMode);
        }
        #endregion

        #region Lines.Transform
        internal void CT()
        {
            this.StartingPoint0 = this.Point0;
            this.StartingPoint1 = this.Point1;

            this.Host.Matrix = Matrix3x2.Identity;
        }

        internal void TD0(Vector2 startingPoint, Vector2 point)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
            this.T();
        }
        internal void TD1(IIndicator indicator, RowLineAnchorMode anchorMode, Vector2 startingPoint, Vector2 point)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
            this.T();
            indicator.ChangeAll(this.Point0, this.Point1, anchorMode);
        }
        internal void TD2(IIndicator indicator, ColumnLineAnchorMode anchorMode, Vector2 startingPoint, Vector2 point)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
            this.T();
            indicator.ChangeAll(this.Point0, this.Point1, anchorMode);
        }

        internal void T0(Vector2 translate)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translate);
            this.T();
        }
        internal void T1(IIndicator indicator, RowLineAnchorMode anchorMode, Vector2 translate)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translate);
            this.T();
            indicator.ChangeAll(this.Point0, this.Point1, anchorMode);
        }
        internal void T2(IIndicator indicator, ColumnLineAnchorMode anchorMode, Vector2 translate)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translate);
            this.T();
            indicator.ChangeAll(this.Point0, this.Point1, anchorMode);
        }

        internal void TXY0(float translateX, float translateY)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translateX, translateY);
            this.T();
        }
        internal void TXY1(IIndicator indicator, RowLineAnchorMode anchorMode, float translateX, float translateY)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translateX, translateY);
            this.T();
            indicator.ChangeAll(this.Point0, this.Point1, anchorMode);
        }
        internal void TXY2(IIndicator indicator, ColumnLineAnchorMode anchorMode, float translateX, float translateY)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translateX, translateY);
            this.T();
            indicator.ChangeAll(this.Point0, this.Point1, anchorMode);
        }

        internal void TX0(float translateX)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translateX, 0f);
            this.TX();
        }
        internal void TX1(IIndicator indicator, RowLineAnchorMode anchorMode, float translateX)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translateX, 0f);
            this.TX();
            indicator.ChangeAll(this.Point0, this.Point1, anchorMode);
        }
        internal void TX2(IIndicator indicator, ColumnLineAnchorMode anchorMode, float translateX)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translateX, 0f);
            this.TX();
            indicator.ChangeAll(this.Point0, this.Point1, anchorMode);
        }

        internal void TY0(float translateY)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(0f, translateY);
            this.TY();
        }
        internal void TY1(IIndicator indicator, RowLineAnchorMode anchorMode, float translateY)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(0f, translateY);
            this.TY();
            indicator.ChangeAll(this.Point0, this.Point1, anchorMode);
        }
        internal void TY2(IIndicator indicator, ColumnLineAnchorMode anchorMode, float translateY)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(0f, translateY);
            this.TY();
            indicator.ChangeAll(this.Point0, this.Point1, anchorMode);
        }

        private void T()
        {
            this.Point0 = new Vector2(this.StartingPoint0.X + this.Host.Matrix.M31, this.StartingPoint0.Y + this.Host.Matrix.M32);
            this.Point1 = new Vector2(this.StartingPoint1.X + this.Host.Matrix.M31, this.StartingPoint1.Y + this.Host.Matrix.M32);
        }
        private void TX()
        {
            this.Point0 = new Vector2(this.StartingPoint0.X + this.Host.Matrix.M31, this.StartingPoint0.Y);
            this.Point1 = new Vector2(this.StartingPoint1.X + this.Host.Matrix.M31, this.StartingPoint1.Y);
        }
        private void TY()
        {
            this.Point0 = new Vector2(this.StartingPoint0.X, this.StartingPoint0.Y + this.Host.Matrix.M32);
            this.Point1 = new Vector2(this.StartingPoint1.X, this.StartingPoint1.Y + this.Host.Matrix.M32);
        }
        #endregion

        #region Lines.Transform2
        internal void CR(Vector2 point)
        {
            this.StartingPoint0 = this.Point0;
            this.StartingPoint1 = this.Point1;

            this.Host.Matrix = Matrix3x2.Identity;

            this.Controller = new LineController(this.StartingPoint0, this.StartingPoint1, point);
        }

        internal void CE0()
        {
            this.StartingPoint0 = this.Point0;
            this.StartingPoint1 = this.Point1;

            this.HostSourceNorm = new LineMatrix(this.StartingPoint1, this.StartingPoint0);
            this.Host.Matrix = Matrix3x2.Identity;

            this.Controller = new LineController(this.StartingPoint1, this.StartingPoint0);
        }

        internal void CE1()
        {
            this.StartingPoint0 = this.Point0;
            this.StartingPoint1 = this.Point1;

            this.HostSourceNorm = new LineMatrix(this.StartingPoint0, this.StartingPoint1);
            this.Host.Matrix = Matrix3x2.Identity;

            this.Controller = new LineController(this.StartingPoint0, this.StartingPoint1);
        }

        internal void CM()
        {
            this.StartingPoint0 = this.Point0;
            this.StartingPoint1 = this.Point1;

            this.HostSourceNorm = new LineMatrix(this.StartingPoint0, this.StartingPoint1);
            this.Host.Matrix = Matrix3x2.Identity;
        }

        internal void R0(Vector2 point, float stepFrequency = float.NaN)
        {
            this.Radians = this.Controller.ToRadians(point, stepFrequency);
            this.Host.Matrix = this.Controller.Rotate(this.Radians);

            this.Point0 = Vector2.Transform(this.StartingPoint0, this.Host.Matrix);
            this.Point1 = Vector2.Transform(this.StartingPoint1, this.Host.Matrix);
        }
        internal void R1(IIndicator indicator, RowLineAnchorMode anchorMode, Vector2 point, float stepFrequency = float.NaN)
        {
            this.Radians = this.Controller.ToRadians(point, stepFrequency);
            this.Host.Matrix = this.Controller.Rotate(this.Radians);

            this.Point0 = Vector2.Transform(this.StartingPoint0, this.Host.Matrix);
            this.Point1 = Vector2.Transform(this.StartingPoint1, this.Host.Matrix);

            indicator.ChangeAll(this.Point0, this.Point1, anchorMode);
        }
        internal void R2(IIndicator indicator, ColumnLineAnchorMode anchorMode, Vector2 point, float stepFrequency = float.NaN)
        {
            this.Radians = this.Controller.ToRadians(point, stepFrequency);
            this.Host.Matrix = this.Controller.Rotate(this.Radians);

            this.Point0 = Vector2.Transform(this.StartingPoint0, this.Host.Matrix);
            this.Point1 = Vector2.Transform(this.StartingPoint1, this.Host.Matrix);

            indicator.ChangeAll(this.Point0, this.Point1, anchorMode);
        }

        internal void E00(Vector2 startingPoint, Vector2 point)
        {
            this.Diff = startingPoint - this.StartingPoint0;
            this.Foot = new LineControllerFoot(this.Controller, this.StartingPoint1, point, this.Diff);
            this.Point0 = this.Foot.Foot;

            this.FindHomography0();
        }
        internal void E10(IIndicator indicator, RowLineAnchorMode anchorMode, Vector2 startingPoint, Vector2 point)
        {
            this.Diff = startingPoint - this.StartingPoint0;
            this.Foot = new LineControllerFoot(this.Controller, this.StartingPoint1, point, this.Diff);
            this.Point0 = this.Foot.Foot;

            this.FindHomography0();

            indicator.ChangeAll(this.Point0, this.Point1, anchorMode);
        }
        internal void E20(IIndicator indicator, ColumnLineAnchorMode anchorMode, Vector2 startingPoint, Vector2 point)
        {
            this.Diff = startingPoint - this.StartingPoint0;
            this.Foot = new LineControllerFoot(this.Controller, this.StartingPoint1, point, this.Diff);
            this.Point0 = this.Foot.Foot;

            this.FindHomography0();

            indicator.ChangeAll(this.Point0, this.Point1, anchorMode);
        }

        internal void E01(Vector2 startingPoint, Vector2 point)
        {
            this.Diff = startingPoint - this.StartingPoint1;
            this.Foot = new LineControllerFoot(this.Controller, this.StartingPoint0, point, this.Diff);
            this.Point1 = this.Foot.Foot;

            this.FindHomography1();
        }
        internal void E11(IIndicator indicator, RowLineAnchorMode anchorMode, Vector2 startingPoint, Vector2 point)
        {
            this.Diff = startingPoint - this.StartingPoint1;
            this.Foot = new LineControllerFoot(this.Controller, this.StartingPoint0, point, this.Diff);
            this.Point1 = this.Foot.Foot;

            this.FindHomography1();

            indicator.ChangeAll(this.Point0, this.Point1, anchorMode);
        }
        internal void E21(IIndicator indicator, ColumnLineAnchorMode anchorMode, Vector2 startingPoint, Vector2 point)
        {
            this.Diff = startingPoint - this.StartingPoint1;
            this.Foot = new LineControllerFoot(this.Controller, this.StartingPoint0, point, this.Diff);
            this.Point1 = this.Foot.Foot;

            this.FindHomography1();

            indicator.ChangeAll(this.Point0, this.Point1, anchorMode);
        }

        internal void M00(Vector2 point)
        {
            this.Point0 = point;

            this.FindHomography();
        }
        internal void M10(IIndicator indicator, RowLineAnchorMode anchorMode, Vector2 point)
        {
            this.Point0 = point;

            this.FindHomography();

            indicator.ChangeAll(this.Point0, this.Point1, anchorMode);
        }
        internal void M20(IIndicator indicator, ColumnLineAnchorMode anchorMode, Vector2 point)
        {
            this.Point0 = point;

            this.FindHomography();

            indicator.ChangeAll(this.Point0, this.Point1, anchorMode);
        }

        internal void M01(Vector2 point)
        {
            this.Point1 = point;

            this.FindHomography();
        }
        internal void M11(IIndicator indicator, RowLineAnchorMode anchorMode, Vector2 point)
        {
            this.Point1 = point;

            this.FindHomography();

            indicator.ChangeAll(this.Point0, this.Point1, anchorMode);
        }
        internal void M21(IIndicator indicator, ColumnLineAnchorMode anchorMode, Vector2 point)
        {
            this.Point1 = point;

            this.FindHomography();

            indicator.ChangeAll(this.Point0, this.Point1, anchorMode);
        }
        #endregion
    }
}