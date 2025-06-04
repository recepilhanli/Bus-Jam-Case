using System;
using UnityEngine;

namespace Game.Level
{
    public partial class Bus
    {
        public Action onBusArrivedDestination;
        public Action<Passenger> onPassengerGetOnBus;
    }
}