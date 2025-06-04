using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using PrimeTween;
using UnityEngine;
using UnityEngine.AI;


namespace Game.Level
{

    public partial class Passenger
    {
        private const float DEFAULT_MOVE_DURATION = 1.5f;
        private const Ease DEFAULT_MOVE_EASE = Ease.OutQuad;

        public Sequence MoveToCell(GridCell cell) //TO DO: Sample NavMesh for pathfinding
        {
            Sequence movementSequence = Sequence.Create()
           .Chain(Tween.Position(transform, cell.worldPosition, DEFAULT_MOVE_DURATION, DEFAULT_MOVE_EASE))
           .OnComplete(DisableMoveAnimation);

            EnableMoveAnimation();
            return movementSequence;
        }

        public void MoveToActiveBus()
        {
            Debug.Assert(activeBus != null, "Active bus is not assigned in GameManager.");
            if (activeBus && !activeBus.TryAddPassenger(this)) return;
            Tween.Position(transform, GameManager.instance.activeBusPosition.position, DEFAULT_MOVE_DURATION, DEFAULT_MOVE_EASE)
            .OnComplete(this, activeBus.onPassengerGetOnBus);
            EnableMoveAnimation();
            return;
        }


    }

}