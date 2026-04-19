namespace FanKit.Transformer.Operators
{
    public interface ICanvasOperator3
    {
        event SingleStartedEventHandler Single_Start;
        event SingleEventHandler Single_Delta;
        event SingleEventHandler Single_Complete;

        event RightEventHandler Right_Start;
        event RightEventHandler Right_Delta;
        event RightEventHandler Right_Complete;

        event DoubleEventHandler Double_Start;
        event DoubleEventHandler Double_Delta;
        event DoubleEventHandler Double_Complete;

        event WheelEventHandler Wheel_Changed;

        long ThresholdTicks { get; set; }

        TouchMode TouchMode { get; set; }

        bool IsInContact { get; }
    }
}