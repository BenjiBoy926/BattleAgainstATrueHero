using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerHealthEffects : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the script that manages the player UI")]
    private PlayerHealthUI ui;

    // VISUAL
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

    // AUDIO
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

    // Reference to the audio source that plays effects for the player
    private AudioSource source;
    private new SpriteRenderer renderer;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        renderer = GetComponent<SpriteRenderer>();
    }

    // Setup the max value on the slider
    // This makes it so that the effects don't need to know how much health the player has
    public void Setup(int max)
    {
        ui.Setup(max);
    }

    public void TakeDamageEffect(int newHealth, float invincibleTime)
    {
        // Update health UI
        ui.UpdateUI(newHealth);

        // Play the damage clip
        source.clip = damageClip;
        source.Play();

        // Start the fading in and out coroutine
        StartCoroutine(Flicker(invincibleTime));
    }

    public void DeathEffect()
    {
        ui.SetUIActive(false);
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(splitDelay);

        // After a second, make the heart crack
        renderer.sprite = crackSprite;

        // Play the heart crack clip
        source.clip = crackClip;
        source.Play();

        yield return new WaitForSeconds(splitDelay);

        // After a second, make the heart crack
        renderer.enabled = false;

        // Play the heart splinter clip
        source.clip = splinterClip;
        source.Play();

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

    private IEnumerator Flicker(float invincibleTime)
    {
        yield return ColorModule.Flicker(Color.white, Color.clear, invincibleTime, fadeTime, SetRendererColor);
        SetRendererColor(Color.white);
    }

    private void SetRendererColor(Color color)
    {
        renderer.color = color;
    }
}
