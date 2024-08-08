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
    TorsoModelChanger torsoModelChanger;

    [Header("Default Naked Models")]
    public GameObject nakedHeadModelGO;
    public string nakedHeadModel;
    public string nakedTorsoModel;
    public BlockingCollider blockingCollider;

    private void Awake()
    {
      inputHandler = GetComponentInParent<InputHandler>();
      playerInverntory = GetComponentInParent<PlayerInventory>();
      helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
      torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();
    }

    private void Start() {
      EquipAllEquipmentModelsOnStart();
    }

    private void EquipAllEquipmentModelsOnStart(){
      helmetModelChanger.UnequipAllHelmetModels();
      torsoModelChanger.UnequipAllTorsoModels();
      if (playerInverntory.currentHelmetEquipment != null)
      {
        nakedHeadModelGO.SetActive(false);
        helmetModelChanger.EquipHelmetModelByName(playerInverntory.currentHelmetEquipment.helmetModelName);
      }
      else{
        helmetModelChanger.EquipHelmetModelByName(nakedHeadModel);
        nakedHeadModelGO.SetActive(true);
      }

      if (playerInverntory.currentTorsoEquipment!= null) 
      {
        torsoModelChanger.EquipTorsoModelByName(playerInverntory.currentTorsoEquipment.torsoModelName);
      }
      else{
        torsoModelChanger.EquipTorsoModelByName(nakedTorsoModel);
      }
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
