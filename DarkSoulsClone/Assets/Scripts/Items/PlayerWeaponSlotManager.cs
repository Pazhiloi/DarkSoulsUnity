using UnityEngine;
namespace MR
{

  public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
  {
    PlayerManager player;

    protected override void Awake()
    {
      base.Awake();
      player = GetComponent<PlayerManager>();
    }

    public override void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {

      if (weaponItem != null)
      {
        if (isLeft)
        {
          leftHandSlot.currentWeapon = weaponItem;
          leftHandSlot.LoadWeaponModel(weaponItem);
          LoadLeftWeaponDamageCollider();
          player.uiManager.quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
          // player.playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
        }
        else
        {
          if (player.inputHandler.twoHandFlag)
          {
            backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
            leftHandSlot.UnloadWeaponAndDestroy();
            player.playerAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);
          }
          else
          {
            backSlot.UnloadWeaponAndDestroy();
          }
          rightHandSlot.currentWeapon = weaponItem;
          rightHandSlot.LoadWeaponModel(weaponItem);
          LoadRightWeaponDamageCollider();
          player.uiManager.quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
          player.animator.runtimeAnimatorController = weaponItem.weaponController;
        }
      }
      else
      {
        weaponItem = unarmedWeapon;
        if (isLeft)
        {
          player.playerInventoryManager.leftWeapon = unarmedWeapon;
          leftHandSlot.currentWeapon = unarmedWeapon;
          leftHandSlot.LoadWeaponModel(weaponItem);
          LoadLeftWeaponDamageCollider();
          player.uiManager.quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
          // player.playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
        }
        else
        {
          player.playerInventoryManager.rightWeapon = unarmedWeapon;
          rightHandSlot.currentWeapon = unarmedWeapon;
          rightHandSlot.LoadWeaponModel(weaponItem);
          LoadRightWeaponDamageCollider();
          player.uiManager.quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
          player.animator.runtimeAnimatorController = weaponItem.weaponController;
        }
      }

    }

    public void SucessfullyThrowFireBomb()
    {
      Destroy(player.playerEffectsManager.instantiatedFXModel);

      BombConsumeableItem fireBombItem = player.playerInventoryManager.currentConsumable as BombConsumeableItem;

      GameObject activeModelBomb = Instantiate(fireBombItem.liveBombModel, rightHandSlot.transform.position, player.cameraHandler.cameraPivotTransform.rotation);

      activeModelBomb.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraPivotTransform.eulerAngles.x, player.lockOnTransform.eulerAngles.y, 0);
      BombDamageCollider damageCollider = activeModelBomb.GetComponentInChildren<BombDamageCollider>();

      damageCollider.explosionDamage = fireBombItem.baseDamage;
      damageCollider.explosionSplashDamage = fireBombItem.explosiveDamage;

      damageCollider.bombRigidBody.AddForce(activeModelBomb.transform.forward * fireBombItem.forwardVelocity);
      damageCollider.bombRigidBody.AddForce(activeModelBomb.transform.up * fireBombItem.upwardVelocity);
      damageCollider.teamIDNumber = player.playerStatsManager.teamIDNumber;
      LoadWeaponOnSlot(player.playerInventoryManager.rightWeapon, false);


    }

  }

}