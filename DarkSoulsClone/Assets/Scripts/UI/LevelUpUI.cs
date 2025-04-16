using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MR
{
  public class LevelUpUI : MonoBehaviour
  {
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
  }
}