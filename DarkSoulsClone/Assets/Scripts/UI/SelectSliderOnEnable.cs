using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MR
{
    public class SelectSliderOnEnable : MonoBehaviour
    {
       public Slider statSlider; // The slider to select when the UI is enabled

        private void OnEnable()
        {
                statSlider.Select();
                statSlider.OnSelect(null);
        }
    }
}
