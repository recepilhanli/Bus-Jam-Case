using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using UnityEngine;

namespace Game.Level
{

    public partial class GameManager
    {
        private const int MAX_BUS = 3;

        [Header("Bus Settings")]

        public List<ColorList> busList = new List<ColorList>();
        public ColorList activeBusColor => activeBus.color;


        [Space]
        public Transform busSpawnPosition;
        public Transform activeBusPosition;
        public Transform nextBusPosition;
        public Transform busDisappearPosition;

        private Bus _activeBus;
        private Bus _nextBus;
        private Bus _reservedBus;
        private int _currentColorIndex = 0;

        public Bus activeBus
        {
            get => _activeBus;
            private set
            {
                if (value == null)
                {
                    _nextBus = null;
                    return;
                }
                _activeBus = value;
                _activeBus.gameObject.SetActive(true);
                _activeBus.Move(activeBusPosition.position);
            }
        }

        public Bus nextBus
        {
            get => _nextBus;
            private set
            {
                if (value == null)
                {
                    _nextBus = null;
                    return;
                }
                else if (_currentColorIndex >= busList.Count) return;

                _nextBus = value;
                _nextBus.gameObject.SetActive(true);
                _nextBus.transform.position = busSpawnPosition.position;
                _nextBus.color = busList[_currentColorIndex + 1];
                _nextBus.Move(nextBusPosition.position);
            }
        }



        public void ActivateNextBus()
        {
            var oldActiveBus = _activeBus;
            _currentColorIndex++;
            oldActiveBus.Move(busDisappearPosition.position).OnComplete(oldActiveBus.ReturnSpawnPoint);

            if (_currentColorIndex >= busList.Count) //TO DO: end the game with animation
            {
                _currentColorIndex = 0; // Reset to loop through colors
                return;
            }

            activeBus = _nextBus;
            nextBus = _reservedBus;
            _reservedBus = oldActiveBus;
        }


        private void InitBuses()
        {
            _activeBus = Bus.GetFromPool(activeBusPosition.position, busList[0]);

            if (busList.Count > 1) _nextBus = Bus.GetFromPool(nextBusPosition.position, busList[1]);
            else _nextBus = Bus.GetFromPool(busSpawnPosition.position, ColorList.White);

            if (busList.Count > 2) _reservedBus = Bus.GetFromPool(busDisappearPosition.position, busList[2]);
            else _reservedBus = Bus.GetFromPool(busSpawnPosition.position, ColorList.White);
        }

    }

}