namespace FanKit.Transformer.Indicators
{
    partial class Indicator
    {
        private void STC4() // Crop
        {
            if (vl == 0f)
            {
                if (hl == 0f)
                    ST1();
                else
                    ST2();
            }
            else
            {
                if (hl == 0f)
                    ST3();
                else
                    ST4();
            }
        }

        private void STC5() // Transform
        {
            if (vl == 0f)
            {
                if (hl == 0f)
                    ST1();
                else
                    ST2();
            }
            else
            {
                if (hl == 0f)
                    ST3();
                else
                    ST5();
            }
        }

        private void ST0()
        {
            const IndicatorSizeType st = IndicatorSizeType.Empty;

            if (ST != st)
            {
                ST = st;
                this.SizeTypeChanged?.Invoke(this, st);
            }
        }

        private void ST1()
        {
            const IndicatorSizeType st = IndicatorSizeType.Point;

            if (ST != st)
            {
                ST = st;
                this.SizeTypeChanged?.Invoke(this, st);
            }
        }

        private void ST2()
        {
            const IndicatorSizeType st = IndicatorSizeType.RowLine;

            if (ST != st)
            {
                ST = st;
                this.SizeTypeChanged?.Invoke(this, st);
            }
        }

        private void ST3()
        {
            const IndicatorSizeType st = IndicatorSizeType.ColumnLine;

            if (ST != st)
            {
                ST = st;
                this.SizeTypeChanged?.Invoke(this, st);
            }
        }

        private void ST4() // Crop
        {
            const IndicatorSizeType st = IndicatorSizeType.Crop;

            if (ST != st)
            {
                ST = st;
                this.SizeTypeChanged?.Invoke(this, st);
            }
        }

        private void ST5() // Transform
        {
            const IndicatorSizeType st = IndicatorSizeType.Transform;

            if (ST != st)
            {
                ST = st;
                this.SizeTypeChanged?.Invoke(this, st);
            }
        }
    }
}