namespace FanKit.Transformer.UI
{
    public readonly struct CarouselItem2
    {
        public readonly CarouselPlacment Placment;
        public readonly float Amount;

        public readonly float Raw;
        public readonly float Actual;

        public readonly Quadrilateral Box;

        internal CarouselItem2(Carousel carousel, int index, float centerX, float centerY, float offsetX, float itemMargin, float itemSpacing)
        {
            Raw = offsetX + index * itemSpacing;

            if (Raw < -itemMargin)
            {
                Placment = CarouselPlacment.Start;
                Amount = default;

                Actual = centerX + Raw - itemMargin;

                Box = carousel.LeftBox(Actual, centerY);
            }
            else if (Raw < itemMargin)
            {
                Placment = CarouselPlacment.Lerp;
                Amount = Raw / itemMargin / 2f;

                Actual = centerX + Raw + Raw;

                Box = carousel.LerpBox(Actual, centerY, Amount);
            }
            else
            {
                Placment = CarouselPlacment.End;
                Amount = default;

                Actual = centerX + Raw + itemMargin;

                Box = carousel.RightBox(Actual, centerY);
            }
        }
    }
}