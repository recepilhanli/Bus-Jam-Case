using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Level
{
    public partial class GameManager
    {
        public event Action onGameStarted;
        public event Action onGamePaused;

        public event Action<float> onTimerChanged;

        public event Action<Bus> onActiveBusArrived;
        public event Action<Bus> onOldActiveBusLeft;
        
        public event Action onLevelCompleted;
        public event Action onLevelFailed;

        public event Action<Passenger, Bus> onPassengerGetOnBus;
        public event Action<Passenger, bool> onPlayerAttemptedToMovePassenger; //bool - true: success, false: failure

        private void DispatchOldActiveBusLeftEvent(Bus bus) //Prevent lambda allocation
        {
            onOldActiveBusLeft?.Invoke(bus);
            bus.onBusArrivedDestination?.Invoke();
            bus.ReturnSpawnPoint();
        }

        private void DispatchNewActiveBusArrivedEvent(Bus bus) //Prevent lambda allocation
        {
            onActiveBusArrived?.Invoke(bus);
            bus.onBusArrivedDestination?.Invoke();
        }

    }

}