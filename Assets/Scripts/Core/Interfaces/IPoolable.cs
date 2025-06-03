
using System;
using UnityEngine;


namespace Game
{
    public interface IPoolable
    {
        public void OnSpawn(in Vector3 position, in Quaternion rotation = default);
        public void OnDespawn();
        public void ReturnToPool();
    }
}