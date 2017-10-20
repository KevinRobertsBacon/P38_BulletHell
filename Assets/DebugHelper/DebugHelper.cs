namespace P38
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class DebugHelper
    {
        [SerializeField]
        private static bool ShouldDebug = true;

        public static void Log (string log)
        {
#if UNITY_EDITOR
            if (ShouldDebug)
                Debug.Log(log);
#endif
        }
    }
}
