using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour, IMusicBeatListener
{
    [SerializeField]
    [Tooltip("Item that is spawned by the spawner")]
    private GameObject item;
    [SerializeField]
    [Tooltip("List of points in the music when the items are spawned")]
    private List<MusicPosition> spawnTimes;

    private void Awake()
    {
        spawnTimes.Sort();
    }

    public void OnMusicBeat(MusicCursor cursor)
    {
        // Use binary search for efficient lookup
        int index = spawnTimes.BinarySearch(cursor.currentPosition);
        if (index >= 0) StartCoroutine(SpawnItemRoutine(cursor, spawnTimes[index]));
    }

    private IEnumerator SpawnItemRoutine(MusicCursor cursor, MusicPosition position)
    {
        yield return new WaitForSeconds(cursor.BeatsToSeconds(position.beat - position.baseBeat));
        SpawnItem();
    }

    private void SpawnItem()
    {
        float x = Random.Range(Field.leftXInside, Field.rightXInside);
        float y = Random.Range(Field.bottomYInside, Field.topYInside);
        Vector3 position = new Vector3(x, y, item.transform.position.z);
        Instantiate(item, position, item.transform.rotation);
    }
}
