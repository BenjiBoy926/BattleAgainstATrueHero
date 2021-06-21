using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerInvincibilityUI : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Slider that affects the UI wheel")]
    private Slider slider;
    [SerializeField]
    [Tooltip("Image that displays invincibility")]
    private Image image;
    [SerializeField]
    [Tooltip("Time for which the deflect visual effect is active")]
    private float deflectTime;

    // Base color of the invincibility gauge
    private Color baseImageColor;
    // Stores the routine
    private Coroutine deflectRoutine;

    private void Awake()
    { 
        slider.value = 0f;
        baseImageColor = image.color;
        image.enabled = false;
    }

    public void Activate()
    {
        slider.value = 1f;
        image.enabled = true;
        image.color = baseImageColor;
    }

    public void Recharge(float rechargeTime)
    {
        StopDeflect();

        // This function updates the slider
        UnityAction<float> updateSlider = t =>
        {
            slider.value = Mathf.Lerp(1f, 0f, t);
        };
        StartCoroutine(CoroutineModule.LerpForTime(rechargeTime, updateSlider));

        // Make the image flash while we are not invincible
        UnityAction<Color> updateSliderColor = color =>
        {
            image.color = color;
        };
        // Store a faded version of the base color
        Color fadedColor = new Color(baseImageColor.r, baseImageColor.g, baseImageColor.b, 0.2f);
        // Flicker the slider color
        StartCoroutine(ColorModule.Flicker(baseImageColor, fadedColor, rechargeTime, 0.5f, updateSliderColor));
    }

    public void StartDeflect()
    {
        StopDeflect();
        deflectRoutine = StartCoroutine(DeflectRoutine());
    }

    private void StopDeflect()
    {
        if(deflectRoutine != null)
        {
            StopCoroutine(deflectRoutine);
        }
        image.color = baseImageColor;
    }

    private IEnumerator DeflectRoutine()
    {
        image.color = Color.white;
        yield return new WaitForSeconds(deflectTime);
        image.color = baseImageColor;
    }

    public void Ready()
    {
        image.enabled = false;
    }
}
