using System;
using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using UnityEngine;


namespace Game.Data
{


    [Serializable]
    public struct PassengerData
    {
        public Vector2Int gridPosition;
        public ColorList color;

        public PassengerData(Vector2Int position, ColorList color)
        {
            gridPosition = position;
            this.color = color;
        }
    }

}