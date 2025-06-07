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
        private const float DEFAULT_MOVEMENT_DURATION = 1f;
        private const float DEFAULT_SHAKE_ROTATION_DURATION = .35f;
        private const Ease DEFAULT_MOVEMENT_EASE = Ease.InOutQuad;

        private static readonly Vector3 _shakeRotationStrength = new Vector3(2f, 0f, 2f);

        public static Vector3 spawnPosition => GameManager.instance.busSpawnPosition.position;
        public static Vector3 dissappearingPosition => GameManager.instance.busDisappearPosition.position;

        private Tween _currentTween;

        private void InitMovement()
        {
            onBusArrivedDestination += OnBusArrivedItsDestination;
        }

        public Tween Move(in Vector3 targetPosition)
        {
            ReleaseCurrentTween();
            _currentTween = Tween.Position(transform, targetPosition, DEFAULT_MOVEMENT_DURATION, DEFAULT_MOVEMENT_EASE);
            return _currentTween;
        }

        public void ShakeBus()
        {
            if (_currentTween.isAlive) return;
            _currentTween = Tween.ShakeLocalRotation(transform, _shakeRotationStrength, DEFAULT_SHAKE_ROTATION_DURATION, 10, asymmetryFactor: .6f, cycles: -1, easeBetweenShakes: Ease.InOutBounce);
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