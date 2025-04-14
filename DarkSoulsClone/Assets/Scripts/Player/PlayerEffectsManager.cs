using UnityEngine;
namespace MR
{
  public class PlayerEffectsManager : CharacterEffectsManager
  {
    PlayerStatsManager playerStatsManager;
    PlayerWeaponSlotManager playerWeaponSlotManager;

    PoisonBuildUpBar poisonBuildUpBar;
    PoisonAmountBar poisonAmountBar;
    public GameObject currentParticleFX, instantiatedFXModel;
    public int amountToBeHealed;

    protected override void Awake()
    {
      base.Awake();
      playerStatsManager = GetComponentInParent<PlayerStatsManager>();
      playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
      poisonBuildUpBar = FindObjectOfType<PoisonBuildUpBar>();
      poisonAmountBar = FindObjectOfType<PoisonAmountBar>();
    }

    public void HealPlayerFromEffect()
    {
      playerStatsManager.HealPlayer(amountToBeHealed);
      GameObject healParticles = Instantiate(currentParticleFX, playerStatsManager.transform);
      Destroy(instantiatedFXModel.gameObject);
      playerWeaponSlotManager.LoadBothWeaponsOnSlots();
    }

    protected override void HandlePoisonBuildUp()
    {
      if (poisonBuildup <= 0)
      {
        poisonBuildUpBar.gameObject.SetActive(false);
      }else {
        poisonBuildUpBar.gameObject.SetActive(true);
      }
      base.HandlePoisonBuildUp();
      poisonBuildUpBar.SetPoisonBuildUpAmount(Mathf.RoundToInt(poisonBuildup));
    }
    protected override void HandleIsPoisonedEffect()
    {
      if (!isPoisoned)
      {
        poisonAmountBar.gameObject.SetActive(false);
      }else{
        poisonAmountBar.gameObject.SetActive(true);
      }
      base.HandleIsPoisonedEffect();
      poisonAmountBar.SetPoisonAmount(Mathf.RoundToInt(poisonAmount));
    }
  }
}
