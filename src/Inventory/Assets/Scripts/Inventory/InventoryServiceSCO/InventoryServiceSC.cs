using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = nameof(InventoryServiceSC))]
public class InventoryServiceSC : ScriptableObject
{
    public List<Slot> slots;

    private InventoryCheckServiceSCO inventoryCheckServiceSCO;

    public InventoryServiceSC()
    {
        slots = new List<Slot>();
        inventoryCheckServiceSCO = new InventoryCheckServiceSCO();
    }

    public bool AddItem(CollectableItem item)
    {
        var slot = FindSlot(item);

        if (slot != null)
        {
            inventoryCheckServiceSCO.ExistSlotCountAndIsFull(slot, item);
        }
        else
        {
            var mapSlot = inventoryCheckServiceSCO.MappModel(item);

            slots.Add(new Slot() { Id = mapSlot.Id, IsFull = mapSlot.IsFull, SlotCount = mapSlot.SlotCount, item = mapSlot.item });
            return true;
        }
        return false;
    }

    private Slot FindSlot(CollectableItem item)
    {
        foreach (var slot in slots)
        {
            if (slot.item == item && !slot.IsFull)
            {
                return slot;
            }
        }
        return null;
    }

    public bool Remove(int dropCount, string id, CollectableItem collectableItem)
    {
        var slot = slots.Where(x => x.Id == id && x.item.CollectablItemType == collectableItem.CollectablItemType).FirstOrDefault();
        if (slot == null)
            return false;

        slot.SlotCount = Math.Max(slot.SlotCount - dropCount, 0);

        if (slot.SlotCount < slot.item.MaxSlotCount)
        {
            slot.IsFull = false;
        }

        if (slot.SlotCount == 0)
        {
            slot.IsFull = false;
            slot.item = null;

            slots.Remove(slot);
        }
        return true;
    }

}
