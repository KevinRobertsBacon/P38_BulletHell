namespace P38
{
    public class EventOnInputRecieved : IEvent
    {
        public const string EventName = "EventOnInputRecieved";
        string IEvent.GetName() { return EventName; }

        InputData inputData;

        public EventOnInputRecieved (InputData _newInputData)
        {
            inputData = _newInputData;
        }

        object IEvent.GetData() { return inputData; }

    }

    public enum InputKey
    {
        Up,
        Down,
        Right,
        Left,
        Horizontal,
        MainButton1,
        MainButton2,
    }

    public enum InputType
    {
        Hold,
        Down,
        Up
    }

    public class InputData
    {
        public InputKey myInputKey;
        public InputType myInputType;

        public InputData(InputKey inputkey, InputType inputType)
        {
            myInputKey = inputkey;
            myInputType = inputType;
        }
    }
}
