using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{
public class SpellDamageCollider : DamageCollider
{
   public GameObject impactParticles, projectileParticles, muzzleParticles;
   bool hasCollided = false;
   CharacterStatsManager spellTarget;
   Rigidbody rb;
   Vector3 impactNormal;

   private void Awake() {
      rb = GetComponent<Rigidbody>();
   }

   private void Start() {
    projectileParticles = Instantiate(projectileParticles, transform.position, transform.rotation);
    projectileParticles.transform.parent = transform;
    if (muzzleParticles)
    {
      muzzleParticles = Instantiate(muzzleParticles, transform.position, transform.rotation);
      Destroy(muzzleParticles, 2f);
    }
   }

   private void OnCollisionEnter(Collision collision) {
    if (!hasCollided)
    {

      spellTarget = collision.transform.GetComponent<CharacterStatsManager>();

      if (spellTarget != null)
      {
          spellTarget.TakeDamage(currentWeaponDamage);
      }
      hasCollided = true;
      impactParticles = Instantiate(impactParticles, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal));

      Destroy(projectileParticles);
      Destroy(impactParticles, 5f);
      Destroy(gameObject, 5f);
    }
   }
}
}
