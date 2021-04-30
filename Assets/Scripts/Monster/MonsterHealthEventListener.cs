using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterHealthEventListener : MonoBehaviour
{
    [SerializeField]
    [TagSelector]
    [Tooltip("Tag on the object where we should find the enemy health script attached")]
    private string monsterTag;
    [SerializeField]
    [Tooltip("Event invoked when the monster takes damage")]
    private IntEvent takeDamageEvent;
    [SerializeField]
    [Tooltip("Event invoked when the monster dies")]
    private UnityEvent deathEvent;

    private void Awake()
    {
        GameObject monster = GameObject.FindGameObjectWithTag(monsterTag);
        MonsterHealth health = monster.GetComponent<MonsterHealth>();

        // Add listeners that invoke these events when those events are invoked
        health.takeDamageEvent.AddListener(takeDamageEvent.Invoke);
        health.deathEvent.AddListener(deathEvent.Invoke);
    }

    [System.Serializable]
    private class IntEvent : UnityEvent<int> { }
}
