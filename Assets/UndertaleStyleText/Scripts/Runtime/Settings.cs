using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace UndertaleStyleText
{
    public class Settings : ScriptableObject
    {
        public enum TextSpeed
        {
            Fast, Medium, Slow
        }

        private static Settings instance;
        private static Settings Instance
        {
            get
            {
                // If instance is null, load it from resources
                if (instance == null)
                {
                    instance = Resources.Load<Settings>("UndertaleStyleTextSettings");

                    // If instance is still null after load, throw an exception
                    if (instance == null)
                    {
                        throw new MissingReferenceException("Cannot find the undertale style text settings object");
                    }
                }
                return instance;
            }
        }

        public static string[] CharacterNames => Instance.characterRoster.Select(x => x.CharacterName).ToArray();
        public static float ReadTime => Instance.readTime;
        public static string AdvanceButtonName => Instance.advanceButtonName;

        [SerializeField]
        [Tooltip("List of all the characters available in the text system")]
        private List<CharacterSettings> characterRoster;
        [SerializeField]
        [Tooltip("Different delays between the reveal of each character")]
        private float[] textSpeeds = { 0.1f, 0.2f, 0.3f };
        [SerializeField]
        [Tooltip("Default time given for a player to read a paragraph that is auto-advancing")]
        private float readTime = 1f;
        [SerializeField]
        [Tooltip("Name of the button in the input manager that advances the text")]
        private string advanceButtonName = "Submit";

        public static CharacterSettings GetCharacter(int index)
        {
            if (index >= 0 && index < Instance.characterRoster.Count) return Instance.characterRoster[index];
            else return null;
        }
        public static float GetTextSpeed(TextSpeed speed)
        {
            return Instance.textSpeeds[(int)speed];
        }
    }
}
