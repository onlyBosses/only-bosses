using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    public Transform characterSpawnPoint;
    public Transform bossSpawnPoint;

    void Start()
    {
        SpawnCharacter();
        SpawnBoss();
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

        // (6, 0, 0)
        Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
    }
}
