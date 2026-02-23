namespace FanKit.Transformer.UI
{
    public struct FlyoutLocation
    {
        // HorizontalAlignment
        public const byte Left = 0;
        public const byte Center = 1;
        public const byte Right = 2;
        public const byte Stretch = 3;

        // VerticalAlignment
        public const byte Top = 0;
        //public const byte Center = 1;
        public const byte Bottom = 2;
        //public const byte Stretch = 3;

        // Placement
        internal const byte Left0 = 0b00;
        internal const byte Right0 = 0b01;
        internal const byte Top0 = 0b10;
        internal const byte Bottom0 = 0b11;

        internal const byte Left6 = Left0 << 6;
        internal const byte Right6 = Right0 << 6;
        internal const byte Top6 = Top0 << 6;
        internal const byte Bottom6 = Bottom0 << 6;

        internal const byte Left4 = Left0 << 4;
        internal const byte Right4 = Right0 << 4;
        internal const byte Top4 = Top0 << 4;
        internal const byte Bottom4 = Bottom0 << 4;

        internal const byte Left2 = Left0 << 2;
        internal const byte Right2 = Right0 << 2;
        internal const byte Top2 = Top0 << 2;
        internal const byte Bottom2 = Bottom0 << 2;

        public FlyoutPlacementPriority Priority;

        public byte FixedHorizontalAlignment;
        public byte FixedVerticalAlignment;

        public Rectangle Target;
        public Rectangle Bounds;

        public Bounds BoundsPadding;
        public Bounds ContentMargin;
        public float CornerRadius;

        public float ArrowWidth;
        public float ArrowHeight;

        public float ContentWidth;
        public float ContentHeight;
    }
}