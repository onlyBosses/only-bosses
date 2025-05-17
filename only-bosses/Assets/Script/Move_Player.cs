using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.VFX;

public class Move_Player : MonoBehaviour
{
    private float vx;
    private bool isJump;
    private bool isJumpPush;
    private Rigidbody2D rbody;
    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        vx = 0;
        isJumpPush = false;
        isJump = false;
    }

    void Update()
    {
        vx = 0;
        if(Input.GetKey("a")) {
            vx = -1;
        }

        if(Input.GetKey("d")) {
            vx = 1;
        }
        if(Input.GetKey("w")) {
            if(!isJumpPush) {
                if(Physics2D.Raycast(transform.position, Vector3.down, 1F, LayerMask.GetMask("Ground"))) {
                    isJump = true;
                }
                isJumpPush = true;
            }
        }
        else {
            isJumpPush = false;
        }

        if(Input.GetKey("s")) {

        }
    }

    void FixedUpdate()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        this.GetComponent<SpriteRenderer>().flipX = worldPos.x < this.transform.position.x;
        rbody.linearVelocity = new Vector2(vx, rbody.linearVelocityY);
        if(isJump) {
            isJump = false;
            rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
        }
    }
}
