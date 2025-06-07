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
        private const float DEFAULT_MOVE_MIN_DURATION = .05f;
        private const float DEFAULT_MOVE_MAX_DURATION = .85f;
        private const float DEFAULT_MOVE_MAX_DISTANCE = 12f;
        private const float DEFAULT_MOVE_MIN_DISTANCE = 0.1f;
        private const Ease DEFAULT_MOVE_EASE = Ease.Linear;

        private static NavMeshPath _navMeshPath = null;
        private Sequence _movementSequence;

        public Sequence MoveToCell(GridCell cell)
        {
            //get position array from navmesh
            Debug.Assert(cell != null, "Cell cannot be null when moving passenger.");

            GoTo(cell.worldPosition);
            EnableMoveAnimation();
            _movementSequence.OnComplete(DisableMoveAnimation);


            return _movementSequence;
        }


        public void MoveToActiveBus()
        {
            Debug.Assert(activeBus != null, "Active bus is not assigned in GameManager.");
            if (activeBus && !activeBus.TryAddPassenger(this)) return;

            GoTo(activeBusPosition);

            _movementSequence.OnComplete(this, activeBus.onPassengerGetOnBus);
            EnableMoveAnimation();
            return;
        }


        /// <summary>
        /// Moves the passenger to the target position using NavMesh.
        /// </summary>
        private void GoTo(in Vector3 targetPosition)
        {
            if (_movementSequence.isAlive) _movementSequence.Stop();
            _movementSequence = Sequence.Create();
            _navMeshPath.ClearCorners();

            if (NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, _navMeshPath))
            {
                var corners = _navMeshPath.corners;
                for (int i = (corners.Length > 1) ? 1 : 0; i < _navMeshPath.corners.Length; i++)
                {
                    Vector3 corner = corners[i];
                    float duration = CalculateMovementDuration(in corner);
                    _movementSequence.Chain(Tween.Position(transform, corner, duration, DEFAULT_MOVE_EASE));
                }
            }
            else
            {
                _movementSequence.Chain(Tween.Position(transform, targetPosition, DEFAULT_MOVE_MAX_DURATION, DEFAULT_MOVE_EASE));
                Debug.LogWarning($" Failed to calculate path to target position {targetPosition}. Moving directly to position.");
            }
        }

        private float CalculateMovementDuration(in Vector3 targetPosition) //smoothness is important :)
        {
            float distance = Vector3.Distance(transform.position, targetPosition);

            //remap 
            float duration = Mathf.Lerp(DEFAULT_MOVE_MIN_DURATION, DEFAULT_MOVE_MAX_DURATION, Mathf.InverseLerp(DEFAULT_MOVE_MIN_DISTANCE, DEFAULT_MOVE_MAX_DISTANCE, distance));
            return duration;
        }



    }

}