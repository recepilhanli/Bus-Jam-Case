using System.Collections;
using System.Collections.Generic;
using Game;
using Game.Level.Pooling;
using UnityEngine;

namespace Game.Level
{
    public partial class Bus : MonoBehaviour, IPoolable
    {
        private const int MAX_PASSENGERS = 3;

        [Header("Passenger Settings")]
        [Tooltip("Read-Only")][SerializeField] Passenger[] passengers = null;

        private void InitPassengers()
        {
            passengers = new Passenger[MAX_PASSENGERS];
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
        }

        public void AddPassenger(Passenger passenger)
        {
            int lastIndex = 0;
            for (int i = 0; i < passengers.Length; i++)
            {
                if (passengers[i] == null)
                {
                    passengers[i] = passenger;
                    lastIndex = i;
                    break;
                }
            }

            if (lastIndex == MAX_PASSENGERS - 1) GameManager.instance.ActivateNextBus();
           


        
        }



    }
}