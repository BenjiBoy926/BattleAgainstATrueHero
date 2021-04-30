using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterHealth : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Health of the enemy")]
    private int health;

    [SerializeField]
    [Tooltip("Event invoked when the enemy health is first active")]
    private IntEvent startEvent;
    [SerializeField]
    [Tooltip("Event invoked when the enemy takes damage")]
    private IntEvent _takeDamageEvent;
    [SerializeField]
    [Tooltip("Event invoked when the enemy dies")]
    private UnityEvent _deathEvent;

    public UnityEvent<int> takeDamageEvent => _takeDamageEvent;
    public UnityEvent deathEvent => _deathEvent;
    public bool dead => health <= 0;

    private void Start()
    {
        startEvent.Invoke(health);
    }

    public void TakeDamage()
    {
        // Decrease health, invoke the event
        health--;
        _takeDamageEvent.Invoke(health);

        // If health is depleted, then die
        if(health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _deathEvent.Invoke();
    }

    [System.Serializable]
    private class IntEvent : UnityEvent<int> { }
}
