using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBarUI : MonoBehaviour
{
    public Image fillImage;
    [SerializeField] private TMP_Text hpText;

    public void SetHP(float current, float max)
    {
        fillImage.fillAmount = current / max;
        hpText.text = $"{current} / {max}";
    }
}
