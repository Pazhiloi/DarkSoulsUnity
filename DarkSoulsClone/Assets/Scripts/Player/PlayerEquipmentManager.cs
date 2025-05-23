using UnityEngine;
namespace MR
{
  public class PlayerEquipmentManager : MonoBehaviour
  {

    PlayerManager player;
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

    private void Awake()
    {
      player = GetComponent<PlayerManager>();

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
    public void EquipAllEquipmentModelsOnStart()
    {
      float poisonResistance = 0;
      UnequipAllEquipmentModels();

      if (player.playerInventoryManager.currentHelmetEquipment != null)
      {
        nakedHeadModelGO.SetActive(false);
        helmetModelChanger.EquipHelmetModelByName(player.playerInventoryManager.currentHelmetEquipment.helmetModelName);
        player.playerStatsManager.physicalDamageAbsorptionHead = player.playerInventoryManager.currentHelmetEquipment.physicalDefense;
        poisonResistance += player.playerInventoryManager.currentHelmetEquipment.poisonResistance;
      }
      else
      {
        helmetModelChanger.EquipHelmetModelByName(nakedHeadModel);
        nakedHeadModelGO.SetActive(true);
        player.playerStatsManager.physicalDamageAbsorptionHead = 0;
      }

      if (player.playerInventoryManager.currentBodyEquipment != null)
      {
        torsoModelChanger.EquipTorsoModelByName(player.playerInventoryManager.currentBodyEquipment.torsoModelName);
        upperLeftArmModelChanger.EquipModelByName(player.playerInventoryManager.currentBodyEquipment.upperLeftArmModelName);
        upperRightArmModelChanger.EquipModelByName(player.playerInventoryManager.currentBodyEquipment.upperRightArmModelName);
        player.playerStatsManager.physicalDamageAbsorptionBody = player.playerInventoryManager.currentBodyEquipment.physicalDefense;
        poisonResistance += player.playerInventoryManager.currentBodyEquipment.poisonResistance;
      }
      else
      {
        torsoModelChanger.EquipTorsoModelByName(nakedTorsoModel);
        upperLeftArmModelChanger.EquipModelByName(nakedUpperLeftArm);
        upperRightArmModelChanger.EquipModelByName(nakedUpperRightArm);
        player.playerStatsManager.physicalDamageAbsorptionBody = 0;
      }

      if (player.playerInventoryManager.currentLegEquipment != null)
      {
        hipModelChanger.EquipHipModelByName(player.playerInventoryManager.currentLegEquipment.hipModelName);
        leftLegModelChanger.EquipLegModelByName(player.playerInventoryManager.currentLegEquipment.leftLegName);
        rightLegModelChanger.EquipLegModelByName(player.playerInventoryManager.currentLegEquipment.rightLegName);
        player.playerStatsManager.physicalDamageAbsorptionLegs = player.playerInventoryManager.currentLegEquipment.physicalDefense;
        poisonResistance += player.playerInventoryManager.currentLegEquipment.poisonResistance;
      }
      else
      {
        hipModelChanger.EquipHipModelByName(nakedHipModel);
        leftLegModelChanger.EquipLegModelByName(nakedLeftLeg);
        rightLegModelChanger.EquipLegModelByName(nakedRightLeg);
        player.playerStatsManager.physicalDamageAbsorptionLegs = 0;
      }

      if (player.playerInventoryManager.currentHandEquipment != null)
      {
        lowerLeftArmModelChanger.EquipModelByName(player.playerInventoryManager.currentHandEquipment.lowerLeftArmModelName);
        lowerRightArmModelChanger.EquipModelByName(player.playerInventoryManager.currentHandEquipment.lowerRightArmModelName);
        leftHandModelChanger.EquipModelByName(player.playerInventoryManager.currentHandEquipment.leftHandModelName);
        rightHandModelChanger.EquipModelByName(player.playerInventoryManager.currentHandEquipment.rightHandModelName);
        player.playerStatsManager.physicalDamageAbsorptionHands = player.playerInventoryManager.currentHandEquipment.physicalDefense;
        poisonResistance += player.playerInventoryManager.currentHandEquipment.poisonResistance;
      }
      else
      {
        lowerLeftArmModelChanger.EquipModelByName(nakedLowerLeftArm);
        lowerRightArmModelChanger.EquipModelByName(nakedLowerRightArm);
        leftHandModelChanger.EquipModelByName(nakedLeftHand);
        rightHandModelChanger.EquipModelByName(nakedRightHand);
        player.playerStatsManager.physicalDamageAbsorptionHands = 0;
      }

      player.playerStatsManager.poisonResistance = poisonResistance;


    }

  }
}