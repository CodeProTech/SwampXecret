using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_Bar : MonoBehaviour
{
    [SerializeField] private Slider slider;  // Tippfehler korrigiert: SerilizeField -> SerializeField

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        if (slider != null)
        {
            slider.value = currentValue / maxValue;
        }
        else
        {
            Debug.LogError("Slider is not assigned in the Health_Bar script.");
        }
    }


    // Update is called once per frame
    void Update()
    {
        // Kein Code notwendig in Update, da die HealthBar nur aktualisiert werden muss, wenn sich die Werte ändern.
    }
}

