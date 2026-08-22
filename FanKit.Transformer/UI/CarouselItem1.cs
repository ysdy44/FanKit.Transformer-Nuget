namespace FanKit.Transformer.UI
{
    public readonly struct CarouselItem1
    {
        public readonly CarouselState State;
        public readonly float Amount;

        public readonly Quadrilateral TextureOutline;

        // -0.5 ~ +0.5
        internal CarouselItem1(Carousel carousel, float centerX, float centerY, float amount)
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
        }
    }
}