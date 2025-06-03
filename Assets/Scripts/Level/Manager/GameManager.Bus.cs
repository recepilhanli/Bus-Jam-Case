using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Level
{

    public partial class GameManager
    {
        [Header("Buses")]
        public Bus activeBus;
        public Bus[] buses = new Bus[3];
    }

}