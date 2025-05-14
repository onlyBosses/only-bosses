using UnityEngine;

public enum Character {
    Samurai, Magician, Gunner
}

public class DataMgr : MonoBehaviour {
    
    public static DataMgr instance;

    private void Awake() {
        if (instance == null) instance = this;
        else if (instance != null) return;

        DontDestroyOnLoad(gameObject); // 씬 전환 후에도 오브젝트 파괴 X 
    }

    // 선택한 캐릭터 다른 씬에서도 사용 
    public Character currentCharacter;
}
