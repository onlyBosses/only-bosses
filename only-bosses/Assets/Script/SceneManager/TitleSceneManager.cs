using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour {
    
    public void StartGame() {

        SceneManager.LoadScene("ChooseCharacterScene");
    }

    public void QuitGame() {

        Application.Quit();
        Debug.Log("에디터에서는 게임종료 안 된다고 하네요");
    }
}
