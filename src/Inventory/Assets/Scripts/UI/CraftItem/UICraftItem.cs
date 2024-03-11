using UnityEngine;

public class UICraftItem : MonoBehaviour
{
    public CraftItemData item;
    public GameObject modal;

    public void ShowModalAndSetItem()
    {
        var uiModal = modal.GetComponent<UICraftModalController>();
        uiModal.item = item;

        uiModal.SetActviveModal(true);
    }
}
