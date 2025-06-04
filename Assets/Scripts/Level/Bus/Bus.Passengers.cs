using System.Collections;
using System.Collections.Generic;
using Game;
using Game.Level.Pooling;
using UnityEngine;

namespace Game.Level
{
    public partial class Bus : MonoBehaviour, IPoolable
    {
        public const int MAX_PASSENGERS = 3;

        [Header("Passenger Settings")]
        [Tooltip("Read-Only")][SerializeField] Passenger[] passengers = null;
        private int _totalPassengersInBus = 0;

        private void InitPassengers()
        {
            passengers = new Passenger[MAX_PASSENGERS];
            onPassengerGetOnBus += OnPassengerGetOn;
        }

        public void ReleasePassengers()
        {
            foreach (var passenger in passengers)
            {
                if (passenger != null)
                {
                    passenger.ReturnToPool();
                }
            }

            _totalPassengersInBus = 0;
        }

        public bool TryAddPassenger(Passenger passenger)
        {
            for (int i = 0; i < passengers.Length; i++)
            {
                if (passengers[i] == null)
                {
                    passengers[i] = passenger;
                    return true;
                }
            }
            return false; // No empty slot found
        }

        private void OnPassengerGetOn(Passenger passenger)
        {
            _totalPassengersInBus++; 
            Debug.Log($"Passenger {passenger.name} got on the bus. Total passengers: {_totalPassengersInBus}/{MAX_PASSENGERS}");
            Debug.Assert(GameManager.instance.activeBus == this, "This must called only for the active bus.");
            if (_totalPassengersInBus >= MAX_PASSENGERS) GameManager.instance.ActivateNextBus();
        }


    }
}