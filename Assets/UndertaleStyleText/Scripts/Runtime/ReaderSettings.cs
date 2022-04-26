using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UndertaleStyleText
{
    [System.Serializable]
    public class ReaderSettings
    {
        // Get scaled/unscaled time based on local boolean variable
        public float MyTime => useCurrentTimescale ? Time.time : Time.unscaledTime;
        // True if the player pressed the "skip" button for this reader
        public bool Skip { get; private set; }

        [SerializeField]
        [Tooltip("Set the delay time between each character reveal")]
        private TextDelay delay;
        [SerializeField]
        [Tooltip("If true, the text advancement is affected by the current timescale. " +
            "If false, the text advances at the defined speed irrespective of timescale")]
        private bool useCurrentTimescale = true;
        [SerializeField]
        [Tooltip("If true, the text will auto-advance and cannot be skipped")]
        private bool autoAdvance;

        // Return a wait command that either waits until the current character is finished displaying,
        // or if auto advance is false, also additionally checks for a press of the advance button
        public WaitUntil CharacterWait(Button advanceButton)
        {
            // Set advance button object to its game object or null if we don't have a UI button
            float startTime = MyTime;

            // Reset skip to false
            Skip = false;

            // A function that returns true if it is time to skip this character
            // and false if it is not time yet
            bool DetermineCharacterSkip()
            {
                bool result = MyTime >= startTime + delay.GetDelay();

                // Return true we are not auto advancing and the advance button was clicked
                if (!autoAdvance && GetAdvanceButton(advanceButton))
                {
                    result = true;
                    Skip = true;
                }

                return result;
            }

            return new WaitUntil(DetermineCharacterSkip);
        }

        // Return a wait command that waits until the player is finished reading the finished text
        // For auto advance, we wait for the read time, for non auto advance we wait for the button to be pressed
        public WaitUntil ReadWait(Button advanceButton)
        {
            // Set advance button object to its game object or null if we don't have a UI button
            float startTime = MyTime;

            bool DetermineReadFinished()
            {
                // If we are auto advancing, return true when the read time has passed
                return autoAdvance && MyTime >= startTime + Settings.ReadTime ||
                    // Or, if we are not auto advancing, then wait for the advance button to be pressed
                    !autoAdvance && GetAdvanceButton(advanceButton);
            }

            return new WaitUntil(DetermineReadFinished);
        }

        public bool GetAdvanceButton(Button advanceButton)
        {
            GameObject advanceButtonObject = advanceButton == null ? null : advanceButton.gameObject;
            return (Input.GetButtonDown(Settings.AdvanceButtonName) || Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject == advanceButtonObject);
        }
    }
}
