using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UndertaleStyleText
{
    [System.Serializable]
    public class ParagraphEvents
    {
        #region Public Properties
        public UnityEvent StartEvent => startEvent;
        public UnityEvent EndEvent => endEvent;
        #endregion

        #region Private Fields
        [SerializeField]
        private UnityEvent startEvent;
        [SerializeField]
        private UnityEvent endEvent;
        #endregion
    }
}
