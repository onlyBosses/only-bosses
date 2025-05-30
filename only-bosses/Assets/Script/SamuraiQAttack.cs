using UnityEngine;

public class SamuraiQAttack : MonoBehaviour
{

    private Samurai_Script samuraiScript;

    void Start()
    {
        samuraiScript = GetComponentInParent<Samurai_Script>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        int dmg = samuraiScript.status.getDamage();

        if (other.CompareTag("Boss"))
        {

            Debug.Log("Samurai Q 적중");
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