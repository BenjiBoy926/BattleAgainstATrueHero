using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonologueAdvanceSettings
{
    [SerializeField]
    [Tooltip("If true, the monologue advances even if the game is paused using Time.timeScale = 0f")]
    private bool _advanceIfPaused = false;
    [SerializeField]
    [Tooltip("If true, the monologue will auto-advance after the speech is done")]
    private bool _autoAdvance;
    [SerializeField]
    [Tooltip("Amount of time to give the reader to read each speech part before advancing to the next")]
    private float _readTime;
    [SerializeField]
    [Tooltip("If advancing is not automatic, then this is the button that advances the text")]
    private string _advanceButton;
    [SerializeField]
    [Tooltip("If advancing is not automatic, this is the indicator used to signal that the speech is ready to advance")]
    private GameObject _advanceIndicator;

    public bool advanceIfPaused => _advanceIfPaused;
    public bool autoAdvance => _autoAdvance;
    public float readTime => _readTime;
    public string advanceButton => _advanceButton;
    public GameObject advanceIndicator => _advanceIndicator;

    public bool getAdvanceButtonDown
    {
        get
        {
            if(!_autoAdvance && (!_advanceIfPaused && !PauseManager.isPaused))
            {
                return Input.GetButtonDown(_advanceButton);
            }
            else
            {
                return false;
            }
        }
    }

    public void SetIndicatorActive(bool active)
    {
        if (!_autoAdvance)
        {
            _advanceIndicator.SetActive(active);
        }
    }
}
