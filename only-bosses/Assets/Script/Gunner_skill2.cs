using UnityEngine;

public class Gunner_skill2 : MonoBehaviour
{
    public int dmg;
    private int count;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void OnTriggerEnter2D(Collider2D collison)
    {
        Boss boss = collison.gameObject.GetComponent<Boss>();
        if (boss != null)
        {
            boss.OnDamage(dmg);
        }
    }

    void Start()
    {
        Physics2D.IgnoreLayerCollision(8, 7, true);
        Physics2D.IgnoreLayerCollision(8, 6, true);
        Physics2D.IgnoreLayerCollision(8, 8, true);
        count = 15;
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
