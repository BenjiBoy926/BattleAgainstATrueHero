using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharaOffer : MonoBehaviour
{
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
    [Tooltip("Time after the scene opens that the opening monologue begins")]
    private float beginDelayTime = 1f;

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
        yield return monologue.Speak();
        SceneManager.LoadScene(sceneCallback);
    }
    public static void Begin(string sceneCallback)
    {
        CharaOffer.sceneCallback = sceneCallback;
        SceneManager.LoadScene("CharaOffer");
    }
}
