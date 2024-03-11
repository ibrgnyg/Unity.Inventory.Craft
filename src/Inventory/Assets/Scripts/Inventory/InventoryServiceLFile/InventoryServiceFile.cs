using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class InventoryServiceFile : InvetorCheckService
{
    private static string fileName = "inventory";

    private static string fileType = "json";

    public static string path = $"{Application.persistentDataPath}/{fileName}.{fileType}";

    private List<SlotDB> slots = new List<SlotDB>();

    public string GetFilePath() => path;

    public bool Update(CollectableItem item)
    {
        try
        {
            if (item == null)
                return false;

            var slot = FindSlot(item);

            if (slot != null)
            {
                slot = ExistSlotCountAndIsFull(slot, item);

                UpdateIntoJsonFile(slot);

                return true;
            }

            SaveIntoJsonFile(item);

            return true;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);

            return false;
        }
    }

    public List<SlotDB> GetSlotsFile()
    {
        Debug.Log(path);
        List<SlotDB> slots = new List<SlotDB>();

        if (!File.Exists(path))
        {
            return null;
        }

        using (StreamReader reader = new StreamReader(path))
        {
            string json = reader.ReadToEnd();

            return JsonConvert.DeserializeObject<List<SlotDB>>(json);
        }
    }

    private SlotDB FindSlot(CollectableItem item)
    {
        slots = GetSlotsFile();

        if (slots == null)
            return null;

        foreach (var slot in slots)
        {
            if (slot.CollectableItem.CollectablItemType == item.CollectablItemType && !slot.IsFull)
            {
                return slot;
            }
        }
        return null;
    }

    private void UpdateIntoJsonFile(SlotDB slot)
    {
        try
        {
            List<SlotDB> existingItems = new List<SlotDB>();

            if (File.Exists(path))
            {
                string existingJsonContent = File.ReadAllText(path);
                existingItems = JsonConvert.DeserializeObject<List<SlotDB>>(existingJsonContent);
            }

            bool updated = false;

            for (int i = 0; i < existingItems.Count; i++)
            {
                if (existingItems[i].Id == slot.Id)
                {
                    existingItems[i] = slot;
                    updated = true;
                    break;
                }
            }

            if (!updated)
            {
                existingItems.Add(slot);
            }

            string jsonContents = JsonConvert.SerializeObject(existingItems, Formatting.Indented);
            File.WriteAllText(path, jsonContents);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private void SaveIntoJsonFile(CollectableItem item)
    {
        try
        {
            if (!File.Exists(path))
            {
                File.WriteAllText(path, string.Empty);
            }

            var slot = AddMapModel(item);

            List<SlotDB> existingItems = new List<SlotDB>();

            string existingJsonContent = File.ReadAllText(path);
            existingItems = JsonConvert.DeserializeObject<List<SlotDB>>(existingJsonContent);

            if (existingItems != null && existingItems.Count > 0)
            {
                existingItems.Add(slot);

                string jsonContents = JsonConvert.SerializeObject(existingItems, Formatting.Indented);
                File.WriteAllText(path, jsonContents);
            }
            else
            {
                var slots = new List<SlotDB>
                {
                    slot
                };

                string jsonContent = JsonConvert.SerializeObject(slots, Formatting.Indented);
                File.WriteAllText(path, jsonContent);
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public bool Remove(int dropCount, string id, CollectableItem collectableItem)
    {
        var getSlot = GetSlotsFile().Where(x => x.Id == id && x.CollectableItem.CollectablItemType == collectableItem.CollectablItemType).FirstOrDefault();

        if (getSlot == null)
            return false;

        var updateSlot = RemoveUpdateMap(getSlot, dropCount);

        if (updateSlot.CollectableItem == null)
        {
            UpdateRead(updateSlot);
            return true;
        }
        UpdateIntoJsonFile(updateSlot);
        return true;
    }

    private void UpdateRead(SlotDB removeSlot)
    {
        var slots = GetSlotsFile();

        slots.RemoveAll(x => x.Id == removeSlot.Id);

        string jsonContents = JsonConvert.SerializeObject(slots, Formatting.Indented);
        File.WriteAllText(path, jsonContents);
    }
}
