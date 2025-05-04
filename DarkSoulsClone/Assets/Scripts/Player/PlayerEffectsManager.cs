using UnityEngine;
namespace MR
{
  public class PlayerEffectsManager : CharacterEffectsManager
  {
    PlayerManager player;

    PoisonBuildUpBar poisonBuildUpBar;
    PoisonAmountBar poisonAmountBar;
    public GameObject currentParticleFX, instantiatedFXModel;
    public int amountToBeHealed;

    protected override void Awake()
    {
      base.Awake();
      player = GetComponent<PlayerManager>();
      poisonBuildUpBar = FindObjectOfType<PoisonBuildUpBar>();
      poisonAmountBar = FindObjectOfType<PoisonAmountBar>();
    }

    public void HealPlayerFromEffect()
    {
      player.playerStatsManager.HealCharacter(amountToBeHealed);
      GameObject healParticles = Instantiate(currentParticleFX, player.playerStatsManager.transform);
      Destroy(instantiatedFXModel.gameObject);
      player.playerWeaponSlotManager.LoadBothWeaponsOnSlots();
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
