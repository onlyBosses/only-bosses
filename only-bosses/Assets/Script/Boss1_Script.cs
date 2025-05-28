using UnityEngine;

public class Boss1_Script : MonoBehaviour
{
    public Transform player; // 플레이어
    private BossStatus status; // 상태
    [SerializeField] private HPBarUI bossHPBar; // HPBar UI -> GameManager에서 초기화 

    private float behaviorTimer; // 각 Behavior 얼마동안 실행할지 

    private int currentHp; // 현재 체력 

    private float baseAttackCoolTime = 0.1f;
    private float baseAttackTimer;

    [SerializeField] private Collider2D baseAttackRange;


    private Rigidbody2D rbody;
    private Animator animator;


    enum BossBehavior { Idle, Chase, Teleport, Skill1, Skill2, Skill3, Skill4 }
    BossBehavior currentBehavior = BossBehavior.Idle;


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

        baseAttackTimer = baseAttackCoolTime;
        baseAttackRange.enabled = false;
    }

    void Update()
    {
        switch (currentBehavior)
        {
            case BossBehavior.Idle:
                rbody.linearVelocity = Vector2.zero;
                animator.SetBool("IsWalking", false);

                behaviorTimer -= Time.deltaTime;
                if (behaviorTimer <= 0f) ChangeBehavior();
                // Debug.Log("Idle 상태");
                break;

            case BossBehavior.Chase:
                ChasePlayer();
                animator.SetBool("IsWalking", true);

                behaviorTimer -= Time.deltaTime;
                if (behaviorTimer <= 0f) ChangeBehavior();
                break;

            case BossBehavior.Teleport:
                UseTeleport();
                ChangeBehavior();
                break;

            case BossBehavior.Skill1:
                UseSkill1();
                ChangeBehavior();
                break;

            case BossBehavior.Skill2:
                UseSkill2();
                ChangeBehavior();
                break;

            case BossBehavior.Skill3:
                UseSkill3();
                ChangeBehavior();
                break;

            case BossBehavior.Skill4:
                UseSkill4();
                ChangeBehavior();
                break;
        }

        // 평타
        baseAttackTimer -= Time.deltaTime;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= 2.5f && baseAttackTimer <= 0f)
        {
            BaseAttack();
            baseAttackTimer = baseAttackCoolTime;
        }

    }

    void ChasePlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        rbody.linearVelocity = dir * status.getMoveSpeed();

        if (player.position.x > transform.position.x) GetComponent<SpriteRenderer>().flipX = true;
        else GetComponent<SpriteRenderer>().flipX = false;

        // Debug.Log("ChasePlayer 실행");
    }

    void UseTeleport()
    {
        // Debug.Log("Teleport 실행");
    }

    void UseSkill1()
    {
        // Debug.Log("스킬1 사용");
    }

    void UseSkill2()
    {
        // Debug.Log("스킬2 사용");
    }

    void UseSkill3()
    {
        // Debug.Log("스킬3 사용");
    }

    void UseSkill4()
    {
        // Debug.Log("스킬4 사용");
    }

    void ChangeBehavior()
    {
        int rand = Random.Range(0, 100);

        // 5% Idle 
        if (rand < 5)
        {
            currentBehavior = BossBehavior.Idle;
            behaviorTimer = Random.Range(1f, 1.5f); // 1 ~ 1.5초 둠칫? 
        }
        // 25% Chase
        else if (rand < 30)
        {
            currentBehavior = BossBehavior.Chase;
            behaviorTimer = Random.Range(1f, 1.5f); // 1 ~ 1.5초 추적? 
        }
        // 15% Teleport
        else if (rand < 45)
        {
            currentBehavior = BossBehavior.Teleport;
        }
        // 55% Skill 
        else
        {
            int skillNum = Random.Range(1, 5);
            // enum에서 몇 번째 고르기 
            currentBehavior = (BossBehavior)((int)BossBehavior.Skill1 + skillNum - 1);
        }
    }

    public void BaseAttack()
    {
        animator.SetTrigger("Attack");

        float offsetX = 0.5f;
        Vector3 offset = new Vector3(offsetX, -0.2f, 0f);

        if (GetComponent<SpriteRenderer>().flipX) offset.x = offsetX; 
        else offset.x = -offsetX; 

        baseAttackRange.transform.localPosition = offset;
    }


    public void EnableAttackCollider()
    {
        baseAttackRange.enabled = true;
    }
    
    public void DisableAttackCollider()
    {
        baseAttackRange.enabled = false;
    }


    // HPBar UI -> GameManager에서 초기화 
    public void SetBossHPBar(HPBarUI hpBar)
    {
        bossHPBar = hpBar;
    }

    public void TakeDamage(int dmg)
    {
        currentHp -= dmg;
        Debug.Log($"받은 데미지: {dmg} | 현재 HP: {currentHp}");

        bossHPBar.SetHP(currentHp, status.getHp());
    }

    public int getDamage()
    {
        return status.getDamage();
    }
}
