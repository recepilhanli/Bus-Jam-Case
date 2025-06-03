using System.Collections;
using System.Collections.Generic;
using Game;
using Game.Level.Pooling;
using UnityEngine;

namespace Game.Level
{
    public partial class Bus : MonoBehaviour, IPoolable
    {


        public void OnSpawn(in Vector3 position, in Quaternion rotation = default)
        {

        }

        public void OnDespawn()
        {

        }


        public void ReturnToPool() => PoolManager.GetPool(PoolTypes.Bus).ReturnToPool(this);
    }
}