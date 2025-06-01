using UnityEngine;

public class SamuraiBaseAttack : MonoBehaviour
{

    private Samurai_Script samuraiScript;
    private int dmg;

    public void SetDamage(int dmg)
    {
        this.dmg = dmg;
    }

    void Start()
    {
        samuraiScript = GetComponentInParent<Samurai_Script>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        int dmg = samuraiScript.status.getDamage();

        if (other.CompareTag("Boss"))
        {

            Debug.Log("Samurai 평타 적중");
            BossInterface boss = other.GetComponent<BossInterface>();
            boss.OnDamage(dmg);
        }

        if (other.CompareTag("Monster"))
        {
            Monster monster = other.GetComponent<Monster>();
            if (monster != null)
            {
                monster.OnDamage(dmg);
            }
        }
    }
}