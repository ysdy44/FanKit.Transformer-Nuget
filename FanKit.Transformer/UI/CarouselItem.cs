using FanKit.Transformer.Mathematics;
using System.Numerics;

namespace FanKit.Transformer.UI
{
    public readonly struct CarouselItem
    {
        public readonly CarouselState State;
        public readonly float RotattionXAmount;

        public readonly float RawX;
        public readonly float ActualX;

        public readonly Quadrilateral TextureOutline;

        public readonly Matrix4x4 TextureTransformMatrix;

        // -1 ~ 1
        internal CarouselItem(Carousel carousel, SizeMatrix sourceNormalize, Vector2 center, float rotattionXAmount)
        {
            RawX = 0f;
            ActualX = 0f;

            if (rotattionXAmount <= -1f)
            {
                State = CarouselState.DockLeft;
                RotattionXAmount = -1f;

                TextureOutline = carousel.GetDockLeftTextureOutline(center);
            }
            else if (rotattionXAmount < 1f)
            {
                State = CarouselState.Float;
                RotattionXAmount = rotattionXAmount;

                TextureOutline = carousel.GetFloatTextureOutline(center, RotattionXAmount);
            }
            else
            {
                State = CarouselState.DockRight;
                RotattionXAmount = 1f;

                TextureOutline = carousel.GetDockRightTextureOutline(center);
            }

            TextureTransformMatrix = sourceNormalize.ToPerspMatrix(TextureOutline);
        }

        internal CarouselItem(Carousel carousel, SizeMatrix sourceNormalize, Vector2 center, int index, float offsetX, float itemMargin, float itemSpacing)
        {
            RawX = offsetX + index * itemSpacing;

            if (RawX <= -itemMargin)
            {
                State = CarouselState.DockLeft;
                RotattionXAmount = -1f;

                ActualX = center.X + RawX - itemMargin;

                TextureOutline = carousel.GetDockLeftTextureOutline(ActualX, center.Y);
            }
            else if (RawX < itemMargin)
            {
                State = CarouselState.Float;
                RotattionXAmount = RawX / itemMargin;

                ActualX = center.X + RawX + RawX;

                TextureOutline = carousel.GetFloatTextureOutline(ActualX, center.Y, RotattionXAmount);
            }
            else
            {
                State = CarouselState.DockRight;
                RotattionXAmount = 1f;

                ActualX = center.X + RawX + itemMargin;

                TextureOutline = carousel.GetDockRightTextureOutline(ActualX, center.Y);
            }

            TextureTransformMatrix = sourceNormalize.ToPerspMatrix(TextureOutline);
        }
    }
}