using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    // Box collider on the field
    private CachedComponent<BoxCollider2D> trigger = new CachedComponent<BoxCollider2D>();

    // Singleton pattern
    private static Field _instance = null;
    public static Field instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<Field>();
            }
            return _instance;
        }
    }

    // X-Y Positions
    public static float topY
    {
        get
        {
            return instance.trigger.Get(instance).TopY();
        }
    }
    public static float bottomY
    {
        get
        {
            return instance.trigger.Get(instance).BottomY();
        }
    }
    public static float leftX
    {
        get
        {
            return instance.trigger.Get(instance).LeftX();
        }
    }
    public static float rightX
    {
        get
        {
            return instance.trigger.Get(instance).RightX();
        }
    }

    // Diagonal extents
    public static float diagonalExtent
    {
        get
        {
            return instance.trigger.Get(instance).DiagonalExtent();
        }
    }

    // Center and size
    public static Vector2 center
    {
        get
        {
            return instance.trigger.Get(instance).WorldCenter();
        }
    }
    public static Vector2 size
    {
        get
        {
            return instance.trigger.Get(instance).size;
        }
    }
}
