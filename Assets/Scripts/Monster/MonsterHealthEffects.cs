using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class MonsterHealthEffects : MonoBehaviour
{ 
    [SerializeField]
    [Tooltip("Transform on the monster's costume")]
    private Transform costumeTransform;
    [SerializeField]
    [Tooltip("Object instantiated to create the slash animation")]
    private GameObject slashObject;
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
    private Transform textPrefab;
    [SerializeField]
    [Tooltip("Event invoked when the effect begins")]
    private UnityEvent onEffectStart;
    [SerializeField]
    [Tooltip("Event invoked when the effect finishes")]
    private UnityEvent onEffectEnd;

    [Header("Damage animation")]

    [SerializeField]
    [Tooltip("Position that the monster is in while taking damage")]
    private TransformData damageTransform;
    [SerializeField]
    [Tooltip("Name of the animation of the monster taking damage")]
    private string damageAnimation;
    [SerializeField]
    [Tooltip("Time that the algorithm waits after the slash to display the other effects")]
    private float slashTime;
    [SerializeField]
    [Tooltip("Health slider for the monster")]
    private Slider healthSlider;
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

    [Header("Death animation")]

    [SerializeField]
    [Tooltip("Position of the monster when in the death animation")]
    private TransformData dyingTransform;
    [SerializeField]
    [Tooltip("Name of the dying animation of the monster")]
    private string dyingAnimation;
    [SerializeField]
    [Tooltip("Time it takes for the monster to move between ping-pong positions when shaking after taking the last hit")]
    private float deathShakeTime;
    [SerializeField]
    [Tooltip("Number of times that the monster shakes when they take the last hit")]
    private int numDeathShakes;
    [SerializeField]
    [Tooltip("Event invoked when the monster takes the last hit")]
    private UnityEvent deathAnimationEvent;

    private void Start()
    {
        healthSlider.gameObject.SetActive(false);
    }

    public void SetupEffects(int startingHealth)
    {
        healthSlider.maxValue = startingHealth;
    }

    public void TakeDamageEffect(int currentHealth)
    {
        StartCoroutine(TakeDamageEffectRoutine(currentHealth));
    }

    private IEnumerator TakeDamageEffectRoutine(int currentHealth)
    {
        // Invoke the effect start event
        onEffectStart.Invoke();

        // Setup the shake time and animation events
        TransformData transformData = currentHealth > 0 ? damageTransform : dyingTransform;
        string trigger = currentHealth > 0 ? damageAnimation : dyingAnimation;
        float shakeTime = currentHealth > 0 ? damageShakeTime : deathShakeTime;
        int numShakes = currentHealth > 0 ? numDamageShakes : numDeathShakes;
        UnityEvent animationEvent = currentHealth > 0 ? damageAnimationEvent : deathAnimationEvent; 

        // Instantiate the slash object at the enemy's position
        Instantiate(slashObject, costumeTransform.transform.position, slashObject.transform.rotation);

        // Wait for the slash to complete
        yield return new WaitForSeconds(slashTime);

        // Play the damage audio clip
        audio.clip = damageClip;
        audio.Play();

        // Put the monster in the damage animation
        transformData.SetTransform(costumeTransform);
        animator.SetTrigger(trigger);

        // Invoke the animation event
        animationEvent.Invoke();

        // Update the health slider
        healthSlider.gameObject.SetActive(true);
        healthSlider.value = currentHealth;

        // Instantiate text at our position telling the damage that the monster received
        Instantiate(textPrefab, costumeTransform.position, textPrefab.transform.rotation);

        // Quiver back and forth
        UnityAction<int> tick = currentTick =>
        {
            damageTransform.SetTransform(costumeTransform);

            // Move slightly to the left or right, depending on the current tick value
            if(currentTick % 2 == 0)
            {
                costumeTransform.localPosition += Vector3.right * shakeMagnitude;
            }
            else
            {
                costumeTransform.localPosition += Vector3.left * shakeMagnitude;
            }
        };
        yield return CoroutineModule.Tick(shakeTime, numShakes, tick);

        // Disable the health slider when we return to normal
        healthSlider.gameObject.SetActive(false);

        // Set animation back to idle, only if the monster still has health left
        if(currentHealth > 0)
        {
            // Invoke the event for when the idle animation starts
            idleAnimationEvent.Invoke();

            // Set back to idle animation
            idleTransform.SetTransform(costumeTransform);
            animator.SetTrigger(idleAnimation);
        }

        // Invoke the effect end event
        onEffectEnd.Invoke();
    }
}
