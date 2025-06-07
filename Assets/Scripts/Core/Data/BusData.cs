using System;
using System.Collections;
using System.Collections.Generic;
using Game.Level;
using Game.Utils;
using UnityEngine;


namespace Game.Data
{

    [Serializable]
    public struct BusData
    {
        public List<ColorList> buses;

        public static BusData GetCurrentBuses()
        {
            BusData data = new BusData();
            if (GameManager.instance == null) return data;
            var manager = GameManager.instance;
            data.buses = new List<ColorList>();
            for(int i = 0; i < manager.busList.Count; i++)
            {
                if(manager.currentBusIndex > i) continue; // Skip old buses that the player already passed
                var bus = manager.busList[i];
                data.buses.Add(bus);
            }
            
            return data;
        }
    }

}