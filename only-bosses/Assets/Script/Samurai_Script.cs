using UnityEngine;

public class Samurai_Script : Move_Player
{
    private Animator animator;

    private int baseAttackCount;
    private bool isBaseAttack = false;
    
    [SerializeField] private Collider2D baseAttackRange;

    private float firstSkillCoolTime;
    private float secondSkillCoolTime;
    private float thirdSkillCoolTime;

    private float firstSkillCoolTimer = 10f;
    private float secondSkillCoolTimer = 25f;
    private float thirdSkillCoolTimer = 8f;

    // private bool isSecondSkill = false;
    // private float isSecondSkillTimer = 0f;

    private bool isThirdSkill = false;
    private float isThirdSkillTimer = 0f;
    private bool isRingExtraDamge = false; // Ring1 스킬 피해량 10% 증가 
    private bool isBaseAttackExtraDamage = false; // Ring3 평타 데미지 증가 

    void Start()
    {
        init();
        animator = GetComponent<Animator>();
        isBaseAttack = false;
        status = new PlayerStatus(500, 500, 1, 20, 0, 5, 30, 3, 0, 0);

        avoid.avoidAnimation = "samurai_jump";

        switch (DataMgr.instance.selectedWeapon)
        {
            case Weapon.Sword:
                // 공격력은 높은데 공격속도 느림
                status.setDamage(status.getDamage() + 10);
                status.setAttackSpeed(status.getAttackSpeed() - 1);
                break;
            case Weapon.Sword2:
                // 공격력 준수, 공격속도 준수
                break;
        }

        switch (DataMgr.instance.selectedRing)
        {
            case Ring.Ring1:
                // 스킬 피해량 10% 증가
                isRingExtraDamge = true;
                break;
            case Ring.Ring2:
                // 치명타 피해량 증가
                status.setCriticalDamage(status.getCriticalDamage() + 10);
                break;
            case Ring.Ring3:
                // 평타 피해량 증가
                isBaseAttackExtraDamage = true; 
                break;
        }

        switch (DataMgr.instance.selectedNecklace)
        {
            case Necklace.Necklace1:
                // 스킬 쿨타임 감소 
                firstSkillCoolTimer = 8f;
                secondSkillCoolTimer = 23f;
                thirdSkillCoolTimer = 6f;
                break;
            case Necklace.Necklace2:
                // 최대 체력 증가 
                status.setMaxHealth(status.getMaxHealth() + 100);
                break;
            case Necklace.Necklace3:
                // 치명타 확률 증가 
                status.setCriticalChance(status.getCriticalChance() + 10);
                break;
        }

        playerHPBar.SetHP(status.getMaxHealth(), status.getMaxHealth());

        firstSkillCoolTime = 0f;
        secondSkillCoolTime = 0f;
        thirdSkillCoolTime = 0f;
    }

    void FixedUpdate()
    {
        move();
        if (!isBaseAttack)
        {
            if (--baseAttackCount <= 0)
            {
                isBaseAttack = true;
            }
        }

        if (firstSkillCoolTime > 0f)
        {
            firstSkillCoolTime -= Time.deltaTime;
            skillQCooldownImage.fillAmount = firstSkillCoolTime / firstSkillCoolTimer;
        }
        if (secondSkillCoolTime > 0f)
        {
            secondSkillCoolTime -= Time.deltaTime;
            skillECooldownImage.fillAmount = secondSkillCoolTime / secondSkillCoolTimer;
        }
        if (thirdSkillCoolTime > 0f)
        {
            thirdSkillCoolTime -= Time.deltaTime;
            skillRCooldownImage.fillAmount = thirdSkillCoolTime / thirdSkillCoolTimer;
        }
    }

