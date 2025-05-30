using UnityEngine;

public class Boss1_Script : Boss
{
    public Transform player; // 플레이어
    // [SerializeField] private HPBarUI bossHPBar; // HPBar UI -> GameManager에서 초기화 

    private float behaviorTimer; // 각 Behavior 얼마동안 실행할지 

    // private int currentHp; // 현재 체력 

    // 기본 공격
    private float baseAttackCoolTime = 3f;
    private float baseAttackTimer;
    [SerializeField] private Collider2D baseAttackRange;

    // 텔포 위치 
    private float teleportOffsetX = 1.5f;
    private float teleportOffsetY = 0f;

    // 스킬1 
    [SerializeField] private GameObject deathShadowPrefab;

    // 스킬3 
    [SerializeField] private GameObject bladePrefab;

    // 스킬4 
    [SerializeField] private GameObject[] littleMonsterPrefabs;


    private float teleportCoolTime = 25f;
    private float skill1CoolTime = 25f;
    private float skill2CoolTime = 25f;
    private float skill3CoolTime = 25f;
    private float skill4CoolTime = 25f;


    private float teleportTimer = 0f;
    private float skill1Timer = 0f;
    private float skill2Timer = 0f;
    private float skill3Timer = 0f;
    private float skill4Timer = 0f;

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

        status = new BossStatus(16000, 16000, 2, 10, 0, 10, 50, 5);

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

        // currentHp = status.getHp();

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
                if (teleportTimer <= 0f)
                {
                    animator.SetTrigger("Teleport");
                    teleportTimer = teleportCoolTime;
                }
                currentBehavior = BossBehavior.Idle;
                behaviorTimer = Random.Range(1f, 1.5f);
                break;

            case BossBehavior.Skill1:
                if (skill1Timer <= 0f)
                {
                    animator.SetTrigger("Skill1Cast");
                    skill1Timer = skill1CoolTime;
                }
                currentBehavior = BossBehavior.Idle;
                behaviorTimer = Random.Range(1f, 1.5f);
                break;

            case BossBehavior.Skill2:
                if (skill2Timer <= 0f)
                {
                    animator.SetTrigger("Skill2Cast");
                    skill2Timer = skill2CoolTime;
                }
                ChangeBehavior();
                break;

            case BossBehavior.Skill3:
                if (skill3Timer <= 0f)
                {
                    animator.SetTrigger("Skill3Cast");
                    skill3Timer = skill3CoolTime;
                }
                currentBehavior = BossBehavior.Idle;
                behaviorTimer = Random.Range(1f, 1.5f);
                break;

            case BossBehavior.Skill4:
                if (skill4Timer <= 0f)
                {
                    animator.SetTrigger("Skill4Cast");
                    skill4Timer = skill4CoolTime;
                }
                currentBehavior = BossBehavior.Idle;
                behaviorTimer = Random.Range(1f, 1.5f);
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

        skill1Timer -= Time.deltaTime;
        skill2Timer -= Time.deltaTime;
        skill3Timer -= Time.deltaTime;
        skill4Timer -= Time.deltaTime;
        teleportTimer -= Time.deltaTime;

    }

    void ChasePlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        rbody.linearVelocity = dir * (status.getMoveSpeed() - 4);

        if (player.position.x > transform.position.x) GetComponent<SpriteRenderer>().flipX = true;
        else GetComponent<SpriteRenderer>().flipX = false;

        // Debug.Log("ChasePlayer 실행");
    }

    // void UseTeleport()
    // {
    //     animator.SetTrigger("Teleport");
    // }

    public void DoTeleportMove()
    {
        int dir = Random.Range(0, 2);
        float offsetX = (dir == 0) ? -teleportOffsetX : teleportOffsetX;

        Vector3 newPos = new Vector3(player.position.x + offsetX, player.position.y + teleportOffsetY, transform.position.z);
        transform.position = newPos;
    }

    // void UseSkill1()
    // {
    //     Debug.Log("스킬1 사용");

    //     animator.SetTrigger("Skill1Cast");
    // }

    public void DoSkill1()
    {
        Vector3 spawnPos = new Vector3(player.position.x, player.position.y + 1f, 0f);
        GameObject shadow = Instantiate(deathShadowPrefab, spawnPos, Quaternion.identity);

        // 10초 뒤에 파괴
        Destroy(shadow, 10f);
    }

    // void UseSkill2()
    // {   
    //     Debug.Log("스킬2 사용");

    //     animator.SetTrigger("Skill2Cast");
    // }

    public void DoSkill2()
    {
        // string characterType = DataMgr.instance.currentCharacter.ToString();
        // if (characterType == "Magician")
        // {
        //     player.GetComponent<Magician_Script>().Fear(transform.position);
        // }
        // else if (characterType == "Gunner")
        // {
        //     player.GetComponent<Gunner_Script>().Fear(transform.position);
        // }
        // else if (characterType == "Samurai")
        // {
        //     player.GetComponent<Samurai_Script>().Fear(transform.position);
        // }

        // player.GetComponent<Move_Player>().Fear(transform.position);
        // 상속 받는 거는 못 찾는다는데
        // Fearable 인터페이스로 해결 
        Fearable fearable = player.GetComponent<Fearable>();
        fearable.Fear(transform.position);
    }

    // void UseSkill3()
    // {

    //     // Debug.Log("스킬3 사용");
    // }

    public void DoSkill3()
    {
        int bladeCount = 5;

        for (int i = 0; i < bladeCount; i++)
        {
            GameObject blade = Instantiate(bladePrefab, transform.position, Quaternion.identity);
            RotatingBlade rb = blade.GetComponent<RotatingBlade>();

            rb.boss = this.transform; // 보스 중심으로 회전
            rb.angle = i * (360f / bladeCount); // 분산된 각도로 배치
            rb.radius = 1.5f; // 원 반지름
            rb.speed = 100f; // 회전 속도

            Destroy(blade, 5f); // 5초 뒤에 제거
        }
    }

    // void UseSkill4()
    // {

    //     // Debug.Log("스킬4 사용");
    // }

    public void DoSkill4()
    {
        int littleMonsterCount = 2;

        for (int i = 0; i < littleMonsterCount; i++)
        {
            float offsetX = (i == 0) ? -1f : 1f;
            Vector3 spawnPos = new Vector3(transform.position.x + offsetX, -4f, transform.position.z);
            GameObject littleMonster = Instantiate(littleMonsterPrefabs[i], spawnPos, Quaternion.identity);

            littleMonster.GetComponent<LittleMonster>().target = player;
        }
    }

    void ChangeBehavior()
    {
        int rand = Random.Range(0, 100);

        // 10% Idle 
        if (rand < 10)
        {
            currentBehavior = BossBehavior.Idle;
            behaviorTimer = Random.Range(1f, 1.5f); // 1 ~ 1.5초 둠칫? 
        }
        // 30% Chase
        else if (rand < 45)
        {
            currentBehavior = BossBehavior.Chase;
            behaviorTimer = Random.Range(1f, 1.5f); // 1 ~ 1.5초 추적? 
        }
        // 15% Teleport
        else if (rand < 60)
        {
            currentBehavior = BossBehavior.Teleport;
        }
        // 45% Skill 
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
    // public void SetBossHPBar(HPBarUI hpBar)
    // {
    //     bossHPBar = hpBar;
    // }

    // public void TakeDamage(int dmg)
    // {
    //     currentHp -= dmg;
    //     Debug.Log($"받은 데미지: {dmg} | 현재 HP: {currentHp}");

    //     bossHPBar.SetHP(currentHp, status.getHp());
    // }
}
