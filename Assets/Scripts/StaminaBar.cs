using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StaminaBar : MonoBehaviour
{
    public Slider slider;
    public void SetMaxStamina(int MaxStamina){
        slider.maxValue = MaxStamina;
        slider.value = MaxStamina;
    }
    public void SetCurrentStamina(int currentStamina){
        slider.value = currentStamina;
    }
}
