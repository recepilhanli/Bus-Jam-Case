using System;
using System.Collections;
using System.Collections.Generic;
using Game.Level.Pooling;
using UnityEngine;
using UnityEngine.AI;


namespace Game.Level
{

    public partial class Passenger : MonoBehaviour, IPoolable
    {

        public void OnSpawn(in Vector3 position, in Quaternion rotation)
        {
            gameObject.SetActive(true);
        }

        public void OnDespawn()
        {
            gameObject.SetActive(false);
        }

        public void ReturnToPool() => PoolManager.GetPool(PoolTypes.Passenger).ReturnToPool(this);
    }

}