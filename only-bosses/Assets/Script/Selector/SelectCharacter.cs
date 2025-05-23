using UnityEngine;

public class SelectCharacter : MonoBehaviour
{
    public Character character;
    public GameObject highlight; 

    Animator animator;
    public SelectCharacter[] chars;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (DataMgr.instance.currentCharacter == character)
            OnSelect();
        else
            OnDeSelect();
    }

    private void OnMouseUpAsButton()
    {
        DataMgr.instance.currentCharacter = character;
        OnSelect();

        for (int i = 0; i < chars.Length; i++)
        {
            if (chars[i] != this) chars[i].OnDeSelect();
        }
    }

    void OnDeSelect()
    {
        animator.SetBool("walk", false);
        if (highlight != null) highlight.SetActive(false); 
    }

    void OnSelect()
    {
        animator.SetBool("walk", true);
        if (highlight != null) highlight.SetActive(true);  
    }
}
