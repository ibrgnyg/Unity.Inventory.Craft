public class SlotDB : BaseEntity
{
    public bool IsFull { get; set; } = false;
    public int SlotCount { get; set; } = 0;
    public CollectableItemDB CollectableItem { get; set; } = new CollectableItemDB();
}
