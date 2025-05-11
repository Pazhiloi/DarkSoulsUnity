using UnityEngine;

namespace MR
{
  [CreateAssetMenu(menuName = "A.I/Humanoid Actions/Item Based Attack Action")]
  public class ItemBasedAttackAction : ScriptableObject
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

    public void PerformAttackAction(AICharacterManager aiCharacter)
    {
      if (isRightHandedAction)
      {
        aiCharacter.UpdateWhichHandCharacterIsUsing(true);
        PerformRightHandItemActionBasedOnAttackType(aiCharacter);
      }
      else
      {
        aiCharacter.UpdateWhichHandCharacterIsUsing(false);
        PerformLeftHandItemActionBasedOnAttackType(aiCharacter);
      }
    }
    private void PerformRightHandItemActionBasedOnAttackType(AICharacterManager aiCharacter)
    {
      if (actionAttackType == AIAttackActionType.meleeAttackAction)
      {
       PerformRightHandMeleeAction(aiCharacter);
      }
      else if (actionAttackType == AIAttackActionType.rangedAttackAction)
      {
        //PERFORM RIGHT HAND RANGED ACTION
      }
    }

    private void PerformLeftHandItemActionBasedOnAttackType(AICharacterManager aiCharacter)
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

    private void PerformRightHandMeleeAction(AICharacterManager aiCharacter)
    {
      if (aiCharacter.isTwoHandingWeapon)
      {
        if (attackType == AttackType.light)
        {
          aiCharacter.characterInventoryManager.rightWeapon.th_tap_RB_Action.PerformAction(aiCharacter);
        }
        else if (attackType == AttackType.heavy)
        {
          aiCharacter.characterInventoryManager.rightWeapon.th_tap_RT_Action.PerformAction(aiCharacter);
        }
      }
      else
      {
        if (attackType == AttackType.light)
        {
          aiCharacter.characterInventoryManager.rightWeapon.oh_tap_RB_Action.PerformAction(aiCharacter);
        }
        else if (attackType == AttackType.heavy)
        {
          aiCharacter.characterInventoryManager.rightWeapon.oh_tap_RT_Action.PerformAction(aiCharacter);
        }
      }
    }


  }
}
