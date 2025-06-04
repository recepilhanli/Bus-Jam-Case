using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Level
{
    public partial class GameManager
    {
        [Header("Grid Settings")]
        public Grid primaryGrid; //Near to the bus
        public Grid secondaryGrid;

        private void InitGrids()
        {
            onActiveBusArrived += CheckPrimaryGrid;
        }

        private void CheckPrimaryGrid(Bus arrivedBus)
        {
            Debug.Log($"Checking primary grid for bus: {arrivedBus.name}");
            int totalMovedPassengers = 0;
            foreach (var cell in primaryGrid.cells)
            {
                if (cell.isEmpty) continue;
                Passenger passenger = cell.passenger;
                Debug.Assert(passenger != null, "Primary grid cell has a passenger but it's null.");
                if (passenger.Color == arrivedBus.color)
                {
                    RemovePassengerFromPrimaryGrid(cell);
                    totalMovedPassengers++;
                    if (totalMovedPassengers >= Bus.MAX_PASSENGERS) break;
                }
            }
        }
    }

}