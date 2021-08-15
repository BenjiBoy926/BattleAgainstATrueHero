using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UndertaleStyleText
{
    [System.Serializable]
    public class ParagraphSet
    {
        [SerializeField]
        [Tooltip("Index of the character in the settings that speaks these paragraphs")]
        private int characterIndex = -1;
        [SerializeField]
        [Tooltip("Visual type for the character that speaks these paragraphs")]
        private CharacterExpression.VisualExpressionType visualType;
        [SerializeField]
        [Tooltip("List of paragraphs to say")]
        private List<Paragraph> paragraphs;

        public IEnumerator SayParagraphs(CharacterRuntimeData characterData, ReaderRuntimeData readerData)
        {
            foreach (Paragraph p in paragraphs)
            {
                yield return p.SayParagraph(characterData, readerData);
            }
        }
    }
}
