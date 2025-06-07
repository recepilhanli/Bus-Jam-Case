using System.Collections;
using System.Collections.Generic;
using Game.Level;
using UnityEngine;

namespace Game.Data
{
    public class LevelSaveData
    {
        public List<PassengerData> secondaryGridPassengers = new List<PassengerData>();
        public List<PassengerData> primaryGridPassengers = new List<PassengerData>();
        public BusData remainingBuses = new BusData();
        public float remainingTime = 0f;


        public static LevelSaveData GetCurrentLevel()
        {
            if (GameManager.instance == null) return null;
            var manager = GameManager.instance;

            var primaryGridPassengers = new List<PassengerData>();
            var secondaryGridPassengers = new List<PassengerData>();

            var primaryGrid = manager.primaryGrid;
            var secondaryGrid = manager.secondaryGrid;


            foreach (var cell in primaryGrid.cells)
            {
                if (cell.isEmpty) continue;
                PassengerData passengerData = PassengerData.CreateFromPassenger(cell.passenger, cell);
                primaryGridPassengers.Add(passengerData);
            }

            foreach (var cell in secondaryGrid.cells)
            {
                if (cell.isEmpty) continue;
                PassengerData passengerData = PassengerData.CreateFromPassenger(cell.passenger, cell);
                secondaryGridPassengers.Add(passengerData);
            }

            BusData remainingBusesData = BusData.GetCurrentBuses();

            LevelSaveData saveData = new LevelSaveData
            {
                primaryGridPassengers = primaryGridPassengers,
                secondaryGridPassengers = secondaryGridPassengers,
                remainingBuses = remainingBusesData,
                remainingTime = manager.remainingTime
            };

            return saveData;
        }

    }

}