using UnityEngine;

public class Gunner_Script : MonoBehaviour
{
    private string nowMode = "";
    private string walkAnime = "gunner_walk";
    private string idleAnime = "gunner_idle";
    private string baseAttackAnim = "gunner_base_attack";
    private bool isBaseAttack = false;
    void Start()
    {
        isBaseAttack = false;
    }

    void Update()
    {
        bool isAnime = false;
        if (Input.GetKey("a") || Input.GetKey("d") || Input.GetKey("w"))
        {
            isAnime = true;
            nowMode = walkAnime;
        }
        if (Input.GetMouseButtonDown(0))
        {
            isAnime = true;
            nowMode = baseAttackAnim;
        }
        if (!isAnime)
        {
            nowMode = idleAnime;
        }
    }

    void FixedUpdate()
    {
        this.GetComponent<Animator>().Play(nowMode);
        if (isBaseAttack)
        {
            
        }
    }
}
