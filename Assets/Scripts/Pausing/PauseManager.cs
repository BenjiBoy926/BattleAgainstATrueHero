using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour, IMusicStartListener, IMusicEndListener
{
    [SerializeField]
    [TagSelector]
    [Tooltip("Tag of the object that has the script attached that manages the music")]
    private string musicTag = "GameController";
    [SerializeField]
    [Tooltip("Reference to the button that pauses the game")]
    private Button pauseButton;
    [SerializeField]
    [Tooltip("Button that unpauses the game")]
    private Button unpauseButton;
    [SerializeField]
    [Tooltip("Source used to play the audio for the pause manager")]
    private new AudioSource audio;
    [SerializeField]
    [Tooltip("Audio that plays when the game unpauses")]
    private AudioClip unpauseClip;
    [SerializeField]
    [Tooltip("Information on the controls that appear when the game is paused")]
    private PauseControls controls;
    [SerializeField]
    [Tooltip("Info for the countdown when unpausing the game")]
    private UnpauseCountdown countdown;

    private SynchronizedMusic music;
    // Time scale of the system before it was paused
    private float oldTimescale;
    // Holds the cursor, or "null" if the music hasn't started or it has already ended
    private MusicCursor? cursor = null;

    public static bool isPaused = false;

    // True if the music has started and false if it has not started
    private bool musicHasStarted => cursor.HasValue;

    private void Start()
    {
        // Find the music object
        music = GameObject.FindGameObjectWithTag(musicTag).GetComponent<SynchronizedMusic>();
        // Set pause to true when the pause button is clicked
        pauseButton.onClick.AddListener(() => Pause(true));
        // Set pause to false when unpause button is clicked
        unpauseButton.onClick.AddListener(() => Unpause());
        // Initialize the controls and disable them so they are invisible
        controls.Start(audio);
        controls.SetActive(false);
        // Initialize countdown
        countdown.Start();
    }

    // When music starts, assign cursor so we know the beat of the music
    public void OnMusicStart(MusicCursor cursor)
    {
        this.cursor = cursor;
    }
    // When music ends, null cursor again
    public void OnMusicEnd(MusicCursor cursor)
    {
        this.cursor = null;
    }

    public void Pause(bool useUI)
    {
        // Enable controls if we want to use them
        controls.SetActive(useUI);
        DoPause(true);
    }

    public void Unpause()
    {
        // In any case, disable the controls
        controls.SetActive(false);
        // If music has started, do a countdown before fully unpausing
        if (musicHasStarted) countdown.StartCountdown(this, cursor.Value, audio, DoPause);
        // If music has not started, unpause without a countdown
        else DoPause(false);
    }

    private void DoPause(bool pause)
    {
        // Set is paused
        isPaused = pause;
        // Pause button only interactable when not paused
        pauseButton.interactable = !pause;

        if(pause)
        {
            // Pause the music
            music.PauseMusic();
            // Make sure to save the old timescale so it is restored to normal on unpause
            oldTimescale = Time.timeScale;
            Time.timeScale = 0f;
        }
        else
        {
            // Resume the music
            music.ResumeMusic();
            // Restore the old timescale
            Time.timeScale = oldTimescale;
        }
    }
}
