using System.Collections;
using System.Collections.Generic;
using Game;
using Game.Level.Pooling;
using PrimeTween;
using UnityEngine;

namespace Game.Level
{
    public partial class Bus : MonoBehaviour, IPoolable
    {
        private const float DEFAULT_SHAKE_DURATION = .4f;
        private static readonly Vector3 _shakeScaleStrength = new Vector3(.5f, .5f, 0.5f);

        public const int MAX_PASSENGERS = 3;

        [Header("Passenger Settings")]
        [Tooltip("Read-Only")][SerializeField] Passenger[] passengers = null;
        private int _totalPassengersInBus = 0;

        public int totalPassengersInBus => _totalPassengersInBus;
        public int availablePassengerSlots
        {
            get
            {
                int emptySlots = 0;
                for (int i = 0; i < passengers.Length; i++)
                {
                    if (passengers[i] == null) emptySlots++;
                }
                return emptySlots;
            }
        }
        public bool hasSpace => availablePassengerSlots > 0;

        private void InitPassengers()
        {
            passengers = new Passenger[MAX_PASSENGERS];
            onPassengerGetOnBus += OnPassengerGetOn;
        }

        public void ReleasePassengers()
        {
            for (int i = 0; i < passengers.Length; i++)
            {
                var passenger = passengers[i];
                if (passenger != null)
                {
                    passenger.ReturnToPool();
                    passengers[i] = null;
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
            passenger.DisableMoveAnimation();
            Tween.PunchScale(transform, _shakeScaleStrength, DEFAULT_SHAKE_DURATION);
            passenger.transform.SetParent(transform, true);
        }


    }
}