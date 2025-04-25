using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class IsPerformingFullyChargedAttack : StateMachineBehaviour
    {
        override public  void OnStateEnter(Animator animator, AnimatorStateInfo StateInfo, int layerIndex) {
          animator.SetBool("isPerformingFullyChargedAttack", true);
        }
    }
