using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TimeSignatureLocation
{
    [SerializeField]
    [Tooltip("The time signature at this point in the music")]
    private TimeSignature _signature;
    [SerializeField]
    [Tooltip("The measure number where the signature begins")]
    private int _measure;

    public TimeSignature signature => _signature;
    public int measure => _measure;
}
