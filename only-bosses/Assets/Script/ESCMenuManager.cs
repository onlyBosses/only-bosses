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
    }

    public void GoToCharacterSelect()
    {
        SceneManager.LoadScene("ChooseCharacterScene");
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void CloseESCMenu()
    {
        escMenuPanel.SetActive(false);
    }
}
