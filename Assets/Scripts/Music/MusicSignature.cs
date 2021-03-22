using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MusicSignature
{
    [SerializeField]
    [Tooltip("The time signature for the first measure")]
    private TimeSignature _startingSignature;
    [SerializeField]
    [Tooltip("List of time signatures throughout the piece of music")]
    private List<TimeSignatureLocation> subsequentSignatures;

    public TimeSignatureLocation startingSignature => new TimeSignatureLocation(_startingSignature, 1);

    // Property to get all time signatures as time signature locations
    public List<TimeSignatureLocation> allSignatures
    {
        get
        {
            List<TimeSignatureLocation> timeSignatureLocations = new List<TimeSignatureLocation>();
            timeSignatureLocations.Add(startingSignature);
            timeSignatureLocations.AddRange(subsequentSignatures);
            timeSignatureLocations.Sort();
            return timeSignatureLocations;
        }
    }

    // Given the signature number, return the number of measures that that signature is applied to in the music
    public int CountMeasuresInSignature(int signature)
    {
        if(signature < allSignatures.Count - 1)
        {
            return allSignatures[signature + 1].measure - allSignatures[signature].measure;
        }
        // The last signature could have any number of measures in it, 
        // so return the max integer value
        else if(signature == allSignatures.Count - 1)
        {
            return int.MaxValue;
        }
        else
        {
            throw new System.IndexOutOfRangeException("Signature #" + signature + " is not defined in this music signature!");
        }
    }

    // Get the measures in the current signature as a range from starting to ending measure
    public MeasureRange MeasuresInSignature(int signature)
    {
        return new MeasureRange(allSignatures[signature].measure, CountMeasuresInSignature(signature));
    }
}
