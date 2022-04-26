using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UndertaleStyleText
{
    [System.Serializable]
    public class LabelledCharacterReferences
    {
        public CharacterReferences References => references;

        [SerializeField]
        [Tooltip("Name of the character that these references apply to")]
        private string characterName;
        [SerializeField]
        [Tooltip("References for this character")]
        private CharacterReferences references;
    }
}
