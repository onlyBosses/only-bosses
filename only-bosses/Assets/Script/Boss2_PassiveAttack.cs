using UnityEngine;

public class Boss2_PassiveAttack : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("트리거 진입: " + other.name);
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
        
    }
}
