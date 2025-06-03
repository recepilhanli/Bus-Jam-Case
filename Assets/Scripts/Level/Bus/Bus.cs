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

        private void Awake()
        {
            InitPassengers();
        }

        #region  Pooling
        public void OnSpawn(in Vector3 position, in Quaternion rotation = default)
        {
            gameObject.SetActive(true);
        }

        public void OnDespawn()
        {
            gameObject.SetActive(false);

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