using System.Collections;
using UnityEngine;

public class Boss2_summon : Monster
{
    private int damage;
    private int attackCoolTime;
    void OnCollisionEnter2D(Collision2D collision)
    {
        Move_Player mPlayer = collision.gameObject.GetComponent<Move_Player>();
        if (mPlayer != null)
        {
            rbody.linearVelocity = Vector2.zero;
            mPlayer.OnDamage(damage);
        }

    }

    void Start()
    {
        init();
        rbody.gravityScale = 0;
        damage = 10;
        attackCoolTime = Random.Range(0, 4) * 50;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        
        if (attackCoolTime <= 0)
        {
            rbody.linearVelocity = Vector2.zero;
            attackCoolTime = 3 * 50;
            Vector2 dir = (target.position - transform.position) * 1.2F;
            GetComponent<SpriteRenderer>().flipX = dir.x < 0;
            rbody.linearVelocity = new Vector2(dir.x, dir.y);
            StartCoroutine(fly(rbody, 0.8F));
            GetComponent<Animator>().Play("fliy_attack");
        }
        else
        {
            attackCoolTime--;
        }
    }

    IEnumerator fly(Rigidbody2D rb, float delay)
    {
        yield return new WaitForSeconds(delay);
        rbody.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        StartCoroutine(StopInAir(rb, 0.5F));
    }

    IEnumerator StopInAir(Rigidbody2D rb, float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.linearVelocity = Vector2.zero;
    }
}
