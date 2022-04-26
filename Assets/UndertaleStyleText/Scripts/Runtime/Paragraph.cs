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
        // Public access to the character's expression
        public CharacterExpression Expression => expression;

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

        public IEnumerator SayParagraph(CharacterReferences references)
        {
            references.text.text = "";

            // Animate the character and set the font of their text
            expression.SetFont(references.text);
            expression.SetVisualExpression(references.animator, references.visualRenderer);

            foreach (char c in paragraph)
            {
                // Add the character to the text
                references.text.text += c;
                // If the character is not whitespace, play the speech sound
                if (!char.IsWhiteSpace(c)) expression.PlayVoiceClip(references.voiceSource);
                // Wait for one character to pass
                yield return reader.CharacterWait(references.advanceButton);

                // If the reader says to skip, then skip
                if (reader.Skip)
                {
                    references.text.text = paragraph;
                    expression.PlayVoiceClip(references.voiceSource);
                    // Wait a frame to prevent the ReadWait immediately returning true
                    yield return null;
                    break;
                }
            }

            // Wait for the text to finish reading
            yield return reader.ReadWait(references.advanceButton);
            // We must wait another frame to prevent double-skipping back to back paragraphs
            yield return null;
        }
    }
}
