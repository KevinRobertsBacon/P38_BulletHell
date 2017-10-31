namespace P38
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerCollision : MonoBehaviour
    {
        [SerializeField]
        private LayerMask triggerLayersToIgnore;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1<<collision.gameObject.layer) & triggerLayersToIgnore) != 0)
            {
                //we hit an ignore layer
                //DebugHelper.Log("We hit Something, but let's ignore it.");
            }
            else
            {
                //DebugHelper.Log("We hit something, let's do something about it.");
                EventManager.TriggerEvent(new EventOnPlayerCollision());
            }
        }
    }

    public class EventOnPlayerCollision : IEvent
    {
        public const string EventName = "EventOnPlayerCollision";
        string IEvent.GetName() { return EventName; }
        object IEvent.GetData() { return null; }
    }
}
