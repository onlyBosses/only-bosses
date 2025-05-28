using System.Runtime.CompilerServices;
using Mono.Cecil;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Rendering;

class FirstSkill
{
    public int coolTime;
    public bool isSuccess;
    public int duration;

    public FirstSkill()
    {
        coolTime = 0;
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
        coolTime = 0;
        isSuccess = false;
        duration = 0;
        power = 0;
    }
}

public class Boss2_Script : Boss
{
    public string playerName;
    private GameObject player;
    private int attackCoolTime;
    private FirstSkill firstSkill;
    private SecondSkill secondSkill;
    private int thirdSkillCoolTime;
    private int fourthSkillCoolTime;
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        rbody.gravityScale = 0;
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        player = GameObject.Find(playerName);
        status = new BossStatus(16000, 2, 10, 5, 10, 50, 5);
        attackCoolTime = status.getAttackSpeed() * 50;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Vector2 dir = (player.transform.position - transform.position).normalized;
        rbody.linearVelocity = new Vector2(dir.x, dir.y);
        float distance = Vector2.Distance(player.transform.position, transform.position);
        if (attackCoolTime <= 0)
        {
            if (distance <= 2)
            {
                attackCoolTime = status.getAttackSpeed() * 50;
                Move_Player mPlayer = player.GetComponent<Move_Player>();
                mPlayer.onDamage(setCritical());
            }
        }
        else
        {
            attackCoolTime--;
        }
    }

    private int setCritical()
    {
        int dmg = status.getDamage();
        int chance = Random.Range(0, 100);
        if (chance < status.getCriticalChance())
        {
            dmg *= 1 + (status.getCriticalDamage() / 100);
        }
        return dmg;
    }

    private void useFirstSkill()
    {
        if (firstSkill.coolTime <= 0)
        {
            firstSkill.coolTime = Random.Range(32, 49) * 50;
            if (Physics2D.Raycast(player.transform.position, Vector3.down, 1F, LayerMask.GetMask("Ground")))
            {
                Rigidbody2D rbody = player.GetComponent<Rigidbody2D>();
                rbody.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
                rbody.gravityScale = 0;
                firstSkill.isSuccess = true;
                firstSkill.duration = 3 * 50;
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
                secondSkill.duration = 5;
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
}
