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
    [Tooltip("Manage the controls for unbreakable mode")]
    private UnbreakableModePauseControls unbreakableMode;

    // Start is called before the first frame update
    public void Start(AudioSource audio)
    {
        unbreakableMode.Start(audio);
    }

    // Enable/disable the pause controls
    public void SetActive(bool active)
    {
        controlsParent.SetActive(active);
    }   
}
