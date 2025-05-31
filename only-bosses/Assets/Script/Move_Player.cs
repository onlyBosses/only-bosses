using UnityEngine;

public class Move_Player : MonoBehaviour, PlayerInterface, Fearable
{
    private float vx;
    private bool isJump;
    private bool isJumpPush;
    public int stiffenTime;
    protected Rigidbody2D rbody;
    protected SpriteRenderer spriteRenderer;
    public PlayerStatus status;

    // 보스1 스킬2: 공포 -> 보스 반대 방향으로 이동 (움직임 제어)
    private bool isFeared = false;
    private Vector2 fearDirection;
    private float fearTimer = 0f;

    void Start()
    {
        // rbody = this.GetComponent<Rigidbody2D>();
        // rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        // spriteRenderer = this.GetComponent<SpriteRenderer>();
        // vx = 0;
        // isJumpPush = false;
        // isJump = false;
    }

    public void init()
    {
        rbody = this.GetComponent<Rigidbody2D>();
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        vx = 0;
        isJumpPush = false;
        isJump = false;
        stiffenTime = 0;
    }

    void Update()
    {
        // vx = 0;
        // if (Input.GetKey("a"))
        // {
        //     vx = -1;
        // }

        // if (Input.GetKey("d"))
        // {
        //     vx = 1;
        // }
        // if (Input.GetKey("w"))
        // {
        //     if (!isJumpPush)
        //     {
        //         if (Physics2D.Raycast(transform.position, Vector3.down, 1F, LayerMask.GetMask("Ground")))
        //         {
        //             isJump = true;
        //         }
        //         isJumpPush = true;
        //     }
        // }
        // else
        // {
        //     isJumpPush = false;
        // }

        // if (Input.GetKey("s"))
        // {
        // }
    }

    public void inputMove()
    {
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

        if (Input.GetKey("s"))
        {

        }

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
    }

    public void move()
    {
        if (stiffenTime > 0)
        {
            stiffenTime--;
            return;
        }
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        GetComponent<SpriteRenderer>().flipX = worldPos.x < transform.position.x;
        Debug.Log(status.getMoveSpeed());
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

    public void onDamage(int damage)
    {
        if (stiffenTime < 25)
        {
            stiffenTime = 25;
        }
        spriteRenderer.color = new Color(200 / 255F, 200 / 255F, 200 / 255F);
        int health = status.getHealth();
        health -= damage;
        if (health <= 0)
        {

        }
        status.setHealth(health);
    }

    // 보스1 스킬2: 공포 -> 보스 반대 방향으로 이동 (움직임 제어)
    public void Fear(Vector3 bossPos)
    {
        isFeared = true;
        fearTimer = 2f;
        Vector2 dirToBoss = (bossPos - transform.position).normalized;
        fearDirection = -dirToBoss;
    }
}
