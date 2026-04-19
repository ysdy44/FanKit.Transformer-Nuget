namespace FanKit.Transformer.Operators
{
    public interface ICanvasOperator3<T>
    {
        event SingleStartedEventHandler<T> Single_Start;
        event SingleEventHandler<T> Single_Delta;
        event SingleEventHandler<T> Single_Complete;

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