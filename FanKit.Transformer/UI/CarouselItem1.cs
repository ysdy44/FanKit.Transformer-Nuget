namespace FanKit.Transformer.UI
{
    public readonly struct CarouselItem1
    {
        public readonly CarouselPlacment Placment;
        public readonly float Amout;

        public readonly Quadrilateral Box;

        // -0.5 ~ +0.5
        public CarouselItem1(Carousel carousel, float centerX, float centerY, float amout)
        {
            if (amout < -0.5f)
            {
                Placment = CarouselPlacment.Start;
                Amout = -0.5f;

                Box = carousel.LeftBox(centerX, centerY);
            }
            else if (amout > 0.5f)
            {
                Placment = CarouselPlacment.End;
                Amout = 0.5f;

                Box = carousel.RightBox(centerX, centerY);
            }
            else
            {
                Placment = CarouselPlacment.Lerp;
                Amout = amout;

                Box = carousel.LerpBox(centerX, centerY, this.Amout);
            }
        }
    }
}