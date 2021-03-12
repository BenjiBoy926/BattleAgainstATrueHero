using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnPlayerDeath : MonoBehaviour
{
    private void Start()
    {
        // Find the player health script
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();

        // Add disable function to listen for the death event
        playerHealth.deathEvent.AddListener(Disable);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
