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
    [Tooltip("Position undyne should be in while an 'undying' animation is playing")]
    private Vector3 undyingPosition;
    [SerializeField]
    [Tooltip("Size undyng should be while an 'undying' animation is playing")]
    private Vector3 undyingScale;

    [SerializeField]
    [Tooltip("Time it takes for the scene to fade into view")]
    private float openingFadeTime;
    [SerializeField]
    [TagSelector]
    [Tooltip("Tag of the object with a panel that overlays the full scene")]
    private string overlayTag;

    [SerializeField]
    [Tooltip("Monologue for the character when the level is being introduced")]
    private Monologue monologue;

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
    }

    // Set an animation with dying position and scale
    public void SetDyingAnimation(string trigger)
    {
        SetAnimation(dyingPosition, dyingScale, trigger);
    }
    // Set an animation with undying position and scale
    public void SetUndyingAnimation(string trigger)
    {
        SetAnimation(undyingPosition, undyingScale, trigger);
    }
    // Set an animation with position and scale
    private void SetAnimation(Vector3 pos, Vector3 scale, string trigger)
    {
        transform.position = pos;
        transform.localScale = scale;
        animator.Get(this).SetTrigger(trigger);
    }
}
