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
        public Triangle PanelDestination => this.Panel.Triangle;

        #region Triangles.Reset
        public void Reset(Bounds source, Triangle destination, Matrix3x2 transformMatrix)
        {
            this.Count = 1;
            this.SizeType = SizeType.Panel;
            this.SourceBounds = source;

            this.Panel.StartingTriangle = this.Panel.Triangle = destination;

            this.Host.Matrix = transformMatrix;
        }

        public void BeginExtend()
        {
            this.Count = 0;

            this.SourceBounds = Bounds.Infinity;
        }

        public void Extend(Bounds bounds)
        {
            switch (this.Count)
            {
                case 0:
                    this.Count = 1;
                    this.SourceBounds = Bounds.Infinity;

                    this.Eb(bounds);

                    this.Panel.StartingTriangle = this.Panel.Triangle = new Triangle(bounds);

                    this.Host.Matrix = Matrix3x2.Identity;
                    break;
                case 1:
                    this.Count = 2;
                    this.SourceBounds = Bounds.Infinity;

                    this.E3(this.Panel.Triangle);

                    this.Eb(bounds);

                    this.Host.Matrix = Matrix3x2.Identity;
                    break;
                case 2:
                default:
                    this.Count++;

                    this.Eb(bounds);

                    this.Host.Matrix = Matrix3x2.Identity;
                    break;
            }
        }

        public void Extend(Triangle triangle)
        {
            switch (this.Count)
            {
                case 0:
                    this.Count = 1;
                    this.SourceBounds = Bounds.Infinity;

                    this.E3(triangle);

                    this.Panel.StartingTriangle = this.Panel.Triangle = triangle;

                    this.Host.Matrix = Matrix3x2.Identity;
                    break;
                case 1:
                    this.Count = 2;
                    this.SourceBounds = Bounds.Infinity;

                    this.E3(this.Panel.Triangle);

                    this.E3(triangle);

                    this.Host.Matrix = Matrix3x2.Identity;
                    break;
                case 2:
                default:
                    this.Count++;

                    this.E3(triangle);

                    this.Host.Matrix = Matrix3x2.Identity;
                    break;
            }
        }

        public void Extend(Quadrilateral quad)
        {
            switch (this.Count)
            {
                case 0:
                    this.Count = 1;
                    this.SourceBounds = Bounds.Infinity;

                    this.E4(quad);

                    this.Panel.StartingTriangle = this.Panel.Triangle = quad.ToTriangle();

                    this.Host.Matrix = Matrix3x2.Identity;
                    break;
                case 1:
                    this.Count = 2;
                    this.SourceBounds = Bounds.Infinity;

                    this.E3(this.Panel.Triangle);

                    this.E4(quad);

                    this.Host.Matrix = Matrix3x2.Identity;
                    break;
                default:
                    this.Count++;

                    this.E4(quad);

                    this.Host.Matrix = Matrix3x2.Identity;
                    break;
            }
        }

        private void Ex(Vector2 point)
        {
            if (this.SourceBounds.Left > point.X) this.SourceBounds.Left = point.X;
            if (this.SourceBounds.Top > point.Y) this.SourceBounds.Top = point.Y;
            if (this.SourceBounds.Right < point.X) this.SourceBounds.Right = point.X;
            if (this.SourceBounds.Bottom < point.Y) this.SourceBounds.Bottom = point.Y;
        }
        private void Eb(Bounds bounds)
        {
            if (this.SourceBounds.Left > bounds.Left) this.SourceBounds.Left = bounds.Left;
            if (this.SourceBounds.Top > bounds.Top) this.SourceBounds.Top = bounds.Top;
            if (this.SourceBounds.Right < bounds.Left) this.SourceBounds.Right = bounds.Left;
            if (this.SourceBounds.Bottom < bounds.Top) this.SourceBounds.Bottom = bounds.Top;

            if (this.SourceBounds.Left > bounds.Right) this.SourceBounds.Left = bounds.Right;
            if (this.SourceBounds.Top > bounds.Bottom) this.SourceBounds.Top = bounds.Bottom;
            if (this.SourceBounds.Right < bounds.Right) this.SourceBounds.Right = bounds.Right;
            if (this.SourceBounds.Bottom < bounds.Bottom) this.SourceBounds.Bottom = bounds.Bottom;
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
                    this.SizeType = SizeType.Empty;
                    break;
                case 1:
                    this.SizeType = SizeType.Panel;
                    break;
                default:
                    this.SizeType = SizeType.Panel;

                    this.Panel.StartingTriangle = this.Panel.Triangle = new Triangle(this.SourceBounds);

                    this.Host.Matrix = Matrix3x2.Identity;
                    break;
            }
        }
        #endregion

        #region Triangles.Set
        public void PanelSetTranslation(Vector2 translate)
        {
            this.Panel.ST0(translate);
        }
        public void PanelSetTranslation(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.Panel.ST1(indicator, mode, translate);
        }

        public void PanelSetTranslationX(float translateX)
        {
            this.Panel.STX0(translateX);
        }
        public void PanelSetTranslationX(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.Panel.STX1(indicator, mode, translateX);
        }

        public void PanelSetTranslationY(float translateY)
        {
            this.Panel.STY0(translateY);
        }
        public void PanelSetTranslationY(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.Panel.STY1(indicator, mode, translateY);
        }

        public void PanelSetTransform(Matrix3x2 matrix)
        {
            this.Panel.SF0(matrix);
        }
        public void PanelSetTransform(IIndicator indicator, BoxMode mode, Matrix3x2 matrix)
        {
            this.Panel.SF1(indicator, mode, matrix);
        }

        public void PanelSetWidth(IIndicator indicator, BoxMode mode, float value, bool keepRatio)
        {
            this.Panel.SW(indicator, mode, value, keepRatio);
        }
        public void PanelSetHeight(IIndicator indicator, BoxMode mode, float value, bool keepRatio)
        {
            this.Panel.SH(indicator, mode, value, keepRatio);
        }

        public void PanelSetRotation(IIndicator indicator, BoxMode mode, float rotationAngleInDegrees)
        {
            this.Panel.SR(indicator, mode, rotationAngleInDegrees);
        }
        public void PanelSetSkew(IIndicator indicator, BoxMode mode, float skewAngleInDegrees, float minimum = -85f, float maximum = 85f)
        {
            this.Panel.SS(indicator, mode, skewAngleInDegrees, minimum,   maximum);
        }
        #endregion

        #region Triangles.Transform
        public void PanelCacheTranslation()
        {
            this.Panel.CT();
        }

        public void PanelCacheTransform()
        {
            this.Panel.CF();
        }

        public void PanelTranslate(Vector2 startingPoint, Vector2 point)
        {
            this.Panel.TD0(startingPoint, point);
        }
        public void PanelTranslate(IIndicator indicator, BoxMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.Panel.TD1(indicator, mode, startingPoint, point);
        }

        public void PanelTranslate(Vector2 translate)
        {
            this.Panel.T0(translate);
        }
        public void PanelTranslate(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.Panel.T1(indicator, mode, translate);
        }

        public void PanelTranslate(float translateX, float translateY)
        {
            this.Panel.TXY0(translateX, translateY);
        }
        public void PanelTranslate(IIndicator indicator, BoxMode mode, float translateX, float translateY)
        {
            this.Panel.TXY1(indicator, mode, translateX, translateY);
        }

        public void PanelTranslateX(float translateX)
        {
            this.Panel.TX0(translateX);
        }
        public void PanelTranslateX(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.Panel.TX1(indicator, mode, translateX);
        }

        public void PanelTranslateY(float translateY)
        {
            this.Panel.TY0(translateY);
        }
        public void PanelTranslateY(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.Panel.TY1(indicator, mode, translateY);
        }

        public void PanelTransform(Matrix3x2 matrix)
        {
            this.Panel.F(matrix);
        }
        #endregion

        #region Triangles.Transform2
        public void PanelCacheRotation(Vector2 point)
        {
            this.Panel.CR(point);
        }

        public void PanelCacheTransform(TransformMode mode)
        {
            this.Panel.CF(mode);
        }

        public void PanelRotate(Vector2 point, float stepFrequency = float.NaN)
        {
            this.Panel.R0(point, stepFrequency);
        }
        public void PanelRotate(IIndicator indicator, BoxMode mode, Vector2 point, float stepFrequency = float.NaN)
        {
            this.Panel.R1(indicator, mode, point, stepFrequency);
        }

        public void PanelTransformSize(Vector2 point, bool keepRatio, bool centeredScaling)
        {
            this.Panel.TWH0(point, keepRatio, centeredScaling);
        }
        public void PanelTransformSize(IIndicator indicator, BoxMode mode, Vector2 point, bool keepRatio, bool centeredScaling)
        {
            this.Panel.TWH1(indicator, mode, point, keepRatio, centeredScaling);
        }

        public void PanelTransformSkew(Vector2 point, bool keepRatio, bool centeredScaling)
        {
            this.Panel.TS0(point, keepRatio, centeredScaling);
        }
        public void PanelTransformSkew(IIndicator indicator, BoxMode mode, Vector2 point, bool keepRatio, bool centeredScaling)
        {
            this.Panel.TS1(indicator, mode, point, keepRatio, centeredScaling);
        }
        #endregion
    }
}