using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
namespace MR
{
  public class DamageCollider : MonoBehaviour
  {
    public CharacterManager characterManager;
    protected Collider damageCollider;
    public bool enabledDamageColliderOnStartUP = false;

    [Header("Team I.D")]
    public int teamIDNumber = 0;

    [Header("Poise")]
    public float poiseDamage, offensivePoiseBonus;
    [Header("Damage")]
    public int physicalDamage;
    public int fireDamage;
    public int magicDamage;
    public int lightningDamage;
    public int darkDamage;

    [Header("Guard Break Modifier")]
    public float guardBreakModifier = 1;

    protected bool shieldHasBeenHit;
    protected bool hasBeenParried;
    protected string currentDamageAnimation;

    protected Vector3 contactPoint;
    protected float angleHitFrom;

    private List<CharacterManager> charactersDamagedDuringThisCalculation = new List<CharacterManager>();
    protected virtual void Awake()
    {
      damageCollider = GetComponent<Collider>();
      damageCollider.gameObject.SetActive(true);
      damageCollider.isTrigger = true;
      damageCollider.enabled = enabledDamageColliderOnStartUP;
    }

    public void EnableDamageCollider()
    {
      damageCollider.enabled = true;
    }
    public void DisableDamageCollider()
    {
      if (charactersDamagedDuringThisCalculation.Count > 0)
      {
        charactersDamagedDuringThisCalculation.Clear();
      }
      damageCollider.enabled = false;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
      if (other.gameObject.layer == LayerMask.NameToLayer("Damageable Character"))
      {
        shieldHasBeenHit = false;
        hasBeenParried = false;

        CharacterManager enemyManager = other.GetComponentInParent<CharacterManager>();


        if (enemyManager != null)
        {

          AICharacterManager aICharacter = enemyManager as AICharacterManager;
          if (charactersDamagedDuringThisCalculation.Contains(enemyManager)) return;
          if (enemyManager.characterStatsManager.teamIDNumber == teamIDNumber) return;
          charactersDamagedDuringThisCalculation.Add(enemyManager);
          CheckForParry(enemyManager);
          CheckForBlock(enemyManager);

          if (enemyManager.characterStatsManager.teamIDNumber == teamIDNumber) return;
          if (hasBeenParried) return;
          if (shieldHasBeenHit) return;

          enemyManager.characterStatsManager.poiseResetTimer = enemyManager.characterStatsManager.totalPoiseResetTime;
          enemyManager.characterStatsManager.totalPoiseDefence = enemyManager.characterStatsManager.totalPoiseResetTime - poiseDamage;

          contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
          angleHitFrom = (Vector3.SignedAngle(characterManager.transform.forward, enemyManager.transform.forward, Vector3.up));
          // ChooseWhichDirectionDamageCameFrom(angleHitFrom);
          DealDamage(enemyManager);

          if (aICharacter != null)
          {
            aICharacter.currentTarget = characterManager;
          }
        }
      }

      if (other.tag == "Illusionary Wall")
      {
        IllusionaryWall illusionaryWall = other.GetComponent<IllusionaryWall>();

        illusionaryWall.wallHasBeenHit = true;




      }
    }

    protected virtual void CheckForParry(CharacterManager enemyManager)
    {
      if (enemyManager.isParrying)
      {
        characterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Parried", true);
        hasBeenParried = true;
      }
    }

    protected virtual void CheckForBlock(CharacterManager enemyManager)
    {
      Vector3 directionFromPlayerToEnemy = (characterManager.transform.position - enemyManager.transform.position);
      float dotValueFromPlayerToEnemy = Vector3.Dot(directionFromPlayerToEnemy, enemyManager.transform.forward);

      if (enemyManager.isBlocking && dotValueFromPlayerToEnemy > 0.3f)
      {
        shieldHasBeenHit = true;

        TakeBlockedDamageEffect takeBlockedDamage = Instantiate(WorldCharacterEffectsManager.instance.takeBlockedDamageEffect);
        takeBlockedDamage.physicalDamage = physicalDamage;
        takeBlockedDamage.fireDamage = fireDamage;
        takeBlockedDamage.poiseDamage = poiseDamage;
        takeBlockedDamage.staminaDamage = poiseDamage;
        enemyManager.characterEffectsManager.ProcessEffectInstantly(takeBlockedDamage);
      }
    }

    protected virtual void DealDamage(CharacterManager enemyManager)
    {
      float finalPhysicalDamage = physicalDamage;
      if (characterManager.isUsingRightHand)
      {
        if (characterManager.characterCombatManager.currentAttackType == AttackType.light)
        {
          finalPhysicalDamage = finalPhysicalDamage * characterManager.characterInventoryManager.rightWeapon.lightAttackDamageModifier;
        }
        else if (characterManager.characterCombatManager.currentAttackType == AttackType.heavy)
        {
          finalPhysicalDamage = finalPhysicalDamage * characterManager.characterInventoryManager.rightWeapon.heavyAttackDamageModifier;
        }
      }
      else if (characterManager.isUsingLeftHand)
      {
        if (characterManager.characterCombatManager.currentAttackType == AttackType.light)
        {
          finalPhysicalDamage = finalPhysicalDamage * characterManager.characterInventoryManager.leftWeapon.lightAttackDamageModifier;
        }
        else if (characterManager.characterCombatManager.currentAttackType == AttackType.heavy)
        {
          finalPhysicalDamage = finalPhysicalDamage * characterManager.characterInventoryManager.leftWeapon.heavyAttackDamageModifier;
        }
      }

      TakeDamageEffect takeDamageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
      takeDamageEffect.physicalDamage = finalPhysicalDamage;
      takeDamageEffect.fireDamage = fireDamage;
      takeDamageEffect.poiseDamage = poiseDamage;
      takeDamageEffect.contactPoint = contactPoint;
      takeDamageEffect.angleHitFrom = angleHitFrom;
      enemyManager.characterEffectsManager.ProcessEffectInstantly(takeDamageEffect);
    }


  }
}