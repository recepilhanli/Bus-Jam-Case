using System;
using System.Collections;
using System.Collections.Generic;
using Game.Level.Pooling;
using Game.Utils;
using UnityEngine;

namespace Game.Level
{

    public partial class Passenger : MonoCached<Passenger>, IPoolable
    {
        public GridCell attachedCell = null;
        private Bus activeBus => GameManager.instance.activeBus;


        #region Pooling
        public void OnSpawn(in Vector3 position, in Quaternion rotation)
        {
            gameObject.SetActive(true);
            transform.SetPositionAndRotation(position, rotation);
        }
        public void OnDespawn()
        {
            gameObject.SetActive(false);
            transform.SetParent(PoolManager.poolParent, true);
            UnmarkPassenger();
        }

        public void ReturnToPool() => PoolManager.GetPool(PoolTypes.Passenger).ReturnToPool(this);

        public static Passenger GetFromPool(GridCell cell, ColorList color)
        {
            Debug.Assert(cell != null, "GridCell cannot be null when getting a Passenger from the pool.");
            Debug.Assert(cell.isEmpty, "GridCell must be empty when getting a Passenger from the pool.");

            Vector3 position = cell.worldPosition;

            var passenger = PoolManager.GetObject<Passenger>(PoolTypes.Passenger, in position);
            passenger.Color = color;
            cell.SetPassenger(passenger);
            return passenger;
        }
        #endregion
    }

}