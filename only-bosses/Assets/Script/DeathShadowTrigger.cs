using UnityEngine;

public class DeathShadowTrigger : MonoBehaviour
{
    private float stayTime = 0f;

    private Boss1_Script bossScript;

    void Start()
    {
        GameObject bossObject = GameObject.FindWithTag("Boss");
        bossScript = bossObject.GetComponent<Boss1_Script>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(other.name);
        if (other.CompareTag("Player"))
        {
            Move_Player player = other.GetComponent<Move_Player>();

            stayTime += Time.deltaTime;
            Debug.Log(stayTime);
            if (stayTime >= 3f)
            {
                // Debug.Log("즉사");
                player.OnDamage(99999);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {               
            Move_Player player = other.GetComponent<Move_Player>();
            player.OnDamage(bossScript.status.getDamage());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) stayTime = 0f;
    }
}
