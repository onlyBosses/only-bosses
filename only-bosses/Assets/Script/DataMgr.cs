using UnityEngine;

public enum Character
{
    Samurai, Magician, Gunner
}

public enum Weapon
{
    None, Sword, Sword2, Wand, Wand2, Gun, Gun2
}

public enum Ring
{
    None, Ring1, Ring2, Ring3
}

public enum Necklace
{
    None, Necklace1, Necklace2, Necklace3
}


public class DataMgr : MonoBehaviour
{

    public static DataMgr instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    // 선택한 캐릭터 다른 씬에서도 사용 
    public Character currentCharacter;

    public Weapon selectedWeapon = Weapon.None;
    public Ring selectedRing = Ring.None;
    public Necklace selectedNecklace = Necklace.None;
}
