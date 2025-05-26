using UnityEngine;

public class Boss1_Script : MonoBehaviour
{
    public Transform player; // 플레이어
    private BossStatus status; // 상태
    [SerializeField] private HPBarUI bossHPBar; // HPBar UI
    private int currentHp; // 현재 체력 
    private float baseAttackCoolTime = 3f; // 기본 공격 쿨타임 (초)
    private float baseAttackTimer = 0f;

    private Rigidbody2D rbody;
    private Animator animator;


    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        // 맞아도 뒤로 안 밀림
        // rbody.bodyType = RigidbodyType2D.Kinematic;

        rbody.gravityScale = 0;
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        animator = GetComponent<Animator>();

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
        Vector2 direction = (player.position - transform.position).normalized;

        bool isWalking = direction.magnitude > 0.1f;
        animator.SetBool("IsWalking", isWalking);
        
        float xDifference = player.position.x - transform.position.x;
        float flipThreshold = 3f;  // flip 전환 최소 거리

        if (Mathf.Abs(xDifference) > flipThreshold)
        {
            Vector3 bossScale = transform.localScale;

            if (xDifference > 0)
            {
                bossScale.x = -Mathf.Abs(bossScale.x);
            }
            else
            {
                bossScale.x = Mathf.Abs(bossScale.x);
            }

            transform.localScale = bossScale;
        }


        direction = direction.normalized;
        rbody.MovePosition(rbody.position + direction * status.getMoveSpeed() * Time.deltaTime);

        // 쿨타임 감소
        baseAttackTimer -= Time.deltaTime;

        if (baseAttackTimer <= 0f)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            // 가까이 접근했을 때만 공격
            if (distance <= 1.5f)
            {
                baseAttack();
                baseAttackTimer = baseAttackCoolTime;
            }
        }


    }

    public void TakeDamage(int dmg)
    {
        currentHp -= dmg;
        Debug.Log($"받은 데미지: {dmg} | 현재 HP: {currentHp}");

        bossHPBar.SetHP(currentHp, status.getHp());
    }

    void baseAttack()
    {
        Debug.Log("공격함");
    }


    public void SetBossHPBar(HPBarUI hpBar)
    {
        bossHPBar = hpBar;
    }
}
