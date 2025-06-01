using UnityEngine;

public class Element : MonoBehaviour
{

    public GameObject playerObject;
    public Vector2 vector;
    public int dmg;
    public int MaxDistance;
    private Rigidbody2D rbody;
    private Vector2 startPos;

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Boss"))
        {
            Boss boss = other.gameObject.GetComponent<Boss>();
            boss.OnDamage(dmg);
            Destroy(gameObject);
        }

        else if (other.CompareTag("Monster"))
        {
            Monster monster = other.GetComponent<Monster>();
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
        Physics2D.IgnoreLayerCollision(8, 12, true);
        startPos = transform.position;
    }

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
