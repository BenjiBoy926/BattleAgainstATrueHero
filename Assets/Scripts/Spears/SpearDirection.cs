using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SpearDirection
{
    // Whether the spear is thrown in a fixed direction or towards the player
    public enum Type
    {
        Fixed, Homing
    }

    // Initial facing direction of the spear
    public Type type { get; private set; }
    public Vector2 initialDirection { get; private set; }
    // Targets the player when it is a homing spear
    private System.Lazy<Transform> target;

    private SpearDirection(Type type, Vector2 initialDirection)
    {
        this.type = type;
        this.initialDirection = initialDirection;

        // Lazily load the target when requested
        target = new System.Lazy<Transform>(() =>
        {
            return GameObject.FindGameObjectWithTag("Player").transform;
        });
    }

    // Static factories prevent clients from providing unnecessary information
    public static SpearDirection Homing()
    {
        return new SpearDirection(Type.Homing, Vector2.zero);
    }
    public static SpearDirection Fixed(Vector2 initialDirection)
    {
        return new SpearDirection(Type.Fixed, initialDirection);
    }

    public Vector2 GetDirection(Vector2 currentPosition)
    {
        if (type == Type.Fixed)
        {
            return initialDirection;
        }
        else
        {
            return (Vector2)target.Value.position - currentPosition;
        }
    }
}
