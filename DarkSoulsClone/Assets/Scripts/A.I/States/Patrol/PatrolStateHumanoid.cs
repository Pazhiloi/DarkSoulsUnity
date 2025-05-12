using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  public class PatrolStateHumanoid : State
  {

    public LayerMask detectionLayer;
    public LayerMask layersThatBlockLineOfSight;
    public PursueTargetStateHumanoid pursueTargetState;
    public bool patrolComplete;
    public bool repeatPatrol;

    // How long before next patrol
    [Header("Patrol Rest Time")]
    public float endOfPatrolRestTime;
    public float endOfPatrolTimer;

    [Header("Patrol Position")]
    public int patrolDestinationIndex;
    public bool hasPatrolDestination;
    public Transform currentPatrolDestination;
    public float distanceFromCurrentPatrolPoint;
    public List<Transform> listOfPatrolDestinations = new List<Transform>();

    public override State Tick(AICharacterManager aiCharacter)
    {
      SearchForTargetWhilstPatroling(aiCharacter);
      // If the A.I is performing some action, halt all movement and return
      if (aiCharacter.isInteracting)
      {
        aiCharacter.animator.SetFloat("Vertical", 0);
        aiCharacter.animator.SetFloat("Horizontal", 0);
        return this;
      }

      if (aiCharacter.currentTarget != null)
      {
        return pursueTargetState;
      }

      if (patrolComplete && repeatPatrol)
      {
        // If we've completed our patrol and We do want to repeat it, We do so

        // We count down our rest time, and reset all of our patrol flags
        if (endOfPatrolRestTime > endOfPatrolTimer)
        {
          aiCharacter.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
          endOfPatrolTimer += Time.deltaTime;
          return this;
        }
        else if (endOfPatrolTimer >= endOfPatrolRestTime)
        {
          patrolDestinationIndex = -1;
          hasPatrolDestination = false;
          currentPatrolDestination = null;
          patrolComplete = false;
          endOfPatrolTimer = 0;
        }
      }else if(patrolComplete && !repeatPatrol){
        aiCharacter.navMeshAgent.enabled = false;
        aiCharacter.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
        return this;
      }

      if (hasPatrolDestination)
      {
        if (currentPatrolDestination != null)
        {
          distanceFromCurrentPatrolPoint = Vector3.Distance(aiCharacter.transform.position, currentPatrolDestination.transform.position);

          if (distanceFromCurrentPatrolPoint > 1)
          {
            aiCharacter.navMeshAgent.enabled = true;
            aiCharacter.navMeshAgent.destination = currentPatrolDestination.transform.position;
            Quaternion targetRotation = Quaternion.Lerp(aiCharacter.transform.rotation, aiCharacter.navMeshAgent.transform.rotation, 0.5f);
            aiCharacter.transform.rotation = targetRotation;
            aiCharacter.animator.SetFloat("Vertical", 0.5f, 0.2f, Time.deltaTime);
          }
          else
          {
            currentPatrolDestination = null;
            hasPatrolDestination = false;
          }
        }
        
      }

      if (!hasPatrolDestination)
      {
        patrolDestinationIndex = patrolDestinationIndex + 1;

        if (patrolDestinationIndex > listOfPatrolDestinations.Count - 1)
        {
          patrolComplete = true;
          return this;
        }

        currentPatrolDestination = listOfPatrolDestinations[patrolDestinationIndex];
        hasPatrolDestination = true;
      }

      
      return this;
    }

    private void SearchForTargetWhilstPatroling(AICharacterManager aiCharacter)
    {
      Collider[] colliders = Physics.OverlapSphere(transform.position, aiCharacter.detectionRadius, detectionLayer);
      for (int i = 0; i < colliders.Length; i++)
      {
        CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

        if (targetCharacter != null)
        {
          if (targetCharacter.characterStatsManager.teamIDNumber != aiCharacter.enemyStatsManager.teamIDNumber)
          {
            Vector3 targetDirection = targetCharacter.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            if (viewableAngle > aiCharacter.minimumDetectionAngle && viewableAngle < aiCharacter.maximumDetectionAngle)
            {
              if (Physics.Linecast(aiCharacter.lockOnTransform.position, targetCharacter.lockOnTransform.position, layersThatBlockLineOfSight))
              {
                return;
              }
              else
              {
                aiCharacter.currentTarget = targetCharacter;
              }
            }
          }
        }
      }

      if (aiCharacter.currentTarget != null)
      {
        return;
      }
      else
      {
        return;
      }
    }
  }
}
