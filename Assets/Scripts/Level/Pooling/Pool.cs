using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Game.Data;
using UnityEngine;


namespace Game.Level.Pooling
{
    public sealed class Pool
    {
        public Queue<IPoolable> PooledObjects => _pooledObjects;
        public HashSet<IPoolable> ActiveObjects => _activeObjects;
        
        private Queue<IPoolable> _pooledObjects;
        private HashSet<IPoolable> _activeObjects;
        public GameObject prefab;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Pool CreateFromContainer(PoolContainerObject containerObject)
        {
            if (containerObject.prefab == null)
            {
                Debug.LogError("Prefab is null. Cannot create pool.");
                return null;
            }

            Pool pool = new();
            pool.prefab = containerObject.prefab;
            pool._pooledObjects = new Queue<IPoolable>();
            pool._activeObjects = new HashSet<IPoolable>();
            pool._activeObjects.EnsureCapacity(10);
            return pool;
        }


        public IPoolable GetObject(in Vector3 position, in Quaternion rotation)
        {
            if (_pooledObjects.Count > 0)
            {
                IPoolable obj = _pooledObjects.Dequeue();
                _activeObjects.Add(obj);
                obj.OnSpawn(position, rotation);
                return obj;
            }

            IPoolable newObj = Create(in position, in rotation);
            _activeObjects.Add(newObj);
            newObj.OnSpawn(position, rotation);
            return newObj;
        }

        public void ReturnToPool(IPoolable obj)
        {
            if (_pooledObjects.Contains(obj))
            {
                Debug.LogWarning("Object is already in the pool.");
                return;
            }

            obj.OnDespawn();
            _pooledObjects.Enqueue(obj);
            _activeObjects.Remove(obj);
        }

        private IPoolable Create(in Vector3 position, in Quaternion rotation)
        {
            GameObject instance = UnityEngine.Object.Instantiate(prefab);
            IPoolable poolable = instance.GetComponent<IPoolable>();
            if (poolable == null)
            {
                Debug.LogError($"Prefab {prefab.name} does not implement IPoolable interface.");
                return null;
            }
            instance.transform.SetParent(PoolManager.poolParent, false);
            poolable.OnSpawn(position, rotation);
            return poolable;
        }

        public void ReturnAllToPool()
        {
            foreach (var obj in _activeObjects)
            {
                obj.OnDespawn();
                _pooledObjects.Enqueue(obj);
            }
            _activeObjects.Clear();
        }


        private Pool() { }
    }

}