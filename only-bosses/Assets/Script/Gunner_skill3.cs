using System.Collections.Generic;
using UnityEngine;

public class Gunner_skill3 : MonoBehaviour
{
    public GameObject target;
    public int dmg;
    private Rigidbody2D rbody;
    void OnTriggerEnter2D(Collider2D collison)
    {
        Boss boss = collison.gameObject.GetComponent<Boss>();
        if (boss != null)
        {
            Destroy(gameObject);
            boss.OnDamage(dmg);
        }
    }
    void Start()
    {
        Physics2D.IgnoreLayerCollision(8, 7, true);
        Physics2D.IgnoreLayerCollision(8, 6, true);
        Physics2D.IgnoreLayerCollision(8, 8, true);
        rbody = GetComponent<Rigidbody2D>();
        rbody.gravityScale = 0;
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (target == null)
        {
            rbody.linearVelocity = new Vector2(1, 1);
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 10F);
            foreach (Collider2D col in hitColliders)
            {
                if (col.gameObject.layer != 10)
                {
                    continue;
                }
                target = col.gameObject;
            }
        }
        else
        {
            Vector3 dir = (target.transform.position - transform.position).normalized;
            float vx = dir.x * 5;
            float vy = dir.y * 5;
            rbody.linearVelocity = new Vector2(vx, vy);
        }
    }
}
