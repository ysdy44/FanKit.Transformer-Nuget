namespace FanKit.Transformer.UI
{
    public readonly struct CarouselItem2
    {
        public readonly CarouselPlacment Placment;
        public readonly float Amout;

        public readonly float Raw;
        public readonly float Actual;

        public readonly Quadrilateral Box;

        public CarouselItem2(Carousel carousel, int index, float centerX, float centerY, float offsetX, float itemMargin = 60f, float itemSpacing = 110f)
        {
            Raw = offsetX + index * itemSpacing;

            if (Raw < -itemMargin)
            {
                Placment = CarouselPlacment.Start;
                Amout = default;

                Actual = centerX + Raw - itemMargin;

                Box = carousel.LeftBox(Actual, centerY);
            }
            else if (Raw < itemMargin)
            {
                Placment = CarouselPlacment.Lerp;
                Amout = Raw / itemMargin / 2f;

                Actual = centerX + Raw + Raw;

                Box = carousel.LerpBox(Actual, centerY, Amout);
            }
            else
            {
                Placment = CarouselPlacment.End;
                Amout = default;

                Actual = centerX + Raw + itemMargin;

                Box = carousel.RightBox(Actual, centerY);
            }
        }
    }
}