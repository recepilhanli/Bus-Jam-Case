using System.Collections;
using System.Collections.Generic;
using Game;
using Game.Level.Pooling;
using Game.Utils;
using UnityEngine;

namespace Game.Level
{
    public partial class Bus : MonoBehaviour, IPoolable
    {
        public bool isActiveBus => GameManager.instance.activeBus == this;

        private void Awake()
        {
            InitPassengers();
            InitMovement();
        }

        #region  Pooling
        public void OnSpawn()
        {
            gameObject.SetActive(true);
        }

        public void OnDespawn()
        {
            gameObject.SetActive(false);
            for (int i = 0; i < passengers.Length; i++)
            {
                passengers[i] = null;
            }

            if (_currentTween.isAlive) _currentTween.Stop();
        }

        public void ReturnToPool() => PoolManager.GetPool(PoolTypes.Bus).ReturnToPool(this);

        public static Bus GetFromPool(in Vector3 position, ColorList color)
        {
            var bus = PoolManager.GetObject<Bus>(PoolTypes.Bus, in position);
            bus.transform.position = position;
            bus.color = color;
            return bus;
        }
        #endregion
    }
}