using UnityEngine;

public class Gunner_Script : MonoBehaviour
{
    private string walkAnime = "gunner_walk";
    private string idleAnime = "gunner_idle";
    private string baseAttackAnim = "gunner_pistolH";
    private bool isBaseAttack = false;
    void Start()
    {
        isBaseAttack = false;
    }

    void Update()
    {
        if (Input.GetKey("a") || Input.GetKey("d") || Input.GetKey("w"))
        {
            this.GetComponent<Animator>().Play(walkAnime);
        }
        if (Input.GetMouseButtonDown(0))
        {
            this.GetComponent<Animator>().Play(baseAttackAnim);
            baseAttack();
        }
    }

    void FixedUpdate()
    {
        if (isBaseAttack)
        {

        }
    }

    private void baseAttack()
    {
        GameObject characterPrefab = Resources.Load<GameObject>($"etc/bullet");
        characterPrefab.transform.position = new Vector3(1, 1, 1);

        // (-6, 0, 0)
        Instantiate(characterPrefab, characterPrefab.transform.position, characterPrefab.transform.rotation);
    }
}
