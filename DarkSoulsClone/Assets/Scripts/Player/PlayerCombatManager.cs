using UnityEngine;
namespace MR
{
  public class PlayerCombatManager : CharacterCombatManager
  {
    PlayerManager player;



    

    protected override void Awake()
    {
      base.Awake();
      player = GetComponent<PlayerManager>();
    }
    public override void AttemptBlock(DamageCollider attackingWeapon, float physicalDamage, float fireDamage, string blockAnimation)
    {
      base.AttemptBlock(attackingWeapon, physicalDamage, fireDamage, blockAnimation);
      player.playerStatsManager.staminaBar.SetCurrentStamina(player.playerStatsManager.currentStamina);
    }

    public override void DrainStaminaBasedOnAttack()
    {
      if (player.isUsingRightHand)
      {
        if (currentAttackType == AttackType.light)
        {
          player.playerStatsManager.DeductStamina(player.playerInventoryManager.rightWeapon.baseStaminaCost * player.playerInventoryManager.rightWeapon.lightAttackStaminaMultiplier);

        }
        else if (currentAttackType == AttackType.heavy)
        {
          player.playerStatsManager.DeductStamina(player.playerInventoryManager.rightWeapon.baseStaminaCost * player.playerInventoryManager.rightWeapon.heavyAttackStaminaMultiplier);

        }
      }
      else if (player.isUsingLeftHand)
      {
        if (currentAttackType == AttackType.light)
        {
          player.playerStatsManager.DeductStamina(player.playerInventoryManager.leftWeapon.baseStaminaCost * player.playerInventoryManager.leftWeapon.lightAttackStaminaMultiplier);

        }
        else if (currentAttackType == AttackType.heavy)
        {
          player.playerStatsManager.DeductStamina(player.playerInventoryManager.leftWeapon.baseStaminaCost * player.playerInventoryManager.leftWeapon.heavyAttackStaminaMultiplier);

        }
      }
    }


  }
}