using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Hellmade.Sound;

public class CharaOffer : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Root of the objects that display Chara and speech")]
    private GameObject charaRoot;
    [SerializeField]
    [Tooltip("Parent of the object that asks if the user will accept or reject unbreakable mode")]
    private GameObject offerParent;
    [SerializeField]
    [Tooltip("Button the user clicks if they accept unbreakable mode")]
    private Button acceptButton;
    [SerializeField]
    [Tooltip("Button that the user clicks if they reject unbreakable mode")]
    private Button rejectButton;
    [SerializeField]
    [Tooltip("Clip that plays creepy music")]
    private AudioClip creepyMusicClip;
    [SerializeField]
    [Tooltip("Audio that plays when chara disappears")]
    private AudioClip disappearClip;
    [SerializeField]
    [Tooltip("Time after the scene opens that the opening monologue begins")]
    private float beginDelayTime = 1f;
    [SerializeField]
    [Tooltip("Time it takes after disappearing to load the next scene")]
    private float endDelayTime = 1f;

    [Header("Monologues")]

    [SerializeField]
    [Tooltip("Monologue that Chara speaks when they offer unbreakable mode")]
    private Monologue offerMonologue;
    [SerializeField]
    [Tooltip("Monologue that Chara speaks if you accept unbreakable mode")]
    private Monologue acceptMonologue;
    [SerializeField]
    [Tooltip("Monologue that Chara speaks if you reject unbreakable mode")]
    private Monologue rejectMonologue;

    private Audio creepyMusicAudio;
    private static string sceneCallback;

    // When the scene begins, 
    private void Start()
    {
        // Set unbreakable mode to true if the user accepts unbreakable mode
        acceptButton.onClick.AddListener(() =>
        {
            PlayerHealth.unbreakable = true;
            StartCoroutine(CharaOfferChoiceRoutine(acceptMonologue));
        });

        // Set unbreakable mode to false if the user rejects unbreakable mode
        rejectButton.onClick.AddListener(() =>
        {
            PlayerHealth.unbreakable = false;
            StartCoroutine(CharaOfferChoiceRoutine(rejectMonologue));
        });

        // Start the routine that offers unbreakable mode
        StartCoroutine(CharaOfferRoutine());
    }

    private IEnumerator CharaOfferRoutine()
    {
        yield return new WaitForSeconds(beginDelayTime);
        yield return offerMonologue.Speak();

        // Enable the objects that are used to offer unbreakable mode to the user
        offerParent.SetActive(true);
    }
    private IEnumerator CharaOfferChoiceRoutine(Monologue monologue)
    {
        StopCreepyMusic();
        offerParent.SetActive(false);
        yield return monologue.Speak();
        yield return EndRoutine();
    }
    private IEnumerator EndRoutine()
    {
        // Disable the root game object
        charaRoot.SetActive(false);
        // Play disappear audio
        EazySoundManager.PlaySound(disappearClip);

        yield return new WaitForSeconds(endDelayTime);

        SceneManager.LoadScene(sceneCallback);
    }
    public static void Begin(string sceneCallback)
    {
        CharaOffer.sceneCallback = sceneCallback;
        SceneManager.LoadScene("CharaOffer");
    }

    public void StartCreepyMusic()
    {
        int id = EazySoundManager.PlayMusic(creepyMusicClip, 1f, true, true);
        creepyMusicAudio = EazySoundManager.GetMusicAudio(id);
    }

    public void StopCreepyMusic()
    {
        if(creepyMusicAudio != null) creepyMusicAudio.Stop();
    }
}
