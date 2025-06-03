using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Level
{
    public partial class GridManager
    {

  
        public GridCell[,] primaryGrid;
        public int primaryWidth => primaryGrid.GetLength(0);
        public int primaryHeight => primaryGrid.GetLength(1);
        

        

    }
}