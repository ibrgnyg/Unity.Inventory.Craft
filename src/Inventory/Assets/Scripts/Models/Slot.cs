using System;

[Serializable]
public class Slot
{
    public string Id; 

    public bool IsFull;

    public int SlotCount;

    public CollectableItem item;
}
