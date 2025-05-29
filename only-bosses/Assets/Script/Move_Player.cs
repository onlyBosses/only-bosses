using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.VFX;

public class Move_Player : MonoBehaviour, PlayerInterface
{
    private float vx;
    private bool isJump;
    private bool isJumpPush;
    protected Rigidbody2D rbody;
    protected SpriteRenderer spriteRenderer;
    public PlayerStatus status;
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
    }

    public void move()
    {
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
        PlayerStatus status = GetComponent<Move_Player>().status;
        spriteRenderer.color = new Color(200 / 255F, 200 / 255F, 200 / 255F);
        int health = status.getHealth();
        health -= damage;
        if (health <= 0) //죽음
        {

        }
        status.setHealth(health);
        Debug.Log(health);
    }
}
