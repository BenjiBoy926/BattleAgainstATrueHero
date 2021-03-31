using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Audio source that plays the game over jingle")]
    private AudioSource music;
    [SerializeField]
    [Tooltip("Main text that appears on the game over screen")]
    private Text titleText;
    [SerializeField]
    [Tooltip("Time it takes for the game over title text to fade it")]
    private float fadeTime;

    // Scene that the game over script loads when the game over is finished
    private static string sceneCallback;

    private void Start()
    {
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        // Fade the text in
        yield return ColorModule.Fade(Color.clear, Color.white, fadeTime, SetTextColor);
        
        // Wait a few seconds (this will change when we have a little monologue setup)
        yield return new WaitForSeconds(3f);

        // Fade the music and the text
        StartCoroutine(music.FadeOut(fadeTime));
        yield return ColorModule.Fade(Color.white, Color.clear, fadeTime, SetTextColor);

        // Load the scene that the game over manager was told to load after the game over screen
        SceneManager.LoadScene(sceneCallback);
    }

    private void SetTextColor(Color color)
    {
        titleText.color = color;
    }

    // Begin the game over by loading the scene and setting the scene to callback to
    public static void BeginGameOver(string sceneCallback)
    {
        GameOver.sceneCallback = sceneCallback;
        SceneManager.LoadScene("GameOver");
    }
}
