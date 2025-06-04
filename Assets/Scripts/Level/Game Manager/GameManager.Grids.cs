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
            CheckSecondaryGridFrontLine();
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

        [ContextMenu("Check Secondary Grid Front Line")]
        public void CheckSecondaryGridFrontLine()
        {
            int y = secondaryGrid.height - 1;
            for (int x = 0; x < secondaryGrid.width; x++)
            {
                var cell = secondaryGrid.cells[x, y];
                if (cell.isEmpty) continue;
                cell.passenger.MarkPassenger();
            }
        }

        public void CheckSecondaryGridNeighbourSpace(Vector2Int position)
        {
            foreach (var direction in Grid.directions)
            {
                Vector2Int neighborPosition = position + direction;
                if (!secondaryGrid.IsValidPosition(neighborPosition)) continue;

                GridCell neighborCell = secondaryGrid.cells[neighborPosition.x, neighborPosition.y];
                if (neighborCell.isEmpty) continue;
                else if (neighborCell.hasSpaceToMove) neighborCell.passenger.MarkPassenger();
            }
        }
    }

}