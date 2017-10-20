namespace P38
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    public static class EventManager
    {
        private static Hashtable _listenerTable = new Hashtable();
        private static Dictionary<IEventListener, GameObject> _listenerGameObjects = new Dictionary<IEventListener, GameObject>();

        public static bool ReportEventWarnings = false;

        /// <summary>
        /// The maximum amount of events to be stored per event type
        /// </summary>
        private const int MaximumBufferSizePerEvent = 10000;

        /// <summary>
        /// The number of event attaches that constitues a "stale" unhandled buffer event
        /// </summary>
        private const int StaleBufferedEventTime = 100;

        /// <summary>
        /// Adding in support for buffering events - events that will get triggered once when triggered and once for any handlers subscribe later in buffered mode for that event type
        /// </summary>
        private static Dictionary<string, Queue<BufferedEventHolder>> bufferedEvents = new Dictionary<string, Queue<BufferedEventHolder>>();

        /// <summary>
        /// The group of all unhandled BufferedEvents used for checking stale unhandled events
        /// </summary>
        private static HashSet<BufferedEventHolder> unhandledBuffers = new HashSet<BufferedEventHolder>();

        /// <summary>
        /// Holds one buffered event and its birthdate used for accounting how unhandled a event is
        /// </summary>
        private class BufferedEventHolder
        {

            /// <summary>
            /// Initializes a new instance of the <see cref="BufferedEventHolder" /> class.
            /// </summary>
            /// <param name="inEvent">The event for this buffered event</param>
            public BufferedEventHolder(IEvent inEvent)
            {
                this.theEvent = inEvent;
                this.LastHandledDate = ListenerAttachDate;
                this.HasEverBeenHandled = false;
            }

            /// <summary>
            /// Gets or sets the event for this buffered event
            /// </summary>
            public IEvent theEvent
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the last time this buffered event was handled for this buffered event.
            /// </summary>
            public int LastHandledDate
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets whether or not this event has ever been handled.
            /// </summary>
            public bool HasEverBeenHandled
            {
                get;
                set;
            }


        }

        /// <summary>
        /// A incrementing date of attached listeners
        /// </summary>
        private static int ListenerAttachDate = 0;

        public static Hashtable ListenerTable
        {
            get
            {
                return _listenerTable;
            }
        }
        /// <summary>
        /// Adds a listener to the EventManager that will receive any events of the supplied event name
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static bool AddListener(IEventListener listener, string eventName)
        {
            if (listener == null || eventName == null)
            {
                Debug.LogError("Event Manager: AddListener failed due to no listener or event name specified.");
                return false;
            }

            if (!_listenerTable.ContainsKey(eventName))
                _listenerTable.Add(eventName, new ArrayList());

            ArrayList listenerList = _listenerTable[eventName] as ArrayList;
            if (listenerList == null)
                return false;

            if (listenerList.Contains(listener))
            {
                Debug.LogWarning("Event Manager: Listener: " + listener.GetType().ToString() + " is already in list for event: " + eventName + " May want to check your subscription logic.");
                return false; //listener already in list
            }

            ListenerAttachDate++;

            //adding in the buffered events for when a listener is added - all listeners recieve buffered events
            if (bufferedEvents.ContainsKey(eventName))
            {
                foreach (BufferedEventHolder evt in bufferedEvents[eventName])
                {

                    if (listener.HandleEvent(evt.theEvent) == ListenerResult.Ignored)
                    {
                        Debug.LogError("The listener, " + listener + " appears to be ignoring the event, " + evt.theEvent.GetName() + " to which it is subscribed. This is not optimal.");
                    }
                    else
                    {
                        evt.HasEverBeenHandled = true;
                        evt.LastHandledDate = ListenerAttachDate;
                        if (unhandledBuffers.Contains(evt))
                        {
                            unhandledBuffers.Remove(evt);
                        }
                    }
                }
     
            }

            //Checking for stale buffered events
            if (ReportEventWarnings)
            {
                foreach (BufferedEventHolder evt in unhandledBuffers)
                {
                    if ((ListenerAttachDate - evt.LastHandledDate) > StaleBufferedEventTime)
                    {
                        Debug.LogWarning("Event Manager: Stale (never handled for a long time) buffered event detected for Event \"" + evt.theEvent.GetName() + "\".");
                    }
                }
            }

            listenerList.Add(listener);

            var behaviour = listener as MonoBehaviour;
            if (behaviour != null)
            {
                var gob = behaviour.gameObject;
                if (gob != null)
                {
                    _listenerGameObjects[listener] = gob;
                }
            }
            return true;
        }



        /// <summary>
        /// Either adds or removes a listener from the subscribed event
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="listener"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static bool ManageSubscriber(SubscribeMode mode, IEventListener listener, string eventName)
        {
            switch (mode)
            {
                case SubscribeMode.Subscribe:
                    return AddListener(listener, eventName);
                case SubscribeMode.Unsubscribe:
                default:
                    return DetachListener(listener, eventName);

            }
        }


        /// <summary>
        /// Removes a listener from the subscribed event
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static bool DetachListener(IEventListener listener, string eventName)
        {

            if (!_listenerTable.ContainsKey(eventName))
                return false;

            ArrayList listenerList = _listenerTable[eventName] as ArrayList;
            if (listenerList == null)
                return false;

            if (!listenerList.Contains(listener))
                return false;

            listenerList.Remove(listener);

            _listenerGameObjects.Remove(listener);

            return true;
        }

        static string CheckEventName(IEvent evt)
        {
            var eventName = evt.GetName();

#if UNITY_EDITOR

            if (!eventName.Contains("Get") && !eventName.Contains("On") && !eventName.Contains("Request"))
            {
                Debug.LogWarning("Event class names should contain 'Get', 'On' or 'Request' -- " + eventName);
            }
            if (eventName != evt.GetType().Name)
            {
                Debug.LogWarning("EventName string constants should match the respective class names -- " + eventName + " != " + evt.GetType().Name);
            }
#endif
            return eventName;
        }


        static void BufferEvent(IEvent evt, ref BufferedEventHolder holder)
        {
            string eventName = evt.GetName();

            Queue<BufferedEventHolder> currentBuffer;
            if (!bufferedEvents.ContainsKey(eventName))
            {
                currentBuffer = new Queue<BufferedEventHolder>();
                bufferedEvents.Add(eventName, currentBuffer);
            }
            else
            {
                currentBuffer = bufferedEvents[eventName];
            }

            //if we have exceeded the maximum buffer size for this event then discard the oldest event
            if (currentBuffer.Count > MaximumBufferSizePerEvent)
            {
                Debug.LogWarning("Event Manager: Maximum buffered event size reach for Event \"" + eventName + "\".");
                currentBuffer.Dequeue();
            }

            holder = new BufferedEventHolder(evt);
            unhandledBuffers.Add(holder);
            currentBuffer.Enqueue(holder);

        }


        static bool HasNoListeners(string eventName)
        {
            if (!_listenerTable.ContainsKey(eventName))
            {
                if (ReportEventWarnings)
                {
                    Debug.LogWarning("Event Manager: Event \"" + eventName + "\" triggered has no listeners!");
                }
                return true; //No listeners for event so ignore it
            }
            return false;
        }

        /// <summary>
        /// Triggers an event instantly. 
        /// All listeners will execute their handleEvent inside this block, even if it is a widely subscribed event
        /// </summary>
        /// <param name="evt"></param>
        /// <returns></returns>
        public static bool TriggerEvent(IEvent evt, bool shouldBuffer = false)
        {
            string eventName = CheckEventName(evt);
            BufferedEventHolder currentBufferHolder = null;

            //checking to see if the event should get saved for buffered recievers
            if (shouldBuffer)
                BufferEvent(evt, ref currentBufferHolder);
            else if (HasNoListeners(eventName))
                return false;

            ArrayList listenerList = _listenerTable[eventName] as ArrayList;
            if (listenerList == null)
                return true;

            //  Changed this to a for loop because detaching a listener during the enumeration causes errors.
            for (var i = listenerList.Count - 1; i >= 0; --i)
            {
                i = Mathf.Clamp(i, 0, listenerList.Count - 1);  //  Ran into a case once where i became out of range.  Band-aid fix.  I tried moving anything that modifies _listenerTable to outside this loop with no good results.
                var listener = (IEventListener)listenerList[i];
                if (!ValidateListener(1, listener, eventName))
                    continue;

                // THE ACTUAL EVENT HANDLER //
                if (listener.HandleEvent(evt) == ListenerResult.Ignored)
                    Debug.LogError("The listener, " + listener + " appears to be ignoring the event, " + evt.GetName() + " to which it is subscribed. This is not optimal.");
                else if (shouldBuffer)
                    UnbufferEvent(currentBufferHolder);
            }

            return true;
        }


        static bool ValidateListener(int i, IEventListener listener, string eventName)
        {
            if (listener == null)
            {
                ((ArrayList)_listenerTable[eventName]).RemoveAt(i);
                return false;
            }

            if (_listenerGameObjects.ContainsKey(listener) && _listenerGameObjects[listener] == null)
            {
                RemoveListenerOnNullObject(i, listener, eventName);
                return false;
            }

            return true;
        }

        static void RemoveListenerOnNullObject(int i, IEventListener listener, string eventName)
        {
#if UNITY_EDITOR
            Debug.LogError("A listener for " + eventName + " is attached to a null game object and is being removed by the EventManger. (you need to have unsubscribe on destroy!!!)");
#endif
            ((ArrayList)_listenerTable[eventName]).RemoveAt(i);
            _listenerGameObjects.Remove(listener);

        }

        static void UnbufferEvent(BufferedEventHolder holder)
        {
            if (holder == null)
                return;

            holder.HasEverBeenHandled = true;
            holder.LastHandledDate = ListenerAttachDate;
            if (unhandledBuffers.Contains(holder))
                unhandledBuffers.Remove(holder);
        }


        /// <summary>
        /// returns whether the event has a registered listener
        /// </summary>
        /// <param name="evt"></param>
        /// <returns></returns>
        public static bool HasListener(IEvent evt)
        {
            return _listenerTable.ContainsKey(evt.GetName());
        }

        /// <summary>
        /// Completely restarts the manager
        /// forgets all subscribers of all event types
        /// </summary>
        public static void Reset()
        {
            _listenerTable = new Hashtable();
            _listenerGameObjects = new Dictionary<IEventListener, GameObject>();
        }
    }
}