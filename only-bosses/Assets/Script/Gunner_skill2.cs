using UnityEngine;

public class Gunner_skill2 : MonoBehaviour
{
    public int dmg;
    private int count;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss"))
        {
            Boss boss = collision.GetComponent<Boss>();
            if (boss != null)
            {
                boss.OnDamage(dmg);
            }
        }
        else if (collision.CompareTag("Monster"))
        {
            Monster monster = collision.GetComponent<Monster>();
            if (monster != null)
            {
                monster.OnDamage(dmg);
            }
        }
        
    }

    void Start()
    {
        Physics2D.IgnoreLayerCollision(8, 7, true);
        Physics2D.IgnoreLayerCollision(8, 6, true);
        Physics2D.IgnoreLayerCollision(8, 8, true);
        count = 20;
    }

    // Update is called once per frame
    void Update()
    {

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
}
