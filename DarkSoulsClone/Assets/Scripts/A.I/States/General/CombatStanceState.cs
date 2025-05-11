using UnityEngine;
namespace MR
{
  public class CombatStanceState : State
  {

    public AttackState attackState;
    public AICharacterAttackAction[] enemyAttacks;

    public PursueTargetState pursueTargetState;


   protected bool randomDestinationSet = false;
    protected float verticalMovementValue = 0;
    protected float horizontalMovementValue = 0;
    public override State Tick(AICharacterManager aiCharacter)
    {
      aiCharacter.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
      aiCharacter.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
      attackState.hasPerformedAttack = false;

      if (aiCharacter.isInteracting) {
        aiCharacter.animator.SetFloat("Vertical", 0);
        aiCharacter.animator.SetFloat("Horizontal", 0);
        return this;
      }

      if (aiCharacter.distanceFromTarget > aiCharacter.maximumAggroRadius)
      {
        return pursueTargetState;
      }

      if (!randomDestinationSet)
      {
        randomDestinationSet = true;
        DecideCirclingAction(aiCharacter);
      }

      HandleRotateTowardsTarget(aiCharacter);

      if (aiCharacter.currentRecoveryTime <= 0 && attackState.currentAttack != null)
      {
        randomDestinationSet = false;
        return attackState;
      }
      else
      {
        GetNewAttack(aiCharacter);
      }
      return this;

    }

    protected void HandleRotateTowardsTarget(AICharacterManager aiCharacter)
    {
      // Rotate Manually
      if (aiCharacter.isPreformingAction)
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
      // Rotate with pathfinding(navmesh)
      else
      {
        Vector3 relativeDirection = transform.InverseTransformDirection(aiCharacter.navMeshAgent.desiredVelocity);
        Vector3 targetVelocity = aiCharacter.enemyRigidbody.velocity;

        aiCharacter.navMeshAgent.enabled = true;
        aiCharacter.navMeshAgent.SetDestination(aiCharacter.currentTarget.transform.position);
        aiCharacter.enemyRigidbody.velocity = targetVelocity;
        aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, aiCharacter.navMeshAgent.transform.rotation, aiCharacter.rotationSpeed / Time.deltaTime);
      }
    }


    protected void DecideCirclingAction(AICharacterManager aiCharacter)
    {
      WalkAroundTarget(aiCharacter);
    }

    protected void WalkAroundTarget(AICharacterManager aiCharacter)
    {
      verticalMovementValue = 0.5f;

      horizontalMovementValue = Random.Range(-1, 1);

      if (horizontalMovementValue <= 1 && horizontalMovementValue > 0)
      {
        horizontalMovementValue = 0.5f;
      }
      else if (horizontalMovementValue >= -1 && horizontalMovementValue < 0)
      {
        horizontalMovementValue = -0.5f;
      }
    }

    protected virtual void GetNewAttack(AICharacterManager aiCharacter)
    {
      int maxScore = 0;

      for (int i = 0; i < enemyAttacks.Length; i++)
      {
        AICharacterAttackAction enemyAttackAction = enemyAttacks[i];

        if (aiCharacter.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack &&
        aiCharacter.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
        {
          if (aiCharacter.viewableAngle <= enemyAttackAction.maximumAttackAngle && aiCharacter.viewableAngle >= enemyAttackAction.minimumAttackAngle)
          {
            maxScore += enemyAttackAction.attackScore;
          }
        }
      }


      int randomValue = Random.Range(0, maxScore);
      int temporaryScore = 0;
      for (int i = 0; i < enemyAttacks.Length; i++)
      {
        AICharacterAttackAction enemyAttackAction = enemyAttacks[i];

        if (aiCharacter.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack &&
        aiCharacter.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
        {
          if (aiCharacter.viewableAngle <= enemyAttackAction.maximumAttackAngle && aiCharacter.viewableAngle >= enemyAttackAction.minimumAttackAngle)
          {
            if (attackState.currentAttack != null)
            {
              return;
            }

            temporaryScore += enemyAttackAction.attackScore;

            if (temporaryScore > randomValue)
            {
              attackState.currentAttack = enemyAttackAction;
            }
          }
        }
      }

    }

  }
}