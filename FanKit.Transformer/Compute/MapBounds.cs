using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace FanKit.Transformer.Compute
{
    public abstract partial class MapBounds
    {
        internal Bounds StartingBounds;
        internal Bounds Bounds;

        internal Matrix2x2 DestNorm;

        internal Matrix2x2 StartingMatrix;
        internal Matrix2x2 Matrix;

        internal Matrix2x2 Host;

        internal abstract void Find();

        #region Bounds.Initialize
        internal void UD(Bounds destination)
        {
            this.StartingBounds = this.Bounds = destination;

            this.Find();

            this.Host = Matrix2x2.Identity;
        }
        #endregion

        #region Bounds.Set
        internal void ST0(Vector2 translate)
        {
            this.Host = Matrix2x2.CreateTranslation(translate);

            this.StartingBounds = this.Bounds;
            this.Bounds = Bounds.Translate(this.StartingBounds, this.Host.TranslateX, this.Host.TranslateY);

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.Translate(this.StartingMatrix, this.Host.TranslateX, this.Host.TranslateY);
        }
        internal void ST1(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.Host = Matrix2x2.CreateTranslation(translate);

            this.StartingBounds = this.Bounds;
            this.Bounds = Bounds.Translate(this.StartingBounds, this.Host.TranslateX, this.Host.TranslateY);

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.Translate(this.StartingMatrix, this.Host.TranslateX, this.Host.TranslateY);

            indicator.ChangeXY(this.Bounds, mode);
        }

        internal void STX0(float translateX)
        {
            this.Host = Matrix2x2.CreateTranslation(translateX, 0f);

            this.StartingBounds = this.Bounds;
            this.Bounds = Bounds.TranslateX(this.StartingBounds, this.Host.TranslateX);

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.TranslateX);
        }
        internal void STX1(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.Host = Matrix2x2.CreateTranslation(translateX, 0f);

            this.StartingBounds = this.Bounds;
            this.Bounds = Bounds.TranslateX(this.StartingBounds, this.Host.TranslateX);

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.TranslateX);

            indicator.ChangeX(this.Bounds, mode);
        }

        internal void STY0(float translateY)
        {
            this.Host = Matrix2x2.CreateTranslation(0f, translateY);

            this.StartingBounds = this.Bounds;
            this.Bounds = Bounds.TranslateY(this.StartingBounds, this.Host.TranslateY);

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.TranslateY);
        }
        internal void STY1(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.Host = Matrix2x2.CreateTranslation(0f, translateY);

            this.StartingBounds = this.Bounds;
            this.Bounds = Bounds.TranslateY(this.StartingBounds, this.Host.TranslateY);

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.TranslateY);

            indicator.ChangeY(this.Bounds, mode);
        }

        internal void SF0(Matrix2x2 matrix)
        {
            this.Host = matrix;

            this.StartingBounds = this.Bounds;
            this.Bounds = Bounds.Transform(this.StartingBounds, this.Host);

            this.StartingMatrix = this.Matrix;
            this.Matrix = this.StartingMatrix * this.Host;
        }
        internal void SF1(IIndicator indicator, BoxMode mode, Matrix2x2 matrix)
        {
            this.Host = matrix;

            this.StartingBounds = this.Bounds;
            this.Bounds = Bounds.Transform(this.StartingBounds, this.Host);

            this.StartingMatrix = this.Matrix;
            this.Matrix = this.StartingMatrix * this.Host;

            indicator.ChangeAll(this.Bounds, mode);
        }

        internal void SW(IIndicator indicator, BoxMode mode, float value, bool keepRatio)
        {
            this.StartingBounds = this.Bounds;
            this.Bounds = indicator.CreateWidth(this.StartingBounds, mode, value, keepRatio);

            this.Find();

            indicator.ChangeXYWH(this.Bounds, mode);
        }
        internal void SH(IIndicator indicator, BoxMode mode, float value, bool keepRatio)
        {
            this.StartingBounds = this.Bounds;
            this.Bounds = indicator.CreateHeight(this.StartingBounds, mode, value, keepRatio);

            this.Find();

            indicator.ChangeXYWH(this.Bounds, mode);
        }
        #endregion

        #region Bounds.Transform
        internal void CT()
        {
            this.StartingBounds = this.Bounds;
            this.StartingMatrix = this.Matrix;

            this.Host = Matrix2x2.Identity;
        }

        internal void CF()
        {
            this.StartingBounds = this.Bounds;
            this.StartingMatrix = this.Matrix;

            this.Host = Matrix2x2.Identity;
        }

        internal void TD0(Vector2 startingPoint, Vector2 point)
        {
            this.Host = Matrix2x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
            this.T();
        }
        internal void TD1(IIndicator indicator, BoxMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.Host = Matrix2x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
            this.T();
            indicator.ChangeXY(this.Bounds, mode);
        }

        internal void T0(Vector2 translate)
        {
            this.Host = Matrix2x2.CreateTranslation(translate);
            this.T();
        }
        internal void T1(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.Host = Matrix2x2.CreateTranslation(translate);
            this.T();
            indicator.ChangeXY(this.Bounds, mode);
        }

        internal void TXY0(float translateX, float translateY)
        {
            this.Host = Matrix2x2.CreateTranslation(translateX, translateY);
            this.T();
        }
        internal void TXY1(IIndicator indicator, BoxMode mode, float translateX, float translateY)
        {
            this.Host = Matrix2x2.CreateTranslation(translateX, translateY);
            this.T();
            indicator.ChangeXY(this.Bounds, mode);
        }

        internal void TX0(float translateX)
        {
            this.Host = Matrix2x2.CreateTranslation(translateX, 0f);
            this.TX();
        }
        internal void TX1(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.Host = Matrix2x2.CreateTranslation(translateX, 0f);
            this.TX();
            indicator.ChangeXY(this.Bounds, mode);
        }

        internal void TY0(float translateY)
        {
            this.Host = Matrix2x2.CreateTranslation(0f, translateY);
            this.TY();
        }
        internal void TY1(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.Host = Matrix2x2.CreateTranslation(0f, translateY);
            this.TY();
            indicator.ChangeXY(this.Bounds, mode);
        }

        internal void Transform2(Matrix2x2 matrix)
        {
            this.Host = matrix;

            this.Bounds = Bounds.Transform(this.StartingBounds, this.Host);
            this.Matrix = this.StartingMatrix * this.Host;
        }

        private void T()
        {
            this.Bounds = Bounds.Translate(this.StartingBounds, this.Host.TranslateX, this.Host.TranslateY);
            this.Matrix = Math.Translate(this.StartingMatrix, this.Host.TranslateX, this.Host.TranslateY);
        }
        private void TX()
        {
            this.Bounds = Bounds.TranslateX(this.StartingBounds, this.Host.TranslateX);
            this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.TranslateX);
        }
        private void TY()
        {
            this.Bounds = Bounds.TranslateY(this.StartingBounds, this.Host.TranslateY);
            this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.TranslateY);
        }
        #endregion
    }
}