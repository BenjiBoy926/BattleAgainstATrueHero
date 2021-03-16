using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Info on how to place the spear when it arrives
public struct SpearPositionInfo
{
    public static Vector2 padding = new Vector2(2f, 1f);

    // Whether the spear will start at a random position or a fixed position
    public enum PositionType
    {
        Fixed, Random
    }

    // Initial position of the spear
    public PositionType positionType { get; private set; }
    public Vector2 initialPosition { get; private set; }

    private SpearPositionInfo(PositionType positionType, Vector2 initialPosition)
    {
        this.positionType = positionType;
        this.initialPosition = initialPosition;
    }

    // Static factory methods allow clients to initialize the data without providing data that is ultimately unused
    public static SpearPositionInfo Random()
    {
        return new SpearPositionInfo(PositionType.Random, Vector2.zero);
    }
    public static SpearPositionInfo Fixed(Vector2 initialPosition)
    {
        return new SpearPositionInfo(PositionType.Fixed, initialPosition);
    }

    public Vector2 GetInitialPosition()
    {
        if (positionType == PositionType.Fixed)
        {
            return initialPosition;
        }
        else
        {
            // Select random x-y coordinates
            float x;
            float y = UnityEngine.Random.Range(Field.bottomY - padding.y, Field.topY + padding.y);

            // Select if we want to go on the left or right
            int side = UnityEngine.Random.Range(0, 2);

            if(side == 0)
            {
                x = Field.leftX - padding.x;
            }
            else
            {
                x = Field.rightX + padding.x;
            }

            return new Vector2(x, y);
        }
    }
}
