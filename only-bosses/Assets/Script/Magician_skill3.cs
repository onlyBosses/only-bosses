using UnityEngine;

public class Magician_skill3 : MonoBehaviour
{

    public int dmg;
    private int count;

    void Start()
    {
        Physics2D.IgnoreLayerCollision(8, 7, true);
        Physics2D.IgnoreLayerCollision(8, 6, true);
        Physics2D.IgnoreLayerCollision(8, 8, true);
        
        Destroy(gameObject, 0.5f);
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            // 보스 데미지 주기
        }
    }
}
