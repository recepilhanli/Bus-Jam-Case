using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{

    /// <summary>
    /// Reduce Get Component calls by caching MonoBehaviour instances.
    /// </summary>
    /// <typeparam name="T"> Type of MonoBehaviour to cache.</typeparam>
    public class MonoCached<T> : MonoBehaviour where T : MonoBehaviour
    {
        public int instanceId => gameObject.GetInstanceID();
        public static Dictionary<int, T> _cache = new Dictionary<int, T>(); //<instanceId, ActualType>

        public static T GetActualType(GameObject gameObject)
        {
            if (_cache.TryGetValue(gameObject.GetInstanceID(), out T cachedInstance))
            {
                return cachedInstance;
            }

            Debug.LogError($"No cached instance found for GameObject with ID: {gameObject.GetInstanceID()}");
            return null;
        }

        public static T GetActualType(int instanceId)
        {
            if (_cache.TryGetValue(instanceId, out T cachedInstance))
            {
                return cachedInstance;
            }

            Debug.LogError($"No cached instance found for GameObject with ID: {instanceId}");
            return null;
        }


        protected virtual void Awake()
        {
            _cache.Add(instanceId, this as T);
        }

        protected virtual void OnDestroy()
        {
            _cache.Remove(instanceId);
        }

    }

}