namespace P38
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerMovement : MonoBehaviour, IEventListener
    {
        [SerializeField]
        private float speed = 2f;

        [SerializeField]
        private Vector2 playerExtants = new Vector2(9.5f, 9.5f);

        [SerializeField]
        private Transform playerMeshTransform;

        [SerializeField]
        private Vector2 meshTilt;


        Vector3 lastPos;
        Vector3 currentPos;
        Vector3 difference;

        private void Update()
        {
            currentPos = transform.position;
            difference = (currentPos - lastPos);

            if (difference.x > 0)
                difference.x *= -1 / difference.x;
            else if (difference.x < 0)
                difference.x *= 1 / difference.x;

            if (difference.y > 0)
                difference.y *= 1 / difference.y;
            if (difference.y < 0)
                difference.y *= -1 / difference.y;

            playerMeshTransform.localRotation = Quaternion.Euler(meshTilt.x * difference.y, meshTilt.y * difference.x, 0);

            lastPos = transform.position;
        }

        public ListenerResult HandleEvent(IEvent evt)
        {
            string evtName = evt.GetName();
            switch (evtName)
            {
                case EventRequestPlayerMove.EventName:
                    var data = (EventRequestPlayerMove.Data)evt.GetData();
                    HandleMovement(data.Movement);
                    return ListenerResult.Handled;
                case EventGetPlayerTransform.EventName:
                    var castedEvent = (EventGetPlayerTransform)evt;
                    castedEvent.playerTransform = transform;
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
            if (position.x > -playerExtants.x && position.x < playerExtants.x && position.y > -playerExtants.y && position.y < playerExtants.y)
                return true;
            else
                return false;
        }

        public void Subscribe(SubscribeMode mode)
        {
            EventManager.ManageSubscriber(mode, this, EventRequestPlayerMove.EventName);
            EventManager.ManageSubscriber(mode, this, EventGetPlayerTransform.EventName);
        }

        private void Awake()
        {
            Subscribe(SubscribeMode.Subscribe);

            lastPos = transform.position;
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

    public class EventGetPlayerTransform : IEvent
    {
        public const string EventName = "EventGetPlayerTransform";
        string IEvent.GetName() { return EventName; }

        public object GetData()
        {
            return playerTransform;
        }

        public Transform playerTransform;
    }
}
