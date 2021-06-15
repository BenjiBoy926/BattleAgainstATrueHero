using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvincibilityControlsUI : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Transform of the parent with the controls UI")]
    private Transform controlsParent;

    private void Start()
    {
        // Get the controls position in the world
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(controlsParent.position);
        // Set the y of the world position to the center of the field
        worldPos.Set(worldPos.x, Field.center.y, worldPos.z);
        // Convert back to screen point and set the transform
        controlsParent.position = Camera.main.WorldToScreenPoint(worldPos);
    }
}
