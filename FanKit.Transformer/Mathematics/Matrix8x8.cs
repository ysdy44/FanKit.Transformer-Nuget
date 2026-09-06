namespace FanKit.Transformer.Mathematics
{
    internal struct Matrix8x8
    {
        public float M00;
        public float M01;
        public float M02;
        public float M03;
        public float M04;
        public float M05;
        public float M06;
        public float M07;

        public float M10;
        public float M11;
        public float M12;
        public float M13;
        public float M14;
        public float M15;
        public float M16;
        public float M17;

        public float M20;
        public float M21;
        public float M22;
        public float M23;
        public float M24;
        public float M25;
        public float M26;
        public float M27;

        public float M30;
        public float M31;
        public float M32;
        public float M33;
        public float M34;
        public float M35;
        public float M36;
        public float M37;

        public float M40;
        public float M41;
        public float M42;
        public float M43;
        public float M44;
        public float M45;
        public float M46;
        public float M47;

        public float M50;
        public float M51;
        public float M52;
        public float M53;
        public float M54;
        public float M55;
        public float M56;
        public float M57;

        public float M60;
        public float M61;
        public float M62;
        public float M63;
        public float M64;
        public float M65;
        public float M66;
        public float M67;

        public float M70;
        public float M71;
        public float M72;
        public float M73;
        public float M74;
        public float M75;
        public float M76;
        public float M77;

        public float this[int index1, int index0]
        {
            get
            {
                switch (index1)
                {
                    case 0:
                        switch (index0)
                        {
                            case 0: return M00;
                            case 1: return M01;
                            case 2: return M02;
                            case 3: return M03;
                            case 4: return M04;
                            case 5: return M05;
                            case 6: return M06;
                            case 7: return M07;
                            default: return default;
                        }
                    case 1:
                        switch (index0)
                        {
                            case 0: return M10;
                            case 1: return M11;
                            case 2: return M12;
                            case 3: return M13;
                            case 4: return M14;
                            case 5: return M15;
                            case 6: return M16;
                            case 7: return M17;
                            default: return default;
                        }
                    case 2:
                        switch (index0)
                        {
                            case 0: return M20;
                            case 1: return M21;
                            case 2: return M22;
                            case 3: return M23;
                            case 4: return M24;
                            case 5: return M25;
                            case 6: return M26;
                            case 7: return M27;
                            default: return default;
                        }
                    case 3:
                        switch (index0)
                        {
                            case 0: return M30;
                            case 1: return M31;
                            case 2: return M32;
                            case 3: return M33;
                            case 4: return M34;
                            case 5: return M35;
                            case 6: return M36;
                            case 7: return M37;
                            default: return default;
                        }
                    case 4:
                        switch (index0)
                        {
                            case 0: return M40;
                            case 1: return M41;
                            case 2: return M42;
                            case 3: return M43;
                            case 4: return M44;
                            case 5: return M45;
                            case 6: return M46;
                            case 7: return M47;
                            default: return default;
                        }
                    case 5:
                        switch (index0)
                        {
                            case 0: return M50;
                            case 1: return M51;
                            case 2: return M52;
                            case 3: return M53;
                            case 4: return M54;
                            case 5: return M55;
                            case 6: return M56;
                            case 7: return M57;
                            default: return default;
                        }
                    case 6:
                        switch (index0)
                        {
                            case 0: return M60;
                            case 1: return M61;
                            case 2: return M62;
                            case 3: return M63;
                            case 4: return M64;
                            case 5: return M65;
                            case 6: return M66;
                            case 7: return M67;
                            default: return default;
                        }
                    case 7:
                        switch (index0)
                        {
                            case 0: return M70;
                            case 1: return M71;
                            case 2: return M72;
                            case 3: return M73;
                            case 4: return M74;
                            case 5: return M75;
                            case 6: return M76;
                            case 7: return M77;
                            default: return default;
                        }
                    default: return default;
                }
            }
            set
            {
                switch (index1)
                {
                    case 0:
                        switch (index0)
                        {
                            case 0: M00 = value; break;
                            case 1: M01 = value; break;
                            case 2: M02 = value; break;
                            case 3: M03 = value; break;
                            case 4: M04 = value; break;
                            case 5: M05 = value; break;
                            case 6: M06 = value; break;
                            case 7: M07 = value; break;
                            default: break;
                        }
                        break;
                    case 1:
                        switch (index0)
                        {
                            case 0: M10 = value; break;
                            case 1: M11 = value; break;
                            case 2: M12 = value; break;
                            case 3: M13 = value; break;
                            case 4: M14 = value; break;
                            case 5: M15 = value; break;
                            case 6: M16 = value; break;
                            case 7: M17 = value; break;
                            default: break;
                        }
                        break;
                    case 2:
                        switch (index0)
                        {
                            case 0: M20 = value; break;
                            case 1: M21 = value; break;
                            case 2: M22 = value; break;
                            case 3: M23 = value; break;
                            case 4: M24 = value; break;
                            case 5: M25 = value; break;
                            case 6: M26 = value; break;
                            case 7: M27 = value; break;
                            default: break;
                        }
                        break;
                    case 3:
                        switch (index0)
                        {
                            case 0: M30 = value; break;
                            case 1: M31 = value; break;
                            case 2: M32 = value; break;
                            case 3: M33 = value; break;
                            case 4: M34 = value; break;
                            case 5: M35 = value; break;
                            case 6: M36 = value; break;
                            case 7: M37 = value; break;
                            default: break;
                        }
                        break;
                    case 4:
                        switch (index0)
                        {
                            case 0: M40 = value; break;
                            case 1: M41 = value; break;
                            case 2: M42 = value; break;
                            case 3: M43 = value; break;
                            case 4: M44 = value; break;
                            case 5: M45 = value; break;
                            case 6: M46 = value; break;
                            case 7: M47 = value; break;
                            default: break;
                        }
                        break;
                    case 5:
                        switch (index0)
                        {
                            case 0: M50 = value; break;
                            case 1: M51 = value; break;
                            case 2: M52 = value; break;
                            case 3: M53 = value; break;
                            case 4: M54 = value; break;
                            case 5: M55 = value; break;
                            case 6: M56 = value; break;
                            case 7: M57 = value; break;
                            default: break;
                        }
                        break;
                    case 6:
                        switch (index0)
                        {
                            case 0: M60 = value; break;
                            case 1: M61 = value; break;
                            case 2: M62 = value; break;
                            case 3: M63 = value; break;
                            case 4: M64 = value; break;
                            case 5: M65 = value; break;
                            case 6: M66 = value; break;
                            case 7: M67 = value; break;
                            default: break;
                        }
                        break;
                    case 7:
                        switch (index0)
                        {
                            case 0: M70 = value; break;
                            case 1: M71 = value; break;
                            case 2: M72 = value; break;
                            case 3: M73 = value; break;
                            case 4: M74 = value; break;
                            case 5: M75 = value; break;
                            case 6: M76 = value; break;
                            case 7: M77 = value; break;
                            default: break;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}