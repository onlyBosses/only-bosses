using UnityEngine;

public class Gunner_skill1 : MonoBehaviour
{
    public int MaxDistance;
    public int dmg;
    public Vector2 vector;
    private Rigidbody2D rbody;
    private Vector2 startPos;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss"))
        {
            Boss boss = collision.GetComponent<Boss>();
            if (boss != null)
            {
                Destroy(gameObject);
                boss.setStiffenTime(75);
                boss.OnDamage(dmg);
            }
        }
        else if (collision.CompareTag("Monster"))
        {
            Monster monster = collision.GetComponent<Monster>();
            if (monster != null)
            {
                monster.OnDamage(dmg);
                Destroy(gameObject);
            }
        }
    }

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
        rbody.linearVelocity = vector * 10;
    }

    void FixedUpdate()
    {
        float distance = Vector2.Distance(startPos, transform.position);
        if (distance >= MaxDistance)
        {
            Destroy(gameObject);
        }
    }
}
