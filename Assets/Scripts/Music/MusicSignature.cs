using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MusicSignature
{
    [SerializeField]
    [Tooltip("List of time signatures throughout the piece of music")]
    private List<TimeSignatureLocation> signatures;
}
