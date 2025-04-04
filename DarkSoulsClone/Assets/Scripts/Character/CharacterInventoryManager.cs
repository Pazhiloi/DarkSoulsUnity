using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class CharacterInventoryManager : MonoBehaviour
    {

     protected CharacterWeaponSlotManager characterWeaponSlotManager;
  [Header("Quick Slots Items")]
    public SpellItem currentSpell;
    public WeaponItem rightWeapon;
    public WeaponItem leftWeapon;
    public ConsumableItem currentConsumable;

    [Header("Current Equipment")]
    public HelmetEquipment currentHelmetEquipment;
    public TorsoEquipment currentTorsoEquipment;
    public LegEquipment currentLegEquipment;
    public HandEquipment currentHandEquipment;

    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[2];
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[2];

    public int currentRightWeaponIndex = -1;
    public int currentLeftWeaponIndex = -1;

    

    private void Awake()
{

}

private void Start()
{
    characterWeaponSlotManager.LoadBothWeaponsOnSlots();
}
    }
}