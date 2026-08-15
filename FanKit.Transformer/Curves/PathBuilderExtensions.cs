using FanKit.Transformer.Controllers;
using System.Collections.Generic;

namespace FanKit.Transformer.Curves
{
    public static partial class PathBuilderExtensions
    {
        #region ClosestPointer
        public static void CreatePreviousPath(this IPathBuilder pathBuilder, ClosestPointer closest, ICanvasMatrix canvasMatrix)
        {
            // ?

            pathBuilder.BeginFigure(canvasMatrix.Transform(closest.Previous.Point));

            AddBezier(pathBuilder, closest.PreviousIsSmooth, closest.NextIsSmooth, canvasMatrix.Transform(closest.Previous), canvasMatrix.Transform(closest.Current));

            pathBuilder.EndFigure(Open);

            // return
        }

        public static void CreateNextPath(this IPathBuilder pathBuilder, ClosestPointer closest, ICanvasMatrix canvasMatrix)
        {
            // ?

            pathBuilder.BeginFigure(canvasMatrix.Transform(closest.Current.Point));

            AddBezier(pathBuilder, closest.PreviousIsSmooth, closest.NextIsSmooth, canvasMatrix.Transform(closest.Current), canvasMatrix.Transform(closest.Next));

            pathBuilder.EndFigure(Open);

            // return
        }
        #endregion
    }
}