using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {
    public ItemData ItemData;
    public Item(ItemData itemData)
    {
        ItemData = itemData;
    }
    public void Use(Player player)
    {
        if (ItemUsageSystem.Instance)
        {
            ItemUsageSystem.Instance.OnItemUse(player, ItemData.Code);
        }
    }

    
}
