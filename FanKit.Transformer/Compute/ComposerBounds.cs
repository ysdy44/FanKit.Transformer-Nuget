using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace FanKit.Transformer.Compute
{
    internal partial class ComposerBounds
    {
        internal Bounds SourceBounds;

        internal Bounds StartingBounds;
        internal Bounds Bounds;

        Matrix2x2 HostSourceNorm;
        Matrix2x2 HostDestNorm;

        CropController Controller;

        internal readonly M2x2 Host;

        internal ComposerBounds(M2x2 host)
        {
            this.Host = host;
        }

        void FindHomography()
        {
            this.HostDestNorm = this.Bounds.Normalize();
            this.Host.Matrix = this.HostSourceNorm.Map(this.HostDestNorm);
        }

        #region Bounds.Set
        internal void ST0(Vector2 translate)
        {
            this.Host.Matrix = Matrix2x2.CreateTranslation(translate);

            this.StartingBounds = this.Bounds;
            this.Bounds = Bounds.Translate(this.StartingBounds, this.Host.Matrix.TranslateX, this.Host.Matrix.TranslateY);
        }
        internal void ST1(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.Host.Matrix = Matrix2x2.CreateTranslation(translate);

            this.StartingBounds = this.Bounds;
            this.Bounds = Bounds.Translate(this.StartingBounds, this.Host.Matrix.TranslateX, this.Host.Matrix.TranslateY);

            indicator.ChangeXY(this.Bounds, mode);
        }

        internal void STX0(float translateX)
        {
            this.Host.Matrix = Matrix2x2.CreateTranslation(translateX, 0f);

            this.StartingBounds = this.Bounds;
            this.Bounds = Bounds.TranslateX(this.StartingBounds, this.Host.Matrix.TranslateX);
        }
        internal void STX1(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.Host.Matrix = Matrix2x2.CreateTranslation(translateX, 0f);

            this.StartingBounds = this.Bounds;
            this.Bounds = Bounds.TranslateX(this.StartingBounds, this.Host.Matrix.TranslateX);

            indicator.ChangeX(this.Bounds, mode);
        }

        internal void STY0(float translateY)
        {
            this.Host.Matrix = Matrix2x2.CreateTranslation(0f, translateY);

            this.StartingBounds = this.Bounds;
            this.Bounds = Bounds.TranslateY(this.StartingBounds, this.Host.Matrix.TranslateY);
        }
        internal void STY1(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.Host.Matrix = Matrix2x2.CreateTranslation(0f, translateY);

            this.StartingBounds = this.Bounds;
            this.Bounds = Bounds.TranslateY(this.StartingBounds, this.Host.Matrix.TranslateY);

            indicator.ChangeY(this.Bounds, mode);
        }

        internal void SF0(Matrix2x2 matrix)
        {
            this.Host.Matrix = matrix;

            this.StartingBounds = this.Bounds;
            this.Bounds = Bounds.Transform(this.StartingBounds, this.Host.Matrix);
        }
        internal void SF1(IIndicator indicator, BoxMode mode, Matrix2x2 matrix)
        {
            this.Host.Matrix = matrix;

            this.StartingBounds = this.Bounds;
            this.Bounds = Bounds.Transform(this.StartingBounds, this.Host.Matrix);

            indicator.ChangeAll(this.Bounds, mode);
        }

        internal void SW(IIndicator indicator, BoxMode mode, float value, bool keepRatio)
        {
            this.StartingBounds = this.Bounds;
            this.Bounds = indicator.CreateWidth(this.StartingBounds, mode, value, keepRatio);

            this.HostSourceNorm = this.StartingBounds.Normalize();
            this.FindHomography();

            indicator.ChangeXYWH(this.Bounds, mode);
        }
        internal void SH(IIndicator indicator, BoxMode mode, float value, bool keepRatio)
        {
            this.StartingBounds = this.Bounds;
            this.Bounds = indicator.CreateHeight(this.StartingBounds, mode, value, keepRatio);

            this.HostSourceNorm = this.StartingBounds.Normalize();
            this.FindHomography();

            indicator.ChangeXYWH(this.Bounds, mode);
        }
        #endregion

        #region Bounds.Transform
        internal void CT()
        {
            this.StartingBounds = this.Bounds;

            this.Host.Matrix = Matrix2x2.Identity;
        }

        internal void CF0()
        {
            this.StartingBounds = this.Bounds;

            this.HostSourceNorm = this.StartingBounds.Normalize();
            this.Host.Matrix = Matrix2x2.Identity;
        }

        internal void TD0(Vector2 startingPoint, Vector2 point)
        {
            this.Host.Matrix = Matrix2x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
            this.T();
        }
        internal void TD1(IIndicator indicator, BoxMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.Host.Matrix = Matrix2x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
            this.T();
            indicator.ChangeXY(this.Bounds, mode);
        }

        internal void T0(Vector2 translate)
        {
            this.Host.Matrix = Matrix2x2.CreateTranslation(translate);
            this.T();
        }
        internal void T1(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.Host.Matrix = Matrix2x2.CreateTranslation(translate);
            this.T();
            indicator.ChangeXY(this.Bounds, mode);
        }

        internal void TXY0(float translateX, float translateY)
        {
            this.Host.Matrix = Matrix2x2.CreateTranslation(translateX, translateY);
            this.T();
        }
        internal void TXY1(IIndicator indicator, BoxMode mode, float translateX, float translateY)
        {
            this.Host.Matrix = Matrix2x2.CreateTranslation(translateX, translateY);
            this.T();
            indicator.ChangeXY(this.Bounds, mode);
        }

        internal void TX0(float translateX)
        {
            this.Host.Matrix = Matrix2x2.CreateTranslation(translateX, 0f);
            this.TX();
        }
        internal void TX1(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.Host.Matrix = Matrix2x2.CreateTranslation(translateX, 0f);
            this.TX();
            indicator.ChangeXY(this.Bounds, mode);
        }

        internal void TY0(float translateY)
        {
            this.Host.Matrix = Matrix2x2.CreateTranslation(0f, translateY);
            this.TY();
        }
        internal void TY1(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.Host.Matrix = Matrix2x2.CreateTranslation(0f, translateY);
            this.TY();
            indicator.ChangeXY(this.Bounds, mode);
        }

        internal void F(Matrix2x2 matrix)
        {
            this.Host.Matrix = matrix;

            this.Bounds = Bounds.Transform(this.StartingBounds, this.Host.Matrix);
        }

        private void T()
        {
            this.Bounds = Bounds.Translate(this.StartingBounds, this.Host.Matrix.TranslateX, this.Host.Matrix.TranslateY);
        }
        private void TX()
        {
            this.Bounds = Bounds.TranslateX(this.StartingBounds, this.Host.Matrix.TranslateX);
        }
        private void TY()
        {
            this.Bounds = Bounds.TranslateY(this.StartingBounds, this.Host.Matrix.TranslateY);
        }
        #endregion

        #region Bounds.Transform2
        internal void CF1(CropMode mode)
        {
            this.StartingBounds = this.Bounds;

            this.HostSourceNorm = this.StartingBounds.Normalize();
            this.Host.Matrix = Matrix2x2.Identity;

            this.Controller = new CropController(this.Bounds, mode);
        }

        internal void TWH0(Vector2 point, bool keepRatio, bool centeredScaling)
        {
            this.Bounds = this.Controller.Crop(this.StartingBounds, point, keepRatio, centeredScaling);

            this.FindHomography();
        }
        internal void TWH1(IIndicator indicator, BoxMode mode, Vector2 point, bool keepRatio, bool centeredScaling)
        {
            this.Bounds = this.Controller.Crop(this.StartingBounds, point, keepRatio, centeredScaling);

            this.FindHomography();

            indicator.ChangeXYWH(this.Bounds, mode);
        }
        #endregion
    }
}