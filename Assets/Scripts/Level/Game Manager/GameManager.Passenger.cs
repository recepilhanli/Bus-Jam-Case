using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using UnityEngine;

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
        public bool MovePassengerToPrimaryGrid(Passenger passenger)
        {
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
            return true;
        }



    }

}