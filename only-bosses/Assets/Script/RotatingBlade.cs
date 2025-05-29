using UnityEngine;

public class RotatingBlade : MonoBehaviour
{
    public Transform boss;
    public float radius = 1.5f;
    public float speed = 100f;
    public float angle = 0f;

    private Boss1_Script bossScript;

    void Start()
    {
        GameObject bossObject = GameObject.FindWithTag("Boss");
        bossScript = bossObject.GetComponent<Boss1_Script>();
    }

    void Update()
    {
        angle += speed * Time.deltaTime;
        if (angle >= 360f) angle -= 360f;

        // 원형 궤도 좌표 계산
        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * radius;

        transform.position = boss.position + offset;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // if (oteher.CompareTag("Player")) other.GetComponent<Move_Player>.onDamage(bossScript.getDamage());
        Debug.Log("칼날 타격");
    }
}
