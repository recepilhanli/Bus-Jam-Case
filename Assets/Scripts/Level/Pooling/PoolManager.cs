using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Game.Data;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Game.Level.Pooling
{

    public static class PoolManager
    {
        private const int GAME_SCENE_BUILD_INDEX = 1;
        private const string POOL_CONTAINER_PATH = "Pooling/Pool Container";

        private static Dictionary<PoolTypes, Pool> _pools = new Dictionary<PoolTypes, Pool>();

        public static Transform poolParent => _poolParent;

        private static Transform _poolParent = null;



        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            var container = Resources.Load<PoolContainer>(POOL_CONTAINER_PATH);
            foreach (var pooledObject in container.pooledObjects)
            {
                _pools.Add(pooledObject.poolType, Pool.CreateFromContainer(pooledObject));
            }

            _poolParent = new GameObject("[Object Pooling]").transform;
            _poolParent.gameObject.SetActive(true);
            _poolParent.hideFlags = HideFlags.HideInInspector;
            Object.DontDestroyOnLoad(_poolParent.gameObject);
            SceneHelper.onRequestSceneLoad += OnSceneChanged;
        }

        private static void OnSceneChanged(Scene newScene)
        {
            if (newScene.buildIndex != GAME_SCENE_BUILD_INDEX) ResetAllPools();
        }

        public static void ResetAllPools()
        {
            foreach (var pool in _pools.Values)
            {
                pool.ReturnAllToPool();
            }
        }

        public static Pool GetPool(PoolTypes poolType)
        {
            if (_pools.TryGetValue(poolType, out var pool))
            {
                return pool;
            }
            Debug.LogError($"Pool type {poolType} not found.");
            return null;
        }


        public static IPoolable GetObject(PoolTypes poolType, in Vector3 position, in Quaternion rotation = default)
        {
            if (_pools.TryGetValue(poolType, out var pool))
            {
                return pool.GetObject(in position, in rotation);
            }
            Debug.LogError($"Pool type {poolType} not found.");
            return null;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetObject<T>(PoolTypes poolType, in Vector3 position, in Quaternion rotation = default) where T : MonoBehaviour
        {
            var gameObject = GetObject(poolType, in position, in rotation);
            return Serialize<T>(gameObject);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T Serialize<T>(IPoolable poolable) where T : MonoBehaviour
        {
            return (T)poolable;
        }

        public static PoolTypes GetPoolType<T>() where T : MonoBehaviour
        {
            if (typeof(T) == typeof(Passenger)) return PoolTypes.Passenger;
            else if (typeof(T) == typeof(Bus)) return PoolTypes.Bus;
            else if (typeof(T) == typeof(GridCell)) return PoolTypes.GridCell;
            return PoolTypes.Unkown;
        }

    }

}