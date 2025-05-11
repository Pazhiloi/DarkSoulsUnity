using UnityEngine;
using UnityEngine.AI;
namespace MR
{
  public class AICharacterLocomotionManager : MonoBehaviour
  {
    AICharacterManager aiCharacter;

    public CapsuleCollider characterCollider, characterCollisionBlockerCollider;

    
    public LayerMask detectionLayer;

    

    private void Awake()
    {
      aiCharacter = GetComponent<AICharacterManager>();
    }

    private void Start() {
      Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
    }

  }
}
