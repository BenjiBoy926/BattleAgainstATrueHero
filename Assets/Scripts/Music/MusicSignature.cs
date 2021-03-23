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

    // Given a beat in the song, get the current measure in the song
    public int GetMeasure(float beat)
    {
        List<TimeSignatureLocation> signatures = allSignatures;
        int measure = 1;
        int i = 0;

        // Loop through all signatures, or until beat is less than the beats per measure in current signature
        while(i < signatures.Count && beat > signatures[i].signature.beatsPerMeasure)
        {
            // Count the number of measures in this signature for the number of beats
            float measuresInSignature = (beat - 1f) / signatures[i].signature.beatsPerMeasure;
            measuresInSignature = Mathf.Min(CountMeasuresInSignature(i), measuresInSignature);

            // Accumulate measures in this signature
            measure += Mathf.FloorToInt(measuresInSignature);

            // Remove the beats that were accumulated to the total measures
            beat -= measuresInSignature * signatures[i].signature.beatsPerMeasure;

            // Increment to check the next signature
            i++;
        }

        return measure;
    }

    // Compute the number of beats between start and ending measures
    public float BeatsBetweenMeasures(MeasureRange range)
    {
        List<TimeSignatureLocation> signatures = allSignatures;
        float beats = 0f;
        
        for(int i = 0; i < signatures.Count; i++)
        {
            MeasureRange currentRange = MeasuresInSignature(i);

            // If the current range ends before the first measure of interest, 
            // continue to next signature
            if (currentRange.end < range.start)
                continue;

            // If the current range starts after the last measure of interest,
            // we can stop looping through time signatures
            if (currentRange.start > range.end)
                break;

            // Compute the number of measures in the range being checked
            MeasureRange overlap = range.Intersect(currentRange);
            beats += signatures[i].signature.beatsPerMeasure * overlap.length;
        }

        return beats;
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

    public int CountBeatsInSignature(int signature)
    {
        return 0;
    }

    // Get the measures in the current signature as a range from starting to ending measure
    // NOTE: the "end" property is the measure AFTER the last measure in the signature
    // So if a piece goes form 4/4 to 3/4 at measure 9, then the measure range for the 4/4
    // signature is 1-9, NOT 1-8
    public MeasureRange MeasuresInSignature(int signature)
    {
        int endMeasure;

        if (signature < allSignatures.Count - 1)
        {
            endMeasure = allSignatures[signature + 1].measure;
        }
        else endMeasure = int.MaxValue;

        return new MeasureRange(allSignatures[signature].measure, endMeasure);
    }
}
