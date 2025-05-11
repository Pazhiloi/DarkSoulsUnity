using UnityEngine;
namespace MR
{
  public class AttackState : State
  {

    public RotateTowardsTargetState rotateTowardsTargetState;
    public CombatStanceState combatStanceState;
    public PursueTargetState pursueTargetState;
    public AICharacterAttackAction currentAttack;

    bool willDoComboOnNextAttack = false;
    public bool hasPerformedAttack = false;
    public override State Tick(AICharacterManager aiCharacter)
    {
      float distanceFromTarget = Vector3.Distance(aiCharacter.currentTarget.transform.position, aiCharacter.transform.position);
      RotateTowardsTargetWhilstAttacking(aiCharacter);

      if (distanceFromTarget > aiCharacter.maximumAggroRadius)
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
      return rotateTowardsTargetState;
    }



    private void AttackTarget(AICharacterManager aiCharacter)
    {
      aiCharacter.isUsingRightHand = currentAttack.isRightHandedAction;
      aiCharacter.isUsingLeftHand = !currentAttack.isRightHandedAction;
      aiCharacter.enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
      aiCharacter.enemyAnimatorManager.PlayWeaponTrailFX();
      aiCharacter.currentRecoveryTime = currentAttack.recoveryTime;
      hasPerformedAttack = true;
    }

    private void AttackTargetWithCombo(AICharacterManager aiCharacter)
    {
      aiCharacter.isUsingRightHand = currentAttack.isRightHandedAction;
      aiCharacter.isUsingLeftHand = !currentAttack.isRightHandedAction;
      willDoComboOnNextAttack = false;
      aiCharacter.enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
      aiCharacter.enemyAnimatorManager.PlayWeaponTrailFX();
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