namespace P38
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Low level class designed to send out input events.
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            //this region is for the get button events.
#region GET_BUTTON
            if (Input.GetButton("Up"))
            {
                EventManager.TriggerEvent(new EventOnInputRecieved(new InputData(InputKey.Up, InputType.Hold)));
            }
            if (Input.GetButton("Down"))
            {
                EventManager.TriggerEvent(new EventOnInputRecieved(new InputData(InputKey.Down, InputType.Hold)));
            }
            if (Input.GetButton("Right"))
            {
                EventManager.TriggerEvent(new EventOnInputRecieved(new InputData(InputKey.Right, InputType.Hold)));
            }
            if (Input.GetButton("Left"))
            {
                EventManager.TriggerEvent(new EventOnInputRecieved(new InputData(InputKey.Left, InputType.Hold)));
            }
#endregion

            //this region is for the get button down events.
#region GET_BUTTON_DOWN
            if (Input.GetButtonDown("MainButton1"))
            {
                EventManager.TriggerEvent(new EventOnInputRecieved(new InputData(InputKey.MainButton1, InputType.Down)));
            }
            if (Input.GetButtonDown("MainButton2"))
            {
                EventManager.TriggerEvent(new EventOnInputRecieved(new InputData(InputKey.MainButton2, InputType.Down)));
            }
#endregion
        }
    }
}
