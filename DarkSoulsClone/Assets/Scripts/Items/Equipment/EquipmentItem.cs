using UnityEngine;
namespace MR
{
  public class EquipmentItem : Item
  {
    [Header("Defense Bonus")]
    public float physicalDefense;
    public float magicDefense;

    [Header("Resistances")]
    public float poisonResistance;
  }
}