using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UndertaleStyleText
{
    public class Dialogue : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("If true, this monologue begins when the scene starts")]
        private bool sayOnAwake;
        [SerializeField]
        [Tooltip("Reference to the dialogue to say")]
        private DialogueData dialogue;
        [SerializeField]
        [Tooltip("Runtime data used by the paragraphs")]
        private List<LabelledCharacterReferences> characterReferences;
    }
}
