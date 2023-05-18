using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarreDeVie : MonoBehaviour
{
    public Slider _slider;
    public Gradient _gradient;
    public Image _jauge;

    public void SetMaxHealth(int health)
    {
        _slider.maxValue = health;
        _slider.value = health;

        _jauge.color = _gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        _slider.value = health;

        _jauge.color = _gradient.Evaluate(_slider.normalizedValue);
    }
}
