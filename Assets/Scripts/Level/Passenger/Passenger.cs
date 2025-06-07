using System;
using System.Collections;
using System.Collections.Generic;
using Game.Level.Pooling;
using Game.Utils;
using PrimeTween;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Level
{

    public partial class Passenger : MonoCached<Passenger>, IPoolable
    {
        public static readonly Vector3 defaultScale = new Vector3(1f, .85f, 1f);
        
        public GridCell attachedCell = null;
        private Bus activeBus => GameManager.instance.activeBus;
        private static Vector3 activeBusPosition => GameManager.instance.activeBusPosition.position;

        private void Start()
        {
            _navMeshPath ??= new NavMeshPath();
        }

        #region Pooling
        public void OnSpawn()
        {
            gameObject.SetActive(true);
            transform.rotation = Quaternion.identity;
        }

        public void OnDespawn()
        {
            gameObject.SetActive(false);
            transform.SetParent(PoolManager.poolParent, true);
            Tween.StopAll(transform);
            if (_movementSequence.isAlive) _movementSequence.Stop();
            UnmarkPassenger();
        }

        public void ReturnToPool() => PoolManager.GetPool(PoolTypes.Passenger).ReturnToPool(this);

        public static Passenger GetFromPool(GridCell cell, ColorList color)
        {
            Debug.Assert(cell != null, "GridCell cannot be null when getting a Passenger from the pool.");
            Debug.Assert(cell.isEmpty, "GridCell must be empty when getting a Passenger from the pool.");

            Vector3 position = cell.worldPosition;

            var passenger = PoolManager.GetObject<Passenger>(PoolTypes.Passenger, in position);
            passenger.color = color;
            cell.SetPassenger(passenger);
            passenger.transform.position = position;
            return passenger;
        }

        public static Passenger GetFromPool(Vector2Int cellPos, ColorList color)
        {
            var cell = GameManager.instance.secondaryGrid.cells[cellPos.x, cellPos.y];
            Debug.Assert(cell != null, "GridCell cannot be null when getting a Passenger from the pool.");
            Debug.Assert(cell.isEmpty, "GridCell must be empty when getting a Passenger from the pool.");

            Vector3 position = cell.worldPosition;

            var passenger = PoolManager.GetObject<Passenger>(PoolTypes.Passenger, in position);
            passenger.color = color;
            cell.SetPassenger(passenger);
            passenger.transform.position = position;
            return passenger;
        }



        #endregion
    }

}