using UnityEngine;
using UnityEngine.Animations.Rigging;
namespace MR
{

  public class CharacterAnimatorManager : MonoBehaviour
  {
    protected CharacterManager character;
    protected RigBuilder rigBuilder;
    public TwoBoneIKConstraint leftHandConstraint;
    public TwoBoneIKConstraint rightHandConstraint;

    bool handIKWeightsReset = false;

    protected virtual void Awake()
    {
      character = GetComponent<CharacterManager>();
      rigBuilder = GetComponent<RigBuilder>();
    }


    public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false, bool mirrorAnim = false)
    {
      character.animator.applyRootMotion = isInteracting;
      character.animator.SetBool("canRotate", canRotate);
      character.animator.SetBool("isInteracting", isInteracting);
      character.animator.SetBool("isMirrored",mirrorAnim);
      character.animator.CrossFade(targetAnim, 0.2f);
    }
    public void PlayTargetAnimationWithRootRotation(string targetAnim, bool isInteracting)
    {
      character.animator.applyRootMotion = isInteracting;
      character.animator.SetBool("isRotatingWithRootMotion", true);
      character.animator.SetBool("isInteracting", isInteracting);
      character.animator.CrossFade(targetAnim, 0.2f);
    }

    public virtual void CanRotate()
    {
      character.animator.SetBool("canRotate", true);
    }

    public virtual void StopRotation()
    {
      character.animator.SetBool("canRotate", false);
    }

    public virtual void EnableCombo()
    {
      character.animator.SetBool("canDoCombo", true);
    }
    public virtual void DisableCombo()
    {
      character.animator.SetBool("canDoCombo", false);
    }

    public virtual void EnableIsInvulnerable()
    {
      character.animator.SetBool("isInvulnerable", true);
    }

    public virtual void DisableIsInvulnerable()
    {
      character.animator.SetBool("isInvulnerable", false);
    }

    public virtual void EnableIsParrying()
    {
      character.isParrying = true;
    }

    public virtual void DisableIsParrying()
    {
      character.isParrying = false;
    }

    public virtual void EnableCanBeRiposted()
    {
      character.canBeRiposted = true;
    }

    public virtual void DisableCanBeRiposted()
    {
      character.canBeRiposted = false;
    }

    public virtual void TakeCriticalDamageAnimationEvent()
    {
      character.characterStatsManager.TakeDamageNoAnimation(character.pendingCriticalDamage, 0);
      character.pendingCriticalDamage = 0;
    }

    public virtual void SetHandIKForWeapon(RightHandIKTarget rightHandTarget, LeftHandIKTarget leftHandTarget, bool isTwoHandingWeapon)
    {
      if (isTwoHandingWeapon)
      {

        if (rightHandTarget != null)
        {
          rightHandConstraint.data.target = rightHandTarget.transform;
          rightHandConstraint.data.targetPositionWeight = 1; // Assign this from a weapon variable if you'd like
          rightHandConstraint.data.targetRotationWeight = 1;
        }

        if (leftHandTarget != null)
        {
          leftHandConstraint.data.target = leftHandTarget.transform;
          leftHandConstraint.data.targetPositionWeight = 1;
          leftHandConstraint.data.targetRotationWeight = 1;
        }

      }
      else
      {
        rightHandConstraint.data.target = null;
        leftHandConstraint.data.target = null;
      }

      rigBuilder.Build();
    }

    public virtual void CheckHandIKWeight(RightHandIKTarget rightHandIK, LeftHandIKTarget leftHandIK, bool isTwoHandingWeapon)
    {
      if (character.isInteracting)
      {
        return;
      }

      if (handIKWeightsReset)
      {
        handIKWeightsReset = false;

        if (rightHandConstraint.data.target != null)
        {
          rightHandConstraint.data.target = rightHandIK.transform;
          rightHandConstraint.data.targetPositionWeight = 1;
          rightHandConstraint.data.targetRotationWeight = 1;
        }

        if (leftHandConstraint.data.target != null)
        {
          leftHandConstraint.data.target = leftHandIK.transform;
          leftHandConstraint.data.targetPositionWeight = 1;
          leftHandConstraint.data.targetRotationWeight = 1;
        }
      }
    }

    public virtual void EraseHandIKForWeapon()
    {
      handIKWeightsReset = true;

      if (rightHandConstraint.data.target != null)
      {
        rightHandConstraint.data.targetPositionWeight = 0;
        rightHandConstraint.data.targetRotationWeight = 0;
      }

      if (leftHandConstraint.data.target != null)
      {
        leftHandConstraint.data.targetPositionWeight = 0;
        leftHandConstraint.data.targetRotationWeight = 0;
      }
    }
  }
}