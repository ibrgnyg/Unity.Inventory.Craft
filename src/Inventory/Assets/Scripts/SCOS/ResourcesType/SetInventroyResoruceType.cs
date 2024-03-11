using UnityEngine;

[CreateAssetMenu(menuName = nameof(SetInventroyResoruceType))]
public class SetInventroyResoruceType : ScriptableObject
{
    public string Name = "inventory resoruce type";
    public string DBName = "InventoryDB";
    public InventoryType InventoryResourceType;

    bool EnableDisableList = false;

    [ShowIf(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(EnableDisableList))]
    public string ConnectionString = "mongodb://localhost:27017";
}