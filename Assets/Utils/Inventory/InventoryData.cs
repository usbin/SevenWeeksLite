using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryData
{
    [SerializeField]
    public List<ItemData> ItemList = new List<ItemData>();

    public InventoryData(List<ItemData> itemList)
    {
        ItemList = itemList;
    }
    public void UseItem(Player player, int itemCode)
    {
        int index = ItemList.FindIndex(match => match.Code == itemCode);
        if(index != -1)
        {
            bool done = ItemList[index].Use(player);
            if(done)
                ItemList.RemoveAt(index);
        }
    }
}
