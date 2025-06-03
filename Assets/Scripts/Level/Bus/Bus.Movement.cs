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

        public static Vector3 spawnPosition => GameManager.instance.busSpawnPosition.position;
        public static Vector3 dissappearingPosition => GameManager.instance.busDisappearPosition.position;

        public Tween Move(in Vector3 targetPosition)
        {
            return Tween.Position(transform, targetPosition, DEFAULT_MOVEMENT_DURATION, DEFAULT_MOVEMENT_EASE).OnComplete(onBusArrivedDestination);
        }

        public void ReturnSpawnPoint()
        {
            transform.position = spawnPosition;
            ReleasePassengers();
        }
    }
}