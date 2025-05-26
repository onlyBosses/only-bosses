using UnityEngine;
using UnityEngine.UI;

public class HPBarUI : MonoBehaviour
{
    public Image fillImage;

    public void SetHP(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }
}
