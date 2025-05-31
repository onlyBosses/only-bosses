using System;
using System.Collections.Generic;
using UnityEngine;

public class Gunner_Script : Move_Player
{
    private string walkAnime = "gunner_walk";
    // private string idleAnime = "gunner_idle";
    private string baseAttackAnim = "gunner_pistolH";
    private bool isBaseAttack = false;
    private int baseAttackCount;
    // private PlayerStatus status;
    private int bulletCount;
    private const int MAXBULLET = 3;
    // private SpriteRenderer spriteRenderer;

    private float firstSkillCoolTime;
    private float secondSkillCoolTime;
    private float thirdSkillCoolTime;

    private float firstSkillCoolTimer = 10f;
    private float secondSkillCoolTimer = 13f;
    private float thirdSkillCoolTimer = 25f;

    private List<GameObject> thirdSkillTargets;

    private bool isRingExtraDamge = false; // Ring1 스킬 피해량 10% 증가 
    private bool isBaseAttackExtraDamage = false; // Ring3 평타 데미지 증가 

    void Start()
    {
        init();
        isBaseAttack = false;
        // Move_Player mPlayer = GetComponent<Move_Player>();
        status = new PlayerStatus(450, 450, 2, 20, 8, 5, 30, 5F, 0, 0);

        // switch (DataMgr.instance.selectedWeapon)
        // {
        //     case Weapon.Gun:
        //         // 이속 높은데 사거리 짧음
        //         status.setMoveSpeed(status.getMoveSpeed() + 2);
        //         status.setAttackDistance(status.getAttackDistance() - 2);
        //         break;
        //     case Weapon.Gun2:
        //         // 이속 느린데 사거리 김
        //         status.setMoveSpeed(status.getMoveSpeed() - 2);
        //         status.setAttackDistance(status.getAttackDistance() + 2);
        //         break;
        // }

        // switch (DataMgr.instance.selectedRing)
        // {
        //     case Ring.Ring1:
        //         // 스킬 피해량 10% 증가
        //         isRingExtraDamge = true;
        //         break;
        //     case Ring.Ring2:
        //         // 치명타 피해량 증가
        //         status.setCriticalDamage(status.getCriticalDamage() + 10);
        //         break;
        //     case Ring.Ring3:
        //         // 평타 피해량 증가
        //         isBaseAttackExtraDamage = true; 
        //         break;
        // }

        // switch (DataMgr.instance.selectedNecklace)
        // {
        //     case Necklace.Necklace1:
        //         // 스킬 쿨타임 감소 
        //         firstSkillCoolTimer = 8f;
        //         secondSkillCoolTimer = 11f;
        //         thirdSkillCoolTimer = 23f;
        //         break;
        //     case Necklace.Necklace2:
        //         // 최대 체력 증가 
        //         status.setMaxHealth(status.getMaxHealth() + 100);
        //         break;
        //     case Necklace.Necklace3:
        //         // 치명타 확률 증가 
        //         status.setCriticalChance(status.getCriticalChance() + 10);
        //         break;
        // }

        playerHPBar.SetHP(status.getMaxHealth(), status.getMaxHealth());

        bulletCount = MAXBULLET;
        firstSkillCoolTime = 0f;
        secondSkillCoolTime = 0f;
        thirdSkillCoolTime = 0f;
        thirdSkillTargets = new List<GameObject>();
        // spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (stiffenTime > 0)
        {
            return; 
        }
        inputMove();
        if (Input.GetKey("a") || Input.GetKey("d") || Input.GetKey("w"))
        {
            GetComponent<Animator>().Play(walkAnime);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (baseAttack())
            {
                GetComponent<Animator>().Play(baseAttackAnim);
            }
        }
        if (Input.GetKey("q"))
        {
            useFirstSkill();
        }
        if (Input.GetKey("e"))
        {
            useSecondSkill();
        }
        if (Input.GetKey("r"))
        {
            useThirdSkill();
        }
    }

