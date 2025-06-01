using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class Magician_Script : Move_Player
{
    private SpriteRenderer sr;
    private Animator animator;
    
    private int baseAttackCount;
    private bool isBaseAttack = false;

    private int elementCount;
    private const int MAX_ELEMNT = 1;

    public Sprite skillQCastSprite;
    private float firstSkillCoolTime;
    private float secondSkillCoolTime;
    private float thirdSkillCoolTime;

    private float firstSkillCoolTimer = 12f;
    private float secondSkillCoolTimer = 20f;
    private float thirdSkillCoolTimer = 15f;

    private bool isExtraDamage = false; // Wand2 스킬 피해량 5% 증가 
    private bool isRingExtraDamge = false; // Ring1 스킬 피해량 10% 증가 
    private bool isBaseAttackExtraDamage = false; // Ring3 평타 데미지 증가 

    void Start()
    {
        init();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        isBaseAttack = false;
        status = new PlayerStatus(450, 450, 2, 20, 6, 5, 30, 3, 0, 0);

        avoid.avoidAnimation = "magician_dash";
        switch (DataMgr.instance.selectedWeapon)
        {
            case Weapon.Wand:
                // 공격력 증가 
                status.setDamage(status.getDamage() + 10);
                break;
            case Weapon.Wand2:
                // 공격력이 낮지만 확률적으로 해당 스킬의 피해량의 5%만큼 추가 피해
                status.setDamage(status.getDamage() - 10);
                isExtraDamage = true;
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
                firstSkillCoolTimer = 10f;
                secondSkillCoolTimer = 18f;
                thirdSkillCoolTimer = 13f;
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
                elementCount = MAX_ELEMNT;
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
        if (stiffenTime > 0)
        {
            return; 
        }
        inputMove();
        bool isWalking = Input.GetKey("a") || Input.GetKey("d");
        animator.SetBool("IsWalking", isWalking && !avoid.isAvoid);

        if (Input.GetMouseButtonDown(0))
        {
            if (baseAttack())
            {
                animator.SetTrigger("AttackA");
            }
        }

        if (Input.GetKey("q")) useFirstSkill();

        Transform fireTransform = transform.Find("magicianQ(Clone)");
        if (fireTransform != null)
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector2 pos = transform.position;
            Vector2 dir = (worldPos - pos).normalized;

            if (dir.x > 0)
            {
                fireTransform.localPosition = new Vector3(1.0f, 0f, -1f);
                fireTransform.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                fireTransform.localPosition = new Vector3(-1.0f, 0f, -1f);
                fireTransform.GetComponent<SpriteRenderer>().flipX = true;
            }
        }

        bool skillE = Input.GetKey("e") && secondSkillCoolTime <= 0f;
        animator.SetBool("SkillE", skillE);

        bool skillR = Input.GetKey("r") && thirdSkillCoolTime <= 0f;
        animator.SetBool("SkillR", skillR);
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

        int dmg = status.getDamage();

        if (--elementCount <= 0)
        {
            dmg = (int)(dmg * 1.2); // ?
            baseAttackCount = status.getAttackSpeed() * 50;
            isBaseAttack = false;
        }

        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 pos = transform.position;
        Vector2 dir = (worldPos - pos).normalized;

        string[] elementNames = { "fire", "ice", "bolt" };
        string selectedElement = elementNames[Random.Range(0, elementNames.Length)];

        GameObject elementPrefab = Resources.Load<GameObject>($"etc/{selectedElement}");

        elementPrefab.transform.position = transform.position;
        Element element = elementPrefab.GetComponent<Element>();
        element.playerObject = gameObject;

        dmg = setCritical(dmg);
        element.dmg = isBaseAttackExtraDamage == true ? dmg + 10 : dmg;
    
        element.MaxDistance = status.getAttackDistance();
        // elementPrefab.GetComponent<SpriteRenderer>().flipX = dir.x < 0;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle > 90 || angle < -90)
        {
            elementPrefab.GetComponent<SpriteRenderer>().flipY = true;
        }
        else
        {
            elementPrefab.GetComponent<SpriteRenderer>().flipY = false;
        }
        element.vector = dir;

        Instantiate(elementPrefab, elementPrefab.transform.position, Quaternion.Euler(0, 0, angle));

        return true;
    }

    public void useFirstSkill()
    {
        if (firstSkillCoolTime > 0f) return;

        firstSkillCoolTime = firstSkillCoolTimer;

        sr.sprite = skillQCastSprite;

        animator.enabled = false;

        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 pos = transform.position;
        Vector2 dir = (worldPos - pos).normalized;

        GameObject magicianQPrefab = Resources.Load<GameObject>("etc/magicianQ");

        GameObject instance = Instantiate(magicianQPrefab, transform.position, Quaternion.identity);

        instance.transform.SetParent(transform);

        if (dir.x > 0)
        {
            instance.transform.localPosition = new Vector3(1.0f, 0f, -1f);
            instance.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            instance.transform.localPosition = new Vector3(-1.0f, 0f, -1f);
            instance.GetComponent<SpriteRenderer>().flipX = true;
        }

        Magician_skill1 script = instance.GetComponent<Magician_skill1>();

        int baseDamage = (int)(status.getDamage() * 1.3f);
        baseDamage = setCritical(baseDamage);
        int extraDamge;
        if (isExtraDamage && isRingExtraDamge) extraDamge = (int)(baseDamage * 1.15f);
        else if (isExtraDamage) extraDamge = (int)(baseDamage * 1.05f);
        else if (isRingExtraDamge) extraDamge = (int)(baseDamage * 1.1f);
        else extraDamge = 0;

        script.dmg = extraDamge == 0 ? baseDamage : extraDamge;

        Invoke(nameof(ResumeIdle), 2f);
    }

    public void useSecondSkill()
    {

        secondSkillCoolTime = secondSkillCoolTimer;

        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        GameObject eSkillPrefab = Resources.Load<GameObject>("etc/magicianE");

        Vector3 spawnPos = new Vector3(worldPos.x, worldPos.y, -1f);

        GameObject instance = Instantiate(eSkillPrefab, spawnPos, Quaternion.identity);

        Magician_skill2 script = instance.GetComponent<Magician_skill2>();

        int baseDamage = (int)(status.getDamage() * 1.2F);
        baseDamage = setCritical(baseDamage);
        int extraDamge;
        if (isExtraDamage && isRingExtraDamge) extraDamge = (int)(baseDamage * 1.15f);
        else if (isExtraDamage) extraDamge = (int)(baseDamage * 1.05f);
        else if (isRingExtraDamge) extraDamge = (int)(baseDamage * 1.1f);
        else extraDamge = 0;

        script.dmg = extraDamge == 0 ? baseDamage : extraDamge;
    }

    public void useThirdSkill()
    {

        thirdSkillCoolTime = thirdSkillCoolTimer;

        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        GameObject rSkillPrefab = Resources.Load<GameObject>("etc/magicianR");

        Vector3 spawnPos = new Vector3(worldPos.x, worldPos.y, -1f);

        GameObject instance = Instantiate(rSkillPrefab, spawnPos, Quaternion.identity);

        Magician_skill3 script = instance.GetComponent<Magician_skill3>();

        int baseDamage = (int)(status.getDamage() * 2f);
        baseDamage = setCritical(baseDamage);
        int extraDamge;
        if (isExtraDamage && isRingExtraDamge) extraDamge = (int)(baseDamage * 1.15f);
        else if (isExtraDamage) extraDamge = (int)(baseDamage * 1.05f);
        else if (isRingExtraDamge) extraDamge = (int)(baseDamage * 1.1f);
        else extraDamge = 0;

        script.dmg = extraDamge == 0 ? baseDamage : extraDamge;
    }

    private void ResumeIdle()
    {
        animator.enabled = true;
        animator.Play("magician_idle");
    }
}
