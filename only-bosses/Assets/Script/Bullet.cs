using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public GameObject playerObject;
    public Vector2 vector;
    public int dmg;
    public int MaxDistance;
    private Rigidbody2D rbody;
    private Vector2 startPos;

    void OnTriggerEnter2D(Collider2D collison)
    {
        Boss boss = collison.gameObject.GetComponent<Boss>();
        if (boss != null)
        {
            Destroy(gameObject);
            boss.OnDamage(dmg);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        rbody.gravityScale = 0;
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        Physics2D.IgnoreLayerCollision(8, 7, true);
        Physics2D.IgnoreLayerCollision(8, 6, true);
        Physics2D.IgnoreLayerCollision(8, 8, true);
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(startPos, transform.position);
        if (distance >= MaxDistance)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        rbody.linearVelocity = vector * 10;
    }
}
