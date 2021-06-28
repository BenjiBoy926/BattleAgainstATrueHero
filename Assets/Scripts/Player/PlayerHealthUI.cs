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
    [SerializeField]
    [Tooltip("Color of the healthbar in its default state")]
    private Color defaultSliderColor = Color.yellow;
    [SerializeField]
    [Tooltip("Color of the healthbar while unbreakable mode is active")]
    private Color unbreakableSliderColor = Color.green;
    [SerializeField]
    [Tooltip("Information about the unbreakable mode UI")]
    private PlayerUnbreakableModeUI unbreakableUI;

    private Graphic healthSliderFill => healthSlider.fillRect.GetComponent<Graphic>();

    private void Awake()
    {
        healthSlider.wholeNumbers = true;
        SetSliderColor();
    }

    public void Setup(int max)
    {
        // Setup unbreakable ui
        unbreakableUI.Setup();

        // Setup the slider to have the new max value
        healthSlider.minValue = 0;
        healthSlider.maxValue = max;
        healthSlider.value = max;

        // Have the text display the values
        healthText.text = max + "/" + max;
    }

    public void UpdateUI(int newHealth)
    {
        // Update health slider value
        healthSlider.value = newHealth;
        healthText.text = newHealth + "/" + healthSlider.maxValue;
        // Always update the unbreakable ui with this ui
        unbreakableUI.UpdateUI();
    }
    public void UnbreakableModeTriggerEffect(int newHealth)
    {
        UpdateUI(newHealth);
        unbreakableUI.UnbreakableModeTriggerEffect();
    }
    public void ToggleUnbreakableModeUI(bool active)
    {
        SetSliderColor();
        unbreakableUI.ToggleUnbreakableModeUI(active);
    }

    private void SetSliderColor()
    {
        if (PlayerHealth.unbreakable) healthSliderFill.color = unbreakableSliderColor;
        else healthSliderFill.color = defaultSliderColor;
    }
}
