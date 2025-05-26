using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MR
{
  public class SpellDamageCollider : DamageCollider
  {
    public GameObject impactParticles, projectileParticles, muzzleParticles;
    bool hasCollided = false;
    CharacterManager spellTarget;
    Rigidbody rb;
    Vector3 impactNormal;

    protected override void Awake()
    {
      base.Awake();
      rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
      projectileParticles = Instantiate(projectileParticles, transform.position, transform.rotation);
      projectileParticles.transform.parent = transform;
      if (muzzleParticles)
      {
        muzzleParticles = Instantiate(muzzleParticles, transform.position, transform.rotation);
        Destroy(muzzleParticles, 2f);
      }
    }

    private void OnCollisionEnter(Collision collision)
    {
      if (!hasCollided)
      {

        spellTarget = collision.transform.GetComponent<CharacterManager>();

        if (spellTarget != null && spellTarget.characterStatsManager.teamIDNumber != teamIDNumber)
        {
          TakeDamageEffect takeDamageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
          takeDamageEffect.physicalDamage = physicalDamage;
          takeDamageEffect.fireDamage = fireDamage;
          takeDamageEffect.poiseDamage = poiseDamage;
          takeDamageEffect.contactPoint = contactPoint;
          takeDamageEffect.angleHitFrom = angleHitFrom;
          spellTarget.characterEffectsManager.ProcessEffectInstantly(takeDamageEffect);
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