using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

[System.Serializable]
public class UnbreakableModeControls
{
    [SerializeField]
    [Tooltip("Toggle that enables/disables unbreakable mode")]
    private Toggle toggle;
    [SerializeField]
    [Tooltip("Sound that plays whenever the value changes")]
    private AudioClip valueChangedSound;
    [SerializeField]
    [Tooltip("Graphic for the outline of the box")]
    private Image outline;
    [SerializeField]
    [Tooltip("Graphic for the inside of the box")]
    private Image checkmark;
    [SerializeField]
    [Tooltip("Toggle text")]
    private TextMeshProUGUI toggleText;
    [SerializeField]
    [Tooltip("Color of the graphics while checkmark is off")]
    private Color offColor;
    [SerializeField]
    [Tooltip("Color of the graphic while checkmark is on")]
    private Color onColor;

    [SerializeField]
    [Tooltip("Manages the head of Chara that appears when unbreakable mode is toggled on")]
    private UnbreakableModeCharaEffect charaEffect;

    // Audio source we use to play the audio
    private AudioSource audio;

    public void Start(AudioSource audio)
    {
        // Audio source from the parent script
        this.audio = audio;

        // Checkmark is only visible while on, so just set the color here at the start
        checkmark.color = onColor;
        // Start the outline as the off color
        outline.color = offColor;
        toggleText.color = offColor;

        // Setup toggle callback
        toggle.onValueChanged.AddListener(ToggleUnbreakableMode);
        toggle.isOn = PlayerHealth.unbreakable;

        charaEffect.Start();
    }

    private void ToggleUnbreakableMode(bool active)
    {
        // Play the value change sound
        audio.clip = valueChangedSound;
        audio.Play();

        // Set player to be unbreakable
        PlayerHealth.unbreakable = active;

        if (active) SetColor(onColor);
        else SetColor(offColor);

        // Activate Chara!
        charaEffect.SetActive(active);
    }

    private void SetColor(Color color)
    {
        outline.color = color;
        toggleText.color = color;
    }
}
