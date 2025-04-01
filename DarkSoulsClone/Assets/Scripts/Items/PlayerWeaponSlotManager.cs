using UnityEngine;
namespace SG
{

  public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
  {
    QuickSlotsUI quickSlotsUI;
    InputHandler inputHandler;
    Animator animator;
    PlayerManager playerManager;
    PlayerInventoryManager playerInventoryManager;
    PlayerStatsManager playerStatsManager;
    PlayerEffectsManager playerEffectsManager;
    CameraHandler cameraHandler;

    [Header("Attacking Weapon")]
    public WeaponItem attackingWeapon;




    private void Awake()
    {
      cameraHandler = FindObjectOfType<CameraHandler>();
      playerStatsManager = GetComponent<PlayerStatsManager>();
      inputHandler = GetComponent<InputHandler>();

      playerManager = GetComponent<PlayerManager>();
      playerInventoryManager = GetComponent<PlayerInventoryManager>();
      playerEffectsManager = GetComponent<PlayerEffectsManager>();
      animator = GetComponent<Animator>();
      quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
      LoadWeaponHolderSlots();
    }

    private void LoadWeaponHolderSlots()
    {

      WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
      foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
      {
        if (weaponSlot.isLeftHandSlot)
        {
          leftHandSlot = weaponSlot;
        }
        else if (weaponSlot.isRightHandSlot)
        {
          rightHandSlot = weaponSlot;
        }
        else if (weaponSlot.isBackSlot)
        {
          backSlot = weaponSlot;
        }
      }
    }


    public void LoadBothWeaponsOnSlots()
    {
      LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
      LoadWeaponOnSlot(playerInventoryManager.leftWeapon, true);
    }
    public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {

      if (weaponItem != null)
      {
        if (isLeft)
        {
          leftHandSlot.currentWeapon = weaponItem;
          leftHandSlot.LoadWeaponModel(weaponItem);
          LoadLeftWeaponDamageCollider();
          quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
          animator.CrossFade(weaponItem.left_hand_idle, 0.2f);
        }
        else
        {
          if (inputHandler.twoHandFlag)
          {
            backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
            leftHandSlot.UnloadWeaponAndDestroy();
            animator.CrossFade(weaponItem.th_idle, 0.2f);
          }
          else
          {
            animator.CrossFade("Both Arms Empty", 0.2f);
            backSlot.UnloadWeaponAndDestroy();
            animator.CrossFade(weaponItem.right_hand_idle, 0.2f);
          }
          rightHandSlot.currentWeapon = weaponItem;
          rightHandSlot.LoadWeaponModel(weaponItem);
          LoadRightWeaponDamageCollider();
          quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
        }
      }
      else
      {
        weaponItem = unarmedWeapon;
        if (isLeft)
        {
          animator.CrossFade("Left Arm Empty", 0.2f);
          playerInventoryManager.leftWeapon = unarmedWeapon;
          leftHandSlot.currentWeapon = unarmedWeapon;
          leftHandSlot.LoadWeaponModel(weaponItem);
          LoadLeftWeaponDamageCollider();
          quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
        }
        else
        {
          animator.CrossFade("Right Arm Empty", 0.2f);
          playerInventoryManager.rightWeapon = unarmedWeapon;
          rightHandSlot.currentWeapon = unarmedWeapon;
          rightHandSlot.LoadWeaponModel(weaponItem);
          LoadRightWeaponDamageCollider();
          quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
        }
      }

    }

    public void SucessfullyThrowFireBomb()
    {
      Destroy(playerEffectsManager.instantiatedFXModel);

      BombConsumeableItem fireBombItem = playerInventoryManager.currentConsumable as BombConsumeableItem;

      GameObject activeModelBomb = Instantiate(fireBombItem.liveBombModel, rightHandSlot.transform.position, cameraHandler.cameraPivotTransform.rotation);

      activeModelBomb.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerManager.lockOnTransform.eulerAngles.y, 0);
      BombDamageCollider damageCollider = activeModelBomb.GetComponentInChildren<BombDamageCollider>();

      damageCollider.explosionDamage = fireBombItem.baseDamage;
      damageCollider.explosionSplashDamage = fireBombItem.explosiveDamage;

      damageCollider.bombRigidBody.AddForce(activeModelBomb.transform.forward * fireBombItem.forwardVelocity);
      damageCollider.bombRigidBody.AddForce(activeModelBomb.transform.up * fireBombItem.upwardVelocity);
      LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
      //CHECK FOR FRIENDLY FIRE


    }


    #region Handle Weapons Damage Collider



    private void LoadLeftWeaponDamageCollider()
    {
      leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
      leftHandDamageCollider.physicalDamage = playerInventoryManager.leftWeapon.physicalDamage;
      leftHandDamageCollider.fireDamage = playerInventoryManager.leftWeapon.fireDamage;
      leftHandDamageCollider.poiseBreak = playerInventoryManager.leftWeapon.poiseBreak;
      playerEffectsManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
    }

    private void LoadRightWeaponDamageCollider()
    {
      rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
      rightHandDamageCollider.physicalDamage = playerInventoryManager.rightWeapon.physicalDamage;
      rightHandDamageCollider.fireDamage = playerInventoryManager.rightWeapon.fireDamage;
      rightHandDamageCollider.poiseBreak = playerInventoryManager.rightWeapon.poiseBreak;
      playerEffectsManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
    }

    public void OpenDamageCollider()
    {
      if (playerManager.isUsingRightHand)
      {
        rightHandDamageCollider.EnableDamageCollider();

      }
      else if (playerManager.isUsingLeftHand)
      {
        leftHandDamageCollider.EnableDamageCollider();
      }
    }

    public void CloseDamageCollider()
    {

      if (rightHandDamageCollider != null)
      {
        rightHandDamageCollider.DisableDamageCollider();
      }
      if (leftHandDamageCollider != null)
      {
        leftHandDamageCollider.DisableDamageCollider();
      }
    }


    #endregion

    #region  Handle Weapons Stamina Drainage
    public void DrainStaminaLightAttack()
    {
      playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
    }
    public void DrainStaminaHeavyAttack()
    {
      playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
    }
    #endregion


    #region Handle Weapon's Poise Bonus
    public void GrantWeaponAttackingPoiseBonus()
    {
      playerStatsManager.totalPoiseDefence += attackingWeapon.offensivePoiseBonus;
    }

    public void ResetWeaponAttackingPoiseBonus()
    {
      playerStatsManager.totalPoiseDefence = playerStatsManager.armorPoiseBonus;
    }
    #endregion

  }

}