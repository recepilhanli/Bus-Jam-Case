using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Level
{
    public partial class GameManager
    {
        public event Action OnGameStarted;
        public event Action OnGamePaused;
        public event Action<Bus> onBusArrived;
        public event Action<Bus> onBusLeft;

        public event Action<Passenger, Bus> onPassengerGetOnBus;
        public event Action<Passenger, GridCell> onPassengerStartedToWaiting; //Primary Grid
        public event Action<Passenger> onPassengerSelected;
    }

}