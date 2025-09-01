using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Used for objects spawning on the inside edge of the field")]
    private float insideMargin = 0.5f;
    [SerializeField]
    [Tooltip("Used for objects spawning on the outside edge of the field")]
    private float outsideMargin = 1f;

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

    // X-Y positions roughly inside the border
    public static float topYInside
    {
        get
        {
            return topY - instance.insideMargin;
        }
    }
    public static float bottomYInside
    {
        get
        {
            return bottomY + instance.insideMargin;
        }
    }
    public static float leftXInside
    {
        get
        {
            return leftX + instance.insideMargin;
        }
    }
    public static float rightXInside
    {
        get
        {
            return rightX - instance.insideMargin;
        }
    }
    // X-Y positions roughly inside the border
    public static float topYOutside
    {
        get
        {
            return topY + instance.outsideMargin;
        }
    }
    public static float bottomYOutside
    {
        get
        {
            return bottomY - instance.outsideMargin;
        }
    }
    public static float leftXOutside
    {
        get
        {
            return leftX - instance.outsideMargin;
        }
    }
    public static float rightXOutside
    {
        get
        {
            return rightX + instance.outsideMargin;
        }
    }

    // Radial positions around the field
    public static float radius
    {
        get
        {
            return diagonalExtent * 1.3f;
        }
    }
    public static Vector2 topLeftRadial
    {
        get
        {
            return center + (new Vector2(-0.7f, 0.7f) * radius);
        }
    }
    public static Vector2 topRightRadial
    {
        get
        {
            return center + (new Vector2(0.7f, 0.7f) * radius);
        }
    }
    public static Vector2 bottomLeftRadial
    {
        get
        {
            return center + (new Vector2(-0.7f, -0.7f) * radius);
        }
    }
    public static Vector2 bottomRightRadial
    {
        get
        {
            return center + (new Vector2(0.7f, -0.7f) * radius);
        }
    }
    public static Vector2 leftRadial
    {
        get
        {
            return center + (Vector2.left * radius);
        }
    }
    public static Vector2 rightRadial
    {
        get
        {
            return center + (Vector2.right * radius);
        }
    }
    public static Vector2 bottomRadial
    {
        get
        {
            return center + (Vector2.down * radius);
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
    public static Vector2 insideMarginVector
    {
        get
        {
            return new Vector2(2f * instance.insideMargin, 2f * instance.insideMargin);
        }
    }
    public static Vector2 insideSize
    {
        get
        {
            return size - insideMarginVector;
        }
    }
    public static Vector2 outsideMarginVector => new Vector2(2f * instance.outsideMargin, 2f * instance.outsideMargin);
    public static Vector2 outsideSize => size + outsideMarginVector;
}
