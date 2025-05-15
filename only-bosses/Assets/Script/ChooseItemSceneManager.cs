using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseItemSceneManager : MonoBehaviour {
    
    public Button nextButton;

    void Update() {
        

        bool allSelected = 
            DataMgr.instance.selectedWeapon != Weapon.None &&
            DataMgr.instance.selectedRing != Ring.None &&
            DataMgr.instance.selectedNecklace != Necklace.None;

        nextButton.interactable = allSelected;
    }

    public void ItemToBoss()
    {

        SceneManager.LoadScene("ChooseBossScene");
    }

    public void ItemToCharacter() {

        SceneManager.LoadScene("ChooseCharacterScene");
    }
}
