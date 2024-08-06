using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{
public class PlayerEquipmentManager : MonoBehaviour
{
    InputHandler inputHandler;
    PlayerInventory playerInverntory;
    [Header("Equipment Model Changers")]
    HelmetModelChanger helmetModelChanger;
    public BlockingCollider blockingCollider;

    private void Awake()
    {
      inputHandler = GetComponentInParent<InputHandler>();
      playerInverntory = GetComponentInParent<PlayerInventory>();
      helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
    }

    private void Start() {
      helmetModelChanger.UnequipAllHelmetModels();
      helmetModelChanger.EquipHelmetModelByName(playerInverntory.currentHelmetEquipment.helmetModelName);
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
