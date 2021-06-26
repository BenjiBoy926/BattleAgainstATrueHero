using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

[System.Serializable]
public class PlayerUnbreakableModeUI
{
    [SerializeField]
    [Tooltip("Displays the red X over Chara's head while unbreakable is not active")]
    private GameObject redX;
    [SerializeField]
    [Tooltip("Image that displays chara's head")]
    private Image charaHead;
    [SerializeField]
    [Tooltip("Text that displays the unbreakable trigger counter")]
    private TextMeshProUGUI text;

    public void Setup()
    {
        ToggleUnbreakableModeUI(PlayerHealth.unbreakable);
        UpdateUI();
    }

    public void ToggleUnbreakableModeUI(bool active)
    {
        redX.SetActive(!active);

        // When unbreakable is active, image and text are opaque
        if(active)
        {
            charaHead.color = new Color(charaHead.color.r, charaHead.color.g, charaHead.color.b, 1f);
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
        }
        // When unbreakable is not active, image and text are dim
        else
        {
            charaHead.color = new Color(charaHead.color.r, charaHead.color.g, charaHead.color.b, 0.1f);
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0.1f);
        }
    }

    public void UpdateUI()
    {
        text.text = PlayerHealth.unbreakableTriggerCounter.ToString();
    }
}
