using UnityEngine;

public class LittleMonster : MonoBehaviour
{

    public Transform target;
    public float speed = 1f;
    private int hp = 20;
    Rigidbody2D rbody;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        // rbody.gravityScale = 0;
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        Physics2D.IgnoreLayerCollision(9, 10, true);
        Physics2D.IgnoreLayerCollision(9, 9, true);
    }

    void Update()
    {
        Vector2 dir = (target.position - transform.position).normalized;

        rbody.linearVelocity = dir * speed;

        if (target.position.x < transform.position.x) GetComponent<SpriteRenderer>().flipX = true;
        else GetComponent<SpriteRenderer>().flipX = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            // other.GetComponent<Move_Player>().onDamage(99999);
            Debug.Log("잡몹 충돌! 플레이어 즉사");
        }
    }
    
    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0) Destroy(gameObject);
    }
}
