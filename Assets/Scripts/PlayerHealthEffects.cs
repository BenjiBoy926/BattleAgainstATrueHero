using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerHealthEffects : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Sound effect that plays when the player takes damage")]
    private AudioClip damageClip;

    // Reference to the audio source that plays effects for the player
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void TakeDamageEffect()
    {
        source.clip = damageClip;
        source.Play();

        // Heart fades in and out while invincible
    }
}
