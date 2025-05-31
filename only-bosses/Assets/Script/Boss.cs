using UnityEditor;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public BossStatus status;
    public int stiffenTime;
    protected Rigidbody2D rbody;
    protected SpriteRenderer spriteRenderer;
    void Start()
    {
       
    }

    protected void init()
    {
        stiffenTime = 0;
        rbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDamage(int damage)
    {
        if (stiffenTime < 25)
        {
            stiffenTime = 25;
        }
        spriteRenderer.color = new Color(200 / 255F, 200 / 255F, 200 / 255F);
        int health = status.getHp();
        health -= damage;
        if (health <= 0) //죽음
        {

        }
        status.setHealth(health);
        Debug.Log("보스 체력: " + status.getHp());
    }
}
