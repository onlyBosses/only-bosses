using UnityEngine;

public class Magician_skill2 : MonoBehaviour
{

    public int dmg;
    private int count;

    void Start()
    {
        Physics2D.IgnoreLayerCollision(8, 7, true);
        Physics2D.IgnoreLayerCollision(8, 6, true);
        Physics2D.IgnoreLayerCollision(8, 8, true);
        count = 5 * 50;
    }

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
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            Boss boss = other.gameObject.GetComponent<Boss>();
            boss.OnDamage(dmg);
        }

        else if (other.CompareTag("Monster"))
        {
            Monster monster = other.GetComponent<Monster>();
            if (monster != null)
            {
                monster.OnDamage(dmg);
            }
        }
    }
}
