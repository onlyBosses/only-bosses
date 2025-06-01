using UnityEngine;
using UnityEngine.UI;

public class Avoid
{
    public bool isAvoid;
    public int avoidCoolTime;
    public int duration;
    public string avoidAnimation;

    public Avoid()
    {
        isAvoid = false;
        avoidCoolTime = 0;
        duration = 0;
    }

    // public bool isAvoiding()
    // {
    //     return isAvoid;
    // }

    // public int getAvoidCoolTime()
    // {
    //     return avoidCoolTime;
    // }

    // public int getDuration()
    // {
    //     return duration;
    // }

    // public string getAvoidAnimation()
    // {
    //     return avoidAnimation;
    // }

    // public void setAvoid(bool avoid)
    // {
    //     isAvoid = avoid;
    // }
}

public class Move_Player : MonoBehaviour, PlayerInterface, Fearable
{
    private float vx;
    private bool isJump;
    private bool isJumpPush;
    protected int stiffenTime;
    protected Rigidbody2D rbody;
    protected SpriteRenderer spriteRenderer;
    protected Avoid avoid;
    public PlayerStatus status;

    public HPBarUI playerHPBar;

    public Image skillQCooldownImage;
    public Image skillECooldownImage;
    public Image skillRCooldownImage;

    // 보스1 스킬2: 공포 -> 보스 반대 방향으로 이동 (움직임 제어)
    private bool isFeared = false;
    private Vector2 fearDirection;
    private float fearTimer = 0f;

    public GameObject endPanel;

    void Start()
    {
        endPanel.SetActive(false);
    }

    public void init()
    {
        rbody = GetComponent<Rigidbody2D>();
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        spriteRenderer = GetComponent<SpriteRenderer>();
        vx = 0;
        isJumpPush = false;
        isJump = false;
        stiffenTime = 0;
        avoid = new Avoid();
        Physics2D.IgnoreLayerCollision(10, 7, true);
    }

    void Update()
    {

    }

    public void inputMove()
    {

        // 보스1 스킬2: 공포 -> 보스 반대 방향으로 이동 (움직임 제어)
        if (isFeared)
        {
            transform.Translate(fearDirection * 2f * Time.deltaTime);
            fearTimer -= Time.deltaTime;
            if (fearTimer <= 0f)
            {
                isFeared = false;
            }
            return;
        }
        
        vx = 0;
        if (Input.GetKey("a"))
        {
            vx = -1;
        }

        if (Input.GetKey("d"))
        {
            vx = 1;
        }
        if (Input.GetKey("w"))
        {
            if (!isJumpPush)
            {
                if (Physics2D.Raycast(transform.position, Vector3.down, 1F, LayerMask.GetMask("Ground")))
                {
                    isJump = true;
                }
                isJumpPush = true;
            }
        }
        else
        {
            isJumpPush = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (avoid.avoidCoolTime <= 0)
            {
                rbody.linearVelocity = Vector2.zero;
                avoid.isAvoid = true;
                avoid.avoidCoolTime = 3 * 50;
                avoid.duration = 50;
                GetComponent<Animator>().Play(avoid.avoidAnimation);
                Vector2 vec = transform.right * 5;
                if (GetComponent<SpriteRenderer>().flipX)
                {
                    vec *= -1;
                }
                rbody.linearVelocity = vec;
            }
        }
    }

    public void move()
    {
        if (stiffenTime > 0)
        {
            stiffenTime--;
            rbody.linearVelocity = Vector2.zero;
            return;
        }
        spriteRenderer.color = new Color(1, 1, 1);
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        GetComponent<SpriteRenderer>().flipX = worldPos.x < transform.position.x;
        if (avoid.avoidCoolTime > 0)
        {
            avoid.avoidCoolTime--;
        }
        if (avoid.isAvoid)
        {
            if (avoid.duration <= 0)
            {
                avoid.isAvoid = false;
            }
            else
            {
                avoid.duration--;
            }
            return;
        }
        rbody.linearVelocity = new Vector2(vx * status.getMoveSpeed(), rbody.linearVelocityY);
        if (isJump)
        {
            isJump = false;
            rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Vector3 mousePos = Input.mousePosition;
        // Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        // this.GetComponent<SpriteRenderer>().flipX = worldPos.x < transform.position.x;
        // rbody.linearVelocity = new Vector2(vx, rbody.linearVelocityY);
        // if (isJump)
        // {
        //     isJump = false;
        //     rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
        // }
    }

    public void SetPlayerHPBar(HPBarUI hpBar)
    {
        playerHPBar = hpBar;
    }

    public void OnDamage(int damage)
    {
        if (avoid.isAvoid)
        {
            return;
        }
        if (stiffenTime < 25)
        {
            stiffenTime = 25;
        }

        spriteRenderer.color = new Color(200 / 255F, 200 / 255F, 200 / 255F);

        int currentHp = status.getHealth();
        currentHp -= damage;


        if (currentHp <= 0) //죽음
        {
            endPanel.SetActive(true);
            Time.timeScale = 0f;
        }

        status.setHealth(currentHp);

        playerHPBar.SetHP(currentHp, status.getMaxHealth());
    }

    // 보스1 스킬2: 공포 -> 보스 반대 방향으로 이동 (움직임 제어)
    public void Fear(Vector3 bossPos)
    {
        isFeared = true;
        fearTimer = 2f;
        Vector2 dirToBoss = (bossPos - transform.position).normalized;
        fearDirection = -dirToBoss;
    }

    public void setStiffenTime(int duration)
    {
        stiffenTime = duration;
    }

    protected int setCritical(int dmg)
    {
        int chance = Random.Range(0, 100);
        if (chance < status.getCriticalChance())
        {
            dmg *= 1 + (status.getCriticalDamage() / 100);
        }
        return dmg;
    }
}
