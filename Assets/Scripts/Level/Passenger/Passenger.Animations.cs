using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;


namespace Game.Level
{

    public partial class Passenger
    {
        private static readonly int anim_moving = Animator.StringToHash("Moving");

        [Header("Animations")]
        public Animator animator;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetAnimatorToMoving(bool isMoving) => animator.SetBool(anim_moving, isMoving);

        //Prevent lambda allocations
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnableMoveAnimation() => SetAnimatorToMoving(true);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisableMoveAnimation() => SetAnimatorToMoving(false);




    }

}