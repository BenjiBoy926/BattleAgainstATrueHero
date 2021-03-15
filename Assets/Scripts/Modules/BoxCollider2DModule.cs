﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoxCollider2DModule
{
    public static Vector2 WorldCenter(this BoxCollider2D boxCollider2D)
    {
        return (Vector2)boxCollider2D.transform.position + boxCollider2D.offset;
    }
    public static Vector2 Extents(this BoxCollider2D boxCollider2D)
    {
        return boxCollider2D.size / 2f;
    }

    // Coordinates of the different parts of the box
    public static float TopY(this BoxCollider2D box)
    {
        return box.WorldCenter().y + box.Extents().y;
    }
    public static float BottomY(this BoxCollider2D box)
    {
        return box.WorldCenter().y - box.Extents().y;
    }
    public static float LeftX(this BoxCollider2D box)
    {
        return box.WorldCenter().x - box.Extents().x;
    }
    public static float RightX(this BoxCollider2D box)
    {
        return box.WorldCenter().x + box.Extents().x;
    }
}
