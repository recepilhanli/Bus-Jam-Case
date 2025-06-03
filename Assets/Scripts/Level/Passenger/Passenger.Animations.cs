using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Game.Level
{

    public partial class Passenger : MonoBehaviour
    {
        private static readonly int anim_moving = Animator.StringToHash("Moving");

        [Header("Animations")]
        public Animator animator;

        public void SetAnimatorToMoving(bool isMoving) => animator.SetBool(anim_moving, isMoving);

    }

}