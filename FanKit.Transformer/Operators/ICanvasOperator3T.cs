namespace FanKit.Transformer.Operators
{
    public interface ICanvasOperator3<T>
    {
        event OperatorSingleStartedEventHandler<T> Single_Start;
        event OperatorSingleEventHandler<T> Single_Delta;
        event OperatorSingleEventHandler<T> Single_Complete;

        event OperatorRightEventHandler Right_Start;
        event OperatorRightEventHandler Right_Delta;
        event OperatorRightEventHandler Right_Complete;

        event OperatorDoubleEventHandler Double_Start;
        event OperatorDoubleEventHandler Double_Delta;
        event OperatorDoubleEventHandler Double_Complete;

        event OperatorWheelEventHandler Wheel_Changed;

        long ThresholdTicks { get; set; }

        TouchMode TouchMode { get; set; }

        bool IsInContact { get; }
    }
}