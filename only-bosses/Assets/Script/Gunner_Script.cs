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
    private SpriteRenderer spriteRenderer;

    private int firstSkillCoolTime;
    private int secondSkillCoolTime;
    private int thirdSkillCoolTime;

    private List<GameObject> thirdSkillTargets;

    void Start()
    {
        init();
        isBaseAttack = false;
        // Move_Player mPlayer = GetComponent<Move_Player>();
        status = new PlayerStatus(450, 2, 20, 8, 5, 30, 5, 0, 0);
        bulletCount = MAXBULLET;
        firstSkillCoolTime = 0;
        secondSkillCoolTime = 0;
        thirdSkillCoolTime = 0;
        thirdSkillTargets = new List<GameObject>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
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
        if (firstSkillCoolTime > 0)
        {
            firstSkillCoolTime--;
        }
        if (secondSkillCoolTime > 0)
        {
            secondSkillCoolTime--;
        }
        if (thirdSkillCoolTime > 0)
        {
            thirdSkillCoolTime--;
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
        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 pos = transform.position;
        Vector2 dir = (worldPos - pos).normalized;

        GameObject bulletPrefab = Resources.Load<GameObject>($"etc/bullet");
        bulletPrefab.transform.position = transform.position;
        Bullet bullet = bulletPrefab.GetComponent<Bullet>();
        bullet.playerObject = gameObject;

        bullet.dmg = dmg;

        bullet.MaxDistance = status.getAttackDistance();

        bulletPrefab.GetComponent<SpriteRenderer>().flipX = dir.x < 0;

        bullet.vector = dir;

        Instantiate(bulletPrefab, bulletPrefab.transform.position, bulletPrefab.transform.rotation);
        return true;
    }

    private void useFirstSkill()
    {
        if (firstSkillCoolTime > 0)
        {
            return;
        }
        firstSkillCoolTime = 10 * 50;
        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 pos = transform.position;
        Vector2 dir = (worldPos - pos).normalized;

        GameObject bulletPrefab = Resources.Load<GameObject>($"etc/bullet");
        bulletPrefab.transform.position = transform.position;
        Bullet bullet = bulletPrefab.GetComponent<Bullet>();
        bullet.playerObject = gameObject;

        // PlayerStatus status = GetComponent<Move_Player>().status;
        bullet.dmg = (int)(status.getDamage() * 0.8);

        bullet.MaxDistance = 10;

        bulletPrefab.GetComponent<SpriteRenderer>().flipX = dir.x < 0;

        bullet.vector = dir;

        Instantiate(bulletPrefab, bulletPrefab.transform.position, bulletPrefab.transform.rotation);
    }

    private void useSecondSkill()
    {
        if (secondSkillCoolTime > 0)
        {
            return;
        }
        secondSkillCoolTime = 13 * 50;
        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 pos = transform.position;
        Vector2 dir = (worldPos - pos).normalized;

        GameObject skillPrefab = Resources.Load<GameObject>($"etc/gunner_skill2");
        skillPrefab.transform.position = new Vector3(transform.position.x + (dir.x * 1), transform.position.y, -1);

        skillPrefab.GetComponent<SpriteRenderer>().flipX = dir.x > 0;

        Gunner_skill2 script = skillPrefab.GetComponent<Gunner_skill2>();
        // PlayerStatus status = GetComponent<Move_Player>().status;
        script.dmg = (int)(status.getDamage() * 1.8);

        Instantiate(skillPrefab, skillPrefab.transform.position, skillPrefab.transform.rotation);

        transform.position = new Vector2(transform.position.x + (dir.x * -0.5F), transform.position.y);
    }

    private void useThirdSkill()
    {
        if (thirdSkillCoolTime > 0)
        {
            return;
        }
        thirdSkillCoolTime = 25 * 50;
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
            script.dmg = (int) (status.getDamage() * 0.6);

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
