using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.UI;
using PrimeTween;
using UnityEngine;

namespace Game.Level
{

    public partial class GameManager : MonoSingleton<GameManager>
    {


        private async UniTaskVoid Start()
        {
            InitTapping();
            await InitLevels();
            InitGrids();
            InitBuses();
            InitPassengers();
        }


        public void Reset()
        {
            _currentBusIndex = 0;
            _wasActiveBusArrived = false;
            _nextBus = null;
            _activeBus = null;
            _reservedBus = null;
            StopTimer();
        }


    }

}