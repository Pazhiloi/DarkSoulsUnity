using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class ResetAnimatorBool : StateMachineBehaviour
    {
        AnimationDemoController controller;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (controller == null)
            {
                controller = animator.GetComponent<AnimationDemoController>();
            }

            controller.isUsingRootMotion = false;
            controller.allowMovement = true;
            controller.allowRotation = true;
            animator.SetBool("isPerformingAction", false);
            animator.SetBool("isPerformingBackStep", false);
        }
    }
}
