
using UnityEngine;

public class CraftItem 
{
    public string Name;

    [TextArea(4, 10)]
    public string Description;

    public Sprite Icon;

    public GameObject Prefab;

    //public CrafItemType types;

    public bool Active;
}
