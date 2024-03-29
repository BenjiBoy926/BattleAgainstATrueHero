﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Hellmade.Sound;

public class UndertaleButton : MonoBehaviour, IPointerEnterHandler
{
    // Normal and highlight colors for an undertale button
    public readonly Color normalColor = new Color(1f, 0.47f, 0f);
    public readonly Color highlightColor = new Color(1f, 1f, 0f);
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

    private void Start()
    {
        heartGraphic.SetActive(false);
        // Button has no transition as it will be customized by this script
        button.transition = Selectable.Transition.None;
        // Play click sound when button pressed
        button.onClick.AddListener(() =>
        {
            EazySoundManager.PlayUISound(clickClip.Value);
        });
    }

    private void Update()
    {
        SetHoveredAppearance(GetTargetRect().Contains(Input.mousePosition));
    }

    public void OnPointerEnter(PointerEventData data)
    {
        // Play the hover audio
        EazySoundManager.PlayUISound(hoverClip.Value);
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

    private Rect GetTargetRect()
    {
        // Get the corners in world space
        Vector3[] worldCorners = new Vector3[4];
        button.targetGraphic.rectTransform.GetWorldCorners(worldCorners);
        // Bottom left is the second item in the corners list
        Vector2 pos = worldCorners[0];
        // Compute the size based on the x coordinate world positions
        Vector2 size = new Vector2(worldCorners[2].x - pos.x, worldCorners[1].y - pos.y);
        // Return the rect
        return new Rect(pos, size);
    }
}
