using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Speed at which the player moves")]
    private float speed;

    // Reference to the rigidbody used to move the player
    private Rigidbody2D rb2D;

    // Store horizontal vertical movement input
    // Used so that we can get input in Update but apply it in FixedUpdate
    private float h;
    private float v;
    private Vector2 move = new Vector2();

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
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
        rb2D.Move(move, speed);
    }
}
