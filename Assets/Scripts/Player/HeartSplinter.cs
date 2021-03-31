using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartSplinter : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Speed that the splinter flies away from the heart")]
    private float speed;
    [SerializeField]
    [Tooltip("Max speed at which the splinter can rotate")]
    private float rotationSpeed;
    [SerializeField]
    [Tooltip("After this amount of time, the splinter destroys itself")]
    private float lifetime;

    private CachedComponent<Rigidbody2D> rb2D = new CachedComponent<Rigidbody2D>();

    // Start is called before the first frame update
    void Start()
    {
        rb2D.Get(this).velocity = Random.insideUnitCircle * speed;
        rb2D.Get(this).angularVelocity = Random.Range(-rotationSpeed, rotationSpeed);
        Destroy(gameObject, lifetime);
    }
}
