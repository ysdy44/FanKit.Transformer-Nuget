using FanKit.Transformer.Mathematics;
using System.Numerics;

namespace FanKit.Transformer.UI
{
    public readonly partial struct CarouselItem1
    {
        public readonly CarouselState State;
        public readonly float Amount;

        public readonly Quadrilateral TextureOutline;

        public readonly Matrix4x4 TextureTransformMatrix;
    }

    public readonly partial struct CarouselItem2
    {
        public readonly CarouselState State;
        public readonly float Amount;

        public readonly float RawX;
        public readonly float ActualX;

        public readonly Quadrilateral TextureOutline;

        public readonly Matrix4x4 TextureTransformMatrix;
    }

    partial struct CarouselItem1
    {
        // -0.5 ~ +0.5
        internal CarouselItem1(Carousel carousel, SizeMatrix sourceNormalize, float centerX, float centerY, float amount)
        {
            if (amount < -0.5f)
            {
                State = CarouselState.DockLeft;
                Amount = -0.5f;

                TextureOutline = carousel.GetDockLeftTextureOutline(centerX, centerY);
            }
            else if (amount > 0.5f)
            {
                State = CarouselState.DockRight;
                Amount = 0.5f;

                TextureOutline = carousel.GetDockRightTextureOutline(centerX, centerY);
            }
            else
            {
                State = CarouselState.Float;
                Amount = amount;

                TextureOutline = carousel.GetFloatTextureOutline(centerX, centerY, this.Amount);
            }

            TextureTransformMatrix = sourceNormalize.ToPerspMatrix(TextureOutline);
        }
    }

    partial struct CarouselItem2
    {
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