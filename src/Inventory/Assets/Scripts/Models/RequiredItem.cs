using System.Collections.Generic;

[System.Serializable]
public class RequiredItem
{
    public CraftItemType CraftItemType;
    public CollectablItemType SetCollectableItemType;
    public List<RequiredType> RequiredTypes = new();
}
