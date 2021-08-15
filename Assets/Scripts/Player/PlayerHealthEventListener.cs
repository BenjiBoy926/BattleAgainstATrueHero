using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealthEventListener : MonoBehaviour
{
    [TagSelector]
    [SerializeField]
    [Tooltip("Tag of the object where the player health should be found")]
    private string playerTag = "Player";

    [SerializeField]
    [Tooltip("Event invoked when the player dies")]
    private UnityEvent deathEvent;
    [SerializeField]
    [Tooltip("Event invoked when unbreakable mode is triggered")]
    private UnityEvent unbreakableTriggerStartEvent;
    [SerializeField]
    [Tooltip("Event invoked when unbreakable mode finishes triggering")]
    private UnityEvent unbreakableTriggerEndEvent;

    // Setup the listener so that the death event occurs when the player dies
    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);

        // Add the listener for the death event
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        health.deathEvent.AddListener(deathEvent.Invoke);

        // Add listeners for the unbreakable events
        PlayerHealthEffects healthEffects = player.GetComponent<PlayerHealthEffects>();
        healthEffects.unbreakableTriggerStartEvent.AddListener(unbreakableTriggerStartEvent.Invoke);
        healthEffects.unbreakableTriggerEndEvent.AddListener(unbreakableTriggerEndEvent.Invoke);
    }
}
