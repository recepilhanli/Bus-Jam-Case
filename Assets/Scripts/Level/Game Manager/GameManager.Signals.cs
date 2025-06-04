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

        public event Action<Bus> onActiveBusArrived;
        public event Action<Bus> onOldActiveBusLeft;

        public event Action<Passenger, Bus> onPassengerGetOnBus;
        public event Action<Passenger, bool> onPlayerAttemptedToMovePassenger; //bool - true: success, false: failure

        private void DispatchOldActiveBusLeftEvent() //Prevent lambda allocation
        {
            if (_reservedBus == null) return;
            onOldActiveBusLeft?.Invoke(_reservedBus);
            _reservedBus.ReturnSpawnPoint();
        }

        private void DispatchNewActiveBusArrivedEvent() //Prevent lambda allocation
        {
            if (_reservedBus == null) return;
            onActiveBusArrived?.Invoke(_reservedBus);
        }

    }

}