using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UndertaleStyleText
{
    public struct CharacterRuntimeData
    {
        [Tooltip("Object used to display the text as the character speaks")]
        public TextMeshProUGUI text;
        [Tooltip("Audio source for the character's voice clip")]
        public AudioSource voiceSource;
        [Tooltip("Animator used to animate the character")]
        public Animator animator;
        [Tooltip("Used to render the character's face or pose")]
        public SpriteRenderer visualRenderer;
        [Tooltip("Button that the user can click to advance the text")]
        public Button advanceButton;
        [Tooltip("Game object that displays the text")]
        public GameObject speechBubble;
    }
}
