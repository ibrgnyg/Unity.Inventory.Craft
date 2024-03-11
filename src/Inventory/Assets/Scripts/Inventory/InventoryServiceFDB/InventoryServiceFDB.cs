using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryServiceFDB : InvetorCheckService
{
    //DatabaseReference db;

    private const string path = "Inventory";

    void Initialize()
    {
        try
        {
            //db = FirebaseDatabase.DefaultInstance.RootReference;
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    public bool UpdateItem(CollectableItem item)
    {
        Initialize();
        try
        {
            Debug.Log("FireBase Test");
            var slot = AddMapModel(item);

            string jsonContent = JsonConvert.SerializeObject(slot);

            //db.Child(path).SetRawJsonValueAsync(jsonContent);
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }

        return true;
    }

    public List<SlotDB> GetSlotDBs()
    {

        return new List<SlotDB>();
    }

}
