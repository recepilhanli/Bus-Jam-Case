using System.Collections;
using System.Collections.Generic;
using Game;
using Game.Level.Pooling;
using Game.Utils;
using PrimeTween;
using TMPro;
using UnityEngine;

namespace Game.Level
{
    public partial class Bus : MonoBehaviour, IPoolable
    {
        private readonly static Color _darkGrayColor = new Color(0.2f, 0.2f, 0.2f, 1f);

        public const int MAX_PASSENGERS = 3;
        private const float DEFAULT_SHAKE_DURATION = .4f;
        private static readonly Vector3 _shakeScaleStrength = new Vector3(.5f, .5f, 0.5f);



        [Header("Passenger Settings")]
        [SerializeField] private TextMeshProUGUI _passengerCountTmp = null;
        [Tooltip("Read-Only")][SerializeField] Passenger[] passengers = null;

        private int _totalPassengersInBus = 0;

        /// <summary>
        /// Currently active passengers in the bus (Workd diffirent than passengers array)
        /// </summary>
        public int totalPassengersInBus
        {
            get => _totalPassengersInBus;

            private set
            {
                Debug.Assert(value >= 0 && value <= MAX_PASSENGERS, $"Total passengers in bus must be between 0 and {MAX_PASSENGERS}. Current value: {value}");
                if (value == _totalPassengersInBus) return;
                _totalPassengersInBus = value;
                _passengerCountTmp.text = $"{_totalPassengersInBus}/{MAX_PASSENGERS}";

                if (value != 0) Tween.Color(_passengerCountTmp, ColorHelper.randomColor, _darkGrayColor, 0.5f);
            }
        }

        

        public bool hasSpace => availablePassengerSlots > 0;

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

            totalPassengersInBus = 0;
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
            totalPassengersInBus++;
            Debug.Log($"Passenger {passenger.name} got on the bus. Total passengers: {totalPassengersInBus}/{MAX_PASSENGERS}");
            Debug.Assert(GameManager.instance.activeBus == this, "This must called only for the active bus.");
            if (totalPassengersInBus >= MAX_PASSENGERS) GameManager.instance.ActivateNextBus();
            Tween.PunchScale(transform, _shakeScaleStrength, DEFAULT_SHAKE_DURATION);

            passenger.DisableMoveAnimation();
            passenger.ReturnToPool();
        }


    }
}