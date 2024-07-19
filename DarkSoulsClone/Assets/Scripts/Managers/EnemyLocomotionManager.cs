using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace SG
{
  public class EnemyLocomotionManager : MonoBehaviour
  {
    EnemyManager enemyManager;
    EnemyAnimatorManager enemyAnimatorManager;

    NavMeshAgent navMeshAgent;
    public Rigidbody enemyRigidbody;
    public LayerMask detectionLayer;

    public float distanceFromTarget;
    public float stoppingDistance = 1f;
    public float rotationSpeed = 15;

    private void Awake()
    {
      enemyManager = GetComponent<EnemyManager>();
      enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
      navMeshAgent = GetComponentInChildren<NavMeshAgent>();
      enemyRigidbody = GetComponent<Rigidbody>();
    }

    private void Start() {
      navMeshAgent.enabled = false;
      enemyRigidbody.isKinematic = false;
    }
   
    public void HandleMoveToTarget()
    {
      if (enemyManager.isPerformingAction) return;
      
      Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
      distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);
      float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

      if (enemyManager.isPerformingAction)
      {
        enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
        navMeshAgent.enabled = false;
      }
      else
      {
        if (distanceFromTarget > stoppingDistance)
        {
          enemyAnimatorManager.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
        }
        else if(distanceFromTarget <= stoppingDistance)
        {
          enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
        }
      }

      HandleRotateTowardsTarget();
      navMeshAgent.transform.localPosition = Vector3.zero;
      navMeshAgent.transform.localRotation = Quaternion.identity;
    }

    private void HandleRotateTowardsTarget(){
      // Rotate Manually
      if (enemyManager.isPerformingAction)
      {
        Vector3 direction = enemyManager.currentTarget.transform.position  - transform.position;
        direction.y = 0;
        direction.Normalize();

        if (direction == Vector3.zero)
        {
          direction = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed / Time.deltaTime);
      }
      // Rotate with pathfinding(navmesh)
      else
      {
        Vector3 relativeDirection = transform.InverseTransformDirection(navMeshAgent.desiredVelocity);
        Vector3 targetVelocity = enemyRigidbody.velocity;

        navMeshAgent.enabled = true;
        navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
        enemyRigidbody.velocity = targetVelocity;
        transform.rotation = Quaternion.Slerp(transform.rotation, navMeshAgent.transform.rotation, rotationSpeed / Time.deltaTime);
      }

     
    }


  }
}
