using UnityEngine;
namespace SG
{
  public class PlayerEquipmentManager : MonoBehaviour
  {
    InputHandler inputHandler;
    PlayerInventory playerInverntory;
    PlayerStats playerStats;
    [Header("Equipment Model Changers")]
    HelmetModelChanger helmetModelChanger;
    TorsoModelChanger torsoModelChanger;
    UpperLeftArmModelChanger upperLeftArmModelChanger;
    UpperRightArmModelChanger upperRightArmModelChanger;
    HipModelChanger hipModelChanger;
    LeftLegModelChanger leftLegModelChanger;
    RightLegModelChanger rightLegModelChanger;
    LowerLeftArmModelChanger lowerLeftArmModelChanger;
    LowerRightArmModelChanger lowerRightArmModelChanger;
    LeftHandModelChanger leftHandModelChanger;
    RightHandModelChanger rightHandModelChanger;


    [Header("Default Naked Models")]
    public GameObject nakedHeadModelGO;
    public string nakedHeadModel;
    public string nakedUpperLeftArm, nakedUpperRightArm, nakedLowerLeftArm, nakedLowerRightArm, nakedLeftHand, nakedRightHand;
    public string nakedTorsoModel;
    public string nakedHipModel, nakedLeftLeg, nakedRightLeg;
    public BlockingCollider blockingCollider;

    private void Awake()
    {
      inputHandler = GetComponentInParent<InputHandler>();
      playerInverntory = GetComponentInParent<PlayerInventory>();
      playerStats = GetComponentInParent<PlayerStats>();

      helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
      torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();
      hipModelChanger = GetComponentInChildren<HipModelChanger>();
      leftLegModelChanger = GetComponentInChildren<LeftLegModelChanger>();
      rightLegModelChanger = GetComponentInChildren<RightLegModelChanger>();
      upperLeftArmModelChanger = GetComponentInChildren<UpperLeftArmModelChanger>();
      upperRightArmModelChanger = GetComponentInChildren<UpperRightArmModelChanger>();
      lowerLeftArmModelChanger = GetComponentInChildren<LowerLeftArmModelChanger>();
      lowerRightArmModelChanger = GetComponentInChildren<LowerRightArmModelChanger>();
      leftHandModelChanger = GetComponentInChildren<LeftHandModelChanger>();
      rightHandModelChanger = GetComponentInChildren<RightHandModelChanger>();
    }

    private void Start()
    {
      EquipAllEquipmentModelsOnStart();
    }

    private void UnequipAllEquipmentModels()
    {
      helmetModelChanger.UnequipAllHelmetModels();
      torsoModelChanger.UnequipAllTorsoModels();
      hipModelChanger.UnequipAllHipModels();
      leftLegModelChanger.UnequipAllLegModels();
      rightLegModelChanger.UnequipAllLegModels();
      upperLeftArmModelChanger.UnequipAllModels();
      upperRightArmModelChanger.UnequipAllModels();
      lowerLeftArmModelChanger.UnequipAllModels();
      lowerRightArmModelChanger.UnequipAllModels();
      leftHandModelChanger.UnequipAllModels();
      rightHandModelChanger.UnequipAllModels();
    }
    private void EquipAllEquipmentModelsOnStart()
    {
      UnequipAllEquipmentModels();
      if (playerInverntory.currentHelmetEquipment != null)
      {
        nakedHeadModelGO.SetActive(false);
        helmetModelChanger.EquipHelmetModelByName(playerInverntory.currentHelmetEquipment.helmetModelName);
        playerStats.physicalDamageAbsorptionHead = playerInverntory.currentHelmetEquipment.physicalDefense;
      }
      else
      {
        helmetModelChanger.EquipHelmetModelByName(nakedHeadModel);
        nakedHeadModelGO.SetActive(true);
        playerStats.physicalDamageAbsorptionHead = 0;
      }

      if (playerInverntory.currentTorsoEquipment != null)
      {
        torsoModelChanger.EquipTorsoModelByName(playerInverntory.currentTorsoEquipment.torsoModelName);
        upperLeftArmModelChanger.EquipModelByName(playerInverntory.currentTorsoEquipment.upperLeftArmModelName);
        upperRightArmModelChanger.EquipModelByName(playerInverntory.currentTorsoEquipment.upperRightArmModelName);
        playerStats.physicalDamageAbsorptionBody = playerInverntory.currentTorsoEquipment.physicalDefense;
      }
      else
      {
        torsoModelChanger.EquipTorsoModelByName(nakedTorsoModel);
        upperLeftArmModelChanger.EquipModelByName(nakedUpperLeftArm);
        upperRightArmModelChanger.EquipModelByName(nakedUpperRightArm);
        playerStats.physicalDamageAbsorptionBody = 0;
      }

      if (playerInverntory.currentLegEquipment != null)
      {
        hipModelChanger.EquipHipModelByName(playerInverntory.currentLegEquipment.hipModelName);
        leftLegModelChanger.EquipLegModelByName(playerInverntory.currentLegEquipment.leftLegName);
        rightLegModelChanger.EquipLegModelByName(playerInverntory.currentLegEquipment.rightLegName);
        playerStats.physicalDamageAbsorptionLegs = playerInverntory.currentLegEquipment.physicalDefense;
      }
      else
      {
        hipModelChanger.EquipHipModelByName(nakedHipModel);
        leftLegModelChanger.EquipLegModelByName(nakedLeftLeg);
        rightLegModelChanger.EquipLegModelByName(nakedRightLeg);
        playerStats.physicalDamageAbsorptionLegs = 0;
      }

      if (playerInverntory.currentHandEquipment != null)
      {
        lowerLeftArmModelChanger.EquipModelByName(playerInverntory.currentHandEquipment.lowerLeftArmModelName);
        lowerRightArmModelChanger.EquipModelByName(playerInverntory.currentHandEquipment.lowerRightArmModelName);
        leftHandModelChanger.EquipModelByName(playerInverntory.currentHandEquipment.leftHandModelName);
        rightHandModelChanger.EquipModelByName(playerInverntory.currentHandEquipment.rightHandModelName);
        playerStats.physicalDamageAbsorptionHands = playerInverntory.currentHandEquipment.physicalDefense;
      }
      else
      {
        lowerLeftArmModelChanger.EquipModelByName(nakedLowerLeftArm);
        lowerRightArmModelChanger.EquipModelByName(nakedLowerRightArm);
        leftHandModelChanger.EquipModelByName(nakedLeftHand);
        rightHandModelChanger.EquipModelByName(nakedRightHand);
        playerStats.physicalDamageAbsorptionHands = 0;
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
