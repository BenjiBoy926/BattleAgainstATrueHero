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
    public UnityEvent deathEvent
    {
        get
        {
            return _deathEvent;
        }
    }

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

    private void Start()
    {
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
            // Die!
            _deathEvent.Invoke();
            effects.DeathEffect();
        }
        // If the player isn't dead yet, play the effect for the player taking damage
        else
        {
            effects.TakeDamageEffect(health, invincibilityAfterHit);
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
        Invoke("DeactivateInvincibility", invincibilityTime);
    }

    private void DeactivateInvincibility()
    {
        // Set time of invincibility deactivation
        timeOfInvincibilityDeactivation = Time.time;
        _invincibilityDeactivatedEvent.Invoke();

        // Let the effects know we have deactivated invincibility
        effects.DeactivateInvincibilityEffect(invincibilityRechargeTime);

        CancelInvoke();
        Invoke("InvincibilityReady", invincibilityRechargeTime);
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
