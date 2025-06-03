using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Level.Pooling
{

    [CreateAssetMenu(fileName = "New Pool Container", menuName = "Game/Pooling/PoolContainer")]
    public class PoolContainer : ScriptableObject
    {
        public List<PoolContainerObject> pooledObjects => _pooledObjects;
        [SerializeField] private List<PoolContainerObject> _pooledObjects;
    }

    [Serializable]
    public class PoolContainerObject
    {
        public PoolTypes poolType;
        public GameObject prefab;
        
    }

}