using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearThrower : MonoBehaviour, IMusicStartListener, IMusicBeatListener
{
    [SerializeField]
    [Tooltip("Prefab for the spear")]
    private Spear spearPrefab;

    private List<Spear> spears = new List<Spear>();

    public void OnMusicStart(MusicCursor cursor)
    {
        if(spears.Count <= 0)
        {
            Spear spear = Instantiate(spearPrefab, transform.parent);

            MusicCursor start = cursor.MoveTo(3f, 4f, 1f);
            MusicCursor bassDrop = cursor.MoveTo(4f, 1f, 1f);

            spear.Setup(SpearPositionInfo.Fixed(new Vector2(-3.2f, 2f)),
                SpearDirectionInfo.Fixed(new Vector2(1f, -1f).normalized),
                10f, start, bassDrop);

            spears.Add(spear);
        }
    }

    public void OnMusicBeat(MusicCursor cursor)
    {
        foreach(Spear spear in spears)
        {
            spear.OnMusicBeat(cursor);
        }
    }
}
