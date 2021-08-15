using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class FightTag : MonoBehaviour
{
    [SerializeField]
    [TagSelector]
    [Tooltip("Tag of the enemy with the health that we will attack")]
    private string enemyTag;
    [SerializeField]
    [TagSelector]
    [Tooltip("Tag of the player to check for collision")]
    private string playerTag;
    [SerializeField]
    [Tooltip("Ending size of the tag after it has been picked up")]
    private float endingSizeScale;

    [Header("Colors")]

    [SerializeField]
    [Tooltip("Color of the tag when it originally appears")]
    private Color startColor;
    [SerializeField]
    [Tooltip("Color of the tag when it is collected")]
    private Color collectColor;
    [SerializeField]
    [Tooltip("Color of the tag when it can no longer be collected")]
    private Color deadColor;

    [Header("References")]

    [SerializeField]
    [Tooltip("Sprite that displays the tag")]
    private SpriteRenderer sprite;
    [SerializeField]
    [Tooltip("Slider that shows how long the fight tag has left")]
    private Slider slider;
    [SerializeField]
    [Tooltip("Reference to the image that displays the slider fill")]
    private Image sliderFill;
    [SerializeField]
    [Tooltip("Reference the to box collider on the tag")]
    private new BoxCollider2D collider2D;
    [SerializeField]
    [Tooltip("Plays the audio effects")]
    private new AudioSource audio;
    [SerializeField]
    [Tooltip("Sound that plays when the tag is picked up")]
    private AudioClip collectClip;

    [Header("Timing")]

    [SerializeField]
    [Tooltip("Time that passes before the fight tag disappears")]
    private float lifetime;
    [SerializeField]
    [Tooltip("Time it takes for the tag to fade away")]
    private float fadeOutTime;
    [SerializeField]
    [Tooltip("Time after collecting that the enemy takes damage")]
    private float collectTime;

    private Vector3 startSize;
    private Coroutine lifetimeRoutine;

    private void Start()
    {
        startSize = sprite.transform.localScale;
        sliderFill.color = new Color(startColor.r, startColor.g, startColor.b, 0.3f);

        SetColor(startColor);
        lifetimeRoutine = StartCoroutine(LifetimeRoutine());
    }

    // Continuously change the slider value until it is time to destroy the object
    private IEnumerator LifetimeRoutine()
    {
        UnityAction<float> updateSlider = t =>
        {
            slider.value = Mathf.Lerp(1f, 0f, t);
        };
        yield return CoroutineModule.LerpForTime(lifetime, updateSlider);

        collider2D.enabled = false;

        // Fade the tag away
        yield return ColorModule.FadeOut(deadColor, fadeOutTime, SetColor);

        // Destroy the tag after it is faded
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTagInParent(playerTag))
        {
            StartCoroutine(CollectRoutine());
        }
    }

    private IEnumerator CollectRoutine()
    {
        // Play the collect sound!
        audio.clip = collectClip;
        audio.Play();

        // Stop the lifetime routine since we will no longer destroy ourselves over time
        StopCoroutine(lifetimeRoutine);

        // Disable the slider object so that we do not see it anymore
        slider.gameObject.SetActive(false);
        // Disable collider so that player cannot re-enter after collecting
        collider2D.enabled = false;

        // Start async process to make the color fade away
        StartCoroutine(ColorModule.FadeOut(collectColor, collectTime, SetColor));

        // Update the size in sync with the algorithm
        UnityAction<float> updateSize = t =>
        {
            sprite.transform.localScale = Vector3.Lerp(startSize, startSize * endingSizeScale, t);
        };
        yield return CoroutineModule.LerpForTime(collectTime, updateSize);

        // Deal damage to the enemy health on the enemy object
        GameObject enemy = GameObject.FindGameObjectWithTag(enemyTag);
        MonsterHealth enemyHealth = enemy.GetComponent<MonsterHealth>();
        enemyHealth.TakeDamage();

        // Destroy the tag
        Destroy(gameObject);
    }

    private void SetColor(Color color)
    {
        sprite.color = color;
    }
}
