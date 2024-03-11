public class CollectableItemDB
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public CollectablItemType CollectablItemType { get; set; } = CollectablItemType.Empty;
    public bool CanStack { get; set; } = true;
    public int MaxSlotCount { get; set; } = 0;
    public int DropCount { get; set; } = 0;
}
