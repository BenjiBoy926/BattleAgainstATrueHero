using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UndyneIntroduction : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Time it takes for the scene to fade into view")]
    private float openingFadeTime;
    [SerializeField]
    [Tooltip("Time it takes to fade in for the transformation")]
    private float transformFadeInTime;
    [SerializeField]
    [Tooltip("Time it takes to fade out for the transformation")]
    private float transformFadeOutTime;
    [SerializeField]
    [Tooltip("Audio clip that plays when undyne transforms")]
    private AudioClip transformClip;
    [SerializeField]
    [TagSelector]
    [Tooltip("Tag of the object with a panel that overlays the full scene")]
    private string overlayTag;

    [SerializeField]
    [Tooltip("Root game object for the speech bubble")]
    private GameObject speechBubble;
    [SerializeField]
    [Tooltip("Audio source to play the music while Undyne is monologing")]
    private AudioSource monologueMusic;

    [SerializeField]
    [TagSelector]
    [Tooltip("Tag attached to the object that plays the music")]
    private string musicTag;

    [Header("Animation")]

    [SerializeField]
    [Tooltip("Animator on undyne's costume")]
    private Animator animator;
    [SerializeField]
    [Tooltip("Transform component on undyne's costume animation")]
    private Transform costumeTransform;
    [SerializeField]
    [Tooltip("Transform data for when a 'dying' animation is playing")]
    private TransformData dyingTransform;
    [SerializeField]
    [Tooltip("Transform data for when undying is standing still after undying transformation")]
    private TransformData undyingStillTransform;
    [SerializeField]
    [Tooltip("Transform data for when the 'undying' animation is playing")]
    private TransformData undyingTransform;

    [Header("Monologues")]

    [SerializeField]
    [Tooltip("Monologue for the character when the level is being introduced")]
    private Monologue monologue;
    [SerializeField]
    [Tooltip("Monologue that undyne speaks after she transforms")]
    private Monologue transformMonologue;

    // Image overlaying the full scene
    private Image overlay;

    // Start is called before the first frame update
    void Start()
    {
        GameObject overlayObject = GameObject.FindGameObjectWithTag(overlayTag);
        overlay = overlayObject.GetComponent<Image>();

        // If this is the player's first attempt, give them the long introduction
        if(PlayerHealth.deathCount == 0)
        {
            StartCoroutine(LongIntroduction());
        }
        // If this is not the first attempt, give them the short introduction
        else
        {
            StartCoroutine(ShortIntroduction());
        }
    }

    private IEnumerator LongIntroduction()
    {
        // Start
        SetDyingAnimation("Dying");

        // Fade in, then speak the opening monologue
        yield return overlay.FadeOut(Color.black, openingFadeTime);
        yield return monologue.Speak();

        // Play the transform clip
        monologueMusic.clip = transformClip;
        monologueMusic.loop = false;
        monologueMusic.Play();

        // Fade out to conceal undyne's transformation
        yield return overlay.FadeIn(Color.white, transformFadeInTime);
        yield return new WaitUntil(() => !monologueMusic.isPlaying);

        // Set the correct animation for undyne to stand still in her new transformation position
        SetUndyingStillAnimation();
        yield return overlay.FadeOut(Color.white, transformFadeOutTime);

        // Do the brief introduction
        yield return UndyneTheUndyingIntroduction();
    }

    private IEnumerator ShortIntroduction()
    {
        SetUndyingStillAnimation();
        yield return overlay.FadeOut(Color.black, openingFadeTime);
        yield return UndyneTheUndyingIntroduction();
    }

    // Breif introduction simply has Undyne the Undying say something, then start the music
    private IEnumerator UndyneTheUndyingIntroduction()
    {
        // "You're going to have to dance a little better than THAT"
        yield return transformMonologue.Speak();

        // Set undying animation
        SetUndyingAnimation();

        // Play that funky music, white boy!
        GameObject musicObject = GameObject.FindGameObjectWithTag(musicTag);
        SynchronizedMusic music = musicObject.GetComponent<SynchronizedMusic>();
        music.StartMusic();
    }

    // Set an animation with dying position and scale
    public void SetDyingAnimation(string trigger)
    {
        SetAnimation(dyingTransform, trigger);
    }
    public void SetUndyingStillAnimation()
    {
        SetAnimation(undyingStillTransform, "UndyingStill");
    }
    // Set an animation with undying position and scale
    public void SetUndyingAnimation()
    {
        SetAnimation(undyingTransform, "Undying");
        speechBubble.SetActive(false);
    }
    // Set an animation with position and scale
    private void SetAnimation(TransformData transformData, string trigger)
    {
        transformData.SetTransform(costumeTransform);
        animator.SetTrigger(trigger);
    }
    public void MusicFadeIn()
    {
        StartCoroutine(monologueMusic.FadeIn(2f));
    }
}
