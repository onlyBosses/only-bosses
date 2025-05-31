using UnityEditor;
using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour, BossInterface
{
    public Transform player;
    public BossStatus status;
    protected int stiffenTime;
    protected Rigidbody2D rbody;

    protected SpriteRenderer spriteRenderer;

    public HPBarUI bossHPBar;

    // magician R skill 
    // private bool isStun = false;
    // private float stunTimer = 0f;

    void Start()
    {

    }

    protected void init()
    {
        stiffenTime = 0;
        rbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // void Update() {}
    public void inputUpdate()
    {
        // if (isStun)
        // {
        //     rbody.linearVelocity = Vector2.zero; 
        //     stunTimer -= Time.deltaTime;
        //     if (stunTimer <= 0f)
        //     {
        //         isStun = false;
        //     }
        //     return; 
        // }
    }

    public void OnDamage(int damage)
    {
        if (stiffenTime < 25)
        {
            stiffenTime = 25;
        }
        spriteRenderer.color = new Color(200 / 255F, 200 / 255F, 200 / 255F);
        int health = status.getHealth();
        health -= damage;
        if (health <= 0) //죽음
        {
            
        }
        status.setHealth(health);
        Debug.Log("보스 체력: " + status.getHealth());
        status = GetComponent<Boss>().status;

        status.setHealth(status.getHealth() - damage);
        Debug.Log($"받은 데미지: {damage} | 현재 HP: {status.getHealth()}");

        bossHPBar.SetHP(status.getHealth(), status.getMaxHealth());
    }

    // magician R skill (BossInterface 구현)
    public void OnStun()
    {
        if (Random.Range(0, 100) < 50)
        {
            Debug.Log("스턴 적중");
            // isStun = true;
            // stunTimer = 3f;
            stiffenTime = 3 * 50;
        }
    }

    // HPBar UI -> GameManager에서 초기화 
    public void SetBossHPBar(HPBarUI hpBar)
    {
        bossHPBar = hpBar;
    }

    public void setStiffenTime(int duration)
    {
        stiffenTime = duration;
    }
}
