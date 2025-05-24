using UnityEngine;
using UnityEngine.UI;

public class HealthTest : MonoBehaviour
{
    public Image playerHpImage; // 드래그로 HP Image 할당
    public Image bossHpImage;   // 드래그로 보스 HP Image 할당

    private float playerHp = 1f; // 시작 체력 (1 = 100%)
    private float bossHp = 1f;   // 시작 체력

    public float decreaseSpeed = 0.1f; // 초당 감소 비율

    void Update()
    {

        Debug.Log("HealthTest Update 실행 중!");
        // 체력을 서서히 감소시킴
        playerHp -= decreaseSpeed * Time.deltaTime;
        bossHp -= (decreaseSpeed / 2f) * Time.deltaTime;

        // 0 밑으로 떨어지지 않게 clamp
        playerHp = Mathf.Clamp01(playerHp);
        bossHp = Mathf.Clamp01(bossHp);

        // fillAmount로 UI 업데이트
        playerHpImage.fillAmount = playerHp;
        bossHpImage.fillAmount = bossHp;
    }
}
