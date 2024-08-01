using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{
public class PlayerEquipmentManager : MonoBehaviour
{
    InputHandler inputHandler;
    PlayerInventory playerInverntory;
    public BlockingCollider blockingCollider;

    private void Awake()
    {
      inputHandler = GetComponentInParent<InputHandler>();
      playerInverntory = GetComponentInParent<PlayerInventory>();
    }

    public void OpenBlockingCollider()
    {
      if (inputHandler.twoHandFlag)
      {
        blockingCollider.SetColliderDamageAbsorption(playerInverntory.rightWeapon);
      }
      else
      {
        blockingCollider.SetColliderDamageAbsorption(playerInverntory.leftWeapon);
      }

      blockingCollider.EnableBlockingCollider();
    }

    public void CloseBlockingCollider()
    {
      blockingCollider.DisableBlockingCollider();
    }
  }
}
