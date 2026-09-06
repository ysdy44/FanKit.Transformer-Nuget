namespace FanKit.Transformer.Mathematics
{
    internal struct Matrix8
    {
        public float M0;
        public float M1;
        public float M2;
        public float M3;
        public float M4;
        public float M5;
        public float M6;
        public float M7;

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return M0;
                    case 1: return M1;
                    case 2: return M2;
                    case 3: return M3;
                    case 4: return M4;
                    case 5: return M5;
                    case 6: return M6;
                    case 7: return M7;
                    default: return default;
                }
            }
            set
            {
                switch (index)
                {
                    case 0: M0 = value; break;
                    case 1: M1 = value; break;
                    case 2: M2 = value; break;
                    case 3: M3 = value; break;
                    case 4: M4 = value; break;
                    case 5: M5 = value; break;
                    case 6: M6 = value; break;
                    case 7: M7 = value; break;
                    default: break;
                }
            }
        }
    }
}