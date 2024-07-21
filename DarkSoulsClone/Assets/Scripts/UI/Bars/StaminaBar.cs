using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SG
{
  public class StaminaBar : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
      slider = GetComponent<Slider>();
    }
    public void SetMaxStamina(float maxStamina)
    {
      slider.maxValue = maxStamina;
      slider.value = maxStamina;
    }

    // function which sets the slider display/visibility to the Stamina amount
    public void SetCurrentStamina(float currentStamina)
    {
      slider.value = currentStamina;
    }
    
  }
}
