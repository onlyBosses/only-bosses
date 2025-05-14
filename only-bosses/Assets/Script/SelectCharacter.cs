using UnityEngine;

public class SelectCharacter : MonoBehaviour {
    
    public Character character;
    Animator animator;
    public SelectCharacter[] chars;
    void Start()
    {
        animator = GetComponent<Animator>();
        if (DataMgr.instance.currentCharacter == character) OnSelect();
        else OnDeSelect();
    }

    // 캐릭터 초기화
    private void OnMouseUpAsButton() {
        DataMgr.instance.currentCharacter = character;
        OnSelect();
        for (int i = 0; i < chars.Length; i++) {
            if (chars[i] != this) chars[i].OnDeSelect();
        }
    }

    void OnDeSelect() {
        animator.SetBool("walk", false);
    }

    void OnSelect() {
        animator.SetBool("walk", true);
    }
}
