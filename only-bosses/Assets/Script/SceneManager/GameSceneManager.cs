using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSceneManager : MonoBehaviour
{
    public Transform characterSpawnPoint;
    public Transform bossSpawnPoint;

    // 캐릭터 이미지 | 캐릭터 HP bar ---------------- 보스 HP bar | 보스 이미지 
    public Image characterImage;
    public Sprite samuraiSprite;
    public Sprite magicianSprite;
    public Sprite gunnerSprite;

    // 스킬 이미지  
    public Image skillQIcon;
    public Image skillEIcon;
    public Image skillRIcon;

    // 쿨다운 이미지 
    public Image skillQCoolDownImage;
    public Image skillECoolDownImage;
    public Image skillRCoolDownImage;

    public Image bossImage;
    public Sprite boss1Sprite;
    public Sprite boss2Sprite;

    public HPBarUI characterHPBar;
    public HPBarUI bossHPBar;

    public Image qSkillImage;
    public TMP_Text qSkillText;
    public Image eSkillImage;
    public TMP_Text eSkillText;
    public Image rSkillImage;
    public TMP_Text rSkillText;

    public GameObject clearPanel;
    public GameObject overPanel;

    public AudioClip gameBGM;

    void Start()
    {
        SpawnCharacter();
        SpawnBoss();
        SetUI();
        
        BGMManager bgmManager = FindFirstObjectByType<BGMManager>();
        bgmManager.PlayBGM(gameBGM);

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
        movePlayer.endPanel = overPanel;

        movePlayer.skillQCooldownImage = skillQCoolDownImage;
        movePlayer.skillECooldownImage = skillECoolDownImage;
        movePlayer.skillRCooldownImage = skillRCoolDownImage;
    }

    void SpawnBoss()
    {
        // Boss1, Boss2
        string bossName = DataMgr.instance.currentBoss.ToString();
        GameObject bossPrefab = Resources.Load<GameObject>($"Bosses/{bossName}");
        GameObject bossInstance = Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);

        Boss bossScript = bossInstance.GetComponent<Boss>();
        bossScript.SetBossHPBar(bossHPBar);
        bossScript.endPanel = clearPanel;

        bossScript.player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void SetUI()
    {

        // 테스트 용 | status에서 가져와야함 
        switch (DataMgr.instance.currentCharacter)
        {
            case Character.Samurai:
                characterImage.sprite = samuraiSprite;

                skillQIcon.sprite = Resources.Load<Sprite>("SkillIcons/SamuraiSkillQ");
                skillEIcon.sprite = Resources.Load<Sprite>("SkillIcons/SamuraiSkillE");
                skillRIcon.sprite = Resources.Load<Sprite>("SkillIcons/SamuraiSkillR");

                qSkillImage.sprite = skillQIcon.sprite;
                qSkillText.text = "짧게 앞으로 이동 후 적을 두 번 벰";
                eSkillImage.sprite = skillEIcon.sprite;
                eSkillText.text = "공격속도에 비례한 횟수만큼 주변 적을 벰";
                rSkillImage.sprite = skillRIcon.sprite;
                rSkillText.text = "적에게 10초 동안 피해량 증가";
                break;

            case Character.Magician:
                characterImage.sprite = magicianSprite;

                skillQIcon.sprite = Resources.Load<Sprite>("SkillIcons/MagicianSkillQ");
                skillEIcon.sprite = Resources.Load<Sprite>("SkillIcons/MagicianSkillE");
                skillRIcon.sprite = Resources.Load<Sprite>("SkillIcons/MagicianSkillR");

                qSkillImage.sprite = skillQIcon.sprite;
                qSkillText.text = "전방으로 화염 마법 발사";
                eSkillImage.sprite = skillEIcon.sprite;
                eSkillText.text = "지정한 위치에 얼음 마법 발사";
                rSkillImage.sprite = skillRIcon.sprite;
                rSkillText.text = "지정한 위치에 번개 마법 발사";
                break;

            case Character.Gunner:
                characterImage.sprite = gunnerSprite;

                skillQIcon.sprite = Resources.Load<Sprite>("SkillIcons/GunnerSkillQ");
                skillEIcon.sprite = Resources.Load<Sprite>("SkillIcons/GunnerSkillE");
                skillRIcon.sprite = Resources.Load<Sprite>("SkillIcons/GunnerSkillR");

                qSkillImage.sprite = skillQIcon.sprite;
                qSkillText.text = "기절탄을 쏴서 기절 부여";
                eSkillImage.sprite = skillEIcon.sprite;
                eSkillText.text = "강력한 대포 발사";
                rSkillImage.sprite = skillRIcon.sprite;
                rSkillText.text = "8발의 유도탄 발사";
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
