using UnityEngine;

public class LittleMonster : Monster
{

    public float speed = 1f;

    void Start()
    {
        init();
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
            Move_Player player = other.gameObject.GetComponent<Move_Player>();
            player.OnDamage(99999);
            Debug.Log("잡몹 충돌! 플레이어 즉사");
        }
    }
}
