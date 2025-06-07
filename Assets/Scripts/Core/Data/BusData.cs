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
            data.buses.AddRange(manager.busList);
            return data;
        }
    }

}