using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveFileManager
{
    private static bool _isLoaded = false;
    private static PlayerData _playerData;

    public static bool SaveData(SaveData saveData)
    {
        try
        {
            string json = JsonUtility.ToJson(saveData);
            File.WriteAllText(SAVE_PATH, json);
            return true;
        } catch(Exception e)
        {
            Debug.Log(e.ToString());
            return false;
        }
    }
    public static PlayerData LoadPlayerData()
    {
        if (!_isLoaded || _playerData == null)
        {
            LoadSaveData();
        }
        return _playerData;
    }
    private static void LoadSaveData()
    {
        if (!File.Exists(SAVE_PATH))
        {
            Debug.Log("세이브 파일이 존재하지 않습니다!");
            return;
        }
        string json = File.ReadAllText(SAVE_PATH);
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);
        InventoryData inventoryData = new InventoryData(saveData.Inventory);
        PlayerData playerData = new PlayerData(saveData.PlayerName, inventoryData);
        _playerData = playerData;

        _isLoaded = true;
    }

    static readonly string SAVE_PATH = Application.dataPath + "/Save/SaveData.json";
}
