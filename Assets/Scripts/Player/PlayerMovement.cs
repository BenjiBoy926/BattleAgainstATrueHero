using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMusicStartListener
{
    [SerializeField]
    [Tooltip("Speed at which the player moves")]
    private float speed;

    // Reference to the rigidbody used to move the player
    private CachedComponent<Rigidbody2D> rb2D = new CachedComponent<Rigidbody2D>();

    // Store horizontal vertical movement input
    // Used so that we can get input in Update but apply it in FixedUpdate
    private float h;
    private float v;
    private Vector2 move = new Vector2();

    // Initial position. Stored so that we can move the player back to initial position at the start of the music
    private Vector2 initialPos;

    private void Awake()
    {
        initialPos = rb2D.Get(this).position;
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        move.Set(h, v);
    }

    private void FixedUpdate()
    {
        rb2D.Get(this).Shift(move, speed);
    }

    // Move to initial position and enable movement once the music begins
    public void OnMusicStart(MusicCursor cursor)
    {
        rb2D.Get(this).position = initialPos;
        enabled = true;
    }
    // Cannot move once player dies
    public void OnPlayerDeath()
    {
        enabled = false;
    }
}
