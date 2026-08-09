namespace FanKit.Transformer.UI
{
    public readonly struct CarouselItem1
    {
        public readonly CarouselPlacment Placment;
        public readonly float Amount;

        public readonly Quadrilateral Box;

        // -0.5 ~ +0.5
        internal CarouselItem1(Carousel carousel, float centerX, float centerY, float amount)
        {
            if (amount < -0.5f)
            {
                Placment = CarouselPlacment.Start;
                Amount = -0.5f;

                Box = carousel.LeftBox(centerX, centerY);
            }
            else if (amount > 0.5f)
            {
                Placment = CarouselPlacment.End;
                Amount = 0.5f;

                Box = carousel.RightBox(centerX, centerY);
            }
            else
            {
                Placment = CarouselPlacment.Lerp;
                Amount = amount;

                Box = carousel.LerpBox(centerX, centerY, this.Amount);
            }
        }
    }
}