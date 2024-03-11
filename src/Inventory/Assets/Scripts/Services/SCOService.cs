using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SCOService : MonoBehaviour
{
    public static List<TEntiy> GetAllScriptableObjects<TEntiy>() where TEntiy : ScriptableObject
    {
        return AssetDatabase.FindAssets($"t: {typeof(TEntiy).Name}").ToList()
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .Select(AssetDatabase.LoadAssetAtPath<TEntiy>)
                    .ToList();
    }

    public static TEntiy GetScriptableObject<TEntiy>() where TEntiy : ScriptableObject
    {
        try
        {
            string[] guids = AssetDatabase.FindAssets($"t: {typeof(TEntiy).Name}");
            if (guids.Length > 0)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
                return AssetDatabase.LoadAssetAtPath<TEntiy>(assetPath);
            }
            return null;
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex);
            throw;
        }
    }
}
