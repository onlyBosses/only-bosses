using UnityEngine;

public class Magician_skill1 : MonoBehaviour
{

    public int dmg;
    private int count;

    private float damageInterval = 0.6f; 
    private float damageTimer = 0f;
    void Start()
    {
        Physics2D.IgnoreLayerCollision(8, 7, true);
        Physics2D.IgnoreLayerCollision(8, 6, true);
        Physics2D.IgnoreLayerCollision(8, 8, true);
        count = 80;
    }

    void Update()
    {
        damageTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (count > 0)
        {
            count--;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (damageTimer > 0f) return;

        if (other.CompareTag("Boss"))
        {
            Boss boss = other.gameObject.GetComponent<Boss>();
            boss.OnDamage(dmg);
            damageTimer = damageInterval;
        }

        else if (other.CompareTag("Monster"))
        {
            Monster monster = other.GetComponent<Monster>();
            if (monster != null)
            {
                monster.OnDamage(dmg);
                damageTimer = damageInterval;
            }
        }
    }
}
