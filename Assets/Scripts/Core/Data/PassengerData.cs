using System;
using System.Collections;
using System.Collections.Generic;
using Game.Level;
using Game.Utils;
using UnityEngine;


namespace Game.Data
{


    [Serializable]
    public struct PassengerData
    {
        public Vector2Int gridPosition;
        public ColorList color;

        public PassengerData(Vector2Int position, ColorList color)
        {
            gridPosition = position;
            this.color = color;
        }

        public static PassengerData CreateFromPassenger(Passenger passenger)
        {
            return CreateFromPassenger(passenger, passenger.attachedCell);
        }

        public static PassengerData CreateFromPassenger(Passenger passenger, GridCell attachedCell)
        {
            if (passenger == null || attachedCell == null)
            {
                Debug.LogError($"Passenger or attached cell is null, cannot create PassengerData.");
                if (passenger != null)
                {
                    Debug.LogError($"Passenger color: {passenger.color}, position: {passenger.transform.position}, instanceId: {passenger.GetInstanceID()}");
                }
                return new();
            }

            return new PassengerData(attachedCell.position, passenger.color);
        }
    }

}