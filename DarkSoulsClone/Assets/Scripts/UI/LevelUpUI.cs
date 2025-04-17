using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MR
{
  public class LevelUpUI : MonoBehaviour
  {
    public PlayerStatsManager playerStatsManager;
    [Header("Player Level")]
    public int currentPlayerLevel; // THE CURRENT LEVEL WE ARE BEFORE LEVELING UP
    public int projectedPlayerLevel; // THE POSSIBLE LEVEL WE WILL BE IF WE ACCEPT LEVELING UP
    public Text currentPlayerLevelText; // THE UI TEXT FOR THE NUMBER OF THE CURRENT PLAYER LEVEL
    public Text projectedPlayerLevelText; // THE UI TEXT FOR THE PROJECTED PLAYER LEVEL NUMBER

    [Header("Souls")]
    public Text currentSouls;
    public Text soulsRequiredToLevelUp;

    [Header("Health")]
    public Slider healthSlider;
    public Text currentHealthLevelText;
    public Text projectedHealthLevelText;

    [Header("Stamina")]
    public Slider staminaSlider;
    public Text currentStaminaLevelText;
    public Text projectedStaminaLevelText;

    [Header("Focus")]
    public Slider focusSlider;
    public Text currentFocusLevelText;
    public Text projectedFocusLevelText;

    [Header("Poise")]
    public Slider poiseSlider;
    public Text currentPoiseLevelText;
    public Text projectedPoiseLevelText;

    [Header("Strength")]
    public Slider strengthSlider;
    public Text currentStrengthLevelText;
    public Text projectedStrengthLevelText;

    [Header("Dexterity")]
    public Slider dexteritySlider;
    public Text currentDexterityLevelText;
    public Text projectedDexterityLevelText;

    [Header("Faith")]
    public Slider faithSlider;
    public Text currentFaithLevelText;
    public Text projectedFaithLevelText;

    [Header("Intelligence")]
    public Slider intelligenceSlider;
    public Text currentIntelligenceLevelText;
    public Text projectedIntelligenceLevelText;

    private void OnEnable()
    {
      currentPlayerLevel = playerStatsManager.playerLevel;
      currentPlayerLevelText.text = currentPlayerLevel.ToString();

      projectedPlayerLevel = playerStatsManager.playerLevel;
      projectedPlayerLevelText.text = projectedPlayerLevel.ToString();

      healthSlider.value = playerStatsManager.healthLevel;
      healthSlider.minValue = playerStatsManager.healthLevel;
      healthSlider.maxValue = 99; 
      currentHealthLevelText.text = playerStatsManager.healthLevel.ToString();
      projectedHealthLevelText.text = playerStatsManager.healthLevel.ToString();

      staminaSlider.value = playerStatsManager.staminaLevel;
      staminaSlider.minValue = playerStatsManager.staminaLevel;
      staminaSlider.maxValue = 99;
      currentStaminaLevelText.text = playerStatsManager.staminaLevel.ToString();
      projectedStaminaLevelText.text = playerStatsManager.staminaLevel.ToString();

      focusSlider.value = playerStatsManager.focusLevel;
      focusSlider.minValue = playerStatsManager.focusLevel;
      focusSlider.maxValue = 99;
      currentFocusLevelText.text = playerStatsManager.focusLevel.ToString();
      projectedFocusLevelText.text = playerStatsManager.focusLevel.ToString();

      poiseSlider.value = playerStatsManager.poiseLevel;
      poiseSlider.minValue = playerStatsManager.poiseLevel;
      poiseSlider.maxValue = 99;
      currentPoiseLevelText.text = playerStatsManager.poiseLevel.ToString();
      projectedPoiseLevelText.text = playerStatsManager.poiseLevel.ToString();

      strengthSlider.value = playerStatsManager.strengthLevel;
      strengthSlider.minValue = playerStatsManager.strengthLevel;
      strengthSlider.maxValue = 99;
      currentStrengthLevelText.text = playerStatsManager.strengthLevel.ToString();
      projectedStrengthLevelText.text = playerStatsManager.strengthLevel.ToString();

      dexteritySlider.value = playerStatsManager.dexterityLevel;
      dexteritySlider.minValue = playerStatsManager.dexterityLevel;
      dexteritySlider.maxValue = 99;
      currentDexterityLevelText.text = playerStatsManager.dexterityLevel.ToString();
      projectedDexterityLevelText.text = playerStatsManager.dexterityLevel.ToString();

      intelligenceSlider.value = playerStatsManager.intelligenceLevel;
      intelligenceSlider.minValue = playerStatsManager.intelligenceLevel;
      intelligenceSlider.maxValue = 99;
      currentIntelligenceLevelText.text = playerStatsManager.intelligenceLevel.ToString();
      projectedIntelligenceLevelText.text = playerStatsManager.intelligenceLevel.ToString();

      faithSlider.value = playerStatsManager.faithLevel;
      faithSlider.minValue = playerStatsManager.faithLevel;
      faithSlider.maxValue = 99;
      currentFaithLevelText.text = playerStatsManager.faithLevel.ToString();
      projectedFaithLevelText.text = playerStatsManager.faithLevel.ToString();
    }


    private void UpdateProjectedPlayerLevel()
    {
      projectedPlayerLevel = currentPlayerLevel;
      projectedPlayerLevel += Mathf.RoundToInt(healthSlider.value) - playerStatsManager.healthLevel;
      projectedPlayerLevel += Mathf.RoundToInt(staminaSlider.value) - playerStatsManager.staminaLevel;
      projectedPlayerLevel += Mathf.RoundToInt(focusSlider.value) - playerStatsManager.focusLevel;
      projectedPlayerLevel += Mathf.RoundToInt(poiseSlider.value) - playerStatsManager.poiseLevel;
      projectedPlayerLevel += Mathf.RoundToInt(strengthSlider.value) - playerStatsManager.strengthLevel;
      projectedPlayerLevel += Mathf.RoundToInt(dexteritySlider.value) - playerStatsManager.dexterityLevel;
      projectedPlayerLevel += Mathf.RoundToInt(intelligenceSlider.value) - playerStatsManager.intelligenceLevel;
      projectedPlayerLevel += Mathf.RoundToInt(faithSlider.value) - playerStatsManager.faithLevel;

      projectedPlayerLevelText.text = projectedPlayerLevel.ToString();
    }

    public void UpdateHealthLevelSlider()
    {
      projectedHealthLevelText.text = healthSlider.value.ToString();
      UpdateProjectedPlayerLevel();
    }
  }
}