using System;

public abstract class InvetorCheckService
{
    public SlotDB AddMapModel(CollectableItem item)
    {
        var slot = new SlotDB
        {
            CollectableItem = new CollectableItemDB()
            {
                CanStack = item.CanStack,
                Description = item.Description,
                CollectablItemType = item.CollectablItemType,
                MaxSlotCount = item.MaxSlotCount,
                DropCount = item.DropCount,
                Name = item.Name,
            }
        };

        if (!item.CanStack)
        {
            slot.IsFull = true;
            slot.SlotCount = 1;
        }
        else
        {
            slot.SlotCount += 1;

            if (slot.SlotCount >= item.MaxSlotCount)
            {
                slot.IsFull = true;
            }
        }
        return slot;
    }

    public SlotDB ExistSlotCountAndIsFull(SlotDB slot, CollectableItem model)
    {
        if (slot != null && slot.CollectableItem.CanStack)
        {
            if (slot.SlotCount < model.MaxSlotCount)
            {
                slot.SlotCount += 1;

                if (slot.SlotCount >= model.MaxSlotCount)
                {
                    slot.IsFull = true;
                }
            }
        }
        return slot;
    }

    public SlotDB RemoveUpdateMap(SlotDB slotDB, int dropCount)
    {
        slotDB.SlotCount = Math.Max(slotDB.SlotCount - dropCount, 0);

        if (slotDB.SlotCount < slotDB.CollectableItem.MaxSlotCount)
        {
            slotDB.IsFull = false;
        }

        if (slotDB.SlotCount == 0)
        {
            slotDB.IsFull = false;
            slotDB.CollectableItem = null;
        }
        return slotDB;
    }


    public SlotDB UpdateMapModel(SlotDB getModel, SlotDB model)
    {
        getModel.SlotCount = model.SlotCount;
        getModel.IsFull = model.IsFull;
        return getModel;
    }

}
