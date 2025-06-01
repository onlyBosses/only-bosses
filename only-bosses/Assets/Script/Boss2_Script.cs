using System.Collections;
using System.Threading;
using NUnit.Framework.Internal;
using UnityEngine;

class FirstSkill
{
    public int coolTime;
    public bool isSuccess;
    public int duration;
    public FirstSkill()
    {
        coolTime = Random.Range(1, 40) * 50;
        isSuccess = false;
        duration = 0;
    }
}

class SecondSkill
{
    public int coolTime;
    public bool isSuccess;
    public int duration;
    public float power;

    public SecondSkill()
    {
        coolTime = Random.Range(1, 30) * 50;;
        isSuccess = false;
        duration = 0;
        power = 0;
    }
}

class FourthSkill
{
    public int coolTime;
    public bool isSuccess;
    public int duration;
    public float power;
    public FourthSkill()
    {
        coolTime = Random.Range(1, 50) * 50;;
        isSuccess = false;
        duration = 0;
        power = 0;
    }
}

public class Boss2_Script : Boss
{
    public string playerName;
    private int attackCoolTime;
    private FirstSkill firstSkill;
    private SecondSkill secondSkill;
    private int thirdSkillCoolTime;
    private FourthSkill fourthSkill;
    private int speed;
    private bool isFlying;
    private int flyDuration;
    private int flyAttackCount;
    private bool isFlyAttack;
    public Camera cam;

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (isFlying)
        {
            Move_Player mPlayer = collision.gameObject.GetComponent<Move_Player>();
            if (mPlayer != null)
            {
                mPlayer.OnDamage(setCritical(status.getDamage()));
            }
        }
    }

    void Start()
    {
        init();
        rbody = GetComponent<Rigidbody2D>();
        rbody.gravityScale = 0;
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        // status = new BossStatus(16000, 16000, 2, 10, 5, 10, 50, 1);

        switch (DataMgr.instance.selectedDifficulty)
        {
            case Difficulty.Easy:
                // 체력, 공격속도, 공격력, 사거리, 치명타 확률, 치명타 데미지, 이동속도 
                status = new BossStatus(16000, 16000, 2, 10, 0, 10, 50, 5);
                break;

            case Difficulty.Hard:
                status = new BossStatus(28000, 20000, 2, 15, 0, 10, 50, 5);
                break;

            case Difficulty.Test:
                status = new BossStatus(10, 10, 2, 15, 0, 10, 50, 5);
                break;
        }

        attackCoolTime = status.getAttackSpeed() * 50;
        firstSkill = new FirstSkill();
        secondSkill = new SecondSkill();
        thirdSkillCoolTime = Random.Range(1, 25) * 50;
        fourthSkill = new FourthSkill();
        speed = 0;
        isFlying = false;
        flyDuration = 0;
        cam = Camera.main;
        flyAttackCount = 0;

        bossHPBar.SetHP(status.getMaxHealth(), status.getMaxHealth());
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (stiffenTime <= 0)
        {
            spriteRenderer.color = new Color(1, 1, 1);
            
            int maxDistance;
            if (isFlying)
            {
                maxDistance = 10;
                if (flyDuration <= 0)
                {
                    isFlying = false;
                    Physics2D.IgnoreLayerCollision(10, 7, true);
                }
                else
                {
                    flyDuration--;
                    // Debug.Log("남은시간:" + flyDuration / 50);
                }
            }
            else
            {
                Vector2 dir = (player.transform.position - transform.position).normalized;
                maxDistance = 3;
                float distance = Vector2.Distance(player.transform.position, transform.position);
                if (distance > maxDistance-1)
                {
                    float moveSpeed = status.getMoveSpeed();
                    GetComponent<SpriteRenderer>().flipX = dir.x < 0;
                    rbody.linearVelocity = new Vector2(dir.x * moveSpeed, dir.y * moveSpeed);
                }
            }
            if (attackCoolTime <= 0)
            {
                string animation;
                float distance = Vector2.Distance(player.transform.position, transform.position);
                if (distance <= maxDistance)
                {
                    int cool = status.getAttackSpeed();
                    if (isFlying && !isFlyAttack)
                    {
                        Vector2 dir = (player.transform.position - transform.position) * 1.2F;
                        GetComponent<SpriteRenderer>().flipX = dir.x < 0;
                        rbody.linearVelocity = new Vector2(dir.x, dir.y);
                        cool += 1;
                        StartCoroutine(fly(rbody, 0.8F));
                        animation = "Attack2";
                    }
                    else
                    {
                        Move_Player mPlayer = player.GetComponent<Move_Player>();
                        mPlayer.OnDamage(setCritical(status.getDamage()));
                        animation = "Attack1";
                    }
                    attackCoolTime = cool * 50;
                    GetComponent<Animator>().Play(animation);
                }
            }
            else
            {
                attackCoolTime--;
            }
        }
        else
        {
            stiffenTime--;
            if (!isFlying)
            {
                rbody.linearVelocity = Vector2.zero;
            }
            
        }
        useFirstSkill();
        useSecondSkill();
        useThirdSkill();
        useFourthSkill();
    }

    private void useFirstSkill()
    {
        if (firstSkill.coolTime <= 0)
        {
            firstSkill.coolTime = Random.Range(32, 49) * 50;
            if (Physics2D.Raycast(player.transform.position, Vector3.down, 1F, LayerMask.GetMask("Ground")))
            {
                Rigidbody2D rbody = player.GetComponent<Rigidbody2D>();
                rbody.linearVelocity = Vector2.zero;
                rbody.gravityScale = 0;
                rbody.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
                addSpeed();
                StartCoroutine(firstSkillDelay(rbody, 0.3F));
            }
        }
        else
        {
            firstSkill.coolTime--;
        }
        if (firstSkill.isSuccess)
        {
            if (firstSkill.duration <= 3)
            {
                firstSkill.isSuccess = false;
                Rigidbody2D rbody = player.GetComponent<Rigidbody2D>();
                rbody.gravityScale = 1;
            }
            else
            {
                firstSkill.duration--;
            }
        }
    }

    IEnumerator fly(Rigidbody2D rb, float delay)
    {
        yield return new WaitForSeconds(delay);
        rbody.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        StartCoroutine(StopInAir(rb, 0.5F));
    }

    IEnumerator StopInAir(Rigidbody2D rb, float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.linearVelocity = Vector2.zero;
    }

    IEnumerator firstSkillDelay(Rigidbody2D rb, float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.linearVelocity = Vector2.zero;
        Move_Player mPlayer = player.GetComponent<Move_Player>();
        mPlayer.setStiffenTime(150);
        firstSkill.isSuccess = true;
        firstSkill.duration = 3 * 50;
    }

    IEnumerator passive(Rigidbody2D rb, float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.linearVelocity = Vector2.zero;
        if (flyAttackCount < 6)
        {
            for (int i = 0; i < 9; i++)
            {
                GameObject prefab = Resources.Load<GameObject>($"Bosses/passive");
                prefab.GetComponent<Boss2_PassiveAttack>().damage = (int)(status.getDamage() * 1.1);
                Instantiate(prefab, transform.position, Quaternion.Euler(0, 0, 210 + (i * 15)));
            }
            StartCoroutine(passive(rbody, 0.5F));
            flyAttackCount++;
        }
        else
        {
            flyAttackCount = 0;
            isFlyAttack = false;
        }
    }

    private void useSecondSkill()
    {
        if (secondSkill.coolTime <= 0)
        {
            secondSkill.coolTime = Random.Range(24, 37) * 50;
            float distance = Vector2.Distance(player.transform.position, transform.position);
            if (distance <= 3.5)
            {
                PlayerStatus playerStatus = player.GetComponent<Move_Player>().status;
                float moveSpeed = playerStatus.getMoveSpeed();
                secondSkill.power = 0.2F;
                moveSpeed -= secondSkill.power;
                playerStatus.setMoveSpeed(moveSpeed);
                secondSkill.isSuccess = true;
                addSpeed();
                secondSkill.duration = 5 * 50;
            }
        }
        else
        {
            secondSkill.coolTime--;
        }
        if (secondSkill.isSuccess)
        {
            if (secondSkill.duration <= 0)
            {
                PlayerStatus playerStatus = player.GetComponent<Move_Player>().status;
                float moveSpeed = playerStatus.getMoveSpeed() + secondSkill.power;
                playerStatus.setMoveSpeed(moveSpeed);
                secondSkill.isSuccess = false;
            }
            else
            {
                secondSkill.duration--;
            }
        }
    }

    private void useThirdSkill()
    {
        if (thirdSkillCoolTime <= 0)
        {
            thirdSkillCoolTime = Random.Range(20, 31) * 50;
            if (isFlying)
            {
                for (int i = 0; i < 9; i++)
                {
                    GameObject prefab = Resources.Load<GameObject>($"Bosses/passive");
                    prefab.GetComponent<Boss2_PassiveAttack>().damage = (int)(status.getDamage() * 1.1);
                    Instantiate(prefab, transform.position, Quaternion.Euler(0, 0, 210 + (i * 15)));
                }
            }
            else
            {
                Vector2 vec = player.transform.position;
                vec.x -= 1F;
                // Rigidbody2D rbody = GetComponent<Rigidbody2D>();
                transform.position = vec;
            }
            // rbody.AddForce(new Vector2(5, 5), ForceMode2D.Impulse);
            // rbody.linearVelocity = vec;
        }
        else
        {
            thirdSkillCoolTime--;
        }
    }

    private void useFourthSkill()
    {
        if (fourthSkill.coolTime <= 0)
        {
            fourthSkill.coolTime = Random.Range(40, 61) * 50;
            float distance = Vector2.Distance(player.transform.position, transform.position);
            if (distance <= 5)
            {
                Move_Player mPlayer = player.GetComponent<Move_Player>();
                int dmg = (int)(status.getDamage() * 1.5);
                mPlayer.OnDamage(setCritical(dmg));
                int chance = Random.Range(0, 100);
                addSpeed();
                if (chance < 20)
                {
                    PlayerStatus playerStatus = mPlayer.status;
                    float moveSpeed = playerStatus.getMoveSpeed();
                    fourthSkill.power = 0.2F;
                    moveSpeed -= fourthSkill.power;
                    playerStatus.setMoveSpeed(moveSpeed);
                    fourthSkill.isSuccess = true;
                    fourthSkill.duration = 5 * 50;
                    addSpeed();
                }
            }
        }
        else
        {
            fourthSkill.coolTime--;
        }
        if (fourthSkill.isSuccess)
        {
            if (fourthSkill.duration <= 0)
            {
                PlayerStatus playerStatus = player.GetComponent<Move_Player>().status;
                float moveSpeed = playerStatus.getMoveSpeed() + fourthSkill.power;
                playerStatus.setMoveSpeed(moveSpeed);
                fourthSkill.isSuccess = false;
            }
            else
            {
                fourthSkill.duration--;
            }
        }
    }

    private void addSpeed()
    {
        speed++;
        status.setMoveSpeed(status.getMoveSpeed() * (1 + (speed * 0.1F)));
        onFlying();
    }

    private void onFlying()
    {
        if (speed % 5 == 0)
        {
            isFlyAttack = true;
            isFlying = true;
            Physics2D.IgnoreLayerCollision(10, 7, false);
            Rigidbody2D rbody = GetComponent<Rigidbody2D>();
            rbody.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
            StartCoroutine(passive(rbody, 0.5F));
            flyDuration = 30 * 50;
            for (int i = 0; i < 5; i++)
            {
                GameObject prefab = Resources.Load<GameObject>($"etc/Flight");
                prefab.GetComponent<Boss2_summon>().target = player;
                Vector2 pos = transform.position;
                pos.x += Random.Range(-2, 3);
                Instantiate(prefab, pos, Quaternion.Euler(0, 0, 0));
            }

        }
    }
}
