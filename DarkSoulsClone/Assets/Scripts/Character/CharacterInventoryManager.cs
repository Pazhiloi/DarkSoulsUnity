using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  public class CharacterInventoryManager : MonoBehaviour
  {

    protected CharacterWeaponSlotManager characterWeaponSlotManager;

    [Header("Current Item Being Used")]
    public Item currentItemBeingUsed;


    [Header("Quick Slots Items")]
    public SpellItem currentSpell;
    public WeaponItem rightWeapon;
    public WeaponItem leftWeapon;
    public ConsumableItem currentConsumable;
    public RangedAmmoItem currentAmmo;

    [Header("Current Equipment")]
    public HelmetEquipment currentHelmetEquipment;
    public TorsoEquipment currentTorsoEquipment;
    public LegEquipment currentLegEquipment;
    public HandEquipment currentHandEquipment;

    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[2];
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[2];

    public int currentRightWeaponIndex = 0;
    public int currentLeftWeaponIndex = 0;



    private void Awake()
    {
      characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
    }

    private void Start()
    {
      characterWeaponSlotManager.LoadBothWeaponsOnSlots();
    }
  }
}