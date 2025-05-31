using UnityEngine;
using UnityEngine.PlayerLoop;

public class Monster : MonoBehaviour
{
    public Transform target;
    protected Rigidbody2D rbody;
    private int health;

    protected void init()
    {
        rbody = GetComponent<Rigidbody2D>();
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        Physics2D.IgnoreLayerCollision(9, 10, true);
        Physics2D.IgnoreLayerCollision(9, 9, true);
    }

     public void OnDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0) Destroy(gameObject);
    }
}