using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMusicBeatListener
{
    void OnMusicBeat(MusicCursor cursor);
}
