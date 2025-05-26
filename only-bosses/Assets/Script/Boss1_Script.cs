using UnityEngine;

public class Boss1_Script : MonoBehaviour
{

    private BossStatus status;

    private int currentHp;

    [SerializeField] private HPBarUI bossHPBar;

    void Start()
    {
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        // 맞아도 뒤로 안 밀림
        rbody.bodyType = RigidbodyType2D.Kinematic;

        status = new BossStatus(16000, 2, 10, 0, 10, 50, 5);

        // switch (DataMgr.instance.selectedDifficulty)    
        // {
        //     case Easy:
        //         // 체력, 공격속도, 공격력, 사거리, 치명타 확률, 치명타 데미지, 이동속도 
        //         status = new BossStatus(16000, 2, 10, 0, 10, 50, 5);
        //         break;

        //     case Hard:
        //         status = new BossStatus(28000, 2, 15, 0, 10, 50, 5);
        //         break;
        // }


        currentHp = status.getHp();

        // 테스트에서 동작 
        // bossHPBar.SetHP(currentHp, status.getHp());
    }

    void Update()
    {

    }

    public void TakeDamage(int dmg)
    {
        currentHp -= dmg;
        Debug.Log($"받은 데미지: {dmg} | 현재 HP: {currentHp}");

        bossHPBar.SetHP(currentHp, status.getHp());
    }

    public void SetBossHPBar(HPBarUI hpBar)
    {
        bossHPBar = hpBar;
    }
}
