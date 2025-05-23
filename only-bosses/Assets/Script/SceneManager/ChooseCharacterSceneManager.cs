using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseCharacterSceneManager : MonoBehaviour {
    
    public void Next() {

        SceneManager.LoadScene("ChooseItemScene");
    }

    public void Back() {

        SceneManager.LoadScene("StartScene");
    }
}
