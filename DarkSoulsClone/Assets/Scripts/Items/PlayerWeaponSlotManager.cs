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
    PlayerAnimatorManager playerAnimatorManager;
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
      playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
      animator = GetComponent<Animator>();
      quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
      LoadWeaponHolderSlots();
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
          playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
        }
        else
        {
          if (inputHandler.twoHandFlag)
          {
            backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
            leftHandSlot.UnloadWeaponAndDestroy();
            playerAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);
          }
          else
          {
            backSlot.UnloadWeaponAndDestroy();
          }
          rightHandSlot.currentWeapon = weaponItem;
          rightHandSlot.LoadWeaponModel(weaponItem);
          LoadRightWeaponDamageCollider();
          quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
          playerAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
        }
      }
      else
      {
        weaponItem = unarmedWeapon;
        if (isLeft)
        {
          playerInventoryManager.leftWeapon = unarmedWeapon;
          leftHandSlot.currentWeapon = unarmedWeapon;
          leftHandSlot.LoadWeaponModel(weaponItem);
          LoadLeftWeaponDamageCollider();
          quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
          playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
        }
        else
        {
          playerInventoryManager.rightWeapon = unarmedWeapon;
          rightHandSlot.currentWeapon = unarmedWeapon;
          rightHandSlot.LoadWeaponModel(weaponItem);
          LoadRightWeaponDamageCollider();
          quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
          playerAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
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
      damageCollider.teamIDNumber = playerStatsManager.teamIDNumber;
      LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);


    }


    #region Handle Weapons Damage Collider



    private void LoadLeftWeaponDamageCollider()
    {
      leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
      leftHandDamageCollider.physicalDamage = playerInventoryManager.leftWeapon.physicalDamage;

      leftHandDamageCollider.fireDamage = playerInventoryManager.leftWeapon.fireDamage;

      leftHandDamageCollider.teamIDNumber = playerStatsManager.teamIDNumber;


      leftHandDamageCollider.poiseBreak = playerInventoryManager.leftWeapon.poiseBreak;
      playerEffectsManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
    }

    private void LoadRightWeaponDamageCollider()
    {
      rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

      rightHandDamageCollider.physicalDamage = playerInventoryManager.rightWeapon.physicalDamage;
      rightHandDamageCollider.fireDamage = playerInventoryManager.rightWeapon.fireDamage;

      rightHandDamageCollider.teamIDNumber = playerStatsManager.teamIDNumber;

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