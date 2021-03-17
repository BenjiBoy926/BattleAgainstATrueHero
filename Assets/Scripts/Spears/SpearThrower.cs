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
            InstantiateAllSpears(cursor);
        }
    }

    public void OnMusicBeat(MusicCursor cursor)
    {
        foreach(Spear spear in spears)
        {
            spear.OnMusicBeat(cursor);
        }
    }

    private void InstantiateSpear(SpearPosition positionInfo, SpearDirection directionInfo, float rushSpeed, MusicCursor appearanceTime, MusicCursor rushTime)
    {
        Spear spear = Instantiate(spearPrefab, transform.parent);
        spear.Setup(positionInfo, directionInfo, rushSpeed, appearanceTime, rushTime);
        spears.Add(spear);
    }

    private void InstantiateSurroundSpears(float rushSpeed, MusicCursor appear, MusicCursor rush)
    {
        // Setup spear at top-left corner
        InstantiateSpear(SpearPosition.Fixed(Field.topLeftRadial),
            SpearDirection.Fixed(new Vector2(1f, -1f)),
            15f, appear, rush);

        // Setup spear at top-right corner
        InstantiateSpear(SpearPosition.Fixed(Field.topRightRadial),
            SpearDirection.Fixed(new Vector2(-1f, -1f)),
            15f, appear, rush);

        // Change start to hit on the second note
        appear = appear.Shift(0.75f);

        // Setup spear on the left
        InstantiateSpear(SpearPosition.Fixed(Field.leftRadial),
            SpearDirection.Fixed(Vector2.right),
            15f, appear, rush);

        // Setup spear on the right
        InstantiateSpear(SpearPosition.Fixed(Field.rightRadial),
            SpearDirection.Fixed(Vector2.left),
            15f, appear, rush);

        // Change appear to hit on the third note
        appear = appear.Shift(0.75f);

        // Setup spear on the bottom left
        InstantiateSpear(SpearPosition.Fixed(Field.bottomLeftRadial),
            SpearDirection.Fixed(Vector2.one),
            15f, appear, rush);

        // Setup spear on the bottom right
        InstantiateSpear(SpearPosition.Fixed(Field.bottomRightRadial),
            SpearDirection.Fixed(new Vector2(-1f, 1f)),
            15f, appear, rush);

        // Change start to hit on the fourth note
        appear = appear.Shift(0.75f);

        // Setup spear on the bottom
        InstantiateSpear(SpearPosition.Fixed(Field.bottomRadial),
            SpearDirection.Fixed(Vector2.up),
            15f, appear, rush);
    }

    private void InstantiateUppercutSpear(int spearIndex, float speed, MusicCursor appear, MusicCursor rush)
    {
        InstantiateSpear(SpearPosition.Fixed(new Vector2(
            MathModule.DiscreteLerp(Field.leftXInside, Field.rightXInside, spearIndex, 8),
            Field.bottomYOutside)),
            SpearDirection.Fixed(Vector2.up),
            speed, appear, rush);
    }

    private void InstantiateAllSpears(MusicCursor cursor)
    {
        MusicCursor appear;
        MusicCursor rush;

        /*
         * SECTION spears that appear with the low bass sounds
         */

        for (float i = 0f; i < 1.1f; i++)
        {
            rush = cursor.MoveTo(2f + i, 1f, 1f);
            appear = rush.Shift(-2f);
            InstantiateSpear(SpearPosition.Random(), SpearDirection.Homing(), 5f, appear, rush);

            rush = cursor.MoveTo(2f + i, 2f, 1f);
            appear = rush.Shift(-2f);
            InstantiateSpear(SpearPosition.Random(), SpearDirection.Homing(), 5f, appear, rush);

            rush = cursor.MoveTo(2f + i, 2f, 4f);
            appear = rush.Shift(-2f);
            InstantiateSpear(SpearPosition.Random(), SpearDirection.Homing(), 5f, appear, rush);

            rush = cursor.MoveTo(2f + i, 3f, 1f);
            appear = rush.Shift(-2f);
            InstantiateSpear(SpearPosition.Random(), SpearDirection.Homing(), 5f, appear, rush);

            if (i < 0.1f)
            {
                rush = cursor.MoveTo(2f, 4f, 1f);
                appear = rush.Shift(-2f);
                InstantiateSpear(SpearPosition.Random(), SpearDirection.Homing(), 5f, appear, rush);
            }
        }

        /*
         * SECTION: spears for the bass drop
         */
        appear = cursor.MoveTo(3f, 4f, 1f);
        rush = cursor.MoveTo(4f, 1f, 1f);
        InstantiateSurroundSpears(15f, appear, rush);

        /*
         * SECTION: spears for the music just after the bass drop
         */

        for (float i = 0f; i < 3.1f; i++)
        {
            rush = cursor.MoveTo(4f + i, 2f, 4f);

            appear = cursor.MoveTo(4f + i, 2f, 1f);
            InstantiateSpear(SpearPosition.Fixed(Field.topLeftRadial),
                SpearDirection.Fixed(new Vector2(1f, -1f)),
                15f, appear, rush);

            appear = cursor.MoveTo(4f + i, 2f, 2.5f);
            InstantiateSpear(SpearPosition.Fixed(Field.topRightRadial),
                SpearDirection.Fixed(-Vector2.one),
                15f, appear, rush);

            rush = cursor.MoveTo(4f + i, 3f, 4f);

            appear = cursor.MoveTo(4f + i, 3f, 1f);
            InstantiateSpear(SpearPosition.Fixed(Field.bottomLeftRadial),
                SpearDirection.Fixed(Vector2.one),
                15f, appear, rush);

            appear = cursor.MoveTo(4f + i, 3f, 2.5f);
            InstantiateSpear(SpearPosition.Fixed(Field.bottomRightRadial),
                SpearDirection.Fixed(new Vector2(-1f, 1f)),
                15f, appear, rush);

            // Make sure not to instantiate the surrounding spears before the transition into the next part of the music
            if (i < 2.1f)
            {
                rush = cursor.MoveTo(5f + i, 1f, 1f);
                appear = cursor.MoveTo(4f + i, 4f, 1f);
                InstantiateSurroundSpears(15f, appear, rush);
            }
        }

        /*
         * SECTION: spears for the music with the intense melody
         */

        for (float i = 0f; i < 4.1f; i += 4f)
        {
            for (float j = 0f; j < 2.1f; j += 2)
            {
                rush = cursor.MoveTo(8f + i + j, 1f, 1f);
                appear = rush.Shift(-3f);
                InstantiateUppercutSpear(0, 15f, appear, rush);

                rush = cursor.MoveTo(8f + i + j, 1f, 1.75f);
                appear = rush.Shift(-3f);
                InstantiateUppercutSpear(1, 15f, appear, rush);

                rush = cursor.MoveTo(8f + i + j, 1f, 2.5f);
                appear = rush.Shift(-3f);
                InstantiateUppercutSpear(3, 15f, appear, rush);

                rush = cursor.MoveTo(8f + i + j, 1f, 3f);
                appear = rush.Shift(-3f);
                InstantiateUppercutSpear(1, 5f, appear, rush);

                rush = cursor.MoveTo(8f + i + j, 2f, 4f);
                appear = rush.Shift(-3f);
                InstantiateUppercutSpear(2, 15f, appear, rush);

                rush = cursor.MoveTo(8f + i + j, 2f, 4.5f);
                appear = rush.Shift(-3f);
                InstantiateUppercutSpear(3, 15f, appear, rush);

                rush = cursor.MoveTo(8f + i + j, 3f, 1f);
                appear = rush.Shift(-3f);
                InstantiateUppercutSpear(4, 5f, appear, rush);

                rush = cursor.MoveTo(8f + i + j, 4f, 3f);
                appear = rush.Shift(-3f);
                InstantiateUppercutSpear(2, 15f, appear, rush);

                rush = cursor.MoveTo(8f + i + j, 4f, 3.5f);
                appear = rush.Shift(-3f);
                InstantiateUppercutSpear(3, 15f, appear, rush);

                rush = cursor.MoveTo(8f + i + j, 4f, 4f);
                appear = rush.Shift(-3f);
                InstantiateUppercutSpear(4, 15f, appear, rush);

                rush = cursor.MoveTo(8f + i + j, 4f, 4.5f);
                appear = rush.Shift(-3f);
                InstantiateUppercutSpear(5, 15f, appear, rush);
            }

            // Ending of that first phrase

            rush = cursor.MoveTo(9f + i, 1f, 1f);
            appear = rush.Shift(-3f);
            InstantiateUppercutSpear(2, 5f, appear, rush);

            rush = cursor.MoveTo(9f + i, 3f, 1f);
            appear = rush.Shift(-3f);
            InstantiateUppercutSpear(2, 5f, appear, rush);

            rush = cursor.MoveTo(9f + i, 3f, 4f);
            appear = rush.Shift(-3f);
            InstantiateUppercutSpear(1, 10f, appear, rush);

            rush = cursor.MoveTo(9f + i, 4f, 1f);
            appear = rush.Shift(-3f);
            InstantiateUppercutSpear(0, 5f, appear, rush);

            // Ending of the second phrase

            rush = cursor.MoveTo(11f + i, 1f, 1f);
            appear = rush.Shift(-3f);
            InstantiateUppercutSpear(6, 5f, appear, rush);

            rush = cursor.MoveTo(11f + i, 2f, 3f);
            appear = rush.Shift(-3f);
            InstantiateUppercutSpear(7, 10f, appear, rush);

            rush = cursor.MoveTo(11f + i, 3f, 1f);
            appear = rush.Shift(-3f);
            InstantiateUppercutSpear(5, 5f, appear, rush);
        }
    }
}
