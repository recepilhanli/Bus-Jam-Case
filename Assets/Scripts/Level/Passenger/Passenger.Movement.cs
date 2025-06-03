using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using UnityEngine;
using UnityEngine.AI;


namespace Game.Level
{

    public partial class Passenger
    {
        public NavMeshAgent agent;

        public void MoveToCell(GridCell cell)
        {
            if (agent == null)
            {
                Debug.LogError("NavMeshAgent is not assigned.");
                return;
            }

            transform.position = cell.worldPosition; //Temp
        }

        public void MoveToBus(Bus bus)
        {
            if (agent == null)
            {
                Debug.LogError("NavMeshAgent is not assigned.");
                return;
            }

            transform.position = bus.transform.position; //Temp: Will be animated later
            bus.AddPassenger(this); //Temp: Will be animated later (It has to called after passenger get on the bus)
        }

        public void MoveToPrimaryGrid() //Temp
        {
            ColorList busColor = activeBus.color;

            if (busColor == Color)
            {
                MoveToBus(activeBus);
                return;
            }


            Grid primaryGrid = GameManager.instance.primaryGrid;
            Debug.Assert(primaryGrid != null, "Primary grid is not assigned in GameManager.");
            GridCell cell = primaryGrid.GetEmptyCell();
            if (cell == null)
            {
                Debug.LogError("No empty cell found in the primary grid.");
                return;
            }
            transform.position = cell.worldPosition; //Temp
            cell.SetPassenger(this); // Set the passenger in the cell
        }


    }

}