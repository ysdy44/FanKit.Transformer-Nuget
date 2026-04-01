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
    public partial class HostBounds
    {
        public int Count { get; private set; }

        public Bounds Destination => this.Panel.Bounds;

        public float TranslationX => this.Host.Matrix.TranslateX;
        public float TranslationY => this.Host.Matrix.TranslateY;
        public Matrix2x2 TransformMatrix => this.Host.Matrix;

        readonly M2x2 Host;
        readonly ComposerBounds Panel;

        public HostBounds()
        {
            this.Host = new M2x2();
            this.Panel = new ComposerBounds(this.Host);
        }

        #region Bounds.Reset
        public void Reset(Bounds source, Bounds bounds, Matrix2x2 matrix)
        {
            this.Count = 1;
            this.Panel.SourceBounds = source;

            this.Panel.StartingBounds = this.Panel.Bounds = bounds;

            this.Host.Matrix = matrix;
        }

        public void BeginExtend()
        {
            this.Count = 0;

            this.Panel.SourceBounds = Bounds.Infinity;
        }

        public void Extend(Bounds bounds)
        {
            switch (this.Count)
            {
                case 0:
                    this.Count = 1;
                    this.Panel.SourceBounds = Bounds.Infinity;

                    this.Eb(bounds);

                    this.Panel.StartingBounds = this.Panel.Bounds = bounds;

                    this.Host.Matrix = Matrix2x2.Identity;
                    break;
                case 1:
                    this.Count = 2;
                    this.Panel.SourceBounds = Bounds.Infinity;

                    this.Eb(this.Panel.Bounds);

                    this.Eb(bounds);

                    this.Host.Matrix = Matrix2x2.Identity;
                    break;
                case 2:
                default:
                    this.Count++;

                    this.Eb(bounds);

                    this.Host.Matrix = Matrix2x2.Identity;
                    break;
            }
        }

        public void Extend(Triangle triangle)
        {
            switch (this.Count)
            {
                case 0:
                    this.Count = 1;
                    this.Panel.SourceBounds = Bounds.Infinity;

                    this.E3(triangle);

                    this.Panel.StartingBounds = this.Panel.Bounds = new Bounds(triangle);

                    this.Host.Matrix = Matrix2x2.Identity;
                    break;
                case 1:
                    this.Count = 2;
                    this.Panel.SourceBounds = Bounds.Infinity;

                    this.Eb(this.Panel.Bounds);

                    this.E3(triangle);

                    this.Host.Matrix = Matrix2x2.Identity;
                    break;
                case 2:
                default:
                    this.Count++;

                    this.E3(triangle);

                    this.Host.Matrix = Matrix2x2.Identity;
                    break;
            }
        }

        public void Extend(Quadrilateral quad)
        {
            switch (this.Count)
            {
                case 0:
                    this.Count = 1;
                    this.Panel.SourceBounds = Bounds.Infinity;

                    this.E4(quad);

                    this.Panel.StartingBounds = this.Panel.Bounds = quad.ToBounds();

                    this.Host.Matrix = Matrix2x2.Identity;
                    break;
                case 1:
                    this.Count = 2;
                    this.Panel.SourceBounds = Bounds.Infinity;

                    this.Extend(this.Panel.Bounds);

                    this.E4(quad);

                    this.Host.Matrix = Matrix2x2.Identity;
                    break;
                default:
                    this.Count++;

                    this.E4(quad);

                    this.Host.Matrix = Matrix2x2.Identity;
                    break;
            }
        }

        private void Ex(Vector2 point)
        {
            if (this.Panel.SourceBounds.Left > point.X) this.Panel.SourceBounds.Left = point.X;
            if (this.Panel.SourceBounds.Top > point.Y) this.Panel.SourceBounds.Top = point.Y;
            if (this.Panel.SourceBounds.Right < point.X) this.Panel.SourceBounds.Right = point.X;
            if (this.Panel.SourceBounds.Bottom < point.Y) this.Panel.SourceBounds.Bottom = point.Y;
        }
        private void Eb(Bounds bounds)
        {
            if (this.Panel.SourceBounds.Left > bounds.Left) this.Panel.SourceBounds.Left = bounds.Left;
            if (this.Panel.SourceBounds.Top > bounds.Top) this.Panel.SourceBounds.Top = bounds.Top;
            if (this.Panel.SourceBounds.Right < bounds.Left) this.Panel.SourceBounds.Right = bounds.Left;
            if (this.Panel.SourceBounds.Bottom < bounds.Top) this.Panel.SourceBounds.Bottom = bounds.Top;

            if (this.Panel.SourceBounds.Left > bounds.Right) this.Panel.SourceBounds.Left = bounds.Right;
            if (this.Panel.SourceBounds.Top > bounds.Bottom) this.Panel.SourceBounds.Top = bounds.Bottom;
            if (this.Panel.SourceBounds.Right < bounds.Right) this.Panel.SourceBounds.Right = bounds.Right;
            if (this.Panel.SourceBounds.Bottom < bounds.Bottom) this.Panel.SourceBounds.Bottom = bounds.Bottom;
        }
        private void E3(Triangle triangle)
        {
            this.Ex(triangle.LeftTop);
            this.Ex(triangle.RightTop);
            this.Ex(triangle.LeftBottom);

            this.Ex(new Vector2(triangle.RightTop.X + triangle.LeftBottom.X - triangle.LeftTop.X,
                triangle.RightTop.Y + triangle.LeftBottom.Y - triangle.LeftTop.Y)); // Triangle -> Transformer
        }
        private void E4(Quadrilateral quad)
        {
            this.Ex(quad.LeftTop);
            this.Ex(quad.RightTop);
            this.Ex(quad.LeftBottom);

            this.Ex(quad.RightBottom);
        }

        public void EndExtend()
        {
            switch (this.Count)
            {
                case 0:
                    break;
                case 1:
                    break;
                default:
                    this.Panel.StartingBounds = this.Panel.Bounds = this.Panel.SourceBounds;

                    this.Host.Matrix = Matrix2x2.Identity;
                    break;
            }
        }
        #endregion

        #region Bounds.Set
        public void SetTranslation(Vector2 translate) => this.Panel.ST0(translate);
        public void SetTranslation(IIndicator indicator, BoxMode mode, Vector2 translate) => this.Panel.ST1(indicator, mode, translate);

        public void SetTranslationX(float translateX) => this.Panel.STX0(translateX);
        public void SetTranslationX(IIndicator indicator, BoxMode mode, float translateX) => this.Panel.STX1(indicator, mode, translateX);

        public void SetTranslationY(float translateY) => this.Panel.STY0(translateY);
        public void SetTranslationY(IIndicator indicator, BoxMode mode, float translateY) => this.Panel.STY1(indicator, mode, translateY);

        public void SetTransform(Matrix2x2 matrix) => this.Panel.SF0(matrix);
        public void SetTransform(IIndicator indicator, BoxMode mode, Matrix2x2 matrix) => this.Panel.SF1(indicator, mode, matrix);

        public void SetWidth(IIndicator indicator, BoxMode mode, float value, bool keepRatio) => this.Panel.SW(indicator, mode, value, keepRatio);
        public void SetHeight(IIndicator indicator, BoxMode mode, float value, bool keepRatio) => this.Panel.SH(indicator, mode, value, keepRatio);
        #endregion

        #region Bounds.Transform
        public void CacheTranslation() => this.Panel.CT();

        public void CacheTransform() => this.Panel.CF0();

        public void Translate(Vector2 startingPoint, Vector2 point) => this.Panel.TD0(startingPoint, point);
        public void Translate(IIndicator indicator, BoxMode mode, Vector2 startingPoint, Vector2 point) => this.Panel.TD1(indicator, mode, startingPoint, point);

        public void Translate(Vector2 translate) => this.Panel.T0(translate);
        public void Translate(IIndicator indicator, BoxMode mode, Vector2 translate) => this.Panel.T1(indicator, mode, translate);

        public void Translate(float translateX, float translateY) => this.Panel.TXY0(translateX, translateY);
        public void Translate(IIndicator indicator, BoxMode mode, float translateX, float translateY) => this.Panel.TXY1(indicator, mode, translateX, translateY);

        public void TranslateX(float translateX) => this.Panel.TX0(translateX);
        public void TranslateX(IIndicator indicator, BoxMode mode, float translateX) => this.Panel.TX1(indicator, mode, translateX);

        public void TranslateY(float translateY) => this.Panel.TY0(translateY);
        public void TranslateY(IIndicator indicator, BoxMode mode, float translateY) => this.Panel.TY1(indicator, mode, translateY);

        public void Transform(Matrix2x2 matrix) => this.Panel.F(matrix);
        #endregion

        #region Bounds.Transform2
        public void CacheTransform(CropMode mode) => this.Panel.CF1(mode);

        public void TransformSize(Vector2 point, bool keepRatio, bool centeredScaling) => this.Panel.TWH0(point, keepRatio, centeredScaling);
        public void TransformSize(IIndicator indicator, BoxMode mode, Vector2 point, bool keepRatio, bool centeredScaling) => this.Panel.TWH1(indicator, mode, point, keepRatio, centeredScaling);
        #endregion
    }
}