namespace P38
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PrimaryWeapon : MonoBehaviour, IEventListener
    {

        [SerializeField]
        private PlayerWeaponType currentWeaponType;

        bool readyToFire = true;

        float timeSinceFired = 0f;

        public ListenerResult HandleEvent(IEvent evt)
        {
            string evtName = evt.GetName();
            switch (evtName)
            {
                case EventRequestPrimaryWeaponFire.EventName:
                    if (readyToFire)
                        FirePrimaryWeapon();
                    return ListenerResult.Handled;
            }
            return ListenerResult.Ignored;
        }

        public void FirePrimaryWeapon()
        {
            readyToFire = false;
            timeSinceFired = 0f;
            Instantiate(currentWeaponType.Bullet, transform.position, transform.rotation);
        }

        private void Update()
        {
            if (!readyToFire)
            {
                if (timeSinceFired > currentWeaponType.FireRate)
                    readyToFire = true;
                else
                    timeSinceFired += Time.deltaTime;
            }
        }

        public void Subscribe(SubscribeMode mode)
        {
            EventManager.ManageSubscriber(mode, this, EventRequestPrimaryWeaponFire.EventName);
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

    public class EventRequestPrimaryWeaponFire : IEvent
    {
        public const string EventName = "EventRequestPrimaryWeaponFire";
        string IEvent.GetName() { return EventName; }
        object IEvent.GetData() { return null; }
    }
}
