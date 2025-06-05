using System.Collections;
using System.Collections.Generic;
using Game.Data;
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
                _activeBus.Move(activeBusPosition.position).OnComplete(_activeBus, DispatchNewActiveBusArrivedEvent);
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
                else if (_currentColorIndex + 1 >= busList.Count) return;

                _nextBus = value;
                _nextBus.gameObject.SetActive(true);
                _nextBus.transform.position = busSpawnPosition.position;
                _nextBus.color = busList[_currentColorIndex + 1];
                _nextBus.Move(nextBusPosition.position);
            }
        }

        public void ActivateNextBus()
        {
            _currentColorIndex++;
            var oldReservedBus = _reservedBus;
            _reservedBus = activeBus;
            _reservedBus.Move(busDisappearPosition.position).OnComplete(_reservedBus, DispatchOldActiveBusLeftEvent);
            Debug.Log($"Bus {_reservedBus.name} left the station. Color: {_reservedBus.color}");

            if (_currentColorIndex >= busList.Count)
            {
                onLevelCompleted?.Invoke();
                _currentColorIndex = 0; // Reset to loop through colors
                Debug.Log("All buses have left. Level completed.");
                return;
            }

            activeBus = _nextBus;
            nextBus = oldReservedBus;
        }



        private void LoadBuses(in BusData data)
        {
            busList.Clear();
            busList.AddRange(data.buses);
            _currentColorIndex = 0;

            if (busList.Count == 0)
            {
                Debug.LogWarning("No buses found in the bus list. Please check your LevelData -> BusData configuration.");
                return;
            }

            if (_activeBus)
            {
                activeBus.color = busList[_currentColorIndex];
                activeBus.transform.position = activeBusPosition.position;
            }
            else activeBus = Bus.GetFromPool(activeBusPosition.position, busList[_currentColorIndex]);

            if (_nextBus && busList.Count > 1)
            {
                nextBus.color = busList[_currentColorIndex + 1];
                nextBus.transform.position = nextBusPosition.position;
            }
            else if (busList.Count > 1) _nextBus = Bus.GetFromPool(nextBusPosition.position, busList[_currentColorIndex + 1]);
            else _nextBus = Bus.GetFromPool(busSpawnPosition.position, ColorList.White);

            if (_reservedBus && busList.Count > 2) _reservedBus.color = busList[_currentColorIndex + 2];
            else if (busList.Count > 2) _reservedBus = Bus.GetFromPool(busSpawnPosition.position, busList[_currentColorIndex + 2]);
            else _reservedBus = Bus.GetFromPool(busSpawnPosition.position, ColorList.White);

            _activeBus.ShakeBus();
        }

    }

}