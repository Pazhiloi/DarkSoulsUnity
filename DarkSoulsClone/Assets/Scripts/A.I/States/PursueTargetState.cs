using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MR
{
  public class PursueTargetState : State
  {
   public CombatStanceState combatStanceState;
   public RotateTowardsTargetState rotateTowardsTargetState;
    public override State Tick(EnemyManager enemy)
    {
      if (enemy.isInteracting) return this;

      if (enemy.isPreformingAction){
        enemy.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
        return this;
      }
      

      if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
      {
        enemy.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
      }
      

      if (enemy.distanceFromTarget <= enemy.maximumAggroRadius)
      {
        return combatStanceState;
      }
      else
      {
        return this;
      }
    }


    private void HandleRotateTowardsTarget(EnemyManager enemy)
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
  }
}