using UnityEngine;
using UnityEngine.AI;
namespace MR
{
  public class EnemyLocomotionManager : MonoBehaviour
  {
    EnemyManager enemy;

    public CapsuleCollider characterCollider, characterCollisionBlockerCollider;

    
    public LayerMask detectionLayer;

    

    private void Awake()
    {
      enemy = GetComponent<EnemyManager>();
    }

    private void Start() {
      Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
    }

  }
}
