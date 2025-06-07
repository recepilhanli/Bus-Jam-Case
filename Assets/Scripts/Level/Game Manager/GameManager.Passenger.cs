using System;
using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.UI;
using Game.Utils;
using PrimeTween;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Level
{

    public partial class GameManager
    {
        [Header("Passenger Settings")]
        public Material markPassengerMaterial;

        private static readonly Vector3 _passengerShakeStrength = new Vector3(0.5f, 0.25f, 0.5f);


        private void InitPassengers()
        {
            onPlayerAttemptedToMovePassenger += AttempMovePassenger;
        }


        /// <summary>
        /// Moves the passenger to the active bus if the bus color matches the passenger's color.
        /// </summary>
        /// <param name="passenger"> The passenger to be moved.</param>
        /// <returns> True if the passenger was successfully moved, false otherwise.</returns>
        public bool TryMovePassengerToPrimaryGrid(Passenger passenger)
        {
            if (_activeBus == null)
            {
                Debug.LogError("Active bus is not assigned.");
                return false;
            }

            var passengerColor = passenger.color;

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

            if (!primaryGrid.hasSpace && _wasActiveBusArrived && activeBus.hasSpace && !PrimaryGridHasActivePassengers())
            {
                onLevelFailed?.Invoke();
                Debug.LogWarning("Primary grid has no space, but the active bus has space. Level failed.");
            }


            return true;
        }

        private void LoadPassengers(LevelContainer levelContainer)
        {
            var passengers = levelContainer.secondaryGrid.passengers;
            if (passengers != null)
            {
                foreach (var passengerData in passengers)
                {
                    var pos = passengerData.gridPosition;
                    if (secondaryGrid.IsValidPosition(pos)) Passenger.GetFromPool(pos, passengerData.color);
                }
            }
        }

        public void LoadPassengers(LevelSaveData levelSaveData)
        {
            if (levelSaveData == null) return;
            var primaryPassengers = levelSaveData.primaryGridPassengers;

            if (primaryPassengers != null)
            {
                foreach (var passengerData in primaryPassengers)
                {
                    var pos = passengerData.gridPosition;
                    if (primaryGrid.IsValidPosition(pos)) Passenger.GetFromPool(primaryGrid.cells[pos.x, pos.y], passengerData.color);
                }
            }

            var secondaryPassengers = levelSaveData.secondaryGridPassengers;
            if (secondaryPassengers != null)
            {
                foreach (var passengerData in secondaryPassengers)
                {
                    var pos = passengerData.gridPosition;
                    if (secondaryGrid.IsValidPosition(pos)) Passenger.GetFromPool(secondaryGrid.cells[pos.x, pos.y], passengerData.color);
                }
            }
        }





        private void AttempMovePassenger(Passenger passenger, bool success)
        {
            if (!success) Tween.ShakeScale(passenger.transform, _passengerShakeStrength, .5f);
        }

        public bool RemovePassengerFromPrimaryGrid(GridCell primaryCell)
        {
            Debug.Assert(primaryCell != null, "Cell is null.");
            Debug.Assert(primaryCell.isInPrimaryGrid, "Cell is not in the primary grid.");
            var passenger = primaryCell.RemovePassenger();
            passenger.MoveToActiveBus();
            Debug.Assert(passenger.color == activeBusColor, "Passenger color does not match the active bus color.");
            return true;
        }



    }

}