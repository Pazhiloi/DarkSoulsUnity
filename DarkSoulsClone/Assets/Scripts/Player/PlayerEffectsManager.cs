using UnityEngine;
namespace MR
{
  public class PlayerEffectsManager : CharacterEffectsManager
  {
    PlayerManager player;

    PoisonBuildUpBar poisonBuildUpBar;
    public PoisonAmountBar poisonAmountBar;
    public GameObject currentParticleFX;
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

    protected override void ProcessBuildUpDecay()
    {
      if (player.characterStatsManager.poisonBuildUp >= 0)
      {
        player.characterStatsManager.poisonBuildUp -= 1;

        poisonBuildUpBar.gameObject.SetActive(true);
        poisonBuildUpBar.SetPoisonBuildUpAmount(Mathf.RoundToInt(player.characterStatsManager.poisonBuildUp));
      }
    }
  }
}