    void FixedUpdate()
    {
        move();
        if (!isBaseAttack)
        {
            if (--baseAttackCount <= 0)
            {
                isBaseAttack = true;
                bulletCount = MAXBULLET;
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

    private bool baseAttack()
    {
        if (!isBaseAttack)
        {
            return false;
        }
        if (!Physics2D.Raycast(transform.position, Vector3.down, 1F, LayerMask.GetMask("Ground")))
        {
            Rigidbody2D rbody = GetComponent<Rigidbody2D>();
            if (rbody.linearVelocity.y < 0)
                rbody.AddForce(new Vector2(0, transform.position.y + 5), ForceMode2D.Impulse);
        }
        // PlayerStatus status = GetComponent<Move_Player>().status;
        int dmg = status.getDamage();
        if (--bulletCount <= 0)
        {
            dmg = (int)(dmg * 1.2);
            baseAttackCount = status.getAttackSpeed() * 50;
            isBaseAttack = false;
        }
        dmg = setCritical(dmg);
        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 pos = transform.position;
        Vector2 dir = (worldPos - pos).normalized;

        GameObject bulletPrefab = Resources.Load<GameObject>($"etc/bullet");
        bulletPrefab.transform.position = transform.position;
        Bullet bullet = bulletPrefab.GetComponent<Bullet>();
        bullet.playerObject = gameObject;

        bullet.dmg = isBaseAttackExtraDamage == true ? dmg + 10 : dmg;

        bullet.MaxDistance = status.getAttackDistance();

        bulletPrefab.GetComponent<SpriteRenderer>().flipX = dir.x < 0;

        bullet.vector = dir;

        Instantiate(bulletPrefab, bulletPrefab.transform.position, bulletPrefab.transform.rotation);
        return true;
    }

    private void useFirstSkill()
    {
        if (firstSkillCoolTime > 0f)
        {
            return;
        }
        firstSkillCoolTime = firstSkillCoolTimer;
        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 pos = transform.position;
        Vector2 dir = (worldPos - pos).normalized;

        GameObject bulletPrefab = Resources.Load<GameObject>($"etc/stunBullet");
        bulletPrefab.transform.position = transform.position;
        Gunner_skill1 bullet = bulletPrefab.GetComponent<Gunner_skill1>();

        // PlayerStatus status = GetComponent<Move_Player>().status;
        int baseDamge = (int)(status.getDamage() * 0.8);
        baseDamge = setCritical(baseDamge);
        int extraDamge = (int)(baseDamge * 1.1f);
        bullet.dmg = isRingExtraDamge == true ? extraDamge : baseDamge;

        bullet.MaxDistance = 10;

        bulletPrefab.GetComponent<SpriteRenderer>().flipX = dir.x < 0;

        bullet.vector = dir;

        Instantiate(bulletPrefab, bulletPrefab.transform.position, bulletPrefab.transform.rotation);
    }

    private void useSecondSkill()
    {
        if (secondSkillCoolTime > 0f)
        {
            return;
        }
        secondSkillCoolTime = secondSkillCoolTimer;
        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 pos = transform.position;
        Vector2 dir = (worldPos - pos).normalized;

        GameObject skillPrefab = Resources.Load<GameObject>($"etc/gunner_skill2");
        skillPrefab.transform.position = new Vector3(transform.position.x + (dir.x * 1), transform.position.y, -1);

        skillPrefab.GetComponent<SpriteRenderer>().flipX = dir.x < 0;

        Gunner_skill2 script = skillPrefab.GetComponent<Gunner_skill2>();
        // PlayerStatus status = GetComponent<Move_Player>().status;

        int baseDamage = (int)(status.getDamage() * 1.8);
        baseDamage = setCritical(baseDamage);
        int extraDamage = (int)(baseDamage * 1.1f);
        script.dmg = isRingExtraDamge == true ? extraDamage : baseDamage;

        Instantiate(skillPrefab, skillPrefab.transform.position, skillPrefab.transform.rotation);

        transform.position = new Vector2(transform.position.x + (dir.x * -0.5F), transform.position.y);
    }

    private void useThirdSkill()
    {
        if (thirdSkillCoolTime > 0f)
        {
            return;
        }
        thirdSkillCoolTime = thirdSkillCoolTimer;
        thirdSkillTargets.Clear();
        float angle = 0;
        for (int i = 0; i < 8; i++)
        {
            angle += (float) Math.PI/9;
            GameObject bulletPrefab = Resources.Load<GameObject>($"etc/bullet2");
            Gunner_skill3 script = bulletPrefab.GetComponent<Gunner_skill3>();
            float x = transform.position.x + Mathf.Cos(angle);
            float y = transform.position.y + Mathf.Sin(angle);

            Vector3 pos = new Vector3(x, y, 0);

            // PlayerStatus status = GetComponent<Move_Player>().status;
            int baseDamage = (int)(status.getDamage() * 0.6);
            baseDamage = setCritical(baseDamage);
            int extraDamge = (int)(baseDamage * 1.1f);
            script.dmg = isRingExtraDamge == true ? extraDamge : baseDamage;

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 5F);
            foreach (Collider2D col in hitColliders)
            {
                if (col.gameObject.layer != 9)
                {
                    continue;
                }
                if (thirdSkillTargets.Contains(col.gameObject))
                {
                    continue;
                }
                script.target = col.gameObject;
            }

            Instantiate(bulletPrefab, pos, Quaternion.Euler(0, 0, 90));
            
        }
    }
}
