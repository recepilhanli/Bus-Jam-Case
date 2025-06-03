using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


namespace Game
{
    /// <summary>
    /// Strip Debug calls in builds except in the Unity Editor.
    /// </summary>
    public static class Debug
    {
        [Conditional("UNITY_EDITOR")]
        public static void Log(object message)
        {
            UnityEngine.Debug.Log(message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void LogWarning(object message)
        {
            UnityEngine.Debug.LogWarning(message);
        }


        [Conditional("UNITY_EDITOR")]
        public static void LogError(object message)
        {
            UnityEngine.Debug.LogError(message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void LogException(System.Exception exception)
        {
            UnityEngine.Debug.LogException(exception);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Assert(bool condition, object message = null)
        {
            if (!condition)
            {
                UnityEngine.Debug.LogAssertion(message);
            }
        }
    }

}