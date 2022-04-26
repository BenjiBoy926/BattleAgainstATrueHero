using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UndertaleStyleText
{
    [System.Serializable]
    public class TextDelay
    {
        [SerializeField]
        [Tooltip("Index currently selected in the speed level enumerated type, or 'custom' if it is past the enum length")]
        private int enumValueIndex = 0;
        [SerializeField]
        [Tooltip("Time between each character reveal")]
        private float customDelay;

        public float GetDelay()
        {
            TextDelayLevel[] delayLevels = (TextDelayLevel[])System.Enum.GetValues(typeof(TextDelayLevel));
            // If the index is within range of the enum, then return that level in the settings
            if (enumValueIndex < delayLevels.Length) return Settings.GetTextDelay(delayLevels[enumValueIndex]);
            // If the index is not in range, return the custom delay
            else return customDelay;
        }
    }
}
