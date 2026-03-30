using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Numerics;

namespace FanKit.Transformer.Compute
{
    internal partial class ComposerPoint
    {
        internal Vector2 StartingPoint;

        internal Vector2 Point;

        readonly M3x2 Host;

        internal ComposerPoint(M3x2 host)
        {
            this.Host = host;
        }

        #region Points.Set
        internal void ST0(Vector2 translate)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translate);

            this.StartingPoint = this.Point;
            this.Point = Math.Translate(this.StartingPoint, this.Host.Matrix.M31, this.Host.Matrix.M32);
        }
        internal void ST1(IIndicator indicator, Vector2 translate)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translate);

            this.StartingPoint = this.Point;
            this.Point = Math.Translate(this.StartingPoint, this.Host.Matrix.M31, this.Host.Matrix.M32);

            indicator.ChangeAll(this.Point);
        }

        internal void STX0(float translateX)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translateX, 0f);

            this.StartingPoint = this.Point;
            this.Point = Math.TranslateX(this.StartingPoint, this.Host.Matrix.M31);
        }
        internal void STX1(IIndicator indicator, float translateX)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translateX, 0f);

            this.StartingPoint = this.Point;
            this.Point = Math.TranslateX(this.StartingPoint, this.Host.Matrix.M31);

            indicator.ChangeAll(this.Point);
        }

        internal void STY0(float translateY)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(0f, translateY);

            this.StartingPoint = this.Point;
            this.Point = Math.TranslateY(this.StartingPoint, this.Host.Matrix.M32);
        }
        internal void STY1(IIndicator indicator, float translateY)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(0f, translateY);

            this.StartingPoint = this.Point;
            this.Point = Math.TranslateY(this.StartingPoint, this.Host.Matrix.M32);

            indicator.ChangeAll(this.Point);
        }
        #endregion

        #region Points.Transform
        internal void CT()
        {
            this.StartingPoint = this.Point;
        }

        internal void TD0(Vector2 startingPoint, Vector2 point)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
            this.T();
        }
        internal void TD1(IIndicator indicator, Vector2 startingPoint, Vector2 point)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
            this.T();
            indicator.ChangeAll(this.Point);
        }

        internal void T0(Vector2 translate)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translate);
            this.T();
        }
        internal void T1(IIndicator indicator, Vector2 translate)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translate);
            this.T();
            indicator.ChangeAll(this.Point);
        }

        internal void TXY0(float translateX, float translateY)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translateX, translateY);
            this.T();
        }
        internal void TXY1(IIndicator indicator, float translateX, float translateY)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translateX, translateY);
            this.T();
            indicator.ChangeAll(this.Point);
        }

        internal void TX0(float translateX)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translateX, 0f);
            this.TX();
        }
        internal void TX1(IIndicator indicator, float translateX)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translateX, 0f);
            this.TX();
            indicator.ChangeAll(this.Point);
        }

        internal void TY0(float translateY)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(0f, translateY);
            this.TY();
        }
        internal void TY1(IIndicator indicator, float translateY)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(0f, translateY);
            this.TY();
            indicator.ChangeAll(this.Point);
        }

        private void T()
        {
            this.Point = new Vector2(this.StartingPoint.X + this.Host.Matrix.M31, this.StartingPoint.Y + this.Host.Matrix.M32);
        }
        private void TX()
        {
            this.Point = new Vector2(this.StartingPoint.X + this.Host.Matrix.M31, this.StartingPoint.Y);
        }
        private void TY()
        {
            this.Point = new Vector2(this.StartingPoint.X, this.StartingPoint.Y + this.Host.Matrix.M32);
        }
        #endregion
    }
}