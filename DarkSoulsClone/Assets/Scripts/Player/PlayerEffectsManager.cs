using UnityEngine;
namespace SG
{
  public class PlayerEffectsManager : CharacterEffectsManager
  {
    PlayerStatsManager playerStatsManager;
    PlayerWeaponSlotManager playerWeaponSlotManager;
    public GameObject currentParticleFX, instantiatedFXModel;
    public int amountToBeHealed;

    private void Awake()
    {
      playerStatsManager = GetComponentInParent<PlayerStatsManager>();
      playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
    }

    public void HealPlayerFromEffect()
    {
      playerStatsManager.HealPlayer(amountToBeHealed);
      GameObject healParticles = Instantiate(currentParticleFX, playerStatsManager.transform);
      Destroy(instantiatedFXModel.gameObject);
      playerWeaponSlotManager.LoadBothWeaponsOnSlots();
    }
  }
}
