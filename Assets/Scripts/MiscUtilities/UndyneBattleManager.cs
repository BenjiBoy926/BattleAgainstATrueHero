using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndyneBattleManager : MonoBehaviour
{
    private static int _attempts = 0;
    public static int attempts => _attempts;

    [SerializeField]
    [TagSelector]
    [Tooltip("Tag of the player game object")]
    private string playerTag;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.deathEvent.AddListener(IncrementAttempts);
    }

    private void IncrementAttempts()
    {
        _attempts++;
    }
}
