using UnityEngine;

[CreateAssetMenu(menuName = nameof(CollectableItem))]
public class CollectableItem : ScriptableObject
{
    public string Name;
    [TextArea(4, 10)]
    public string Description;
    public CollectablItemType CollectablItemType;
    public bool CanStack;
    public int MaxSlotCount;
    public int DropCount;
    public Sprite Icon;
    public GameObject Prefab;
}

