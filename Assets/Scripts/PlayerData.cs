using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public string Name;
    public InventoryData InventoryData;
    public PlayerData(string name, InventoryData inventoryData)
    {
        Name = name;
        InventoryData = inventoryData;
    }
}
