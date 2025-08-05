using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MR
{

  public class WeaponPickUp : Interactable
  {

    [Header("World Item ID")]
    [SerializeField] private int itemPickUpID;
    [SerializeField] private bool hasBeenLooted;
    [Header("item")]
    public WeaponItem weapon;

    protected override void Awake()
    {
      base.Awake();

      
    }

    protected override void Start()
    {
      base.Start();
      if (!WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.ContainsKey(itemPickUpID))
      {
        WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.Add(itemPickUpID, false);
      }


      hasBeenLooted = WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld[itemPickUpID];
      if (hasBeenLooted)
      {
        gameObject.SetActive(false);
      }
    }

    public override void Interact(PlayerManager playerManager)
    {
      base.Interact(playerManager);

      // Notify the character data this item has been looted from the world, so it does not spawn again
      if (WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.ContainsKey(itemPickUpID))
      {
        WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.Remove(itemPickUpID);
      }

      // Saves the pick up to our save data so it does not spawn again when we re-load the area
      WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.Add(itemPickUpID, true);

      hasBeenLooted = true;

      PickUpItem(playerManager);

    }

    private void PickUpItem(PlayerManager playerManager){
      PlayerInventoryManager playerInventoryManager;
      PlayerLocomotionManager playerLocomotionManager;
      PlayerAnimatorManager playerAnimatorManager;

      playerInventoryManager = playerManager.GetComponent<PlayerInventoryManager>();
      playerLocomotionManager = playerManager.GetComponent<PlayerLocomotionManager>();
      playerAnimatorManager = playerManager.GetComponentInChildren<PlayerAnimatorManager>();

      playerLocomotionManager.GetComponent<Rigidbody>().velocity = Vector3.zero;
      playerAnimatorManager.PlayTargetAnimation("Pick Up Item", true);
      playerInventoryManager.weaponsInventory.Add(weapon);
      playerManager.itemInteractableGameObject.GetComponentInChildren<Text>().text = weapon.itemName;
      playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
      playerManager.itemInteractableGameObject.SetActive(true);
      Destroy(gameObject);
    }
  }
}
