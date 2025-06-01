using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ESCMenuManager : MonoBehaviour
{
    public GameObject escMenuPanel; 

    private bool isEsc = false;

    void Start()
    {
        escMenuPanel.SetActive(false); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isEsc = !isEsc;
            escMenuPanel.SetActive(isEsc);
        }

        if (isEsc) Time.timeScale = 0;
        else Time.timeScale = 1f;
    }

    public void GoToCharacterSelect()
    {
        Time.timeScale = 1;
        AudioClip bgm = Resources.Load<AudioClip>("01 Horns Of War");
        BGMManager.instance.PlayBGM(bgm);
        SceneManager.LoadScene("ChooseCharacterScene");
    }

    public void GoToTitle()
    {
        Time.timeScale = 1;
        AudioClip bgm = Resources.Load<AudioClip>("01 Horns Of War");
        BGMManager.instance.PlayBGM(bgm);
        SceneManager.LoadScene("StartScene");
    }

    public void CloseESCMenu()
    {
        escMenuPanel.SetActive(false);
    }
}
