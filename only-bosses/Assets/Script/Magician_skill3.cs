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
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
