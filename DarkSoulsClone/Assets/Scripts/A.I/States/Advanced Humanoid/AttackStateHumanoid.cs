using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  public class AttackStateHumanoid : State
  {
    public RotateTowardsTargetStateHumanoid rotateTowardsTargetState;
    public CombatStanceStateHumanoid combatStanceState;
    public PursueTargetStateHumanoid pursueTargetState;
    public ItemBasedAttackAction currentAttack;

    bool willDoComboOnNextAttack = false;
    public bool hasPerformedAttack = false;

    private void Awake() {
      rotateTowardsTargetState = GetComponent<RotateTowardsTargetStateHumanoid>();
      combatStanceState = GetComponent<CombatStanceStateHumanoid>();
      pursueTargetState = GetComponent<PursueTargetStateHumanoid>();
    }

    public override State Tick(EnemyManager enemy)
    {
      if (enemy.combatStyle == AICombatStyle.swordAndShield)
      {
       return ProcessSwordAndShieldCombatStyle(enemy);
      }
      else if (enemy.combatStyle == AICombatStyle.archer)
      {
       return ProcessArcherCombatStyle(enemy);
      }
      else
      {
        return this;
      }
    }

    private State ProcessSwordAndShieldCombatStyle(EnemyManager enemy)
    {
      RotateTowardsTargetWhilstAttacking(enemy);

      if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
      {
        return pursueTargetState;
      }

      if (willDoComboOnNextAttack && enemy.canDoCombo)
      {
        AttackTargetWithCombo(enemy);
        enemy.currentRecoveryTime = currentAttack.recoveryTime;
      }
      if (!hasPerformedAttack)
      {
        AttackTarget(enemy);
        RollForComboChance(enemy);
      }

      if (willDoComboOnNextAttack && hasPerformedAttack)
      {
        return this;
      }

      ResetStateFlags();
      return rotateTowardsTargetState;
    }

    private State ProcessArcherCombatStyle(EnemyManager enemy)
    {
      RotateTowardsTargetWhilstAttacking(enemy);

      if (enemy.isInteracting)
        return this;

      if (!enemy.isHoldingArrow)
      {
        ResetStateFlags();
        return combatStanceState;
      }
      if (enemy.currentTarget.isDead)
      {
        ResetStateFlags();
        enemy.currentTarget = null;
        return this;
      }

      if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
      {
        ResetStateFlags();
        return pursueTargetState;
      }

      if (!hasPerformedAttack && enemy.isHoldingArrow)
      {
        FireAmmo(enemy);
      }
      ResetStateFlags();

      return rotateTowardsTargetState;
    }


    private void AttackTarget(EnemyManager enemy)
    {
      currentAttack.PerformAttackAction(enemy);
      enemy.currentRecoveryTime = currentAttack.recoveryTime;
      hasPerformedAttack = true;
    }

    private void AttackTargetWithCombo(EnemyManager enemy)
    {
      currentAttack.PerformAttackAction(enemy);
      willDoComboOnNextAttack = false;
      enemy.currentRecoveryTime = currentAttack.recoveryTime;
      currentAttack = null;
    }
    private void RotateTowardsTargetWhilstAttacking(EnemyManager enemy)
    {
      // Rotate Manually
      if (enemy.canRotate && enemy.isInteracting)
      {
        Vector3 direction = enemy.currentTarget.transform.position - enemy.transform.position;
        direction.y = 0;
        direction.Normalize();

        if (direction == Vector3.zero)
        {
          direction = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        enemy.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemy.rotationSpeed / Time.deltaTime);
      }

    }

    private void RollForComboChance(EnemyManager enemy)
    {
      float comboChance = Random.Range(0f, 100f);

      if (enemy.allowAIToPerformCombos && comboChance <= enemy.comboLikelyHood)
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


    private void FireAmmo(EnemyManager enemy)
    {
      if (enemy.isHoldingArrow)
      {
        hasPerformedAttack = true;
        enemy.characterInventoryManager.currentItemBeingUsed = enemy.characterInventoryManager.rightWeapon;
        enemy.characterInventoryManager.rightWeapon.th_tap_RB_Action.PerformAction(enemy);
      }
    }

  }
}
