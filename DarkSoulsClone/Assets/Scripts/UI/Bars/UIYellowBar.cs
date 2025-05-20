using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MR
{

public class UIYellowBar : MonoBehaviour
  {
    public Slider slider;
    UIAICharacterHealthBar parentHealthBar;

   public float timer;

    private void Awake()
    {
      slider = GetComponent<Slider>();
      parentHealthBar = GetComponentInParent<UIAICharacterHealthBar>();
    }

    private void OnEnable()
    {
      if (timer <= 0)
      {
        timer = 2f;  // HOW LONG YOU WANT THE BAR TO BE PRE
      }
    }

    public void SetMaxStat(int maxStat)
    {
      slider.maxValue = maxStat;
      slider.value = maxStat;
    }

    private void Update()
    {
      if (timer <= 0)
      {
        if (slider.value > parentHealthBar.slider.value)
        {
          slider.value = slider.value - 3f;
        }
        else if (slider.value <= parentHealthBar.slider.value)
        {
          gameObject.SetActive(false);
        }
      }
      else
      {
        timer = timer - Time.deltaTime;
      }
    }
  }
}
