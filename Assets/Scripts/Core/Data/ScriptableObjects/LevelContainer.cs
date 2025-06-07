using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "LevelContainer", menuName = "Game/Level Container", order = 1)]
    public class LevelContainer : ScriptableObject
    {
        public GridData primaryGrid = new GridData();
        public ExtendedGridData secondaryGrid = new ExtendedGridData();
        public BusData busData = new BusData();
        public float timeConstraint = 60f;

        public void ChangePrimaryGrid(GridData newGridData)
        {
            GridData.Copy(newGridData, primaryGrid);
        }

        public void ChangeSecondaryGrid(GridData newGridData)
        {
            GridData.Copy(newGridData, secondaryGrid);
        }
    }

}