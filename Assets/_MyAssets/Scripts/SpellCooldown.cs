using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SpellCooldown : MonoBehaviour
{
    [SerializeField] private Image imageCooldown;
    [SerializeField] private TMP_Text textCooldown;
    [SerializeField] private string _touche;

    private bool isCoolDown = false;
    [SerializeField] private float cooldownTime = 10.0f;
    private float cooldownTimer = 0.0f;

    void Start()
    {
        textCooldown.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0.0f;
    }

    void Update()
    {
        KeyCode key;
        if (Enum.TryParse(_touche, out key))
        {
            if (Input.GetKeyDown(key))
            {
                UseSpell();
            }
        }
        else
        {
            Debug.LogError("Invalid key: " + _touche);
        }

        if (isCoolDown)
        {
            ApplyCooldown();
        }
    }

    void ApplyCooldown()
    {
        cooldownTimer -= Time.deltaTime;
        if(cooldownTimer < 0.0f)
        {
            isCoolDown = false;
            textCooldown.gameObject.SetActive(false);
            imageCooldown.fillAmount = 0.0f;
        }
        else
        {
            textCooldown.text = Mathf.RoundToInt(cooldownTimer).ToString();
            imageCooldown.fillAmount = cooldownTimer / cooldownTime;
        }

    }

    public bool UseSpell()
    {
        if(isCoolDown)
        {
            return false;
        }
        else
        {
            isCoolDown = true;
            textCooldown.gameObject.SetActive(true);
            cooldownTimer = cooldownTime;
            textCooldown.text = Mathf.RoundToInt(cooldownTimer).ToString();
            imageCooldown.fillAmount = 1.0f;

            return true; 
        }
    }
}
