using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseBossSceneManager : MonoBehaviour
{

    public void SelectEasy()
    {
        DataMgr.instance.selectedDifficulty = Difficulty.Easy;
        BossToGame();
    }

    public void SelectHard()
    {
        DataMgr.instance.selectedDifficulty = Difficulty.Hard;
        BossToGame();
    }


    void BossToGame()
    {

        SceneManager.LoadScene("GameScene");
    }

    public void GameToItem()
    {
        SceneManager.LoadScene("ChooseItemScene");
    }
}
