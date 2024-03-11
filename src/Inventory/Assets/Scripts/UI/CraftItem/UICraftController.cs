using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UICraftController : MonoBehaviour
{
    private Transform content;
    public GameObject slot, modal;

    [HideInInspector] public List<CraftItemData> craftItems;
    [HideInInspector] public List<Button> buttons;

    void Start()
    {
        content = GetComponent<RectTransform>();
        craftItems = SCOService.GetAllScriptableObjects<CraftItemData>().Where(x => x.Active).ToList();

        for (int i = 0; i < craftItems.Count; i++)
        {
            var craftItemData = craftItems[i];
            var createDFItems = Instantiate(slot, content);
            createDFItems.name = $"CraftItem_{craftItemData.CraftItemType}";

            var uiCreafItem = createDFItems.GetComponent<UICraftItem>();
            uiCreafItem.item = craftItemData;
            uiCreafItem.modal = modal;

            var button = createDFItems.GetComponent<Button>();

            SetSprite(button, craftItemData);
            buttons.Add(button);
        }
    }

    public void SetSprite(Button button, CraftItemData item)
    {
        var image = button.transform.Find("Image").GetComponent<Image>();
        image.sprite = item.Icon;
    }
}
