using System;
using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using UnityEngine;


namespace Game.Data
{
    using Grid = Level.Grid;
    [Serializable]
    public class GridData
    {
        public Vector2Int gridSize = Vector2Int.zero;
        public Vector2 padding = Vector2.zero;
        public Vector2 spacing = Vector2.zero;
        public float cellSize = 1f;

        public static readonly GridData defaultPrimaryGrid = new GridData
        {
            gridSize = new Vector2Int(5, 1),
            padding = Vector2.zero,
            spacing = new Vector2(.5f, 0),
            cellSize = 1f
        };

        public static readonly GridData defaultSecondaryGrid = new GridData
        {
            gridSize = new Vector2Int(4, 5),
            padding = new Vector2(0f, 0),
            spacing = new Vector2(1f, 1f),
            cellSize = 1f
        };

        public static void Copy(GridData source, GridData target)
        {
            if (source == null || target == null) return;

            target.gridSize = source.gridSize;
            target.padding = source.padding;
            target.spacing = source.spacing;
            target.cellSize = source.cellSize;
        }


        public static void Copy(Grid source, GridData target)
        {
            if (source == null || target == null) return;

            target.gridSize = new Vector2Int(source.width, source.height);
            target.padding = source.padding;
            target.spacing = source.spacing;
            target.cellSize = source.cellSize;
        }
    }


    [Serializable]
    public class ExtendedGridData : GridData
    {
        public List<PassengerData> passengers = new List<PassengerData>();
        public List<ObstacleData> obstacles =  new List<ObstacleData>();

    }

}