namespace P38
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class FuelBar : MonoBehaviour, IEventListener
    {
        float playerMaxHealth = 0;
        float playerCurrentHealth = 0;

        float PlayerCurrentHealth
        {
            get
            {
                return playerCurrentHealth;
            }
            set
            {
                playerCurrentHealth = value;
                if (playerMaxHealth != 0)
                {
                    transform.localScale = new Vector3(playerCurrentHealth / playerMaxHealth, 1, 1);
                }
            }
        }

        private void Awake()
        {
            Subscribe(SubscribeMode.Subscribe);
        }

        private void Start()
        {
            EventGetPlayerHealthMaxValue getMaxValue = new EventGetPlayerHealthMaxValue();
            EventManager.TriggerEvent(getMaxValue);
            playerMaxHealth = getMaxValue.maxHealth;
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
                    float current = (float)evt.GetData();
                    PlayerCurrentHealth = current;
                    return ListenerResult.Handled;
            }
            return ListenerResult.Ignored;
        }

        public void Subscribe(SubscribeMode mode)
        {
            EventManager.ManageSubscriber(mode, this, EventOnPlayerHealthValueChange.EventName);
        }
    }
}
