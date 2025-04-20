using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MR
{
  public class CombatStanceState : State
  {

    public AttackState attackState;
    public EnemyAttackAction[] enemyAttacks;

    public PursueTargetState pursueTargetState;


   protected bool randomDestinationSet = false;
    protected float verticalMovementValue = 0;
    protected float horizontalMovementValue = 0;
    public override State Tick(EnemyManager enemy)
    {
      float distanceFromTarget = Vector3.Distance(enemy.currentTarget.transform.position, enemy.transform.position);
      enemy.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
      enemy.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
      attackState.hasPerformedAttack = false;

      if (enemy.isInteracting) {
        enemy.animator.SetFloat("Vertical", 0);
        enemy.animator.SetFloat("Horizontal", 0);
        return this;
      }

      if (distanceFromTarget > enemy.maximumAggroRadius)
      {
        return pursueTargetState;
      }

      if (!randomDestinationSet)
      {
        randomDestinationSet = true;
        DecideCirclingAction(enemy);
      }

      HandleRotateTowardsTarget(enemy);

      if (enemy.currentRecoveryTime <= 0 && attackState.currentAttack != null)
      {
        randomDestinationSet = false;
        return attackState;
      }
      else
      {
        GetNewAttack(enemy);
      }
      return this;

    }

    protected void HandleRotateTowardsTarget(EnemyManager enemy)
    {
      // Rotate Manually
      if (enemy.isPreformingAction)
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
      // Rotate with pathfinding(navmesh)
      else
      {
        Vector3 relativeDirection = transform.InverseTransformDirection(enemy.navMeshAgent.desiredVelocity);
        Vector3 targetVelocity = enemy.enemyRigidbody.velocity;

        enemy.navMeshAgent.enabled = true;
        enemy.navMeshAgent.SetDestination(enemy.currentTarget.transform.position);
        enemy.enemyRigidbody.velocity = targetVelocity;
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, enemy.navMeshAgent.transform.rotation, enemy.rotationSpeed / Time.deltaTime);
      }
    }


    protected void DecideCirclingAction(EnemyManager enemy)
    {
      WalkAroundTarget(enemy);
    }

    protected void WalkAroundTarget(EnemyManager enemy)
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

    protected virtual void GetNewAttack(EnemyManager enemy)
    {
      Vector3 targetsDirection = enemy.currentTarget.transform.position - transform.position;
      float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
      float distanceFromTarget = Vector3.Distance(enemy.currentTarget.transform.position, enemy.transform.position);

      int maxScore = 0;

      for (int i = 0; i < enemyAttacks.Length; i++)
      {
        EnemyAttackAction enemyAttackAction = enemyAttacks[i];

        if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack &&
        distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
        {
          if (viewableAngle <= enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
          {
            maxScore += enemyAttackAction.attackScore;
          }
        }
      }


      int randomValue = Random.Range(0, maxScore);
      int temporaryScore = 0;
      for (int i = 0; i < enemyAttacks.Length; i++)
      {
        EnemyAttackAction enemyAttackAction = enemyAttacks[i];

        if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack &&
        distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
        {
          if (viewableAngle <= enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
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