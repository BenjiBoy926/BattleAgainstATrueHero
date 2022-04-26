using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UndertaleStyleText
{
    [System.Serializable]
    public class CharacterReferences : MonoBehaviour
    {
        [Tooltip("Object used to display the text as the character speaks")]
        public TextMeshProUGUI text;
        [Tooltip("Audio source for the character's voice clip")]
        public AudioSource voiceSource;
        [Tooltip("Animator used to animate the character")]
        public Animator animator;
        [Tooltip("Used to render the character's face or pose")]
        public ImageOrSpriteRenderer visualRenderer;
        [Tooltip("Button that the user can click to advance the text")]
        public Button advanceButton;

        // Get the game object of the button if there is one
        public GameObject AdvanceButtonObject => advanceButton == null ? null : advanceButton.gameObject;
    }
}
