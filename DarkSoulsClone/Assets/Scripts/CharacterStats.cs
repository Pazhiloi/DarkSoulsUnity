using UnityEngine;
namespace SG
{

  public class CharacterStats : MonoBehaviour
  {
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    public int staminaLevel = 10;
    public float maxStamina, currentStamina;

    public int focusLevel = 10;
    public float maxFocusPoints;
    public float currentFocusPoints;
    public int soulCount = 0;

    [Header("Poise")]
    public float totalPoiseDefence, offensivePoiseBonus, armorPoiseBonus;
    public float totalPoiseResetTime = 15;
    public float poiseResetTimer = 0;

    [Header("Armor Absorptions")]
    public float physicalDamageAbsorptionHead;
    public float physicalDamageAbsorptionBody, physicalDamageAbsorptionLegs, physicalDamageAbsorptionHands;

    public bool isDead;

    protected virtual void Update(){
      HandlePoiseResetTimer();
    }

    private void Start() {
      totalPoiseDefence = armorPoiseBonus;
    }


    public virtual void TakeDamage(int physicalDamage, string damageAnimation = "Damage_01")
    {
      if (isDead) return;

      float totalPhysicalDamageAbsorption = 1 - (1 - physicalDamageAbsorptionHead / 100) *
                                                (1 - physicalDamageAbsorptionBody / 100) *
                                                (1 - physicalDamageAbsorptionLegs / 100) *
                                                (1 - physicalDamageAbsorptionHands / 100);

      physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));

      Debug.Log("Total Damage Absorption is: " + totalPhysicalDamageAbsorption + "%");

      float finalDamage = physicalDamage;

      currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

      Debug.Log("Final Damage Dealt is: " + finalDamage);

      if (currentHealth <= 0)
      {
        currentHealth = 0;
        isDead = true;
      }
    }

    public virtual void HandlePoiseResetTimer(){
      if(poiseResetTimer > 0){
        poiseResetTimer -= Time.deltaTime;
      }
      else{
        totalPoiseDefence = armorPoiseBonus;
    }
  }
}
}
