using UnityEngine;
using UnityEngine.AI;
namespace SG
{
  public class EnemyLocomotionManager : MonoBehaviour
  {
    EnemyManager enemyManager;
    EnemyAnimatorManager enemyAnimatorManager;

    public CapsuleCollider characterCollider, characterCollisionBlockerCollider;

    
    public LayerMask detectionLayer;

    

    private void Awake()
    {
      enemyManager = GetComponent<EnemyManager>();
      enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
    }

    private void Start() {
      Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
    }

  }
}
