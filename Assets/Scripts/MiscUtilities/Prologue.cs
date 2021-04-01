using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Prologue : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Wait time at the start of the game before monologue begins")]
    private float startDelay;
    [SerializeField]
    [Tooltip("Wait time after the monoluge until game begins")]
    private float endDelay;
    [SerializeField]
    [Tooltip("Monster death sound effect that plays when undyne 'dies'")]
    private AudioClip deathClip;
    [SerializeField]
    [Tooltip("Name of the scene in the build settings that loads after the prologue")]
    private string nextSceneName;
    [SerializeField]
    [Tooltip("The monologue that begins the prologue")]
    private Monologue monologue;

    private CachedComponent<AudioSource> source = new CachedComponent<AudioSource>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PrologueRoutine());
    }

    private IEnumerator PrologueRoutine()
    {
        yield return new WaitForSeconds(startDelay);
        yield return monologue.Speak();

        // Play the death clip
        source.Get(this).clip = deathClip;
        source.Get(this).Play();

        // Wait for the audio source to finish playing
        yield return new WaitUntil(() => !source.Get(this).isPlaying);

        // Wait for the designated end delay
        yield return new WaitForSeconds(endDelay);
        SceneManager.LoadScene(nextSceneName);
    }
}
