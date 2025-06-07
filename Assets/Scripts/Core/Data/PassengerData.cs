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
            if (passenger == null || passenger.attachedCell == null)
            {
                Debug.LogError("Passenger or attached cell is null, cannot create PassengerData.");
                return new();
            }

            return new PassengerData(passenger.attachedCell.position, passenger.color);
        }
    }

}