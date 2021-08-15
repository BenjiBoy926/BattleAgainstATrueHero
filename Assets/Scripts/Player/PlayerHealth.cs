using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerHealthEffects))]
public class PlayerHealth : MonoBehaviour, IMusicStartListener
{
    [SerializeField]
    [TagSelector]
    [Tooltip("Tag of the objects that can hurt the player")]
    private string hazardTag;
    [SerializeField]
    [Tooltip("Health of the player")]
    private int health;
    [SerializeField]
    [Tooltip("Amount of damage dealt to player on each hit")]
    private int damagePerHit;
    [SerializeField]
    [Tooltip("Time for which the player remains invincible after taking damage")]
    private float invincibilityAfterHit;
    [SerializeField]
    [Tooltip("Time that the player remains invulnerable when activating temporary invulnerability")]
    private float invincibilityTime;
    [SerializeField]
    [Tooltip("Time it takes for invincibility activation to recharge")]
    private float invincibilityRechargeTime;

    [SerializeField]
    [Tooltip("Event invoked when the player is defeated")]
    private UnityEvent _deathEvent;
    // Property for public access to the death event, 
    // but we need the field to be serialized
    public UnityEvent deathEvent => _deathEvent;

    [SerializeField]
    [Tooltip("Event invoked when invincibility is activated")]
    private UnityEvent _invincibilityActivatedEvent;
    [SerializeField]
    [Tooltip("Event invoked when invincibility deactivates")]
    private UnityEvent _invincibilityDeactivatedEvent;
    [SerializeField]
    [Tooltip("Event invoked when the invincibility first becomes ready after activation")]
    private UnityEvent _invincibilityReadyEvent;

    // Reference to the script that manages the health effects
    private PlayerHealthEffects effects;
    // The health that the player starts out with
    private int startingHealth;
    // Time that the application was at when the player last got hit
    private float timeSinceLastHit;
    // Time that the application was at when the player's invincibility activated
    private float timeOfInvincibilityActivation;
    // Time that the application was at when the player's invincibility deactivated
    private float timeOfInvincibilityDeactivation;

    // Player is invulnerable if activated invulnerability
    // or if recently hit by an attack
    private bool invincible =>
        invincibilityActive ||
        Time.time < (timeSinceLastHit + invincibilityAfterHit);
    // Determine if active invincibility is active
    private bool invincibilityActive =>
        Time.time < (timeOfInvincibilityActivation + invincibilityTime);
    // Invincibility can be activated only if time exceeds time of invincibility deactivation by recharge time
    private bool invincibilityReady =>
        Time.time > (timeOfInvincibilityDeactivation + invincibilityRechargeTime);

    // Determines if "unbreakable mode" is active
    // In unbreakable mode the player cannot die
    private static bool _unbreakable = false;
    public static bool unbreakable
    {
        get => _unbreakable;
        set
        {
            _unbreakable = value;

            // Try to find an active player health in the scene
            PlayerHealth activeHealth = FindObjectOfType<PlayerHealth>();
            // If a player health exists in the scene, activate its effects
            if(activeHealth != null)
            {
                activeHealth.effects.UnbreakableModeToggleEffect(value);
            }
        }
    }
    // Counts the number of times that unbreakable mode has triggered during this fight
    public static int unbreakableTriggerCounter { get; private set; }
    // Count the number of times that the player has died
    public static int deathCount { get; private set; }

    private void Awake()
    {
        // At the start of the scene, the counter is always 0
        unbreakableTriggerCounter = 0;
        // Store the health we started at
        startingHealth = health;

        effects = GetComponent<PlayerHealthEffects>();
        effects.Setup(health);

        timeSinceLastHit = -invincibilityAfterHit;
        timeOfInvincibilityActivation = -invincibilityTime;
        timeOfInvincibilityDeactivation = -invincibilityTime;
    }

    private void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            TryActivateInvincibility();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(hazardTag))
        {
            TryTakeDamage();
        }
    }

    private void TryTakeDamage()
    {
        // If current time exceeds time since last hit plus invincibility time, then take damage
        if(!invincible)
        {
            TakeDamage();
        }
        // If we are invincible from active invincibility, tell the effects script
        else if(invincibilityActive)
        {
            effects.AttackDeflectEffect();
        }
    }

    private void TakeDamage()
    {
        // Deplete health and set time since last hit to right now
        health -= damagePerHit;
        timeSinceLastHit = Time.time;

        if (health <= 0)
        {
            Die();
        }
        // If the player isn't dead yet, play the effect for the player taking damage
        else
        {
            effects.TakeDamageEffect(health, invincibilityAfterHit);
        }
    }

    private void Die()
    {
        // If we died while unbreakable, do a different thing
        if(unbreakable)
        {
            health = startingHealth;
            // Increase unbreakable mode trigger counter
            unbreakableTriggerCounter++;
            // Enable a health restore effect
            effects.UnbreakableTriggerEffect(health, invincibilityAfterHit);
        }
        else
        {
            // Die!
            deathCount++;
            _deathEvent.Invoke();
            effects.DeathEffect();
        }
    }


    private void TryActivateInvincibility()
    {
        if(!invincible && invincibilityReady)
        {
            ActivateInvincibility();
        }
    }

    private void ActivateInvincibility()
    {
        // Set time of invincibility activation
        timeOfInvincibilityActivation = Time.time;
        _invincibilityActivatedEvent.Invoke();

        // Let the effects know we have activated invincibility
        effects.ActivateInvincibilityEffect();

        // Invoke deactivation
        CancelInvoke();
        Invoke(nameof(DeactivateInvincibility), invincibilityTime);
    }

    private void DeactivateInvincibility()
    {
        // Set time of invincibility deactivation
        timeOfInvincibilityDeactivation = Time.time;
        _invincibilityDeactivatedEvent.Invoke();

        // Let the effects know we have deactivated invincibility
        effects.DeactivateInvincibilityEffect(invincibilityRechargeTime);

        CancelInvoke();
        Invoke(nameof(InvincibilityReady), invincibilityRechargeTime);
    }

    private void InvincibilityReady()
    {
        // Invoke invincibility ready event
        _invincibilityReadyEvent.Invoke();

        // Let the effects know that invincibility is ready to be activated
        effects.InvincibilityReadyEffect();
    }

    public void OnMusicStart(MusicCursor cursor)
    {
        enabled = true;
    }
}
