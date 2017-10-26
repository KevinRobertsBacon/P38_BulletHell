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
            }
            return ListenerResult.Ignored;
        }

        private void TakeDamage()
        {
            currentHealth -= DAMAGE;
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
                currentHealth -= tickInterval;
            }
        }

        public void Subscribe(SubscribeMode mode)
        {
            EventManager.ManageSubscriber(mode, this, EventOnPlayerCollision.EventName);
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