using UnityEngine;

public class Boss1BaseAttack : MonoBehaviour
{

    private Boss1_Script bossScript;

    void Start() {
        bossScript = GetComponentInParent<Boss1_Script>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            // Debug.Log("데미지 슛");
            PlayerInterface player = other.GetComponent<PlayerInterface>();
            player.OnDamage(bossScript.status.getDamage());
        }
    }
}
