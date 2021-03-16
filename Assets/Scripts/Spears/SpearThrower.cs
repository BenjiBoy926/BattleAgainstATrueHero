using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearThrower : MonoBehaviour, IMusicStartListener, IMusicBeatListener
{
    [SerializeField]
    [Tooltip("Prefab for the spear")]
    private Spear spearPrefab;

    private List<Spear> spears = new List<Spear>();

    // Used to compute the positions of spears placed in circles around the field
    private float radius
    {
        get
        {
            return Field.diagonalExtent * 1.3f;
        }
    }
    private Vector2 topLeft
    {
        get
        {
            return Field.center + (new Vector2(-0.7f, 0.7f) * radius);
        }
    }
    private Vector2 topRight
    {
        get
        {
            return Field.center + (new Vector2(0.7f, 0.7f) * radius);
        }
    }
    private Vector2 bottomLeft
    {
        get
        {
            return Field.center + (new Vector2(-0.7f, -0.7f) * radius);
        }
    }
    private Vector2 bottomRight
    {
        get
        {
            return Field.center + (new Vector2(0.7f, -0.7f) * radius);
        }
    }
    private Vector2 left
    {
        get
        {
            return Field.center + (Vector2.left * radius);
        }
    }
    private Vector2 right
    {
        get
        {
            return Field.center + (Vector2.right * radius);
        }
    }
    private Vector2 bottom
    {
        get
        {
            return Field.center + (Vector2.down * radius);
        }
    }

    public void OnMusicStart(MusicCursor cursor)
    {
        if(spears.Count <= 0)
        {
            MusicCursor appear;
            MusicCursor rush;

            /*
             * SECTION spears that appear with the low bass sounds
             */

            for(float i = 0f; i < 1.1f; i++)
            {
                rush = cursor.MoveTo(2f + i, 1f, 1f);
                appear = rush.Shift(-2f);
                InstantiateSpear(SpearPositionInfo.Random(), SpearDirectionInfo.Homing(), 5f, appear, rush);

                rush = cursor.MoveTo(2f + i, 2f, 1f);
                appear = rush.Shift(-2f);
                InstantiateSpear(SpearPositionInfo.Random(), SpearDirectionInfo.Homing(), 5f, appear, rush);

                rush = cursor.MoveTo(2f + i, 2f, 4f);
                appear = rush.Shift(-2f);
                InstantiateSpear(SpearPositionInfo.Random(), SpearDirectionInfo.Homing(), 5f, appear, rush);

                rush = cursor.MoveTo(2f + i, 3f, 1f);
                appear = rush.Shift(-2f);
                InstantiateSpear(SpearPositionInfo.Random(), SpearDirectionInfo.Homing(), 5f, appear, rush);

                if(i < 0.1f)
                {
                    rush = cursor.MoveTo(2f, 4f, 1f);
                    appear = rush.Shift(-2f);
                    InstantiateSpear(SpearPositionInfo.Random(), SpearDirectionInfo.Homing(), 5f, appear, rush);
                }
            }

            /*
             * SECTION: spears for the bass drop
             */
            appear = cursor.MoveTo(3f, 4f, 1f);
            rush = cursor.MoveTo(4f, 1f, 1f);
            InstantiateSurroundSpears(15f, appear, rush);

            /*
             * SECTION: spears for the intense part of the music
             */

            for(float i = 0f; i < 3.1f; i++)
            {
                rush = cursor.MoveTo(4f + i, 2f, 4f);

                appear = cursor.MoveTo(4f + i, 2f, 1f);
                InstantiateSpear(SpearPositionInfo.Fixed(left),
                    SpearDirectionInfo.Fixed(Vector2.right),
                    15f, appear, rush);

                appear = cursor.MoveTo(4f + i, 2f, 2.5f);
                InstantiateSpear(SpearPositionInfo.Fixed(bottom),
                    SpearDirectionInfo.Fixed(Vector2.up),
                    15f, appear, rush);

                rush = cursor.MoveTo(4f + i, 3f, 4f);

                appear = cursor.MoveTo(4f + i, 3f, 1f);
                InstantiateSpear(SpearPositionInfo.Fixed(bottomLeft),
                    SpearDirectionInfo.Fixed(Vector2.one),
                    15f, appear, rush);

                appear = cursor.MoveTo(4f + i, 3f, 2.5f);
                InstantiateSpear(SpearPositionInfo.Fixed(bottomRight),
                    SpearDirectionInfo.Fixed(new Vector2(-1f, 1f)),
                    15f, appear, rush);

                // Make sure not to instantiate the surrounding spears before the transition into the next part of the music
                if(i < 2.1f)
                {
                    rush = cursor.MoveTo(5f + i, 1f, 1f);
                    appear = cursor.MoveTo(4f + i, 4f, 1f);
                    InstantiateSurroundSpears(15f, appear, rush);
                }
            }
            
        }
    }

    public void OnMusicBeat(MusicCursor cursor)
    {
        foreach(Spear spear in spears)
        {
            spear.OnMusicBeat(cursor);
        }
    }

    private void InstantiateSpear(SpearPositionInfo positionInfo, SpearDirectionInfo directionInfo, float rushSpeed, MusicCursor appearanceTime, MusicCursor rushTime)
    {
        Spear spear = Instantiate(spearPrefab, transform.parent);
        spear.Setup(positionInfo, directionInfo, rushSpeed, appearanceTime, rushTime);
        spears.Add(spear);
    }

    private void InstantiateSurroundSpears(float rushSpeed, MusicCursor appear, MusicCursor rush)
    {
        // Setup spear at top-left corner
        InstantiateSpear(SpearPositionInfo.Fixed(topLeft),
            SpearDirectionInfo.Fixed(new Vector2(1f, -1f)),
            15f, appear, rush);

        // Setup spear at top-right corner
        InstantiateSpear(SpearPositionInfo.Fixed(topRight),
            SpearDirectionInfo.Fixed(new Vector2(-1f, -1f)),
            15f, appear, rush);

        // Change start to hit on the second note
        appear = appear.Shift(0.75f);

        // Setup spear on the left
        InstantiateSpear(SpearPositionInfo.Fixed(left),
            SpearDirectionInfo.Fixed(Vector2.right),
            15f, appear, rush);

        // Setup spear on the right
        InstantiateSpear(SpearPositionInfo.Fixed(right),
            SpearDirectionInfo.Fixed(Vector2.left),
            15f, appear, rush);

        // Change appear to hit on the third note
        appear = appear.Shift(0.75f);

        // Setup spear on the bottom left
        InstantiateSpear(SpearPositionInfo.Fixed(bottomLeft),
            SpearDirectionInfo.Fixed(Vector2.one),
            15f, appear, rush);

        // Setup spear on the bottom right
        InstantiateSpear(SpearPositionInfo.Fixed(bottomRight),
            SpearDirectionInfo.Fixed(new Vector2(-1f, 1f)),
            15f, appear, rush);

        // Change start to hit on the fourth note
        appear = appear.Shift(0.75f);

        // Setup spear on the bottom
        InstantiateSpear(SpearPositionInfo.Fixed(bottom),
            SpearDirectionInfo.Fixed(Vector2.up),
            15f, appear, rush);
    }
}
