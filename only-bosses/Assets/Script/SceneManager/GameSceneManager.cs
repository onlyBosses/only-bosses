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

    // 캐릭터 HP
    private float characterMaxHP;
    private float characterCurrentHP;

    // 보스 HP
    private float bossMaxHP;
    private float bossCurrentHP;
    
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

        // (-6, 0, 0)
        Instantiate(characterPrefab, characterSpawnPoint.position, characterSpawnPoint.rotation);
    }

    void SpawnBoss()
    {
        // Boss1, Boss2
        string bossName = DataMgr.instance.currentBoss.ToString();
        GameObject bossPrefab = Resources.Load<GameObject>($"Bosses/{bossName}");
        GameObject bossInstance = Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);

        if (DataMgr.instance.currentBoss == Boss.Boss1)
        {
            Boss1_Script bossScript = bossInstance.GetComponent<Boss1_Script>();
            bossScript.SetBossHPBar(bossHPBar);
        }
        // else if (DataMgr.instance.currentBoss == Boss.Boss2)
        // {
        //     Boss2_Script bossScript = bossInstance.GetComponent<Boss2_Script>();
        //     bossScript.SetBossHPBar(bossHPBar);
        // }
    }

    void SetUI()
    {

        // 테스트 용 | status에서 가져와야함 

        switch (DataMgr.instance.currentCharacter)
        {
            case Character.Samurai:
                characterImage.sprite = samuraiSprite;
                characterMaxHP = 200f;
                break;
            case Character.Magician:
                characterImage.sprite = magicianSprite;
                characterMaxHP = 120f;
                break;
            case Character.Gunner:
                characterImage.sprite = gunnerSprite;
                characterMaxHP = 150f;
                break;
        }

        characterCurrentHP = characterMaxHP;
        characterHPBar.SetHP(characterCurrentHP, characterMaxHP);

        switch (DataMgr.instance.currentBoss)
        {
            case Boss.Boss1:
                bossImage.sprite = boss1Sprite;
                // status에서 가져오기 
                bossMaxHP = 16000;
                break;
            case Boss.Boss2:
                bossImage.sprite = boss2Sprite;
                bossMaxHP = 800f;
                break;
        }
        bossCurrentHP = bossMaxHP;
        bossHPBar.SetHP(bossCurrentHP, bossMaxHP);
    }
}
