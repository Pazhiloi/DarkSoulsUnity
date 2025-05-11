using UnityEngine;

namespace MR
{
  public class CompanionStateFollowHost : State
  {
    CompanionStateIdle idleState;

    private void Awake() {
      idleState = GetComponent<CompanionStateIdle>();
    }
    
    public override State Tick(AICharacterManager aiCharacter)
    {
      if (aiCharacter.isInteracting) return this;

      if (aiCharacter.isPreformingAction)
      {
        aiCharacter.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
        return this;
      }

      HandleRotateTowardsTarget(aiCharacter);

      if (aiCharacter.distanceFromCompanion > aiCharacter.maxDistanceFromCompanion)
      {
        aiCharacter.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
      }


      if (aiCharacter.distanceFromCompanion <= aiCharacter.returnDistanceFromCompanion)
      {
        return idleState;
      }
      else
      {
        return this;
      }


    }


    private void HandleRotateTowardsTarget(AICharacterManager aiCharacter)
    {
      Vector3 relativeDirection = transform.InverseTransformDirection(aiCharacter.navMeshAgent.desiredVelocity);
      Vector3 targetVelocity = aiCharacter.enemyRigidbody.velocity;

      aiCharacter.navMeshAgent.enabled = true;
      aiCharacter.navMeshAgent.SetDestination(aiCharacter.companion.transform.position);
      aiCharacter.enemyRigidbody.velocity = targetVelocity;
      aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, aiCharacter.navMeshAgent.transform.rotation, aiCharacter.rotationSpeed / Time.deltaTime);
    }
  }
}
