using UnityEngine;
namespace MR
{
  public class EquipmentItem : Item
  {
    [Header("Defense Bonus")]
    public float physicalDefense;
    public float magicDefense;

    [Header("Weight")]
    public float weight = 0;

    [Header("Resistances")]
    public float poisonResistance;
  }
}