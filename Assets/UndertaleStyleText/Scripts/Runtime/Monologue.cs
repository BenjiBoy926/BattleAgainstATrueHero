using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UndertaleStyleText
{
    [CreateAssetMenu(fileName = "Monologue", menuName = "Undertale Style Text/Monologue")]
    public class Monologue : ScriptableObject
    {
        [SerializeField]
        [Tooltip("Set of paragraphs for the character to say")]
        private ParagraphSet paragraphs;

        public IEnumerator SayMonologue(CharacterRuntimeData characterData, ReaderRuntimeData readerData)
        {
            yield return paragraphs.SayParagraphs(characterData, readerData);
        }
    }
}