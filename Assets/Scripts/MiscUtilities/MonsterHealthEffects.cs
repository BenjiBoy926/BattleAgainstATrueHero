using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class MonsterHealthEffects : MonoBehaviour
{
    [Header("References")]

    [SerializeField]
    [Tooltip("Reference to the animator on the monster")]
    private Animator animator;
    [SerializeField]
    [Tooltip("Audio source used to play the damage effect")]
    private new AudioSource audio;
    [SerializeField]
    [Tooltip("Audio clip that plays when the monster takes damage")]
    private AudioClip damageClip;
    [SerializeField]
    [Tooltip("Text instantiated to display the damage that the monster took")]
    private TextMeshPro textPrefab;

    [Header("Damage animation")]

    [SerializeField]
    [Tooltip("Position that the monster is in while taking damage")]
    private TransformData damageTransform;
    [SerializeField]
    [Tooltip("Name of the animation of the monster taking damage")]
    private string damageAnimation;
    [SerializeField]
    [TagSelector]
    [Tooltip("Tag on the object with the monster's health slider")]
    private string healthSliderTag;
    [SerializeField]
    [Tooltip("Min-max possible numbers to display for the monster's damage")]
    private IntRange damageRange;
    [SerializeField]
    [Tooltip("Time it takes for the monster to move between ping-pong positions when shaking")]
    private float damageShakeTime;
    [SerializeField]
    [Tooltip("The number of times that the monster shakes when taking damage")]
    private int numDamageShakes;
    [SerializeField]
    [Tooltip("Amount that the monster moves from the base position when shaking")]
    private float shakeMagnitude;
    [SerializeField]
    [Tooltip("Event invoked when the damage animation activates")]
    private UnityEvent damageAnimationEvent;

    [Header("Idle animation")]

    [SerializeField]
    [Tooltip("Position that the monster is in for the idle animation")]
    private TransformData idleTransform;
    [SerializeField]
    [Tooltip("Name of the normal animaton of the monster")]
    private string idleAnimation;
    [SerializeField]
    [Tooltip("Event invoked when the idle anmation activates")]
    private UnityEvent idleAnimationEvent;

    private GameObject healthSliderObject;
    private Slider healthSlider;

    private void Start()
    {
        healthSliderObject = GameObject.FindGameObjectWithTag(healthSliderTag);
        healthSlider = healthSliderObject.GetComponent<Slider>();
        healthSliderObject.SetActive(false);
    }

    public void TakeDamageEffect()
    {
        StartCoroutine(TakeDamageEffectRoutine());
    }

    private IEnumerator TakeDamageEffectRoutine()
    {
        // Play the damage audio clip
        audio.clip = damageClip;
        audio.Play();

        // Put the monster in the damage animation
        damageTransform.SetTransform(transform);
        animator.SetTrigger(damageAnimation);

        // Invoke the damage animation
        damageAnimationEvent.Invoke();

        // Update the health slider
        healthSliderObject.SetActive(true);
        healthSlider.value--;

        // Instantiate text at our position telling the damage that the monster received
        TextMeshPro text = Instantiate(textPrefab, transform.position, textPrefab.transform.rotation);
        text.text = Mathf.FloorToInt(Random.Range(damageRange.min, damageRange.max)).ToString();

        // Quiver back and forth
        UnityAction<int> tick = currentTick =>
        {
            damageTransform.SetTransform(transform);

            // Move slightly to the left or right, depending on the current tick value
            if(currentTick % 2 == 0)
            {
                transform.position += Vector3.right * shakeMagnitude;
            }
            else
            {
                transform.position += Vector3.left * shakeMagnitude;
            }
        };
        yield return CoroutineModule.Tick(damageShakeTime, numDamageShakes, tick);

        // Invoke the event for when the idle animation starts
        idleAnimationEvent.Invoke();

        // Set back to idle animation
        idleTransform.SetTransform(transform);
        animator.SetTrigger(idleAnimation);

        // Disable the health slider when we return to normal
        healthSliderObject.SetActive(false);
    }
}
