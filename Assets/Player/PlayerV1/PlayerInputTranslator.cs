namespace P38
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerInputTranslator : MonoBehaviour, IEventListener
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
                    //DebugHelper.Log("Getting Up Key");
                    EventManager.TriggerEvent(new EventRequestPlayerMove(new EventRequestPlayerMove.Data(new Vector2(0, 1))));
                    return true;
                case InputKey.Down:
                    //DebugHelper.Log("Getting Down Key");
                    EventManager.TriggerEvent(new EventRequestPlayerMove(new EventRequestPlayerMove.Data(new Vector2(0, -1))));
                    return true;
                case InputKey.Right:
                    //DebugHelper.Log("Getting Right Key");
                    EventManager.TriggerEvent(new EventRequestPlayerMove(new EventRequestPlayerMove.Data(new Vector2(1, 0))));
                    return true;
                case InputKey.Left:
                    //DebugHelper.Log("Getting Left Key");
                    EventManager.TriggerEvent(new EventRequestPlayerMove(new EventRequestPlayerMove.Data(new Vector2(-1, 0))));
                    return true;
                case InputKey.MainButton1:
                    //DebugHelper.Log("Getting Main Button 1 Key");
                    EventManager.TriggerEvent(new EventRequestPrimaryWeaponFire());
                    return true;
                case InputKey.MainButton2:
                    DebugHelper.Log("Getting Main Button 2 Key");
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