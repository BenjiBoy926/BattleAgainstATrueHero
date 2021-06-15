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

    // Get the current beat in the measure
    public int BeatInMeasure(int beat)
    {
        List<TimeSignatureLocation> signatures = allSignatures;
        int beatInMeasure = 0;

        for (int i = 0; i < signatures.Count; i++)
        {
            IntRange currentBeats = BeatsInSignature(i);

            // Check if the beat requested is in the beat range of the current signature
            if (beat >= currentBeats.min && beat < currentBeats.max)
            {
                // Compute the number of beats in this signature
                int beatsInSignature = beat - currentBeats.min;

                // Mod the number of beats into this signature by the number of beats per measure this signature
                return (beatsInSignature % signatures[i].signature.beatsPerMeasure) + 1;
            }
        }

        return beatInMeasure;
    }

    // Given the current beat in the music, compute the current measure in the music
    public int BeatToMeasure(int beat)
    {
        List<TimeSignatureLocation> signatures = allSignatures;
        int measure = 0;

        for(int i = 0; i < signatures.Count; i++)
        {
            IntRange currentBeats = BeatsInSignature(i);

            // Check if the beat requested is in the beat range of the current signature
            if(beat >= currentBeats.min && beat < currentBeats.max)
            {
                // Compute the number of beats in this signature
                int beatsInSignature = beat - currentBeats.min;

                // Compute number of measures in this signature and add it
                measure += beatsInSignature / signatures[i].signature.beatsPerMeasure + 1;

                // Now that we found the signature that the beat falls into, we can terminate the loop
                break;
            }
            // If the 
            else
            {
                measure += CountMeasuresInSignature(i);
            }
        }

        return measure;
    }
    // Given the measure number, compute the starting beat of the measure
    public int MeasureToBeat(int measure)
    {
        List<TimeSignatureLocation> signatures = allSignatures;
        int beat = 0;

        for (int i = 0; i < signatures.Count; i++)
        {
            IntRange currentMeasures = MeasuresInSignature(i);

            // Check if the measure requested is in the measure range of the current signature
            if (measure >= currentMeasures.min && measure < currentMeasures.max)
            {
                // Compute the number of measures in this signature
                int measuresInSignature = measure - currentMeasures.min;

                // Compute number of beats in this signature and add it
                beat += measuresInSignature * signatures[i].signature.beatsPerMeasure + 1;

                // Now that we found the signature that the measure falls into, we can terminate the loop
                break;
            }
            // If the 
            else
            {
                beat += CountBeatsInSignature(i);
            }
        }

        return beat;
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
    // NOTE: the "end" property is the measure AFTER the last measure in the signature
    // So if a piece changes signatures from 4/4 to 3/4 at measure 9, then the measure range for the 4/4
    // signature is 1-9, NOT 1-8
    public IntRange MeasuresInSignature(int signature)
    {
        if (signature < allSignatures.Count - 1)
        {
            int startMeasure = allSignatures[signature].measure;
            return new IntRange(startMeasure, startMeasure + CountMeasuresInSignature(signature));
        }
        // The last signature could have any number of measures in it, 
        // so return the max integer value
        else if (signature == allSignatures.Count - 1)
        {
            return new IntRange(allSignatures[signature].measure, int.MaxValue);
        }
        else
        {
            throw new System.IndexOutOfRangeException("Signature #" + signature + " is not defined in this music signature!");
        }
    }

    // Count the number of beats in the designated signature
    public int CountBeatsInSignature(int signature)
    {
        if (signature < allSignatures.Count - 1)
        {
            TimeSignatureLocation current = allSignatures[signature];
            return CountMeasuresInSignature(signature) * current.signature.beatsPerMeasure;
        }
        // The last signature could have any number of beats in it, 
        // so return the max integer value
        else if (signature == allSignatures.Count - 1)
        {
            return int.MaxValue;
        }
        else
        {
            throw new System.IndexOutOfRangeException("Signature #" + signature + " is not defined in this music signature!");
        }
    }

    // Return the beats inside the desired signature
    public IntRange BeatsInSignature(int signature)
    {
        int currentBeat = 0;

        // Check if signature is out of range
        if (signature >= allSignatures.Count)
        {
            throw new System.IndexOutOfRangeException("Signature #" + signature + " is not defined in this music signature!");
        }

        // Add the beats in all previous signatures
        for (int i = 0; i < signature; i++)
        {
            currentBeat += CountBeatsInSignature(i);
        }

        // Compute the start beat as the beat after the sum of the beats in previous signatures
        int startBeat = currentBeat + 1;

        if (signature < allSignatures.Count - 1)
        {
            return new IntRange(startBeat, startBeat + CountBeatsInSignature(signature));
        }
        // The last signature could have any number of beats in it, 
        // so return the max integer value
        else
        {
            return new IntRange(startBeat, int.MaxValue);
        }
    }
}
