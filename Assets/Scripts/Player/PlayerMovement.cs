﻿using System.Collections;
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

    public void OnMusicStart(MusicCursor cursor)
    {
        enabled = true;
    }
}
