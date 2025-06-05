
using System;
using UnityEngine;


namespace Game
{
    public interface IPoolable
    {
        public void OnSpawn();
        public void OnDespawn();
        public void ReturnToPool();
    }
}