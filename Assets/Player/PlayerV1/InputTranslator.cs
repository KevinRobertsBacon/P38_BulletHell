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

            }
            return ListenerResult.Ignored;
        }

        public void Subscribe(SubscribeMode mode)
        {
            throw new System.NotImplementedException();
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