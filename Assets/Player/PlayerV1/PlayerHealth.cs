namespace P38
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerHealth : MonoBehaviour, IEventListener
    {
        [SerializeField]
        private int MaxHealth = 100;


        [SerializeField]
        private int currentHealth = 100;

        public int CurrentHealth
        {
            get
            {
                return currentHealth;
            }
            set
            {
                currentHealth = value;
                EventManager.TriggerEvent(new EventOnPlayerHealthValueChange(currentHealth));
            }
        }

        private const int tickInterval = 1;

        private const int DAMAGE = 5;

        public ListenerResult HandleEvent(IEvent evt)
        {
            string evtName = evt.GetName();
            switch (evtName)
            {
                case EventOnPlayerCollision.EventName:
                    TakeDamage();
                    return ListenerResult.Handled;
                case EventRequestPlayerDeath.EventName:
                    PlayerDeath();
                    return ListenerResult.Handled;
                case EventGetPlayerHealthMaxValue.EventName:
                    var getter = (EventGetPlayerHealthMaxValue)evt;
                    getter.maxHealth = MaxHealth;
                    return ListenerResult.Handled;
            }
            return ListenerResult.Ignored;
        }

        private void TakeDamage()
        {
            CurrentHealth -= DAMAGE;
        }

        private void PlayerDeath()
        {
            EventManager.TriggerEvent(new EventOnPlayerDeath());
            Destroy(this.gameObject);
        }

        float counter = 0;

        private void Update()
        {
            if (counter < tickInterval)
            {
                counter += Time.deltaTime;
            }
            else
            {
                counter = 0;
                CurrentHealth -= tickInterval;
            }
        }

        public void Subscribe(SubscribeMode mode)
        {
            EventManager.ManageSubscriber(mode, this, EventOnPlayerCollision.EventName);
            EventManager.ManageSubscriber(mode, this, EventRequestPlayerDeath.EventName);
            EventManager.ManageSubscriber(mode, this, EventGetPlayerHealthMaxValue.EventName);
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

    public class EventOnPlayerHealthValueChange : IEvent
    {
        public const string EventName = "EventOnPlayerHealthValueChange";
        string IEvent.GetName() { return EventName; }

        private float newHealth;

        public EventOnPlayerHealthValueChange (float _newHealth)
        {
            newHealth = _newHealth;
        }

        object IEvent.GetData() { return newHealth; }
    }

    public class EventGetPlayerHealthMaxValue : IEvent
    {
        public const string EventName = "EventGetPlayerHealthMaxValue";
        string IEvent.GetName() { return EventName; }

        public float maxHealth = 0;

        object IEvent.GetData() { return maxHealth; }
    }

    public class EventOnPlayerDeath : IEvent
    {
        public const string EventName = "EventOnPlayerDeath";
        string IEvent.GetName() { return EventName; }
        object IEvent.GetData() { return null; }
    }
}