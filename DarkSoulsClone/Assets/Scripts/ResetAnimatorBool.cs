using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{

  public string isUsingRightHand = "isUsingRightHand";
  public bool isUsingRightHandStatus = false;

  public string isUsingLeftHand = "isUsingLeftHand";
  public bool isUsingLeftHandStatus = false;

  public string isInvulnerable = "isInvulnerable";
  public bool isInvulnerableStatus = false;

  public string isInteractingBool = "isInteracting";
  public bool isInteractingStatus = false;
  public string isFiringSpellBool = "isFiringSpell";
  public bool isFiringSpellStatus = false;
  public string isRotationWithRootMotion = "isRotationWithRootMotion";
  public bool isRotationWithRootMotionStatus = false;
  public string canRotateBool = "canRotate";
  public bool canRotateStatus = true;

  public string isMirroredBool = "isMirrored";
  public bool isMirroredStatus = false;
  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    animator.SetBool(isInteractingBool, isInteractingStatus);
    animator.SetBool(isFiringSpellBool, isFiringSpellStatus);
    animator.SetBool(isRotationWithRootMotion, isRotationWithRootMotionStatus);
    animator.SetBool(canRotateBool, canRotateStatus);
    animator.SetBool(isInvulnerable, isInvulnerableStatus);
    animator.SetBool(isUsingRightHand, isUsingRightHandStatus);
    animator.SetBool(isUsingLeftHand,isUsingLeftHandStatus);
    animator.SetBool(isMirroredBool, isMirroredStatus);
  }
}
