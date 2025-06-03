using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Level
{
    public partial class GridManager
    {

        public GridCell[,] secondaryGrid;
        public int secondaryWidth => secondaryGrid.GetLength(0);
        public int secondaryHeight => secondaryGrid.GetLength(1);


    }
}