using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Hellmade.Sound;

public class UndertaleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Normal and highlight colors for an undertale button
    public readonly Color normalColor = new(1f, 0.47f, 0f);
    public readonly Color highlightColor = new(1f, 1f, 0f);
    // Audio clips played for all buttons
    public readonly Lazy<AudioClip> hoverClip = new Lazy<AudioClip>(() => Resources.Load<AudioClip>("Audio/Select"));
    public readonly Lazy<AudioClip> clickClip = new Lazy<AudioClip>(() => Resources.Load<AudioClip>("Audio/Click"));

    [SerializeField]
    [Tooltip("Reference to the main button")]
    private Button button;
    [SerializeField]
    [Tooltip("Audio source that will play the button audio")]
    private new AudioSource audio;
    [SerializeField]
    [Tooltip("Parent of the object with the default graphics for the button")]
    private GameObject defaultGraphic;
    [SerializeField]
    [Tooltip("Parent of the object that displays the heart while button is hovered")]
    private GameObject heartGraphic;
    [SerializeField]
    [Tooltip("List of all graphics in the button to change color")]
    private List<Graphic> graphics;

    private void OnEnable()
    {
        SetHoveredAppearance(EventSystem.current.currentSelectedGameObject == gameObject);
    }
    private void OnDisable()
    {
        SetHoveredAppearance(false);
    }

    private void Start()
    {
        // Button has no transition as it will be customized by this script
        button.transition = Selectable.Transition.None;
        // Play click sound when button pressed
        button.onClick.AddListener(() =>
        {
            EazySoundManager.PlayUISound(clickClip.Value);
        });
    }

    public void OnPointerEnter(PointerEventData data)
    {
        // Play the hover audio
        EazySoundManager.PlayUISound(hoverClip.Value);
        SetHoveredAppearance(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetHoveredAppearance(false);
    }

    // Set if the button is hovered or not using colors and swapping graphics
    private void SetHoveredAppearance(bool hovered)
    {
        defaultGraphic.SetActive(!hovered);
        heartGraphic.SetActive(hovered);

        if (hovered) SetColor(highlightColor);
        else SetColor(normalColor);
    }

    private void SetColor(Color color)
    {
        foreach (Graphic g in graphics)
        {
            g.color = color;
        }
    }
}
