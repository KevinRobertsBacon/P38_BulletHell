namespace P38
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class GameManager : MonoBehaviour, IEventListener
    {
        private void Awake()
        {
            Subscribe(SubscribeMode.Subscribe);
        }

        private void OnDestroy()
        {
            Subscribe(SubscribeMode.Unsubscribe);
        }

        public ListenerResult HandleEvent(IEvent evt)
        {
            string evtName = evt.GetName();
            switch (evtName)
            {
                case EventOnPlayerHealthValueChange.EventName:
                    float health = (float)evt.GetData();
                    if (health <= 0)
                    {
                        EventManager.TriggerEvent(new EventRequestPlayerDeath());
                    }
                    return ListenerResult.Handled;
            }
            return ListenerResult.Ignored;
        }

        public void Subscribe(SubscribeMode mode)
        {
            EventManager.ManageSubscriber(mode, this, EventOnPlayerHealthValueChange.EventName);
        }
    }

    public class EventRequestPlayerDeath : IEvent
    {
        public const string EventName = "EventRequestPlayerDeath";
        string IEvent.GetName() { return EventName; }
        object IEvent.GetData() { return null; }
    }
}
