using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
  public class BombDamageCollider : DamageCollider
  {
    [Header("Explosive Damage & Radius")]
    public int explosiveDamage = 1;
    public int fireExplosionDamage = 1;
    Rigidbody bombRigidBody;
    private bool hasCollided = false;
    public GameObject impactParticles;

    protected override void Awake()
    {
      damageCollider = GetComponent<Collider>();
      bombRigidBody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
      if (!hasCollided)
      {
        hasCollided = true;
      }
    }

    private void Explode()
    {
    }
  }
}
