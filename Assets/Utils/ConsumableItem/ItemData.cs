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
            Debug.Log("������ ��� ����: �������� ����� �� �����ϴ�.(�����۽ý��� ����)");
            return false;
        }

        return ItemUsageSystem.Instance.OnItemUse(player, Code);
        
       
    }

    
}
