using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

using TMPro;

using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerHealthEffects : MonoBehaviour
{
    [SerializeField]
    [TagSelector]
    [Tooltip("Tag of the object that acts as the player's decoy so the player looks like they are in front of the black overlay")]
    private string decoyTag;
    [SerializeField]
    [Tooltip("Reference to the script that manages the player UI")]
    private PlayerHealthUI healthUi;
    [SerializeField]
    [Tooltip("Script that manages the player invincibility UI")]
    private PlayerInvincibilityUI invincibilityUI;

    [Header("Take Damage Effects")]

    [SerializeField]
    [Tooltip("Time it takes for the player to fade in and out just after taking damage")]
    private float fadeTime;
    [SerializeField]
    [Tooltip("Source to play damaging effects")]
    private AudioSource healthAudio;
    [SerializeField]
    [Tooltip("Sound effect that plays when the player takes damage")]
    private AudioClip damageClip;

    [Header("Invincibility Effects")]

    [SerializeField]
    [Tooltip("Source to play invincibility effects")]
    private AudioSource invincibilityAudio;
    [SerializeField]
    [Tooltip("Deflect sound effect")]
    private AudioClip deflectClip;
    [SerializeField]
    [Tooltip("Sound effect for healing")]
    private AudioClip healClip;
    [SerializeField]
    [Tooltip("Effect played when the player powers down")]
    private AudioClip powerDownClip;

    [Header("Unbreakable Effects")]

    [Tooltip("Event invoked when the unbreakable trigger effect begins")]
    public UnityEvent unbreakableTriggerStartEvent;
    [Tooltip("Event invoked when the unbreakable trigger effect ends")]
    public UnityEvent unbreakableTriggerEndEvent;

    private new SpriteRenderer renderer;
    private PlayerDecoy decoy;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        decoy = GameObject.FindGameObjectWithTag(decoyTag).GetComponent<PlayerDecoy>();
    }

    private void Start()
    {
        decoy.gameObject.SetActive(false);
    }

    // Setup the max value on the slider
    // This makes it so that the effects don't need to know how much health the player has
    public void Setup(int max)
    {
        healthUi.Setup(max);
    }

    public void TakeDamageEffect(int newHealth, float invincibleTime)
    {
        // Update health UI
        healthUi.UpdateUI(newHealth);

        // Play the damage clip
        healthAudio.clip = damageClip;
        healthAudio.Play();

        // Start the fading in and out coroutine
        StartCoroutine(Flicker(invincibleTime));
    }

    public void UnbreakableTriggerEffect(int newHealth, float invincibleTime)
    {
        // Make the health UI trigger an effect
        healthUi.UnbreakableModeTriggerEffect(newHealth);
        // Start the unbreakable trigger effect routine
        StartCoroutine(UnbreakableTriggerRoutine(newHealth, invincibleTime));
    }

    private IEnumerator UnbreakableTriggerRoutine(int newHealth, float invincibleTime)
    {
        // Invoke the start
        unbreakableTriggerStartEvent.Invoke();
        // Make the decoy do its effect
        yield return decoy.UnbreakableTriggerRoutine();
        // Invoke the end
        unbreakableTriggerEndEvent.Invoke();
        // Wait until time scale is back to normal, then cause a take damage effect
        yield return new WaitUntil(() => Time.timeScale > 0);
        TakeDamageEffect(newHealth, invincibleTime);
    }

    public void UnbreakableModeToggleEffect(bool active)
    {
        healthUi.ToggleUnbreakableModeUI(active);
    }

    public void DeathEffect()
    {
        decoy.DeathEffect();
    }

    public void ActivateInvincibilityEffect()
    {
        // Play a sound!
        invincibilityAudio.clip = deflectClip;
        invincibilityAudio.Play();

        // Activate the UI
        invincibilityUI.Activate();
    }

    public void DeactivateInvincibilityEffect(float rechargeTime)
    {
        // Play a sound
        invincibilityAudio.clip = powerDownClip;
        invincibilityAudio.Play();

        // This function updates the slider
        invincibilityUI.Recharge(rechargeTime);
    }

    public void AttackDeflectEffect()
    {
        // Play a sound!
        invincibilityAudio.clip = deflectClip;
        invincibilityAudio.Play();

        invincibilityUI.StartDeflect();
    }

    public void InvincibilityReadyEffect()
    {
        // Play a sound!  Or, you know, SOMETHING
        invincibilityAudio.clip = healClip;
        invincibilityAudio.Play();

        invincibilityUI.Ready();
    }

    private IEnumerator Flicker(float invincibleTime)
    {
        // Set low and high alpha colors, so that the heart is never completely invisible
        Color lowColor = new Color(1f, 1f, 1f, 0.2f);
        Color highColor = new Color(1f, 1f, 1f, 0.7f);
        // Return the flicker module result
        yield return ColorModule.Flicker(lowColor, highColor, invincibleTime - 0.1f, fadeTime, SetRendererColor);
        SetRendererColor(Color.white);
    }

    private void SetRendererColor(Color color)
    {
        renderer.color = color;
    }
}
