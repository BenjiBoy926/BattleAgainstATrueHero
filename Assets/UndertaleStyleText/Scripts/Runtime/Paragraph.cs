using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;


namespace UndertaleStyleText
{
    [System.Serializable]
    public class Paragraph
    {
        [SerializeField]
        [TextArea(3, 10)]
        [Tooltip("Text that the character speaks")]
        private string paragraph;
        [SerializeField]
        [Tooltip("Reference to the data on the character speaking this paragraph")]
        private CharacterExpression expression;
        [SerializeField]
        [Tooltip("Reference to the data on the how the text is read")]
        private ReaderSettings reader;
        [SerializeField]
        [Tooltip("Amount of time to wait before the paragraph is said")]
        private float delayTime;

        public IEnumerator SayParagraph(CharacterRuntimeData characterData, ReaderRuntimeData readerData)
        {
            characterData.text.text = "";

            // Animate the character and set the font of their text
            expression.SetFont(characterData);
            expression.SetVisualExpression(characterData);

            foreach (char c in paragraph)
            {
                // If the reader says to skip, then skip
                if (reader.Skip)
                {
                    characterData.text.text = paragraph;
                    expression.PlayVoiceClip(characterData);
                    break;
                }

                // Add the character to the text
                characterData.text.text += c;
                // If the character is not whitespace, play the speech sound
                if (!char.IsWhiteSpace(c)) expression.PlayVoiceClip(characterData);
                // Wait for one character to pass
                yield return reader.CharacterWait(readerData);
            }

            // Wait for the text to finish reading
            yield return reader.ReadWait(readerData);
        }
    }
}
