using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SpearDirectionInfo
{
    // Whether the spear is thrown in a fixed direction or towards the player
    public enum DirectionType
    {
        Fixed, Homing
    }

    // Initial facing direction of the spear
    public DirectionType directionType { get; private set; }
    public Vector2 initialDirection { get; private set; }
    // Targets the player when it is a homing spear
    private System.Lazy<Transform> target;

    private SpearDirectionInfo(DirectionType directionType, Vector2 initialDirection)
    {
        this.directionType = directionType;
        this.initialDirection = initialDirection;

        // Lazily load the target when requested
        target = new System.Lazy<Transform>(() =>
        {
            return GameObject.FindGameObjectWithTag("Player").transform;
        });
    }

    // Static factories prevent clients from providing unnecessary information
    public static SpearDirectionInfo Homing()
    {
        return new SpearDirectionInfo(DirectionType.Homing, Vector2.zero);
    }
    public static SpearDirectionInfo Fixed(Vector2 initialDirection)
    {
        return new SpearDirectionInfo(DirectionType.Fixed, initialDirection);
    }

    public Vector2 GetDirection(Vector2 currentPosition)
    {
        if (directionType == DirectionType.Fixed)
        {
            return initialDirection;
        }
        else
        {
            return (Vector2)target.Value.position - currentPosition;
        }
    }
}
