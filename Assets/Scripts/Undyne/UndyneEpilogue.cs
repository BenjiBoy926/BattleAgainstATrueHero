using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UndyneEpilogue : MonoBehaviour
{
    [Header("References")]

    [SerializeField]
    [Tooltip("Reference to the health script that gives Undyne's health")]
    private MonsterHealth undyneHealth;
    [SerializeField]
    [Tooltip("Transform of undyne's costume")]
    private Transform costumeTransform;
    [SerializeField]
    [Tooltip("Audio source that plays the monster death clip")]
    private AudioSource deathEffectSource;
    [SerializeField]
    [Tooltip("Audio clip that plays when Undyne dies")]
    private AudioClip deathEffect;
    [SerializeField]
    [TagSelector]
    [Tooltip("Tag attached to the object used to fade the scene in and out")]
    private string overlayTag;
    [SerializeField]
    [Tooltip("Name of the next scene in the build settings")]
    private string nextSceneName;

    [Header("Timing")]

    [SerializeField]
    [Tooltip("Delay before starting the epilogue")]
    private float startDelay;
    [SerializeField]
    [Tooltip("Distance away from the base position that the costume shakes")]
    private float shakeMagnitude;
    [SerializeField]
    [Tooltip("Time between each time that undyne costume shakes")]
    private float shakeInterval;
    [SerializeField]
    [Tooltip("Time it takes for Undyne to fade away when she dies")]
    private float fadeTime;
    [SerializeField]
    [Tooltip("Delay after undyne dies before the scene fades to the next scene")]
    private float sceneFadeDelay;
    [SerializeField]
    [Tooltip("Scene fade out time")]
    private float sceneFadeTime;

    [Header("Transforms")]

    [SerializeField]
    [Tooltip("Transform data for when undyne is defeated")]
    private TransformData defeatedTransform;
    [SerializeField]
    [Tooltip("Transform data for when undyne first starts melting away")]
    private TransformData meltingTransform;
    [SerializeField]
    [Tooltip("Transform data for when undyne is finally melting away")]
    private TransformData meltingAwayTransform;

    [Header("Monologue")]

    [SerializeField]
    [Tooltip("Monologue that undyne says as she is dying")]
    private Monologue monologue;

    // Animator on the costume
    private Animator costumeAnimator;
    private SpriteRenderer costumeSprite;
    private Image overlay;

    // Coroutine that makes the costume shake as undyne is dying
    private Coroutine shakeRoutine;
    private Vector3 basePosition;

    private void Awake()
    {
        costumeAnimator = costumeTransform.GetComponent<Animator>();
        costumeSprite = costumeTransform.GetComponent<SpriteRenderer>();

        GameObject overlayObject = GameObject.FindGameObjectWithTag(overlayTag);
        overlay = overlayObject.GetComponent<Image>();
    }

    // Check if undyne is dead, if so start the epilogue
    public void TryStartEpilogue()
    {
        if(undyneHealth.dead)
        {
            StartEpilogue();
        }
    }
    // Start the epilogue
    public void StartEpilogue()
    {
        StartCoroutine(EpilogueRoutine());
    }

    private IEnumerator EpilogueRoutine()
    {
        // Delay for some time
        yield return new WaitForSeconds(startDelay);

        // Start undyne shaking around
        StartShaking();

        // Say the monologue
        yield return monologue.Speak();

        // Play the death clip
        deathEffectSource.clip = deathEffect;
        deathEffectSource.Play();

        // Fade out the sprite
        yield return costumeSprite.Fade(Color.white, Color.clear, fadeTime);

        // Delay fading out the scene
        yield return new WaitForSeconds(sceneFadeDelay);

        // Fade out the scene
        yield return overlay.Fade(Color.clear, Color.black, sceneFadeTime);

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }

    public void StartShaking()
    {
        basePosition = costumeTransform.position;
        shakeRoutine = StartCoroutine(costumeTransform.Shake2D(costumeTransform.position, shakeMagnitude, shakeInterval));
    }
    public void StopShaking()
    {
        if(shakeRoutine != null)
        {
            costumeTransform.position = basePosition;
            StopCoroutine(shakeRoutine);
        }
    }

    public void SetDefeatedAnimation(string suffix)
    {
        SetAnimation(defeatedTransform, suffix);
    }
    public void SetMeltingAnimation()
    {
        SetAnimation(meltingTransform, "Melting");
    }
    public void SetMeltingAwayAnimation()
    {
        SetAnimation(meltingAwayTransform, "MeltingAway");
    }
    public void SetAnimation(TransformData data, string suffix)
    {
        data.SetTransform(costumeTransform);
        costumeAnimator.SetTrigger("Defeated" + suffix);
    }
}
