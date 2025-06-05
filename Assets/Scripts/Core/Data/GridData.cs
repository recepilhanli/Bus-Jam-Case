using System;
using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using UnityEngine;


namespace Game.Data
{

    [Serializable]
    public class GridData
    {
        public Vector2Int gridSize = Vector2Int.zero;
        public Vector2 padding = Vector2.zero;
        public Vector2 spacing = Vector2.zero;
        public float cellSize = .5f;
    }


    [Serializable]
    public class ExtendedGridData : GridData
    {
        public List<PassengerData> passengers = null;
        public List<ObstacleData> obstacles = null;

    }

}