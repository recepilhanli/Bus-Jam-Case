using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game
{

    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<T>(true);

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else _instance = this as T;

        }
    }

}