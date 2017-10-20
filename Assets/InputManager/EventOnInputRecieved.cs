namespace P38
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

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

    public class InputData
    {
        public enum Input
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
    }
}
