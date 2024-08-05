using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{
  public class PlayerEffectsManager : MonoBehaviour
  {
    PlayerStats playerStats;
    WeaponSlotManager weaponSlotManager;
    public GameObject currentParticleFX, instantiatedFXModel;
    public int amountToBeHealed;

    private void Awake()
    {
      playerStats = GetComponentInParent<PlayerStats>();
      weaponSlotManager = GetComponent<WeaponSlotManager>();
    }

    public void HealPlayerFromEffect()
    {
      playerStats.HealPlayer(amountToBeHealed);
      GameObject healParticles = Instantiate(currentParticleFX, playerStats.transform);
      Destroy(instantiatedFXModel.gameObject);
      weaponSlotManager.LoadBothWeaponsOnSlots();
    }
  }
}
