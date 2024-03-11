using System;

public class InventoryCheckServiceSCO
{
    public Slot MappModel(CollectableItem hintItem)
    {
        var slot = new Slot
        {
            Id = Guid.NewGuid().ToString(),
            item = hintItem
        };

        if (!slot.item.CanStack)
        {
            slot.IsFull = true;
            slot.SlotCount = 1;
        }
        else
        {
            slot.SlotCount += 1;

            if (slot.SlotCount >= slot.item.MaxSlotCount)
            {
                slot.IsFull = true;
            }
        }
        return slot;
    }


    public Slot ExistSlotCountAndIsFull(Slot slot, CollectableItem model)
    {
        if (slot.item.CanStack)
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

}
