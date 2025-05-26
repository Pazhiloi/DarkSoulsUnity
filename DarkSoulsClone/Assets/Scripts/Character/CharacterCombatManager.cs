using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  public class CharacterCombatManager : MonoBehaviour
  {
    CharacterManager character;
    [Header("Combat Transform")]
    public Transform backStabReceiverTransform;
    public Transform riposteReceiverTransform;
    public LayerMask characterLayer;
    public float criticalAttackRange = 0.7f;
    [Header("Last Amount Of Poise Damage Taken")]
    public int previousPoiseDamageTaken;
    [Header("Attack Type")]
    public AttackType currentAttackType;
    [Header("Attack Animations")]
    public string oh_light_attack_01 = "OH_Light_Attack_01";
    public string oh_light_attack_02 = "OH_Light_Attack_02";
    public string oh_heavy_attack_01 = "OH_Heavy_Attack_01";
    public string oh_heavy_attack_02 = "OH_Heavy_Attack_02";
    public string oh_running_attack_01 = "OH_Running_Attack_01";
    public string oh_jumping_attack_01 = "OH_Jumping_Attack_01";

    public string oh_charge_attack_01 = "OH_Ch_At_charge_01";
    public string oh_charge_attack_02 = "OH_Ch_At_charge_02";

    public string th_light_attack_01 = "TH_Light_Attack_01";
    public string th_light_attack_02 = "TH_Light_Attack_02";
    public string th_heavy_attack_01 = "TH_Heavy_Attack_01";
    public string th_heavy_attack_02 = "TH_Heavy_Attack_02";
    public string th_running_attack_01 = "TH_Running_Attack_01";
    public string th_jumping_attack_01 = "TH_Jumping_Attack_01";

    public string th_charge_attack_01 = "TH_Ch_At_charge_01";
    public string th_charge_attack_02 = "TH_Ch_At_charge_02";

    public string weapon_art = "Weapon_Art";
    public int pendingCriticalDamage;
    public string lastAttack;


    protected virtual void Awake()
    {
      character = GetComponent<CharacterManager>();
    }

    public virtual void SetBlockingAbsorptionsFromBlockingWeapon()
    {
      if (character.isUsingRightHand)
      {
        character.characterStatsManager.blockingPhysicalDamageAbsorption = character.characterInventoryManager.rightWeapon.physicalBlockingDamageAbsorption;
        character.characterStatsManager.blockingFireDamageAbsorption = character.characterInventoryManager.rightWeapon.fireBlockingDamageAbsorption;
        character.characterStatsManager.blockingStabilityRating = character.characterInventoryManager.rightWeapon.stability;
      }
      else if (character.isUsingLeftHand)
      {
        character.characterStatsManager.blockingPhysicalDamageAbsorption = character.characterInventoryManager.leftWeapon.physicalBlockingDamageAbsorption;
        character.characterStatsManager.blockingFireDamageAbsorption = character.characterInventoryManager.leftWeapon.fireBlockingDamageAbsorption;
        character.characterStatsManager.blockingStabilityRating = character.characterInventoryManager.leftWeapon.stability;
      }
    }

    public virtual void DrainStaminaBasedOnAttack()
    {

    }


   


    private void SuccessfullyCastSpell()
    {
      character.characterInventoryManager.currentSpell.SuccessfullyCastSpell(character);
    }

    public void AttemptBackStabOrRiposte()
    {
      if (character.isInteracting)
        return;

      if (character.characterStatsManager.currentStamina <= 0)
        return;

      RaycastHit hit;
      if (Physics.Raycast(character.criticalAttackRayCastStartPoint.transform.position, character.transform.TransformDirection(Vector3.forward), out hit, criticalAttackRange, characterLayer))
      {
        CharacterManager enemyCharacter = hit.transform.GetComponent<CharacterManager>();
        Vector3 directionFromCharacterToEnemy = transform.position - enemyCharacter.transform.position;
        float dotValue = Vector3.Dot(directionFromCharacterToEnemy, enemyCharacter.transform.forward);

        Debug.Log("CURRENT DOT VALUE IS " + dotValue);

        if (enemyCharacter.canBeRiposted)
        {
          if (dotValue <= 1.2f && dotValue >= 0.6f)
          {
           AttemptRiposte(hit);
           return;
          }
        }

        if (dotValue >= -0.7f && dotValue <= -0.6f)
        {
          AttemptBackStab(hit);
        }
      }
    }

    IEnumerator ForceMoveCharacterToEnemyBackStabPosition(CharacterManager characterPerformingBackStab)
    {
      for (float timer = 0.05f; timer < 0.5f; timer += 0.05f)
      {
        Quaternion backstabRotation = Quaternion.LookRotation(-characterPerformingBackStab.transform.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, backstabRotation, 1);
        transform.parent = characterPerformingBackStab.characterCombatManager.backStabReceiverTransform;
        transform.localPosition = characterPerformingBackStab.characterCombatManager.backStabReceiverTransform.localPosition;
        transform.parent = null;
        yield return new WaitForSeconds(0.05f);
      }
    }
    IEnumerator ForceMoveCharacterToEnemyRipostePosition(CharacterManager characterPerformingRiposte)
    {
      for (float timer = 0.05f; timer < 0.5f; timer += 0.05f)
      {
        Quaternion backstabRotation = Quaternion.LookRotation(-characterPerformingRiposte.transform.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, backstabRotation, 1);
        transform.parent = characterPerformingRiposte.characterCombatManager.riposteReceiverTransform;
        transform.localPosition = characterPerformingRiposte.characterCombatManager.riposteReceiverTransform.localPosition;
        transform.parent = null;
        yield return new WaitForSeconds(0.05f);
      }
    }

    public void GetBackStabbed(CharacterManager characterPerformingBackStab)
    {
      character.isBeingBackstabbed = true;
      StartCoroutine(ForceMoveCharacterToEnemyBackStabPosition(characterPerformingBackStab));
      character.characterAnimatorManager.PlayTargetAnimation("Back_Stabbed_01", true);
    }


    public void GetRiposted(CharacterManager characterPerformingRiposte)
    {
      character.isBeingRiposted = true;
      StartCoroutine(ForceMoveCharacterToEnemyRipostePosition(characterPerformingRiposte));
      character.characterAnimatorManager.PlayTargetAnimation("Riposted_01", true);
    }

    private void AttemptBackStab(RaycastHit hit)
    {
      CharacterManager enemyCharacter = hit.transform.GetComponent<CharacterManager>();
      if (enemyCharacter != null)
      {
        if (!enemyCharacter.isBeingBackstabbed && !enemyCharacter.isBeingRiposted)
        {
          //We make it so the aiCharacter cannot be damaged whilst being critically damaged
          EnableIsInvulnerable();
          character.isPerformingBackstab = true;
          character.characterAnimatorManager.EraseHandIKForWeapon();

          character.characterAnimatorManager.PlayTargetAnimation("Back_Stab_01", true);
          enemyCharacter.characterAnimatorManager.PlayTargetAnimation("Back_Stabbed_01", true);

          float criticalDamage = (character.characterInventoryManager.rightWeapon.criticalDamageMultiplier * (character.characterInventoryManager.rightWeapon.physicalDamage + character.characterInventoryManager.rightWeapon.fireDamage));

          int roundedCriticalDamage = Mathf.RoundToInt(criticalDamage);
          enemyCharacter.characterCombatManager.pendingCriticalDamage = roundedCriticalDamage;
          enemyCharacter.characterCombatManager.GetBackStabbed(character);
        }
      }
    }

    private void AttemptRiposte (RaycastHit hit){
      CharacterManager enemyCharacter = hit.transform.GetComponent<CharacterManager>();
      if (enemyCharacter != null)
      {
        if (!enemyCharacter.isBeingBackstabbed && !enemyCharacter.isBeingRiposted)
        {
          //We make it so the aiCharacter cannot be damaged whilst being critically damaged
          EnableIsInvulnerable();
          character.isPerformingRiposte = true;
          character.characterAnimatorManager.EraseHandIKForWeapon();

          character.characterAnimatorManager.PlayTargetAnimation("Riposte_01", true);

          float criticalDamage = (character.characterInventoryManager.rightWeapon.criticalDamageMultiplier * (character.characterInventoryManager.rightWeapon.physicalDamage + character.characterInventoryManager.rightWeapon.fireDamage));

          int roundedCriticalDamage = Mathf.RoundToInt(criticalDamage);
          enemyCharacter.characterCombatManager.pendingCriticalDamage = roundedCriticalDamage;
          enemyCharacter.characterCombatManager.GetRiposted(character);
        }
      }
    }


    private void EnableIsInvulnerable()
    {
      character.animator.SetBool("isInvulnerable", true);
    }

    public void ApplyPendingDamage(){
      character.characterStatsManager.TakeDamageNoAnimation(pendingCriticalDamage, 0);
    }
    public void EnableCanBeParried(){
      character.canBeParried = true;
    }
    public void DisableCanBeParried(){
      character.canBeParried = false;
    }

  }
}
