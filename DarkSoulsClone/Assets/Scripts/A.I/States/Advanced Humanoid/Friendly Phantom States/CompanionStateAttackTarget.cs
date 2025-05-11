using UnityEngine;

namespace MR
{
    public class CompanionStateAttackTarget : State
  {
     CompanionStateRotateTowardsTarget rotateTowardsTargetState;
     CompanionStateCombatStance combatStanceState;
     CompanionStatePursueTarget pursueTargetState;
    public ItemBasedAttackAction currentAttack;

    bool willDoComboOnNextAttack = false;
    public bool hasPerformedAttack = false;

    private void Awake()
    {
      rotateTowardsTargetState = GetComponent<CompanionStateRotateTowardsTarget>();
      combatStanceState = GetComponent<CompanionStateCombatStance>();
      pursueTargetState = GetComponent<CompanionStatePursueTarget>();
    }

    public override State Tick(AICharacterManager aiCharacter)
    {
      if (aiCharacter.combatStyle == AICombatStyle.swordAndShield)
      {
        return ProcessSwordAndShieldCombatStyle(aiCharacter);
      }
      else if (aiCharacter.combatStyle == AICombatStyle.archer)
      {
        return ProcessArcherCombatStyle(aiCharacter);
      }
      else
      {
        return this;
      }
    }

    private State ProcessSwordAndShieldCombatStyle(AICharacterManager aiCharacter)
    {
      RotateTowardsTargetWhilstAttacking(aiCharacter);

      if (aiCharacter.distanceFromTarget > aiCharacter.maximumAggroRadius)
      {
        return pursueTargetState;
      }

      if (willDoComboOnNextAttack && aiCharacter.canDoCombo)
      {
        AttackTargetWithCombo(aiCharacter);
        aiCharacter.currentRecoveryTime = currentAttack.recoveryTime;
      }
      if (!hasPerformedAttack)
      {
        AttackTarget(aiCharacter);
        RollForComboChance(aiCharacter);
      }

      if (willDoComboOnNextAttack && hasPerformedAttack)
      {
        return this;
      }

      ResetStateFlags();
      return rotateTowardsTargetState;
    }

    private State ProcessArcherCombatStyle(AICharacterManager aiCharacter)
    {
      RotateTowardsTargetWhilstAttacking(aiCharacter);

      if (aiCharacter.isInteracting)
        return this;

      if (!aiCharacter.isHoldingArrow)
      {
        ResetStateFlags();
        return combatStanceState;
      }
      if (aiCharacter.currentTarget.isDead)
      {
        ResetStateFlags();
        aiCharacter.currentTarget = null;
        return this;
      }

      if (aiCharacter.distanceFromTarget > aiCharacter.maximumAggroRadius)
      {
        ResetStateFlags();
        return pursueTargetState;
      }

      if (!hasPerformedAttack && aiCharacter.isHoldingArrow)
      {
        FireAmmo(aiCharacter);
      }
      ResetStateFlags();

      return rotateTowardsTargetState;
    }


    private void AttackTarget(AICharacterManager aiCharacter)
    {
      currentAttack.PerformAttackAction(aiCharacter);
      aiCharacter.currentRecoveryTime = currentAttack.recoveryTime;
      hasPerformedAttack = true;
    }

    private void AttackTargetWithCombo(AICharacterManager aiCharacter)
    {
      currentAttack.PerformAttackAction(aiCharacter);
      willDoComboOnNextAttack = false;
      aiCharacter.currentRecoveryTime = currentAttack.recoveryTime;
      currentAttack = null;
    }
    private void RotateTowardsTargetWhilstAttacking(AICharacterManager aiCharacter)
    {
      // Rotate Manually
      if (aiCharacter.canRotate && aiCharacter.isInteracting)
      {
        Vector3 direction = aiCharacter.currentTarget.transform.position - aiCharacter.transform.position;
        direction.y = 0;
        direction.Normalize();

        if (direction == Vector3.zero)
        {
          direction = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        aiCharacter.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, aiCharacter.rotationSpeed / Time.deltaTime);
      }

    }

    private void RollForComboChance(AICharacterManager aiCharacter)
    {
      float comboChance = Random.Range(0f, 100f);

      if (aiCharacter.allowAIToPerformCombos && comboChance <= aiCharacter.comboLikelyHood)
      {
        if (currentAttack.actionCanCombo)
        {
          willDoComboOnNextAttack = true;
        }
        else
        {
          willDoComboOnNextAttack = false;
          currentAttack = null;
        }
      }
    }

    private void ResetStateFlags()
    {
      willDoComboOnNextAttack = false;
      hasPerformedAttack = false;
    }


    private void FireAmmo(AICharacterManager aiCharacter)
    {
      if (aiCharacter.isHoldingArrow)
      {
        hasPerformedAttack = true;
        aiCharacter.characterInventoryManager.currentItemBeingUsed = aiCharacter.characterInventoryManager.rightWeapon;
        aiCharacter.characterInventoryManager.rightWeapon.th_tap_RB_Action.PerformAction(aiCharacter);
      }
    }
  }
}
