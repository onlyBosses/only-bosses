using UnityEngine;

public class SelectBoss : MonoBehaviour {
    
    public Boss boss; 
    Animator animator;
    public SelectBoss[] bosses;
    public GameObject highlight; 

    void Start()
    {
        animator = GetComponent<Animator>();
        if (DataMgr.instance.currentBoss == boss) OnSelect();
        else OnDeSelect();
    }

    private void OnMouseUpAsButton() {
        DataMgr.instance.currentBoss = boss;
        OnSelect();
        for (int i = 0; i < bosses.Length; i++) {
            if (bosses[i] != this) bosses[i].OnDeSelect();
        }
    }

    void OnDeSelect()
    {
        animator.SetBool("attack", false);
        if (highlight != null) highlight.SetActive(false); 
    }

    void OnSelect()
    {
        animator.SetBool("attack", true);
        if (highlight != null) highlight.SetActive(true); 
    }
}
