using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerHealthEffects : MonoBehaviour
{
    [Header("Sub-Components")]

    [SerializeField]
    [Tooltip("Reference to the script that manages the player UI")]
    private PlayerHealthUI healthUi;
    [SerializeField]
    [Tooltip("Script that manages the player invincibility UI")]
    private PlayerInvincibilityUI invincibilityUI;

    [Header("Visual Elements")]

    [SerializeField]
    [Tooltip("Time between the crack and the splinter of the heart shape when the player dies")]
    private float splitDelay;
    [SerializeField]
    [Tooltip("Time after the player splinters away that the game over text appears")]
    private float gameOverDelay;
    [SerializeField]
    [Tooltip("Default appearance of the player")]
    private Sprite defaultSprite;
    [SerializeField]
    [Tooltip("Appearance of the player when the heart cracks")]
    private Sprite crackSprite;
    [SerializeField]
    [Tooltip("Object that represents the heart splinter")]
    private GameObject splinterObject;
    [SerializeField]
    [Tooltip("The number of splinters instantiated when the player dies")]
    private int splinterCount;

    [Header("Audio Elements")]

    [SerializeField]
    [Tooltip("Source to play damaging effects")]
    private AudioSource healthAudio;
    [SerializeField]
    [Tooltip("Source to play invincibility effects")]
    private AudioSource invincibilityAudio;
    [SerializeField]
    [Tooltip("Sound effect that plays when the player takes damage")]
    private AudioClip damageClip;
    [SerializeField]
    [Tooltip("Clip played when the heart cracks")]
    private AudioClip crackClip;
    [SerializeField]
    [Tooltip("Clip played when the heart splinters to pieces")]
    private AudioClip splinterClip;
    [SerializeField]
    [Tooltip("Time it takes for the player to fade in and out while invincible")]
    private float fadeTime;
    [SerializeField]
    [Tooltip("Deflect sound effect")]
    private AudioClip deflectClip;
    [SerializeField]
    [Tooltip("Sound effect for healing")]
    private AudioClip healClip;
    [SerializeField]
    [Tooltip("Effect played when the player powers down")]
    private AudioClip powerDownClip;

    private new SpriteRenderer renderer;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
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

    public void UnbreakableModeTriggerEffect(int newHealth)
    {
        healthUi.UpdateUI(newHealth);
    }
    public void UnbreakableModeToggleEffect(bool active)
    {
        healthUi.ToggleUnbreakableModeUI(active);
    }

    public void DeathEffect()
    {
        healthUi.SetUIActive(false);
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(splitDelay);

        // After a second, make the heart crack
        renderer.sprite = crackSprite;

        // Play the heart crack clip
        healthAudio.clip = crackClip;
        healthAudio.Play();

        yield return new WaitForSeconds(splitDelay);

        // After a second, make the heart crack
        renderer.enabled = false;

        // Play the heart splinter clip
        healthAudio.clip = splinterClip;
        healthAudio.Play();

        // Instantiate multiple splinters. The objects themselves take care of other things
        // like initial velocity and rotation
        for(int i = 0; i < splinterCount; i++)
        {
            Instantiate(splinterObject, transform.position, splinterObject.transform.rotation);
        }

        yield return new WaitForSeconds(gameOverDelay);

        // Startup game over manager
        GameOver.BeginGameOver("BattleAgainstATrueHero");
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
        yield return ColorModule.Flicker(lowColor, highColor, invincibleTime, fadeTime, SetRendererColor);
        SetRendererColor(Color.white);
    }

    private void SetRendererColor(Color color)
    {
        renderer.color = color;
    }
}
