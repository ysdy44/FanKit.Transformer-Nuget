using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace FanKit.Transformer.Compute
{
    public abstract partial class AffineTriangle2 : AffineTriangle
    {
        internal TransformController Controller;

        internal ControllerRadians Radians;

        #region Triangles.Transform2
        internal void CR(Vector2 point)
        {
            this.StartingTriangle = this.Triangle;
            this.StartingMatrix = this.Matrix;

            this.Host = Matrix3x2.Identity;

            this.Controller = new TransformController(this.Triangle, point);
        }

        internal void CF1(TransformMode mode)
        {
            this.StartingTriangle = this.Triangle;
            this.StartingMatrix = this.Matrix;

            this.Host = Matrix3x2.Identity;

            this.Controller = new TransformController(this.Triangle, mode);
        }

        internal void R0(Vector2 point, float stepFrequency)
        {
            this.Radians = this.Controller.ToRadians(point, stepFrequency);

            this.Host = this.Controller.Rotate(this.Radians);
            this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host);
            this.Matrix = this.StartingMatrix * this.Host;
        }
        internal void R1(IIndicator indicator, PanelAnchorMode anchorMode, Vector2 point, float stepFrequency)
        {
            this.Radians = this.Controller.ToRadians(point, stepFrequency);

            this.Host = this.Controller.Rotate(this.Radians);
            this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host);
            this.Matrix = this.StartingMatrix * this.Host;

            indicator.ChangeXYWHRS(this.Triangle, anchorMode);
        }

        internal void TWH0(Vector2 point, bool keepRatio, bool centeredScaling)
        {
            this.Triangle = this.Controller.Transform(this.StartingTriangle, point, keepRatio, centeredScaling);

            this.Find();
        }
        internal void TWH1(IIndicator indicator, PanelAnchorMode anchorMode, Vector2 point, bool keepRatio, bool centeredScaling)
        {
            this.Triangle = this.Controller.Transform(this.StartingTriangle, point, keepRatio, centeredScaling);

            this.Find();

            indicator.ChangeXYWH(this.Triangle, anchorMode);
        }

        internal void TS0(Vector2 point, bool keepRatio, bool centeredScaling)
        {
            this.Triangle = this.Controller.Transform(this.StartingTriangle, point, keepRatio, centeredScaling);

            this.Find();
        }
        internal void TS1(IIndicator indicator, PanelAnchorMode anchorMode, Vector2 point, bool keepRatio, bool centeredScaling)
        {
            this.Triangle = this.Controller.Transform(this.StartingTriangle, point, keepRatio, centeredScaling);

            this.Find();

            indicator.ChangeXYWHRS(this.Triangle, anchorMode);
        }
        #endregion
    }
}