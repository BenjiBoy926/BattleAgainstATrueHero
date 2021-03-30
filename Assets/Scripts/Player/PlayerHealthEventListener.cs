using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealthEventListener : MonoBehaviour
{
    [TagSelector]
    [SerializeField]
    [Tooltip("Tag of the object where the player health should be found")]
    private string playerTag;

    [SerializeField]
    [Tooltip("Event invoked when the player dies")]
    private UnityEvent deathEvent;

    // Setup the listener so that the death event occurs when the player dies
    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        health.deathEvent.AddListener(deathEvent.Invoke);
    }
}
