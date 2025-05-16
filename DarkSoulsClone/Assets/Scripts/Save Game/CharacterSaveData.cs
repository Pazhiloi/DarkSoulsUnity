using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  [System.Serializable]
  public class CharacterSaveData
  {
    public string characterName;
    public int characterLevel;

    [Header("Equipment")]
    public int currentRightHandWeaponID;
    public int currentLeftHandWeaponID;

    public int currentHeadGearItemID;
    public int currentChestGearItemID;
    public int currentLegGearItemID;
    public int currentHandGearItemID;

    [Header("World Coordinates")]
    public float xPosition;
    public float yPosition;
    public float zPosition;


    [Header("Items Loodted From World")]
    public SerializableDictionary<int, bool> itemsInWorld;

    public CharacterSaveData()
    {
      itemsInWorld = new SerializableDictionary<int, bool>(); 
    }


  }
}