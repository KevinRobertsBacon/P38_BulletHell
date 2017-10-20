namespace P38
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerMovement : MonoBehaviour, IEventListener
    {
        [SerializeField]
        private float speed = 2f;

        public ListenerResult HandleEvent(IEvent evt)
        {
            string evtName = evt.GetName();
            switch (evtName)
            {
                case EventRequestPlayerMove.EventName:
                    var data = (EventRequestPlayerMove.Data)evt.GetData();
                    HandleMovement(data.Movement);
                    return ListenerResult.Handled;
            }
            return ListenerResult.Ignored;
        }

        void HandleMovement(Vector2 moveRequest)
        {
            Vector2 interpretedMovement = moveRequest * speed * Time.deltaTime;
            Vector3 newPosition = transform.position + (Vector3)interpretedMovement;
            if (ValidateNewPosition(newPosition))
                transform.position = newPosition;
        }

        bool ValidateNewPosition(Vector3 position)
        {
            return true;
        }

        public void Subscribe(SubscribeMode mode)
        {
            EventManager.ManageSubscriber(mode, this, EventRequestPlayerMove.EventName);
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

    public class EventRequestPlayerMove : IEvent
    {
        public const string EventName = "EventRequestPlayerMove";
        string IEvent.GetName() { return EventName; }

        private Data _data;

        public EventRequestPlayerMove(Data data)
        {
            _data = data;
        }

        object IEvent.GetData() { return _data; }

        public class Data
        {
            public Vector2 Movement;

            public Data(Vector2 movement)
            {
                Movement = movement;
            }
        }
    }
}
