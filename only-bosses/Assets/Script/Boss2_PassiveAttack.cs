using UnityEngine;

public class Boss2_PassiveAttack : MonoBehaviour
{

    public int damage;
    void OnTriggerEnter2D(Collider2D other)
    {
        Move_Player mPlayer = other.GetComponent<Move_Player>();
        if (mPlayer != null)
        {
            mPlayer.OnDamage(damage);
        }
    }

    void Start()
    {
        Physics2D.IgnoreLayerCollision(9, 9, true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Vector3 rotatedDirection;
        rotatedDirection = Quaternion.Euler(0, 0, 90) * transform.up;
        transform.position -= rotatedDirection * 7 * Time.deltaTime;
        if (transform.position.y <= -4.5)
        {
            Destroy(gameObject);
        }
    }
}
