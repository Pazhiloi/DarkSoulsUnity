using UnityEngine;

namespace MR
{
  public class RangedProjectileDamageCollider : DamageCollider
  {

    public RangedAmmoItem ammoItem;
    protected bool hasAlreadyPenetratedASurface;

    Rigidbody arrowRigidbody;
    CapsuleCollider arrowCapsuleCollider;

    protected override void Awake()
    {
      damageCollider = GetComponent<Collider>();
      damageCollider.gameObject.SetActive(true);
      damageCollider.enabled = true;
      arrowCapsuleCollider = GetComponent<CapsuleCollider>();
      arrowRigidbody = GetComponent<Rigidbody>();
    }
   private void OnCollisionEnter(Collision other)
    {
      shieldHasBeenHit = false;
      hasBeenParried = false;

      CharacterManager enemyManager = other.gameObject.GetComponentInParent<CharacterManager>();

      if (enemyManager != null)
      {
        if (enemyManager.characterStatsManager.teamIDNumber == teamIDNumber)
          return;

        CheckForParry(enemyManager);
        CheckForBlock(enemyManager);

        if (hasBeenParried)
          return;

        if (shieldHasBeenHit)
          return;

        enemyManager.characterStatsManager.poiseResetTimer = enemyManager.characterStatsManager.totalPoiseResetTime;
        enemyManager.characterStatsManager.totalPoiseDefence -= enemyManager.characterStatsManager.totalPoiseDefence - poiseBreak;

        //DETECTS WHERE ON THE COLLIDER OUR WEAPON FIRST MAKES CONTACT
        Vector3 contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
        float directionHitFrom = (Vector3.SignedAngle(characterManager.transform.forward, enemyManager.transform.forward, Vector3.up));
       // ChooseWhichDirectionDamageCameFrom(directionHitFrom);
        enemyManager.characterEffectsManager.PlayBloodSplatterFX(contactPoint);

        if (enemyManager.characterStatsManager.totalPoiseDefence > poiseBreak)
        {
          enemyManager.characterStatsManager.TakeDamageNoAnimation(physicalDamage, 0);
        }
        else
        {
        //  enemyManager.characterStatsManager.TakeDamage(physicalDamage, 0, currentDamageAnimation, characterManager);
        }
      }
      if (other.gameObject.tag == "Illusionary Wall")
      {
        IllusionaryWall illusionaryWall = other.gameObject.GetComponent<IllusionaryWall>();

        illusionaryWall.wallHasBeenHit = true;
      }

      if (!hasAlreadyPenetratedASurface)
      {
        hasAlreadyPenetratedASurface = true;
        arrowRigidbody.isKinematic = true;
        arrowCapsuleCollider.enabled = false;

        gameObject.transform.position = other.GetContact(0).point;
        gameObject.transform.rotation = Quaternion.LookRotation(transform.forward);
        gameObject.transform.parent = other.collider.transform; 

      }
    }


    private void FixedUpdate() {
      if (arrowRigidbody.velocity != Vector3.zero)
      {
        arrowRigidbody.rotation = Quaternion.LookRotation(arrowRigidbody.velocity);
      }
    }
  }
}