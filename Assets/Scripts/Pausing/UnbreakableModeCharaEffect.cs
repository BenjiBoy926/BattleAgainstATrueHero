using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnbreakableModeCharaEffect : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Start delay before the monologue begins")]
    private float monologueStartDelay;
    [SerializeField]
    [Tooltip("Reference to the root object that displays Chara")]
    private GameObject root;
    [SerializeField]
    [Tooltip("Parent of the panel that will display the monologue")]
    private GameObject speechPanel;
    [SerializeField]
    [Tooltip("Source of the audio for the chara effect")]
    private new AudioSource audio;
    [SerializeField]
    [Tooltip("Clip that plays when the effect is enabled")]
    private AudioClip enableClip;
    [SerializeField]
    [Tooltip("Monologue spoken when Chara appears after enabling unbreakable mode")]
    private Monologue monologue;

    public void Start()
    {
        SetActive(PlayerHealth.unbreakable);
    }

    public void SetActive(bool active)
    {
        root.SetActive(active);
        StopAllCoroutines();
        // If the effect is turning on and the game object is active in the heirarchy, start the monologue routine
        if (active && gameObject.activeInHierarchy)
        {
            // Play the enable clip
            audio.clip = enableClip;
            audio.Play();
            // Start the monologue routine
            StartCoroutine(MonologueRoutine());
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        speechPanel.SetActive(false);
    }

    private IEnumerator MonologueRoutine()
    {
        // To start, disable the panel
        speechPanel.SetActive(false);
        // Wait for seconds in realtime
        yield return new WaitForSecondsRealtime(monologueStartDelay);
        // Enable the panel before speaking
        speechPanel.SetActive(true);
        // Say the monologue
        yield return monologue.Speak();
        // Disable the panel when finished speaking
        speechPanel.SetActive(false);
    }
}
