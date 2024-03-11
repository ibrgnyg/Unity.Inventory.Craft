using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICraftModalController : MonoBehaviour
{
    [HideInInspector] public CraftItemData item;
    [HideInInspector] public RequiredItem craftRequiredItem;
    [HideInInspector] public List<GameObject> deleteCloneItems;

    public PickupItemController pickupItemController;
    public UIInventoryController uiInventoryController;
    public GameObject craftItem;
    public Transform pos;
    public Image icon;
    public TextMeshProUGUI text;
    public Button craftBtn, allCraftBtn;
    public GridLayoutGroup gridLayoutGroup;
    public float xOffset = 115f, xTripleOffset, fillTime, craftDuration = 2f;
    public Slider slider;

    private bool isBtnHold, anyExistItemCraftBtn;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetActviveModal(bool activeType)
    {
        if (item == null)
            return;

        text.text = item.Name;
        icon.sprite = item.Icon;
        craftRequiredItem = SCOService.GetScriptableObject<RequiredSetCount>().requiredItems.Where(x => x.CraftItemType == item.CraftItemType).FirstOrDefault();
        CreateRequiredCraftItem(craftRequiredItem.RequiredTypes);

        gameObject.SetActive(activeType);
    }

    private void CreateRequiredCraftItem(List<RequiredType> requiredType)
    {
        DeleteCloneItem();
        for (int i = 0; i < requiredType.Count; i++)
        {
            var requiredItem = requiredType[i];
            var craftPos = Instantiate(craftItem, pos);
            craftPos.SetActive(true);

            if (requiredType.Count == 1)
            {
                gridLayoutGroup.padding.right = 0;
            }
            else if (requiredType.Count == 2)
            {
                gridLayoutGroup.padding.right = 235;
            }
            else if (requiredType.Count == 3)
            {
                gridLayoutGroup.padding.right = 365;
            }
            deleteCloneItems.Add(craftPos);
            SetSpriteAndText(craftPos, requiredItem);
        }
    }

    private void SetSpriteAndText(GameObject posGame, RequiredType requiredType)
    {
        var collectableItem = SCOService.GetAllScriptableObjects<CollectableItem>().Where(x => x.CollectablItemType == requiredType.RequiredTypes).FirstOrDefault();

        posGame.transform.Find("Item_Image").GetComponent<Image>().sprite = collectableItem.Icon;
        var text = posGame.transform.Find("Item_Count").GetComponent<TextMeshProUGUI>();
        var inventoryCount = uiInventoryController.GetItemCount(requiredType.RequiredTypes).ToString();
        text.text = $"{inventoryCount}/{requiredType.RequiredCount}";

        if (inventoryCount == "0" || int.Parse(inventoryCount) < requiredType.RequiredCount)
        {
            posGame.GetComponent<Image>().color = Color.red;
            CanCraftItemBtn(false);
            anyExistItemCraftBtn = false;
        }
        else
        {
            posGame.GetComponent<Image>().color = Color.green;
            CanCraftItemBtn(true);
            anyExistItemCraftBtn = true;
        }
    }

    private void DeleteCloneItem()
    {
        if (deleteCloneItems.Count > 0)
        {
            foreach (var item in deleteCloneItems)
                Destroy(item);

            deleteCloneItems.Clear();
        }
    }

    //Craft

    private void CanCraftItemBtn(bool interactType)
    {
        if (!anyExistItemCraftBtn)
        {
            craftBtn.interactable = false;
            return;
        }
        craftBtn.interactable = interactType;
    }

    private void CraftItem()
    {
        var getCollecTableTypes = GetCollecTableTypes(item.CraftItemType);

        if (getCollecTableTypes == null)
            return;

        uiInventoryController.DropUpdateInventroy(craftRequiredItem, getCollecTableTypes);

        CreateRequiredCraftItem(craftRequiredItem.RequiredTypes);

        var addSlotItem = SCOService.GetAllScriptableObjects<CollectableItem>().Where(x => x.CollectablItemType == craftRequiredItem.SetCollectableItemType).FirstOrDefault();

        pickupItemController.AddItem(addSlotItem, true);
    }

    public List<CollectablItemType> GetCollecTableTypes(CraftItemType hintType)
    {
        List<CollectablItemType> collecTableTypes = new();

        var requiredItems = SCOService.GetScriptableObject<RequiredSetCount>().requiredItems.Where(x => x.CraftItemType == hintType).FirstOrDefault();

        foreach (var item in requiredItems.RequiredTypes)
        {
            collecTableTypes.Add(item.RequiredTypes);
        }

        return collecTableTypes;
    }

    private void Update()
    {
        if (isBtnHold)
        {
            fillTime += Time.deltaTime;

            if (fillTime >= craftDuration)
            {
                isBtnHold = false;
                slider.gameObject.SetActive(false);
                CraftItem();
                fillTime = 0f;
            }

            float fillAmount = fillTime / craftDuration;
            slider.value = fillAmount;
        }
    }

    public void OnButtonPointerDown()
    {
        if (!craftBtn.interactable)
            return;
        slider.gameObject.SetActive(true);
        isBtnHold = true;
        fillTime = 0f;
    }

    public void OnButtonPointerUp()
    {
        isBtnHold = false;
        fillTime = 0f;
        slider.value = fillTime;
        slider.gameObject.SetActive(false);
    }

}
