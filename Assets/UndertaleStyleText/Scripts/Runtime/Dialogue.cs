using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UndertaleStyleText
{
    public class Dialogue : MonoBehaviour
    {
        #region Public Properties
        public DialogueData DialogueData => dialogueData;
        #endregion

        #region Private Properties
        private Dictionary<string, CharacterReferences> CharacterReferencesMap
        {
            get
            {
                Dictionary<string, CharacterReferences> result = new Dictionary<string, CharacterReferences>();
                foreach (LabelledCharacterReferences reference in characterReferences)
                    result.Add(reference.CharacterName, reference.References);
                return result;
            }
        }
        #endregion

        #region Private Fields
        [SerializeField]
        [Tooltip("If true, this monologue begins when the scene starts")]
        private bool sayOnAwake;
        [SerializeField]
        [Tooltip("Reference to the dialogue to say")]
        private DialogueData dialogueData;
        [SerializeField]
        [Tooltip("Runtime data used by the paragraphs")]
        private List<LabelledCharacterReferences> characterReferences;
        [SerializeField]
        private UnityEvent startEvent;
        [SerializeField]
        private ParagraphEvents[] paragraphEvents;
        [SerializeField]
        private UnityEvent endEvent;
        #endregion

        #region Public Methods
        public void SayDialogue()
        {
            StartCoroutine(DialogueRoutine());
        }
        #endregion

        #region Monobehaviour Messages
        private void Awake()
        {
            if (sayOnAwake)
                SayDialogue();
        }
        #endregion

        #region Private Methods
        private IEnumerator DialogueRoutine()
        {
            startEvent.Invoke();
            yield return dialogueData.SayDialogue(CharacterReferencesMap, paragraphEvents);
            endEvent.Invoke();
        }
        #endregion
    }
}
