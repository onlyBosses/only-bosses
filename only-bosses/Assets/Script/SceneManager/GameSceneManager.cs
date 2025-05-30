using UnityEngine;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    public Transform characterSpawnPoint;
    public Transform bossSpawnPoint;

    // 캐릭터 이미지 | 캐릭터 HP bar ---------------- 보스 HP bar | 보스 이미지 
    public Image characterImage;
    public Sprite samuraiSprite;
    public Sprite magicianSprite;
    public Sprite gunnerSprite;

    public Image bossImage;
    public Sprite boss1Sprite;
    public Sprite boss2Sprite;

    public HPBarUI characterHPBar;
    public HPBarUI bossHPBar;
    
    void Start()
    {
        SpawnCharacter();
        SpawnBoss();
        SetUI();
    }

    void SpawnCharacter()
    {
        // Samurai, Magician, Gunner
        string characterName = DataMgr.instance.currentCharacter.ToString();
        GameObject characterPrefab = Resources.Load<GameObject>($"Characters/{characterName}");

        // (-6, 0, 0) // test
        GameObject characterInstance = Instantiate(characterPrefab, characterSpawnPoint.position, characterSpawnPoint.rotation);

        Move_Player movePlayer = characterInstance.GetComponent<Move_Player>();
        movePlayer.SetPlayerHPBar(characterHPBar);
    }

    void SpawnBoss()
    {
        // Boss1, Boss2
        string bossName = DataMgr.instance.currentBoss.ToString();
        GameObject bossPrefab = Resources.Load<GameObject>($"Bosses/{bossName}");
        GameObject bossInstance = Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);

        Boss bossScript = bossInstance.GetComponent<Boss>();
        bossScript.SetBossHPBar(bossHPBar);

        bossScript.player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void SetUI()
    {

        // 테스트 용 | status에서 가져와야함 
        switch (DataMgr.instance.currentCharacter)
        {
            case Character.Samurai:
                characterImage.sprite = samuraiSprite;
                break;
            case Character.Magician:
                characterImage.sprite = magicianSprite;
                break;
            case Character.Gunner:
                characterImage.sprite = gunnerSprite;
                break;
        }


        switch (DataMgr.instance.currentBoss)
        {
            case BossType.Boss1:
                bossImage.sprite = boss1Sprite;
                break;
            case BossType.Boss2:
                bossImage.sprite = boss2Sprite;
                break;
        }
    }
}
