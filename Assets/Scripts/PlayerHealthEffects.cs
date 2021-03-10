using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerHealthEffects : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Sound effect that plays when the player takes damage")]
    private AudioClip damageClip;
    [SerializeField]
    [Tooltip("Time it takes for the player to fade in and out while invincible")]
    private float fadeTime;

    // Reference to the audio source that plays effects for the player
    private AudioSource source;
    private new SpriteRenderer renderer;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        renderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamageEffect(float invincibleTime)
    {
        source.clip = damageClip;
        source.Play();

        // Start the fading in and out coroutine
        StartCoroutine(Flash(invincibleTime));
    }

    private IEnumerator Flash(float invincibleTime)
    {
        yield return ColorModule.Flicker(Color.white, Color.clear, invincibleTime, fadeTime, SetRendererColor);
        SetRendererColor(Color.white);
    }

    private void SetRendererColor(Color color)
    {
        renderer.color = color;
    }
}
