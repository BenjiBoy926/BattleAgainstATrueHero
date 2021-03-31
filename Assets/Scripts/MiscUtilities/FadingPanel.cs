using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FadingPanel : MonoBehaviour
{
       

    [System.Serializable]
    private class ColorEvent : UnityEvent<Color> { }
}
