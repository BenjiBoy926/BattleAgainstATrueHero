using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UndertaleStyleText
{
    [CreateAssetMenu(fileName = "Dialogue Data", menuName = "Undertale Style Text/Dialogue Data")]
    public class DialogueData : ScriptableObject
    {
        public IReadOnlyList<Paragraph> Paragraphs => paragraphs;

        [SerializeField]
        [Tooltip("Default character expression for each new paragraph")]
        private CharacterExpression defaultExpression;
        [SerializeField]
        [Tooltip("Default reader settings to use for each new paragraph")]
        private ReaderSettings defaultReader;
        [SerializeField]
        [Tooltip("List of paragraphs to say")]
        private List<Paragraph> paragraphs;

        // To say the dialogue we need a dictionary mapping the name of the character
        // to the component references for that character
        public IEnumerator SayDialogue(Dictionary<string, CharacterReferences> references, ParagraphEvents[] events)
        {
            for (int i = 0; i < paragraphs.Count; i++)
            {
                events[i].StartEvent.Invoke();

                Paragraph p = paragraphs[i];
                string character = p.Expression.CharacterSettings.CharacterName;
                yield return p.SayParagraph(references[character]);

                events[i].EndEvent.Invoke();
            }
        }
    }
}