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
        if (other.CompareTag("Player"))
        {
            //     string characterType = DataMgr.instance.currentCharacter.ToString();

            //     if (characterType == "Magician")
            //     {
            //         other.GetComponent<Magician_Script>().TakeDamage(status.getDamage());
            //     }
            //     else if (characterType == "Gunner")
            //     {
            //         other.GetComponent<Gunner_Script>().TakeDamage(status.getDamage());
            //     }
            //     else if (characterType == "Samurai")
            //     {
            //         other.GetComponent<Samurai_Script>().TakeDamage(status.getDamage());
            //     }
        
            other.GetComponent<Magician_Script>().TakeDamage(bossScript.getDamage());

            stayTime += Time.deltaTime;

            if (stayTime >= 3f)
            {
                Debug.Log("즉사");
                other.GetComponent<Magician_Script>().TakeDamage(99999);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) stayTime = 0f;
    }
}
