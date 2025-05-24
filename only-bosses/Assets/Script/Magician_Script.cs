using UnityEngine;

public class Magician_Script : MonoBehaviour
{
    private Animator animator;
    private bool isBaseAttack = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        isBaseAttack = false;
    }

    void Update()
    {
        bool isWalking = Input.GetKey("a") || Input.GetKey("d") || Input.GetKey("w");
        animator.SetBool("IsWalking", isWalking);

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("AttackA");
        }
    }

    public void baseAttack()
    {
        string[] projectileNames = { "fire", "ice", "bolt" };
        string selectedProjectile = projectileNames[Random.Range(0, projectileNames.Length)];

        GameObject projectilePrefab = Resources.Load<GameObject>($"etc/{selectedProjectile}");

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z; 

        // 방향 벡터 (마우스 방향)
        Vector3 direction = (mousePos - transform.position).normalized;

        // 프리팹 생성 위치 
        Vector3 spawnOffset = direction * 1.5f; 
        Vector3 spawnPos = transform.position + spawnOffset;

        // 프리팹 생성
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        // 프리팹 좌우 전환
        SpriteRenderer sr = projectile.GetComponent<SpriteRenderer>();
        sr.flipX = direction.x < 0f;

        // 프리팹 슛 
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.linearVelocity = direction * 5f; 
    }
}