    void Update()
    {
        inputMove();
        bool isWalking = Input.GetKey("a") || Input.GetKey("d");
        animator.SetBool("IsWalking", isWalking && !avoid.isAvoid);

        if (Input.GetKeyDown("q"))
        {
            if (useFirstSkill()) animator.SetTrigger("SkillQ");

        }

        if (Input.GetMouseButtonDown(0))
        {
            if (baseAttack()) animator.SetTrigger("AttackA");

        }

        if (Input.GetKeyDown("e"))
        {
            if (secondSkillCoolTime <= 0f) useSecondSkill();

        }

        // if (isSecondSkill)
        // {
        //     if (isSecondSkillTimer > 0f)
        //     {
        //         int skillIndex = Random.Range(1, 3);

        //         switch (skillIndex)
        //         {
        //             case 1:
        //                 animator.SetTrigger("SkillQ");
        //                 break;
        //             case 2:
        //                 animator.SetTrigger("AttackB");
        //                 break;
        //         }

        //         isSecondSkillTimer -= Time.deltaTime;
        //     }
        //     else
        //     {
        //         isSecondSkill = false;
        //     }
        // }

        if (Input.GetKeyDown("r"))
        {
            if (thirdSkillCoolTime <= 0f) useThirdSkill();
        }


        if (isThirdSkillTimer > 0f) isThirdSkillTimer -= Time.deltaTime;
        else if (isThirdSkill) 
        {
            status.setDamage(status.getDamage() - 10); 
            isThirdSkill = false;
        }
    }

    public bool baseAttack()
    {
        if (!isBaseAttack) return false;

        if (!Physics2D.Raycast(transform.position, Vector3.down, 1F, LayerMask.GetMask("Ground"))) // ?
        {
            Rigidbody2D rbody = GetComponent<Rigidbody2D>();
            if (rbody.linearVelocity.y < 0)
                rbody.AddForce(new Vector2(0, transform.position.y + 5), ForceMode2D.Impulse);
        }

        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 pos = transform.position;
        Vector2 dir = (worldPos - pos).normalized;

        float attackRangeDistance = 1.5f;
        Vector2 attackPos = pos + dir * attackRangeDistance;

        baseAttackRange.transform.position = attackPos;

        int baseDamage = setCritical(status.getDamage());
        int extraDamage = (int)(baseDamage * 1.5f);

        // 데미지 설정
        baseAttackRange.GetComponent<SamuraiBaseAttack>().SetDamage(isBaseAttackExtraDamage == true ? extraDamage : baseDamage);    

        baseAttackCount = 0;
        isBaseAttack = false;
        return true;
    }

    public void EnableAttackCollider()
    {
        baseAttackRange.enabled = true;
    }
    
    public void DisableAttackCollider()
    {
        baseAttackRange.enabled = false;
    }

    public bool useFirstSkill()
    {
        if (firstSkillCoolTime > 0f) return false;

        firstSkillCoolTime = firstSkillCoolTimer;

        Vector2 pos = transform.position;

        float moveOffsetX = (spriteRenderer.flipX) ? -1.5f : 1.5f;
        transform.position = new Vector2(pos.x + moveOffsetX, pos.y);

        float attackRangeDistance = 1f;
        Vector2 attackPos = new Vector2(transform.position.x + moveOffsetX * attackRangeDistance, pos.y);
        baseAttackRange.transform.position = attackPos;

        int baseDamage = status.getDamage() * 2;
        int extraDamage = (int)(baseDamage * 1.1f);

        // 데미지 설정
        baseAttackRange.GetComponent<SamuraiBaseAttack>().SetDamage(isRingExtraDamge == true ? extraDamage : baseDamage);  

        return true;
    }

    public void useSecondSkill()
    {

        Debug.Log("useSecondSkill() 작동");
        secondSkillCoolTime = secondSkillCoolTimer;
        // isSecondSkill = true;
        // isSecondSkillTimer = status.getAttackSpeed() * 3f;

        Vector2 pos = transform.position;

        float moveOffsetX = spriteRenderer.flipX ? -1.5f : 1.5f;

        float attackRangeDistance = 1f;
        Vector2 attackPos = new Vector2(transform.position.x + moveOffsetX * attackRangeDistance, pos.y);
        baseAttackRange.transform.position = attackPos;

        int baseDamage = setCritical(status.getDamage() * 2);
        int extraDamage = (int)(baseDamage * 1.1f);

        // 데미지 설정
        baseAttackRange.GetComponent<SamuraiBaseAttack>().SetDamage(isRingExtraDamge == true ? extraDamage : baseDamage);

        animator.SetTrigger("SkillE");
    }

    public void useThirdSkill()
    {
        isThirdSkill = true;
        isThirdSkillTimer = 10f;

        status.setDamage(status.getDamage() + 50);
        thirdSkillCoolTime = thirdSkillCoolTimer;
    }
}
