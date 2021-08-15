using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UndertaleStyleText
{
    public struct ReaderRuntimeData
    {
        [Tooltip("Button that the user can click to advance the text")]
        public Button advanceButton;

        public GameObject AdvanceButtonObject => advanceButton == null ? null : advanceButton.gameObject;
    }
}

