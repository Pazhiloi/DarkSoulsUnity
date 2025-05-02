using UnityEngine;
namespace MR
{
  public class AttackState : State
  {

    public RotateTowardsTargetState rotateTowardsTargetState;
    public CombatStanceState combatStanceState;
    public PursueTargetState pursueTargetState;
    public EnemyAttackAction currentAttack;

    bool willDoComboOnNextAttack = false;
    public bool hasPerformedAttack = false;
    public override State Tick(EnemyManager enemy)
    {
      float distanceFromTarget = Vector3.Distance(enemy.currentTarget.transform.position, enemy.transform.position);
      RotateTowardsTargetWhilstAttacking(enemy);

      if (distanceFromTarget > enemy.maximumAggroRadius)
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
      return rotateTowardsTargetState;
    }



    private void AttackTarget(EnemyManager enemy)
    {
      enemy.isUsingRightHand = currentAttack.isRightHandedAction;
      enemy.isUsingLeftHand = !currentAttack.isRightHandedAction;
      enemy.enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
      enemy.enemyAnimatorManager.PlayWeaponTrailFX();
      enemy.currentRecoveryTime = currentAttack.recoveryTime;
      hasPerformedAttack = true;
    }

    private void AttackTargetWithCombo(EnemyManager enemy)
    {
      enemy.isUsingRightHand = currentAttack.isRightHandedAction;
      enemy.isUsingLeftHand = !currentAttack.isRightHandedAction;
      willDoComboOnNextAttack = false;
      enemy.enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
      enemy.enemyAnimatorManager.PlayWeaponTrailFX();
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
        if (currentAttack.comboAction != null)
        {
          willDoComboOnNextAttack = true;
          currentAttack = currentAttack.comboAction;
        }
        else
        {
          willDoComboOnNextAttack = false;
          currentAttack = null;
        }
      }
    }


  }
}