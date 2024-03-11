using UnityEngine;

public class UIInventoryItem : MonoBehaviour
{
    public CollectableItem item;
    public GameObject modal;

    [HideInInspector] public string Id;
    [HideInInspector] public int SlotCount;

    public void ShowModalAndSetItem()
    {
        var uiModal = modal.GetComponent<UIModalController>();

        uiModal.id = Id;
        uiModal.collectableItem = item;
        uiModal.itemImage.sprite = item.Icon;
        uiModal.itemCountText.text = SlotCount.ToString();

        uiModal.SetActviveModal();
    }
}
