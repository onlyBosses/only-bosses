using UnityEngine;

public class Boss : MonoBehaviour, BossInterface
{
    public BossStatus status;
    protected Rigidbody2D rbody;

    [SerializeField] private HPBarUI bossHPBar;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDamage(int damage)
    {
        status = GetComponent<Boss>().status;

        status.setHealth(status.getHealth() - damage);
        Debug.Log($"받은 데미지: {damage} | 현재 HP: {status.getHealth()}");

        bossHPBar.SetHP(status.getHealth(), status.getMaxHealth());
    }

    // HPBar UI -> GameManager에서 초기화 
    public void SetBossHPBar(HPBarUI hpBar)
    {
        bossHPBar = hpBar;
    }

    // public void TakeDamage(int dmg)
    // {
    //     currentHp -= dmg;
    //     Debug.Log($"받은 데미지: {dmg} | 현재 HP: {currentHp}");

    //     bossHPBar.SetHP(currentHp, status.getHp());
    // }
}
