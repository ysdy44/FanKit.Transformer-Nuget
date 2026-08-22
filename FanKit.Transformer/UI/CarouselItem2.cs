using FanKit.Transformer.Mathematics;
using System.Numerics;

namespace FanKit.Transformer.UI
{
    public readonly struct CarouselItem2
    {
        public readonly CarouselState State;
        public readonly float Amount;

        public readonly float RawX;
        public readonly float ActualX;

        public readonly Quadrilateral TextureOutline;

        public readonly Matrix4x4 TextureTransformMatrix;

        internal CarouselItem2(Carousel carousel, SizeMatrix sourceNormalize, int index, float centerX, float centerY, float offsetX, float itemMargin, float itemSpacing)
        {
            RawX = offsetX + index * itemSpacing;

            if (RawX <= -itemMargin)
            {
                State = CarouselState.DockLeft;
                Amount = 0.5f;

                ActualX = centerX + RawX - itemMargin;

                TextureOutline = carousel.GetDockLeftTextureOutline(ActualX, centerY);
            }
            else if (RawX < itemMargin)
            {
                State = CarouselState.Float;
                Amount = RawX / itemMargin / 2f;

                ActualX = centerX + RawX + RawX;

                TextureOutline = carousel.GetFloatTextureOutline(ActualX, centerY, Amount);
            }
            else
            {
                State = CarouselState.DockRight;
                Amount = -0.5f;

                ActualX = centerX + RawX + itemMargin;

                TextureOutline = carousel.GetDockRightTextureOutline(ActualX, centerY);
            }

            TextureTransformMatrix = sourceNormalize.ToPerspMatrix(TextureOutline);
        }
    }
}