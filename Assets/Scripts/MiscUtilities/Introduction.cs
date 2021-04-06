using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Introduction : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Position undyne should be in while a 'dying' animation is playing")]
    private Vector3 dyingPosition;
    [SerializeField]
    [Tooltip("Size undyne should be while a 'dying' animation is playing")]
    private Vector3 dyingScale;

    [SerializeField]
    [Tooltip("Position of undyne while she is standing still after undying transformation")]
    private Vector3 undyingStillPosition;
    [SerializeField]
    [Tooltip("Size of undyne while she is standing still after undying transformation")]
    private Vector3 undyingStillScale;

    [SerializeField]
    [Tooltip("Position undyne should be in while an 'undying' animation is playing")]
    private Vector3 undyingPosition;
    [SerializeField]
    [Tooltip("Size undyng should be while an 'undying' animation is playing")]
    private Vector3 undyingScale;

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
    [Tooltip("Audio clip that plays when undyn transforms")]
    private AudioClip transformClip;
    [SerializeField]
    [TagSelector]
    [Tooltip("Tag of the object with a panel that overlays the full scene")]
    private string overlayTag;

    [SerializeField]
    [Tooltip("Audio source to play the music while Undyne is monologing")]
    private AudioSource monologueMusic;

    [SerializeField]
    [TagSelector]
    [Tooltip("Tag attached to the object that plays the music")]
    private string musicTag;

    [SerializeField]
    [Tooltip("Monologue for the character when the level is being introduced")]
    private Monologue monologue;
    [SerializeField]
    [Tooltip("Monologue that undyne speaks after she transforms")]
    private Monologue transformMonologue;

    // Reference to the animator on Undyne
    private CachedComponent<Animator> animator = new CachedComponent<Animator>();
    // Image overlaying the full scene
    private Image overlay;

    // Start is called before the first frame update
    void Start()
    {
        GameObject overlayObject = GameObject.FindGameObjectWithTag(overlayTag);
        overlay = overlayObject.GetComponent<Image>();

        SetDyingAnimation("Dying");

        StartCoroutine(IntroductionRoutine());
    }

    private IEnumerator IntroductionRoutine()
    {
        yield return overlay.Fade(Color.black, Color.clear, openingFadeTime);
        yield return monologue.Speak();

        // Play the transform clip
        monologueMusic.clip = transformClip;
        monologueMusic.loop = false;
        monologueMusic.Play();

        yield return overlay.Fade(Color.clear, Color.white, transformFadeInTime);
        yield return new WaitUntil(() => !monologueMusic.isPlaying);

        // Set the correct animation for undyne to stand still in her new transformation position
        SetUndyingStillAnimation();
        yield return overlay.Fade(Color.white, Color.clear, transformFadeOutTime);

        // Do the brief introduction
        yield return BreifIntroduction();
    }

    // Breif introduction simply has Undyne the Undying say something, then start the music
    private IEnumerator BreifIntroduction()
    {
        SetUndyingStillAnimation();
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
        SetAnimation(dyingPosition, dyingScale, trigger);
    }
    public void SetUndyingStillAnimation()
    {
        SetAnimation(undyingStillPosition, undyingStillScale, "UndyingStill");
    }
    // Set an animation with undying position and scale
    public void SetUndyingAnimation()
    {
        SetAnimation(undyingPosition, undyingScale, "Undying");
    }
    // Set an animation with position and scale
    private void SetAnimation(Vector3 pos, Vector3 scale, string trigger)
    {
        transform.position = pos;
        transform.localScale = scale;
        animator.Get(this).SetTrigger(trigger);
    }
    public void MusicFadeIn()
    {
        StartCoroutine(monologueMusic.FadeIn(2f));
    }
}
