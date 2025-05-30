using UnityEngine;

public class Element : MonoBehaviour
{

    public GameObject playerObject;
    public Vector2 vector;
    public int dmg;
    public int MaxDistance;
    private Rigidbody2D rbody;
    private Vector2 startPos;

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Boss"))
        {
            BossInterface boss = collision.gameObject.GetComponent<BossInterface>();
            boss.OnDamage(dmg);
        }
            
        

        Destroy(gameObject);
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
