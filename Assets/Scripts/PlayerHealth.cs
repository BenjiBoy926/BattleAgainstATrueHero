using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerHealthEffects))]
public class PlayerHealth : MonoBehaviour
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
    private float invincibilityTime;

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

    private PlayerHealthEffects effects;
    private float timeSinceLastHit;

    private void Start()
    {
        effects = GetComponent<PlayerHealthEffects>();
        timeSinceLastHit = -invincibilityTime;
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
        if(Time.time > timeSinceLastHit + invincibilityTime)
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        // Deplete health and set time since last hit to right now
        health -= damagePerHit;
        timeSinceLastHit = Time.time;

        if(health <= 0)
        {
            // Die!
            _deathEvent.Invoke();
        }
        // If the player isn't dead yet, play the effect for the player taking damage
        else
        {
            effects.TakeDamageEffect(invincibilityTime);
        }
    }
}
