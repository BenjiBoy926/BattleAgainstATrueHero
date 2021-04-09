using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceIndicator : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Proportion of the indicator height that the indicator floats up and down")]
    private float heightProportion;
    [SerializeField]
    [Tooltip("Time it takes for the indicator to float up and down")]
    private float floatTime;

    private CachedComponent<RectTransform> rect = new CachedComponent<RectTransform>();
    private float floatDistance => rect.Get(this).rect.height * heightProportion;
    private Vector3 basePosition;

    private void Awake()
    {
        basePosition = transform.position;
    }

    private void OnEnable()
    {
        StartCoroutine(transform.PingPong(basePosition, basePosition + Vector3.up * floatDistance, floatTime));
    }
}
