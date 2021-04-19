using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Time after the object is instantiated that it is destroyed")]
    private float lifetime;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
