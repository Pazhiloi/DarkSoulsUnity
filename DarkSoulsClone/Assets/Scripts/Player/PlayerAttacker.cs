using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{
  public class PlayerAttacker : MonoBehaviour
  {
    AnimatorHandler animatorHandler;
    PlayerManager playerManager;
    PlayerStats playerStats;
    PlayerInventory playerInventory;
    InputHandler inputHandler;
    WeaponSlotManager weaponSlotManager;
    public string lastAttack;
    public LayerMask backStabLayer = 1 << 12;

    private void Awake()
    {
      animatorHandler = GetComponent<AnimatorHandler>();
      playerManager = GetComponentInParent<PlayerManager>();
      playerStats = GetComponentInParent<PlayerStats>();
      playerInventory = GetComponentInParent<PlayerInventory>();
      weaponSlotManager = GetComponent<WeaponSlotManager>();
      inputHandler = GetComponentInParent<InputHandler>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {

      if (inputHandler.comboFlag)
      {
        animatorHandler.anim.SetBool("canDoCombo", false);

        if (lastAttack == weapon.oh_light_attack_01)
        {
          animatorHandler.PlayTargetAnimation(weapon.oh_light_attack_02, true);
        }
        else if (lastAttack == weapon.th_light_attack_01)
        {
          animatorHandler.PlayTargetAnimation(weapon.th_light_attack_02, true);
        }
      }

    }

    public void HandleLightAttack(WeaponItem weapon)
    {
      weaponSlotManager.attackingWeapon = weapon;
      if (inputHandler.twoHandFlag)
      {
        animatorHandler.PlayTargetAnimation(weapon.th_light_attack_01, true);
        lastAttack = weapon.th_light_attack_01;
      }
      else
      {
        animatorHandler.PlayTargetAnimation(weapon.oh_light_attack_01, true);
        lastAttack = weapon.oh_light_attack_01;
      }
    }
    public void HandleHeavyAttack(WeaponItem weapon)
    {
      weaponSlotManager.attackingWeapon = weapon;

      if (inputHandler.twoHandFlag)
      {
        animatorHandler.PlayTargetAnimation(weapon.oh_light_attack_01, true);
        lastAttack = weapon.oh_light_attack_01;
      }
      else
      {
        animatorHandler.PlayTargetAnimation(weapon.oh_light_attack_01, true);
        lastAttack = weapon.oh_light_attack_01;
      }
    }


    #region  Input Actions
    public void HandleRBAction()
    {
      if (playerInventory.rightWeapon.isMelleWeapon)
      {
        PerformRBMelleAction();
      }
      else if (playerInventory.rightWeapon.isSpellCaster || playerInventory.rightWeapon.isFaithCaster || playerInventory.rightWeapon.isPyroCaster)
      {
        PerformRBMagicAction(playerInventory.rightWeapon);
      }
    }

    #endregion

    #region Attack Actions

    private void PerformRBMelleAction()
    {
      if (playerManager.canDoCombo)
      {
        inputHandler.comboFlag = true;
        HandleWeaponCombo(playerInventory.rightWeapon);
        inputHandler.comboFlag = false;
      }
      else
      {
        if (playerManager.isInteracting)
        {
          return;
        }
        if (playerManager.canDoCombo)
        {
          return;
        }
        animatorHandler.anim.SetBool("isUsingRightHand", true);
        HandleLightAttack(playerInventory.rightWeapon);
      }
    }


    private void PerformRBMagicAction(WeaponItem weapon)
    {
      if (playerManager.isInteracting) return;
      if (weapon.isFaithCaster)
      {
        if (playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
        {
          if (playerStats.currentFocusPoints >= playerInventory.currentSpell.focusPointCost)
          {
            playerInventory.currentSpell.AttemptToCastSpell(animatorHandler, playerStats);
          }
          else
          {
            animatorHandler.PlayTargetAnimation("Damage_01", true);
          }

        }
      }
    }

    private void SuccessfullyCastSpell()
    {
      playerInventory.currentSpell.SuccessfullyCastSpell(animatorHandler, playerStats);
    }

    #endregion

    public void AttemptBackStabOrRiposte()
    {
      RaycastHit hit;
      if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
      {
        CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
        if (enemyCharacterManager != null)
        {

          playerManager.transform.position = enemyCharacterManager.backStabCollider.backStaberStandPoint.position;

          // rotate towards enemy transform
          Vector3 rotationDirection = playerManager.transform.root.eulerAngles; 
          rotationDirection = hit.transform.position - playerManager.transform.position;
          rotationDirection.y = 0;
          rotationDirection.Normalize();
          Quaternion tr = Quaternion.LookRotation(rotationDirection);
          Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
          playerManager.transform.rotation = targetRotation;





          animatorHandler.PlayTargetAnimation("Back Stab", true);
          enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Back Stabbed", true);
        }
      }
    }

  }
}
