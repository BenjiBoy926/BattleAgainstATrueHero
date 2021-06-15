using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PauseControls
{
    [SerializeField]
    [Tooltip("Parent of all the pause controls")]
    private GameObject controlsParent;
    [SerializeField]
    [Tooltip("Toggle that changes player unbreakable mode")]
    private Toggle unbreakableModeToggle;

    // Start is called before the first frame update
    public void Start()
    {
        // Setup toggle callback
    }

    // Enable/disable the pause controls
    public void SetActive(bool active)
    {
        controlsParent.SetActive(active);
    }   
}
