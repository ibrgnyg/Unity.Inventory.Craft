using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = nameof(RequiredSetCount))]
public class RequiredSetCount : ScriptableObject
{
    public List<RequiredItem> requiredItems = new();
}
