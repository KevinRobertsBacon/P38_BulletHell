namespace P38
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class InputTranslator : MonoBehaviour, IEventListener
    {
        public ListenerResult HandleEvent(IEvent evt)
        {
            string evtName = evt.GetName();
            switch (evtName)
            {
                case EventOnInputRecieved.EventName:
                    var data = (InputData)evt.GetData();
                    ParseInputData(data);
                    return ListenerResult.Handled;
            }
            return ListenerResult.Ignored;
        }

        bool ParseInputData(InputData data)
        {
            switch (data.myInputKey)
            {
                case InputKey.Up:
                    return true;
                case InputKey.Down:
                    return true;
                case InputKey.Right:
                    return true;
                case InputKey.Left:
                    return true;
                case InputKey.MainButton1:
                    return true;
                case InputKey.MainButton2:
                    return true;
            }
            return false;
        }

        public void Subscribe(SubscribeMode mode)
        {
            EventManager.ManageSubscriber(mode, this, EventOnInputRecieved.EventName);
        }

        private void Awake()
        {
            Subscribe(SubscribeMode.Subscribe);
        }

        private void OnDestroy()
        {
            Subscribe(SubscribeMode.Unsubscribe);
        }
    }
}