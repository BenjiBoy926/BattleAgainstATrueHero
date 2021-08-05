using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleData : MonoBehaviour
{
    private static int attempts = 0;
    public static int Attempts => attempts;

    [SerializeField]
    [TagSelector]
    [Tooltip("Tag of the object that has the player health on it")]
    private string playerTag = "Player";

    // Reference to the script that manages player health
    private PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag(playerTag).GetComponent<PlayerHealth>();
        playerHealth.deathEvent.AddListener(() => attempts++);
    }
}
