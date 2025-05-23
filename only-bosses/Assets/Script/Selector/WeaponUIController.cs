using UnityEngine;

public class WeaponUIController : MonoBehaviour
{
    public GameObject samuraiWeaponGroup;
    public GameObject magicianWeaponGroup;
    public GameObject gunnerWeaponGroup;

    void Start()
    {
        ShowWeaponForCharacter(DataMgr.instance.currentCharacter);
    }

    void ShowWeaponForCharacter(Character character)
    {
        samuraiWeaponGroup.SetActive(false);
        magicianWeaponGroup.SetActive(false);
        gunnerWeaponGroup.SetActive(false);

        switch (character)
        {
            case Character.Samurai:
                samuraiWeaponGroup.SetActive(true);
                break;
            case Character.Magician:
                magicianWeaponGroup.SetActive(true);
                break;
            case Character.Gunner:
                gunnerWeaponGroup.SetActive(true);
                break;
        }
    }
}
