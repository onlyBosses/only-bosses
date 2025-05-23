using UnityEngine;
using UnityEngine.EventSystems;

public class SelectItem : MonoBehaviour, IPointerClickHandler
{
    public enum ItemType { Weapon, Ring, Necklace }
    public ItemType itemType;
    public int itemId;

    public GameObject highlight; 

    public static SelectItem currentSelectedWeapon;
    public static SelectItem currentSelectedRing;
    public static SelectItem currentSelectedNecklace;

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (itemType)
        {
            case ItemType.Weapon:
                DataMgr.instance.selectedWeapon = (Weapon)itemId;
                if (currentSelectedWeapon != null)
                    currentSelectedWeapon.highlight.SetActive(false);

                currentSelectedWeapon = this;
                highlight.SetActive(true);
                break;

            case ItemType.Ring:
                DataMgr.instance.selectedRing = (Ring)itemId;
                if (currentSelectedRing != null)
                    currentSelectedRing.highlight.SetActive(false);

                currentSelectedRing = this;
                highlight.SetActive(true);
                break;

            case ItemType.Necklace:
                DataMgr.instance.selectedNecklace = (Necklace)itemId;
                if (currentSelectedNecklace != null)
                    currentSelectedNecklace.highlight.SetActive(false);

                currentSelectedNecklace = this;
                highlight.SetActive(true);
                break;
        }
    }
}
