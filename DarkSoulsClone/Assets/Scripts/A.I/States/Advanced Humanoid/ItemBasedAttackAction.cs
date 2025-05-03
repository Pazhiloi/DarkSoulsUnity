using UnityEngine;

namespace MR
{
    public class ItemBasedAttackAction : MonoBehaviour
    {
    [Header("Attack Type")]
    public AIAttackActionType actionAttackType = AIAttackActionType.meleeAttackAction;
    public AttackType attackType = AttackType.light;

    [Header("Action Combo Settings")]
    public bool actionCanCombo = false;

    [Header("Right Hand Or Left Hand Action")]
    bool isRightHandedAction = true;

    [Header("Action Settings")]
    public int attackScore = 3;
    public float recoveryTime = 2;
    public float maximumAttackAngle = 35;
    public float minimumAttackAngle = -35;
    public float minimumDistanceNeededToAttack = 0;
    public float maximumDistanceNeededToAttack = 3;

    public void PerformAttackAction(EnemyManager enemy)
    {
      if (isRightHandedAction)
      {
        enemy.UpdateWhichHandCharacterIsUsing(true);
        PerformRightHandItemActionBasedOnAttackType(enemy);
      }
      else
      {
        enemy.UpdateWhichHandCharacterIsUsing(false);
        PerformLeftHandItemActionBasedOnAttackType(enemy);
      }
    }
    private void PerformRightHandItemActionBasedOnAttackType(EnemyManager enemy)
    {
      if (actionAttackType == AIAttackActionType.meleeAttackAction)
      {
       PerformRightHandMeleeAction(enemy);
      }
      else if (actionAttackType == AIAttackActionType.rangedAttackAction)
      {
        //PERFORM RIGHT HAND RANGED ACTION
      }
    }

    private void PerformLeftHandItemActionBasedOnAttackType(EnemyManager enemy)
    {
      if (actionAttackType == AIAttackActionType.meleeAttackAction)
      {
        //PERFORM LEFT HAND MELEE ACTION
      }
      else if (actionAttackType == AIAttackActionType.rangedAttackAction)
      {
        //PERFORM LEFT HAND RANGED ACTION
      }
    }

    private void PerformRightHandMeleeAction(EnemyManager enemy)
    {
      if (enemy.isTwoHandingWeapon)
      {
        if (attackType == AttackType.light)
        {
          enemy.characterInventoryManager.rightWeapon.th_tap_RB_Action.PerformAction(enemy);
        }
        else if (attackType == AttackType.heavy)
        {
          enemy.characterInventoryManager.rightWeapon.th_tap_RT_Action.PerformAction(enemy);
        }
      }
      else
      {
        if (attackType == AttackType.light)
        {
          enemy.characterInventoryManager.rightWeapon.oh_tap_RB_Action.PerformAction(enemy);
        }
        else if (attackType == AttackType.heavy)
        {
          enemy.characterInventoryManager.rightWeapon.oh_tap_RT_Action.PerformAction(enemy);
        }
      }
    }


  }
}
