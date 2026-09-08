namespace FanKit.Transformer.UI
{
    public readonly struct GraticuleUV
    {
        public static GraticuleUV U10V7 { get; } = new GraticuleUV(5);
        public static GraticuleUV U12V8 { get; } = new GraticuleUV(6);
        public static GraticuleUV U18V11 { get; } = new GraticuleUV(9);
        public static GraticuleUV U20V12 { get; } = new GraticuleUV(10);
        public static GraticuleUV U24V14 { get; } = new GraticuleUV(12);
        public static GraticuleUV U30V17 { get; } = new GraticuleUV(15);
        public static GraticuleUV U36V20 { get; } = new GraticuleUV(18);

        // -180° West ~ 180° East
        public const float West = -180f;
        public const float East = 180f;
        public const float Longitude = 360f;

        // -90° South ~ 90° North
        public const float South = -90f;
        public const float North = 90f;
        public const float Latitude = 180f;

        readonly int Count;

        internal readonly int VCountMinus;
        internal readonly int VCount;
        internal readonly int VCountPlus;

        readonly int VCountMinusTwice;
        internal readonly float VCountMinusTwiceF;

        internal readonly float VCountF;
        internal readonly float VCountTwiceF;

        readonly int UCountHalf;
        internal readonly float UCountHalfF;

        internal readonly int UCount;
        internal readonly float UCountF;

        internal readonly int UCountMinus;
        //internal readonly int UCountPlus;

        public int U => UCount;
        public int V => VCountPlus;

        private GraticuleUV(int count)
        {
            Count = count;

            VCountMinus = Count;
            VCount = VCountMinus + 1;
            VCountPlus = VCountMinus + 1 + 1;

            VCountMinusTwice = VCountMinus + VCountMinus;
            VCountMinusTwiceF = VCountMinusTwice;

            VCountF = VCount;
            VCountTwiceF = VCountF + VCountF;

            UCountHalf = Count;
            UCountHalfF = UCountHalf;

            UCount = UCountHalf + UCountHalf;
            UCountF = UCount;

            UCountMinus = UCount - 1;
            //UCountPlus = UCount + 1;
        }

        public GraticuleUV(int u, int v)
        {
            //Count = (VCountMinus + UCountHalf) / 2;

            VCountMinus = v - 1 - 1;
            VCount = v - 1;
            VCountPlus = v;

            VCountMinusTwice = v - 1 - 1 + v - 1 - 1;
            VCountMinusTwiceF = v - 1f - 1f + v - 1f - 1f;

            VCountF = v - 1;
            VCountTwiceF = v - 1f + v - 1f;

            UCountHalf = u / 2;
            UCountHalfF = u / 2f;

            UCount = u;
            UCountF = u;

            UCountMinus = u - 1;
            //UCountPlus = u + 1;

            Count = (VCountMinus + UCountHalf) / 2;
        }
    }
}