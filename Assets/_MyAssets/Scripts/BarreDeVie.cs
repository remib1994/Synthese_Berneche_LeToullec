using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarreDeVie : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image jauge;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        jauge.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;

        jauge.color = gradient.Evaluate(slider.normalizedValue);
    }
}
