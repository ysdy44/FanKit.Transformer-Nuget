namespace FanKit.Transformer.Indicators
{
    partial class Indicator
    {
        private void STC4()
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

        private void ST4()
        {
            const IndicatorSizeType st = IndicatorSizeType.Panel;

            if (ST != st)
            {
                ST = st;
                this.SizeTypeChanged?.Invoke(this, st);
            }
        }
    }
}