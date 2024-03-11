using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UIInventoryController : MonoBehaviour
{
    /*[HideInInspector]*/
    public List<Button> slotButtons;

    public GameObject slot, modalItem;

    private IInventoryService _inventoryService;
    private InventoryServiceFile _inventoryServiceFile;
    private Transform itemContent;
    private InventoryType selectInventoryType;
    private SetInventroyResoruceType resourceType;

    void Start()
    {
        itemContent = GetComponent<RectTransform>();
        resourceType = SCOService.GetScriptableObject<SetInventroyResoruceType>();

        GetInventoryResources();
        UpdateUISlot();
    }

    private void GetInventoryResources()
    {
        switch (resourceType.InventoryResourceType)
        {
            case InventoryType.LOCAL:
                selectInventoryType = InventoryType.LOCAL;
                var localInventory = _inventoryServiceFile = new InventoryServiceFile();
                var lcInventories = localInventory.GetSlotsFile();
                CreateInventorySlot(lcInventories);
                break;
            case InventoryType.MDB:
                selectInventoryType = InventoryType.MDB;
                var mdbInventoryService = _inventoryService = new InventoryService();
                var slotsMDB = mdbInventoryService.Table.ToList();
                CreateInventorySlot(slotsMDB);
                break;
            case InventoryType.SCO:
                selectInventoryType = InventoryType.SCO;
                var slots = SCOService.GetScriptableObject<InventoryServiceSC>().slots;
                CreateInventorySlot(slots);
                break;
        }
    }

    private void CreateInventorySlot<T>(List<T> values)
    {
        try
        {
            for (int i = 0; i < values.Count; i++)
            {
                var getSlot = values[i];

                if (getSlot is Slot sl)
                {
                    if (sl.item == null || sl.SlotCount == 0)
                        continue;
                }

                UpdateCreateInventorySlot(getSlot, i);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
    }

    private void UpdateCreateInventorySlot<T>(T entity, int i)
    {
        var createSlot = Instantiate(slot, itemContent);
        if (entity is SlotDB slotDB)
        {
            createSlot.name = slotDB.CollectableItem == null ? "Item_" + i : slotDB.CollectableItem.CollectablItemType.ToString();
        }
        else if (entity is Slot slot)
        {
            createSlot.name = slot.item == null ? "Item_" + i : slot.item.CollectablItemType.ToString();
        }

        var button = createSlot.GetComponent<Button>();

        slotButtons.Add(button);
    }

    public void UpdateUISlot(bool reloadAll = false)
    {
        if (reloadAll)
        {
            AllClearInventory();
            GetInventoryResources();
            reloadAll = false;
        }

        if (slotButtons != null && slotButtons.Count == 0)
        {
            GetInventoryResources();
        }

        List<SlotDB> inventoreis = new List<SlotDB>();

        List<Slot> slots = new List<Slot>();

        switch (selectInventoryType)
        {
            case InventoryType.LOCAL:
                inventoreis = _inventoryServiceFile.GetSlotsFile();
                break;
            case InventoryType.MDB:
                inventoreis = _inventoryService.Table.
                    Select(x => new SlotDB() { Id = x.Id, CollectableItem = x.CollectableItem, SlotCount = x.SlotCount }).ToList();
                break;
            case InventoryType.FDB:
                break;
            case InventoryType.SCO:
                slots = SCOService.GetScriptableObject<InventoryServiceSC>().slots;
                break;
            default:
                break;
        }

        for (int i = 0; i < slotButtons.Count; i++)
        {
            var button = slotButtons[i];
            var uIInventoryItem = button.GetComponent<UIInventoryItem>();

            switch (selectInventoryType)
            {
                case InventoryType.LOCAL:
                    var inventoryLocal = inventoreis[i];

                    var mapCollectableLocalItem = MapCollectableItem(inventoryLocal);

                    uIInventoryItem.Id = inventoryLocal.Id;
                    uIInventoryItem.SlotCount = inventoryLocal.SlotCount;
                    uIInventoryItem.modal = modalItem;
                    uIInventoryItem.item = mapCollectableLocalItem;

                    button = UpdateUITextAndSprite(button, mapCollectableLocalItem.Icon, inventoryLocal.SlotCount);

                    if (inventoreis.Count > slotButtons.Count)
                    {
                        if (inventoryLocal.CollectableItem != null && inventoryLocal.SlotCount > 0)
                        {
                            UpdateCreateInventorySlot(inventoryLocal, i);
                        }
                    }
                    break;
                case InventoryType.MDB:

                    var inventoryItem = inventoreis[i];
                    var mapCollectableItem = MapCollectableItem(inventoryItem);

                    uIInventoryItem.Id = inventoryItem.Id;
                    uIInventoryItem.SlotCount = inventoryItem.SlotCount;
                    uIInventoryItem.modal = modalItem;
                    uIInventoryItem.item = mapCollectableItem;

                    button = UpdateUITextAndSprite(button, mapCollectableItem.Icon, inventoryItem.SlotCount);

                    if (inventoreis.Count > slotButtons.Count)
                    {
                        if (inventoryItem.CollectableItem != null && inventoryItem.SlotCount > 0)
                        {
                            UpdateCreateInventorySlot(inventoryItem, i);
                        }
                    }
                    break;
                case InventoryType.FDB:
                    break;
                case InventoryType.SCO:
                    var inventory = slots[i];

                    uIInventoryItem.Id = inventory.Id;
                    uIInventoryItem.SlotCount = inventory.SlotCount;
                    uIInventoryItem.modal = modalItem;
                    uIInventoryItem.item = inventory.item;
                    button = UpdateUITextAndSprite(button, inventory.item.Icon, inventory.SlotCount);

                    if (slots.Count > slotButtons.Count)
                    {
                        if (inventory.item != null && inventory.SlotCount > 0)
                        {
                            UpdateCreateInventorySlot(inventory, i);
                        }
                    }
                    break;
            }
        }
    }

    public void AllClearInventory()
    {
        slotButtons.Clear();

        foreach (Transform child in itemContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private CollectableItem MapCollectableItem(SlotDB slot)
    {
        var collectableItem = SCOService.GetAllScriptableObjects<CollectableItem>().ToList().Where(x => x.CollectablItemType == slot.CollectableItem.CollectablItemType).FirstOrDefault();
        return new CollectableItem()
        {
            CollectablItemType = slot.CollectableItem.CollectablItemType,
            CanStack = slot.CollectableItem.CanStack,
            Icon = collectableItem.Icon,
        };
    }

    private Button UpdateUITextAndSprite(Button btn, Sprite icon, int slotCount)
    {
        var image = btn.transform.Find("Image").GetComponent<Image>();
        image.sprite = slotCount > 0 ? icon : null;

        var count = btn.transform.Find("Count").GetComponent<TextMeshProUGUI>();
        count.text = slotCount > 1 ? slotCount.ToString() : string.Empty;

        return btn;
    }

    public int GetItemCount(CollectablItemType collectablItemType)
    {
        switch (selectInventoryType)
        {
            case InventoryType.LOCAL:
                return _inventoryServiceFile.GetSlotsFile().Where(x => x.CollectableItem.CollectablItemType == collectablItemType).Sum(x => x.SlotCount);
            case InventoryType.MDB:
                return _inventoryService.Table.Where(x => x.CollectableItem.CollectablItemType == collectablItemType).Sum(x => x.SlotCount);
            case InventoryType.FDB:
                break;
            case InventoryType.SCO:
                return SCOService.GetScriptableObject<InventoryServiceSC>().slots.Where(x => x.item.CollectablItemType == collectablItemType).Sum(x => x.SlotCount);
        }
        return 0;
    }

    public void DropUpdateInventroy(RequiredItem requiredItem, List<CollectablItemType> collectablItemTypes)
    {
        try
        {
            switch (selectInventoryType)
            {
                case InventoryType.LOCAL:
                    var slotDBFile = _inventoryServiceFile.GetSlotsFile().Where(x => collectablItemTypes.Contains(x.CollectableItem.CollectablItemType)).ToList();

                    var slotDBFiles = _inventoryServiceFile.GetSlotsFile();

                    foreach (var item in slotDBFile)
                    {
                        var inventories = slotDBFiles.Where(x => x.CollectableItem.CollectablItemType == item.CollectableItem.CollectablItemType).ToList();

                        var dropCount = GetRequiredMaterialsCount(requiredItem, item.CollectableItem.CollectablItemType);

                        foreach (var slot in inventories)
                        {
                            slot.SlotCount = Math.Max(0, slot.SlotCount - dropCount);
                            _inventoryServiceFile.Remove(dropCount, item.Id, MapCollectableItem(item));
                        }
                    }
                    break;
                case InventoryType.MDB:

                    var slotDBs = _inventoryService.Table.Where(x => collectablItemTypes.Contains(x.CollectableItem.CollectablItemType)).ToList();

                    var slotsDB = _inventoryService.Table;

                    foreach (var item in slotDBs)
                    {
                        var inventories = slotsDB.Where(x => x.CollectableItem.CollectablItemType == item.CollectableItem.CollectablItemType).ToList();

                        var dropCount = GetRequiredMaterialsCount(requiredItem, item.CollectableItem.CollectablItemType);

                        foreach (var slot in inventories)
                        {
                            slot.SlotCount = Math.Max(0, slot.SlotCount - dropCount);
                            _inventoryService.Remove(dropCount, item.Id, MapCollectableItem(item));
                        }
                    }
                    break;
                case InventoryType.FDB:
                    break;
                case InventoryType.SCO:
                    var collectableItems = SCOService.GetAllScriptableObjects<CollectableItem>().Where(x => collectablItemTypes.Contains(x.CollectablItemType)).ToList();

                    var slots = SCOService.GetScriptableObject<InventoryServiceSC>().slots;

                    foreach (var item in collectableItems)
                    {
                        var inventories = slots.Where(x => x.item != null && x.item.CollectablItemType == item.CollectablItemType).ToList();

                        var deletedCount = GetRequiredMaterialsCount(requiredItem, item.CollectablItemType);

                        foreach (var slot in inventories)
                        {
                            slot.SlotCount = Math.Max(0, slot.SlotCount - deletedCount);

                            if (slot.SlotCount == 0)
                            {
                                slot.IsFull = false;
                                slot.item = null;
                                slots.RemoveAll(x => x.Id == slot.Id);
                            }
                        }
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    public int GetRequiredMaterialsCount(RequiredItem requiredItem, CollectablItemType collectablItemType)
    {
        return requiredItem.RequiredTypes.Where(x => x.RequiredTypes == collectablItemType).FirstOrDefault().RequiredCount;
    }
}
