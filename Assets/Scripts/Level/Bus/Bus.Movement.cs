using System.Collections;
using System.Collections.Generic;
using Game;
using Game.Level.Pooling;
using Game.Utils;
using PrimeTween;
using UnityEngine;

namespace Game.Level
{
    public partial class Bus
    {
        private const float DEFAULT_MOVEMENT_DURATION = 2f;
        private const Ease DEFAULT_MOVEMENT_EASE = Ease.InOutQuad;

        private static readonly Vector3 _shakeStrength = new Vector3(.5f, .5f, .5f);

        public static Vector3 spawnPosition => GameManager.instance.busSpawnPosition.position;
        public static Vector3 dissappearingPosition => GameManager.instance.busDisappearPosition.position;

        private Tween _currentTween;

        private void InitMovement()
        {
            onBusArrivedDestination += OnBusArrivedItsDestination;
        }

        public Tween Move(in Vector3 targetPosition)
        {
            if (_currentTween.isAlive) _currentTween.Stop();
            _currentTween = Tween.Position(transform, targetPosition, DEFAULT_MOVEMENT_DURATION, DEFAULT_MOVEMENT_EASE);
            return _currentTween;
        }

        public void ShakeBus()
        {
            if (_currentTween.isAlive)
            {
                Debug.LogError("Cannot overwrite a movement tween while the bus is moving.");
                return;
            }

            _currentTween = Tween.ShakeLocalRotation(transform, _shakeStrength, 0.2f, 0.1f, true, Ease.InOutSine, cycles: -1);
        }

        public void ReturnSpawnPoint()
        {
            transform.position = spawnPosition;
            ReleasePassengers();
        }

        private void OnBusArrivedItsDestination()
        {
            if (isActiveBus)
            {
                ShakeBus();
                Debug.Log($"Active bus {name} arrived at its destination. Color: {color}");
            }
        }
    }
}