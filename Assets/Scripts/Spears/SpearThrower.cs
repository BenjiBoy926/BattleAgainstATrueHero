﻿using System.Collections;
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
            rushSpeed, appear, rush);

        // Setup spear at top-right corner
        InstantiateSpear(SpearPosition.Fixed(Field.topRightRadial),
            SpearDirection.Fixed(new Vector2(-1f, -1f)),
            rushSpeed, appear, rush);

        // Change start to hit on the second note
        appear = appear.Shift(0.75f);

        // Setup spear on the left
        InstantiateSpear(SpearPosition.Fixed(Field.leftRadial),
            SpearDirection.Fixed(Vector2.right),
            rushSpeed, appear, rush);

        // Setup spear on the right
        InstantiateSpear(SpearPosition.Fixed(Field.rightRadial),
            SpearDirection.Fixed(Vector2.left),
            rushSpeed, appear, rush);

        // Change appear to hit on the third note
        appear = appear.Shift(0.75f);

        // Setup spear on the bottom left
        InstantiateSpear(SpearPosition.Fixed(Field.bottomLeftRadial),
            SpearDirection.Fixed(Vector2.one),
            rushSpeed, appear, rush);

        // Setup spear on the bottom right
        InstantiateSpear(SpearPosition.Fixed(Field.bottomRightRadial),
            SpearDirection.Fixed(new Vector2(-1f, 1f)),
            rushSpeed, appear, rush);

        // Change start to hit on the fourth note
        appear = appear.Shift(0.75f);

        // Setup spear on the bottom
        InstantiateSpear(SpearPosition.Fixed(Field.bottomRadial),
            SpearDirection.Fixed(Vector2.up),
            rushSpeed, appear, rush);
    }

    private void InstantiateUppercutSpear(int spearIndex, float speed, MusicCursor appear, MusicCursor rush)
    {
        InstantiateSpear(SpearPosition.Fixed(new Vector2(
            MathModule.DiscreteLerp(Field.leftXInside, Field.rightXInside, spearIndex, 8),
            Field.bottomYOutside)),
            SpearDirection.Fixed(Vector2.up),
            speed, appear, rush);
    }

    private void InstantiateSpearWall(Vector2 start, Vector2 end, int numSpears, Vector2 direction, float speed, MusicCursor appear, MusicCursor rush)
    {
        for(int i = 0; i < numSpears; i++)
        {
            Vector2 current = Vector2.Lerp(start, end, (float)i / (numSpears - 1));
            InstantiateSpear(SpearPosition.Fixed(current), SpearDirection.Fixed(direction), speed, appear, rush);
        }
    }

    private void InstantiateAllSpears(MusicCursor cursor)
    {
        MusicCursor appear;
        MusicCursor rush;

        /*
         * SECTION spears that appear with the low bass sounds
         */

        rush = cursor.MoveTo(3, 1, 1f);
        appear = rush.Shift(-2f);
        InstantiateSpear(SpearPosition.Random(), SpearDirection.Homing(), 5f, appear, rush);

        rush = cursor.MoveTo(3, 2, 1f);
        appear = rush.Shift(-2f);
        InstantiateSpear(SpearPosition.Random(), SpearDirection.Homing(), 5f, appear, rush);

        rush = cursor.MoveTo(3, 2, 4f);
        appear = rush.Shift(-2f);
        InstantiateSpear(SpearPosition.Random(), SpearDirection.Homing(), 5f, appear, rush);

        rush = cursor.MoveTo(3, 3, 1f);
        appear = rush.Shift(-2f);
        InstantiateSpear(SpearPosition.Random(), SpearDirection.Homing(), 5f, appear, rush);

        /*
         * SECTION: surround spears for the music at and after the bass drop
         */

        for (int i = 0; i < 2; i++)
        {
            rush = cursor.MoveTo(4 + i, 1, 1f);
            appear = cursor.MoveTo(3 + i, 4, 1f);
            InstantiateSurroundSpears(15f, appear, rush);
        }

        /*
         * SECTION: random slow-moving homing spears for the part with the low, drawn out notes just after the bass drop
         */ 

        for(int i = 0; i < 2; i++)
        {
            rush = cursor.MoveTo(6 + i, 1, 1f);
            appear = rush.Shift(-2f);
            InstantiateSpear(SpearPosition.Random(), SpearDirection.Homing(), 2f, appear, rush);

            rush = cursor.MoveTo(6 + i, 3, 1f);
            appear = rush.Shift(-2f);
            InstantiateSpear(SpearPosition.Random(), SpearDirection.Homing(), 5f, appear, rush);

            rush = cursor.MoveTo(6 + i, 4, 1f);
            appear = rush.Shift(-2f);
            InstantiateSpear(SpearPosition.Random(), SpearDirection.Homing(), 5f, appear, rush);
        }

        /*
         * SECTION: spears for the music with the intense melody
         */

        for (int i = 0; i < 5; i += 4)
        {
            for (int j = 0; j < 3; j += 2)
            {
                rush = cursor.MoveTo(8 + i + j, 1, 1f);
                appear = rush.Shift(-3f);
                InstantiateUppercutSpear(0, 15f, appear, rush);

                rush = cursor.MoveTo(8 + i + j, 1, 1.75f);
                appear = rush.Shift(-3f);
                InstantiateUppercutSpear(1, 15f, appear, rush);

                rush = cursor.MoveTo(8 + i + j, 1, 2.5f);
                appear = rush.Shift(-3f);
                InstantiateUppercutSpear(3, 15f, appear, rush);

                rush = cursor.MoveTo(8 + i + j, 1, 3f);
                appear = rush.Shift(-3f);
                InstantiateUppercutSpear(1, 5f, appear, rush);

                rush = cursor.MoveTo(8 + i + j, 2, 4f);
                appear = rush.Shift(-3f);
                InstantiateUppercutSpear(2, 15f, appear, rush);

                rush = cursor.MoveTo(8 + i + j, 2, 4.5f);
                appear = rush.Shift(-3f);
                InstantiateUppercutSpear(3, 15f, appear, rush);

                rush = cursor.MoveTo(8 + i + j, 3, 1f);
                appear = rush.Shift(-3f);
                InstantiateUppercutSpear(4, 5f, appear, rush);

                rush = cursor.MoveTo(8 + i + j, 4, 3f);
                appear = rush.Shift(-3f);
                InstantiateUppercutSpear(2, 15f, appear, rush);

                rush = cursor.MoveTo(8 + i + j, 4, 3.5f);
                appear = rush.Shift(-3f);
                InstantiateUppercutSpear(3, 15f, appear, rush);

                rush = cursor.MoveTo(8 + i + j, 4, 4f);
                appear = rush.Shift(-3f);
                InstantiateUppercutSpear(4, 15f, appear, rush);

                rush = cursor.MoveTo(8 + i + j, 4, 4.5f);
                appear = rush.Shift(-3f);
                InstantiateUppercutSpear(5, 15f, appear, rush);
            }

            // Ending of that first phrase

            rush = cursor.MoveTo(9 + i, 1, 1f);
            appear = rush.Shift(-3f);
            InstantiateUppercutSpear(2, 5f, appear, rush);

            rush = cursor.MoveTo(9 + i, 3, 1f);
            appear = rush.Shift(-3f);
            InstantiateUppercutSpear(2, 5f, appear, rush);

            rush = cursor.MoveTo(9 + i, 3, 4f);
            appear = rush.Shift(-3f);
            InstantiateUppercutSpear(1, 10f, appear, rush);

            rush = cursor.MoveTo(9 + i, 4, 1f);
            appear = rush.Shift(-3f);
            InstantiateUppercutSpear(0, 5f, appear, rush);

            // Ending of the second phrase

            rush = cursor.MoveTo(11 + i, 1, 1f);
            appear = rush.Shift(-3f);
            InstantiateUppercutSpear(6, 5f, appear, rush);

            rush = cursor.MoveTo(11 + i, 2, 3f);
            appear = rush.Shift(-3f);
            InstantiateUppercutSpear(7, 10f, appear, rush);

            rush = cursor.MoveTo(11 + i, 3, 1f);
            appear = rush.Shift(-3f);
            InstantiateUppercutSpear(5, 5f, appear, rush);
        }

        // SECTION: this is the slow section before the final push to the end of the song!

        for (int i = 0; i < 4; i += 2)
        {
            appear = cursor.MoveTo(16 + i, 1, 1f);
            rush = cursor.MoveTo(16 + i, 2, 1f);

            InstantiateSpear(SpearPosition.Fixed(new Vector2(Field.rightXOutside, Field.topYInside)),
                SpearDirection.Fixed(Vector2.left), 5f, appear, rush);
            InstantiateSpear(SpearPosition.Fixed(new Vector2(Field.rightXOutside, Field.center.y)),
                SpearDirection.Fixed(Vector2.left), 5f, appear, rush);
            InstantiateSpear(SpearPosition.Fixed(new Vector2(Field.rightXOutside, Field.bottomYInside)),
                SpearDirection.Fixed(Vector2.left), 5f, appear, rush);

            InstantiateSpear(SpearPosition.Fixed(new Vector2(Field.leftXOutside, Field.topYInside)),
                SpearDirection.Fixed(Vector2.right), 5f, appear, rush);
            InstantiateSpear(SpearPosition.Fixed(new Vector2(Field.leftXOutside, Field.center.y)),
                SpearDirection.Fixed(Vector2.right), 5f, appear, rush);
            InstantiateSpear(SpearPosition.Fixed(new Vector2(Field.leftXOutside, Field.bottomYInside)),
                SpearDirection.Fixed(Vector2.right), 5f, appear, rush);

            appear = cursor.MoveTo(16 + i, 3, 1f);
            rush = cursor.MoveTo(16 + i, 4, 1f);

            InstantiateSpear(SpearPosition.Fixed(new Vector2(Field.rightXInside, Field.bottomYOutside)),
                SpearDirection.Fixed(Vector2.up), 5f, appear, rush);
            InstantiateSpear(SpearPosition.Fixed(new Vector2(Field.center.x, Field.bottomYOutside)),
                SpearDirection.Fixed(Vector2.up), 5f, appear, rush);
            InstantiateSpear(SpearPosition.Fixed(new Vector2(Field.leftXInside, Field.bottomYOutside)),
                SpearDirection.Fixed(Vector2.up), 5f, appear, rush);
        }
        
        // First time, instantiate a horizontal wall of spears
        appear = cursor.MoveTo(17, 1, 1f);
        rush = cursor.MoveTo(17, 2, 1f);

        // Spears with some space on the bottom of the wall to slip through
        InstantiateSpearWall(new Vector2(Field.leftXOutside, Field.topYInside),
            new Vector2(Field.leftXOutside, Mathf.Lerp(Field.bottomYInside, Field.topYInside, 0.25f)),
            5, Vector2.right, 5f, appear, rush);

        appear = cursor.MoveTo(17, 3, 1f);
        rush = cursor.MoveTo(17, 4, 1f);

        // Spears with some space on the top of the wall to slip through
        InstantiateSpearWall(new Vector2(Field.leftXOutside, Field.bottomYInside),
            new Vector2(Field.leftXOutside, Mathf.Lerp(Field.bottomYInside, Field.topYInside, 0.75f)),
            5, Vector2.right, 5f, appear, rush);

        // Next time instantiate a vertical wall of spears
        appear = cursor.MoveTo(19, 1, 1f);
        rush = cursor.MoveTo(19, 2, 1f);

        // Instantiate vertial wall with some space to slip through on the right
        InstantiateSpearWall(new Vector2(Field.leftXInside, Field.bottomYOutside),
            new Vector2(Mathf.Lerp(Field.leftXInside, Field.rightXInside, 0.75f), Field.bottomYOutside),
            5, Vector2.up, 5f, appear, rush);

        appear = cursor.MoveTo(19, 3, 1f);
        rush = cursor.MoveTo(19, 4, 1f);

        // Instantiate vertical wall with some space to slip through on the left
        InstantiateSpearWall(new Vector2(Mathf.Lerp(Field.leftXInside, Field.rightXInside, 0.25f), Field.bottomYOutside),
            new Vector2(Field.rightXInside, Field.bottomYOutside),
            5, Vector2.up, 5f, appear, rush);

        // SECITON: the final push to the end!

        for (int j = 0; j < 5; j += 4)
        {
            for (int i = 0; i < 3; i++)
            {
                rush = cursor.MoveTo(20 + i + j, 1, 1f);
                appear = rush.Shift(-3f);

                // Top two spears
                InstantiateSpear(SpearPosition.Fixed(new Vector2(Field.rightXOutside, Field.topYInside)),
                    SpearDirection.Fixed(Vector2.left), 25f, appear, rush);
                rush = cursor.MoveTo(20 + i + j, 1, 1.5f);
                InstantiateSpear(SpearPosition.Fixed(new Vector2(Field.leftXOutside, Field.topYInside)),
                    SpearDirection.Fixed(Vector2.right), 25f, appear, rush);

                // Middle two spears
                rush = cursor.MoveTo(20 + i + j, 1, 2f);
                InstantiateSpear(SpearPosition.Fixed(new Vector2(Field.rightXOutside, Field.center.y)),
                    SpearDirection.Fixed(Vector2.left), 25f, appear, rush);
                rush = cursor.MoveTo(20 + i + j, 1, 2.5f);
                InstantiateSpear(SpearPosition.Fixed(new Vector2(Field.leftXOutside, Field.center.y)),
                    SpearDirection.Fixed(Vector2.right), 25f, appear, rush);

                // Bottom two spears
                rush = cursor.MoveTo(20 + i + j, 1, 3f);
                InstantiateSpear(SpearPosition.Fixed(new Vector2(Field.rightXOutside, Field.bottomYInside)),
                    SpearDirection.Fixed(Vector2.left), 25f, appear, rush);
                rush = cursor.MoveTo(20 + i + j, 1, 3.5f);
                InstantiateSpear(SpearPosition.Fixed(new Vector2(Field.leftXOutside, Field.bottomYInside)),
                    SpearDirection.Fixed(Vector2.right), 25f, appear, rush);

                // Rush from bottom to top
                rush = cursor.MoveTo(20 + i + j, 2, 1f);
                InstantiateSpear(SpearPosition.Fixed(new Vector2(Field.rightXInside, Field.bottomYOutside)),
                    SpearDirection.Fixed(Vector2.up), 25f, appear, rush);
                InstantiateSpear(SpearPosition.Fixed(new Vector2(Field.center.x, Field.bottomYOutside)),
                    SpearDirection.Fixed(Vector2.up), 25f, appear, rush);
                InstantiateSpear(SpearPosition.Fixed(new Vector2(Field.leftXInside, Field.bottomYOutside)),
                    SpearDirection.Fixed(Vector2.up), 25f, appear, rush);
            }

            for(int i = 0; i < 2; i++)
            {
                // First time, instantiate a horizontal wall of spears
                rush = cursor.MoveTo(20 + j, 3, 1f);
                appear = rush.Shift(-2f);

                // Spears with some space on the bottom of the wall to slip through
                InstantiateSpearWall(new Vector2(Field.leftXOutside, Field.topYInside),
                    new Vector2(Field.leftXOutside, Mathf.Lerp(Field.bottomYInside, Field.topYInside, 0.25f)),
                    5, Vector2.right, 15f, appear, rush);

                rush = cursor.MoveTo(20 + j, 4, 1f);
                appear = rush.Shift(-2f);

                // Spears with some space on the top of the wall to slip through
                InstantiateSpearWall(new Vector2(Field.leftXOutside, Field.bottomYInside),
                    new Vector2(Field.leftXOutside, Mathf.Lerp(Field.bottomYInside, Field.topYInside, 0.75f)),
                    5, Vector2.right, 15f, appear, rush);

                // Next time instantiate a vertical wall of spears
                rush = cursor.MoveTo(21 + j, 3, 1f);
                appear = rush.Shift(-2f);

                // Instantiate vertial wall with some space to slip through on the right
                InstantiateSpearWall(new Vector2(Field.leftXInside, Field.bottomYOutside),
                    new Vector2(Mathf.Lerp(Field.leftXInside, Field.rightXInside, 0.75f), Field.bottomYOutside),
                    5, Vector2.up, 15f, appear, rush);

                rush = cursor.MoveTo(21 + j, 4, 1f);
                appear = rush.Shift(-2f);

                // Instantiate vertical wall with some space to slip through on the left
                InstantiateSpearWall(new Vector2(Mathf.Lerp(Field.leftXInside, Field.rightXInside, 0.25f), Field.bottomYOutside),
                    new Vector2(Field.rightXInside, Field.bottomYOutside),
                    5, Vector2.up, 15f, appear, rush);

                // Third time, instantiate a horizontal wall of spears from right to left
                rush = cursor.MoveTo(22 + j, 3, 1f);
                appear = rush.Shift(-2f);

                // Spears with some space on the bottom of the wall to slip through
                InstantiateSpearWall(new Vector2(Field.rightXOutside, Field.topYInside),
                    new Vector2(Field.rightXOutside, Mathf.Lerp(Field.bottomYInside, Field.topYInside, 0.25f)),
                    5, Vector2.left, 15f, appear, rush);

                rush = cursor.MoveTo(22 + j, 4, 1f);
                appear = rush.Shift(-2f);

                // Spears with some space on the top of the wall to slip through
                InstantiateSpearWall(new Vector2(Field.rightXOutside, Field.bottomYInside),
                    new Vector2(Field.rightXOutside, Mathf.Lerp(Field.bottomYInside, Field.topYInside, 0.75f)),
                    5, Vector2.left, 15f, appear, rush);

                appear = cursor.MoveTo(23 + j, 1, 1f);

                // Setup spear at top-left corner
                rush = cursor.MoveTo(23 + j, 1, 2.5f);
                InstantiateSpear(SpearPosition.Fixed(Field.topLeftRadial),
                    SpearDirection.Fixed(new Vector2(1f, -1f)),
                    25f, appear, rush);

                // Setup spear on the left
                rush = cursor.MoveTo(23 + j, 1, 3f);
                InstantiateSpear(SpearPosition.Fixed(Field.leftRadial),
                    SpearDirection.Fixed(Vector2.right),
                    25f, appear, rush);

                // Setup spear on the bottom left
                rush = cursor.MoveTo(23 + j, 1, 3.5f);
                InstantiateSpear(SpearPosition.Fixed(Field.bottomLeftRadial),
                    SpearDirection.Fixed(Vector2.one),
                    25f, appear, rush);

                // Setup spear on the bottom
                rush = cursor.MoveTo(23 + j, 2, 1f);
                InstantiateSpear(SpearPosition.Fixed(Field.bottomRadial),
                    SpearDirection.Fixed(Vector2.up),
                    15f, appear, rush);

                // Setup spear on the bottom right
                rush = cursor.MoveTo(23 + j, 2, 2f);
                InstantiateSpear(SpearPosition.Fixed(Field.bottomRightRadial),
                    SpearDirection.Fixed(new Vector2(-1f, 1f)),
                    15f, appear, rush);

                // Setup spear on the right
                rush = cursor.MoveTo(23 + j, 2, 3f);
                InstantiateSpear(SpearPosition.Fixed(Field.rightRadial),
                    SpearDirection.Fixed(Vector2.left),
                    15f, appear, rush);

                // Setup spear at top-right corner
                rush = cursor.MoveTo(23 + j, 3, 1f);
                InstantiateSpear(SpearPosition.Fixed(Field.topRightRadial),
                    SpearDirection.Fixed(new Vector2(-1f, -1f)),
                    5f, appear, rush);
            }
        }
    }
}
