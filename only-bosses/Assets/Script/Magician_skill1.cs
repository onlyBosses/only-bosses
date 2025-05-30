using UnityEngine;

public class Magician_skill1 : MonoBehaviour
{

    public int dmg;
    private int count;

    void Start()
    {
        Physics2D.IgnoreLayerCollision(8, 7, true);
        Physics2D.IgnoreLayerCollision(8, 6, true);
        Physics2D.IgnoreLayerCollision(8, 8, true);
        count = 80;
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
            BossInterface boss = other.GetComponent<BossInterface>();
            boss.OnDamage(dmg);
        }

        if (other.CompareTag("Monster"))
        {
            LittleMonster littleMonster = other.GetComponent<LittleMonster>();
            littleMonster.OnDamage(dmg);
        }
    }
}
