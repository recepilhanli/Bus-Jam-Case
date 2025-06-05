using System.Collections;
using System.Collections.Generic;
using Game.UI;
using Game.Utils;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Level
{

    public partial class GameManager
    {
        [Header("Passenger Settings")]
        public Material markPassengerMaterial;

        /// <summary>
        /// Moves the passenger to the active bus if the bus color matches the passenger's color.
        /// </summary>
        /// <param name="passenger"> The passenger to be moved.</param>
        /// <returns> True if the passenger was successfully moved, false otherwise.</returns>
        public bool TryMovePassengerToPrimaryGrid(Passenger passenger)
        {
            if(_activeBus == null)
            {
                Debug.LogError("Active bus is not assigned.");
                return false;
            }

            var passengerColor = passenger.Color;

            if (activeBusColor == passengerColor)
            {
                passenger.MoveToActiveBus();
                return true;
            }


            Debug.Assert(primaryGrid != null, "Primary grid is not assigned in GameManager.");
            GridCell cell = primaryGrid.GetEmptyCell();
            if (cell == null)
            {
                Debug.LogError("No empty cell found in the primary grid.");
                return false;
            }

            passenger.MoveToCell(cell);
            cell.SetPassenger(passenger);
 
            if (!primaryGrid.hasSpace) onLevelFailed?.Invoke();
            return true;
        }

        public bool RemovePassengerFromPrimaryGrid(GridCell primaryCell)
        {
            Debug.Assert(primaryCell != null, "Cell is null.");
            Debug.Assert(primaryCell.isInPrimaryGrid, "Cell is not in the primary grid.");
            var passenger = primaryCell.RemovePassenger();
            passenger.MoveToActiveBus();
            Debug.Assert(passenger.Color == activeBusColor, "Passenger color does not match the active bus color.");
            return true;
        }



    }

}