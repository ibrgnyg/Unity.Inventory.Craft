using UnityEngine;

[CreateAssetMenu(menuName = nameof(CraftItemData))]
public class CraftItemData : ScriptableObject
{
    public string Name;
    [TextArea(4, 10)]
    public string Description;
    public Sprite Icon;
    public GameObject Prefab;
    public CraftItemType CraftItemType;
    public bool Active;
}
