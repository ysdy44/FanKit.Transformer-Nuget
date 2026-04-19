using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Compute
{
    public abstract partial class MapBounds2 : MapBounds
    {
        CropController Controller;

        #region Bounds.Transform2
        internal void CF1(CropMode mode)
        {
            this.StartingBounds = this.Bounds;
            this.StartingMatrix = this.Matrix;

            this.Host = Matrix2x2.Identity;

            this.Controller = new CropController(this.Bounds, mode);
        }

        internal void TWH0(Vector2 point, bool keepRatio, bool centeredScaling)
        {
            this.Bounds = this.Controller.Crop(this.StartingBounds, point, keepRatio, centeredScaling);

            this.Find();
        }
        internal void TWH1(IIndicator indicator, BoxMode mode, Vector2 point, bool keepRatio, bool centeredScaling)
        {
            this.Bounds = this.Controller.Crop(this.StartingBounds, point, keepRatio, centeredScaling);

            this.Find();

            indicator.ChangeXYWH(this.Bounds, mode);
        }
        #endregion
    }
}