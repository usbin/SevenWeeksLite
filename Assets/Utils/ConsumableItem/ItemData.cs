using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ItemData {
    public int Code;
    public int Amount;
    public ItemData(ItemInfo itemInfo, int amount)
    {
        Code = itemInfo.Code;
        Amount = amount;
    }
    public bool Use(Player player)
    {
        if (!ItemUsageSystem.Instance)
        {
            Debug.Log("아이템 사용 에러: 아이템을 사용할 수 없습니다.(아이템시스템 없음)");
            return false;
        }

        return ItemUsageSystem.Instance.OnItemUse(player, Code);
        
       
    }

    
}
