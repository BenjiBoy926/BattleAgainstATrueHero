using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The slider used to display the player's health")]
    private Slider healthSlider;
    [SerializeField]
    [Tooltip("Reference to the text that displays the player's current health")]
    private Text healthText;

    private void Awake()
    {
        healthSlider.wholeNumbers = true;
    }

    public void Setup(int max)
    {
        // Setup the slider to have the new max value
        healthSlider.minValue = 0;
        healthSlider.maxValue = max;
        healthSlider.value = max;

        // Have the text display the values
        healthText.text = max + "/" + max;

        // Enable ui on setup
        SetUIActive(true);
    }

    public void UpdateUI(int newHealth)
    {
        // Update health slider value
        healthSlider.value = newHealth;
        healthText.text = newHealth + "/" + healthSlider.maxValue;
    }

    public void SetUIActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
