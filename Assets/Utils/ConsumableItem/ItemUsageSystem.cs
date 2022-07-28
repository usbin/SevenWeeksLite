using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class ItemUsageSystem : MonoBehaviour
{
    
    public static ItemUsageSystem Instance;
    //----- �ܺο��� ȣ���ϴ� �Լ�
    //�̺�Ʈ ��� �Լ�.(�������ڵ�, Action)
    // - �̺�Ʈ�� ����ϴ� ������ ������ ��� �̺�Ʈ�� �ʱ�ȭ�Ǿ����� ���� ��� ���.
    // - �̺�Ʈ ��ϰ� ������ ��� �̺�Ʈ�� �ʱ�ȭ ������ ���������� �����ؾ���.
    public bool OnItemUse(Player player, int itemCode)
    {

        if (!ItemTable.LoadDone)
        {
            Debug.Log("������ ��� ����: ���������̺��� ���� �ε���� �ʾҽ��ϴ�.");
            return false;
        }

        ItemInfo? itemInfo = ItemTable.GetItemInfo(itemCode);
        if(!itemInfo.HasValue)
        {
            Debug.Log("������ ��� ����: ���������̺� ���� ������ �ڵ��Դϴ�.");
            return false;
            
        }
        Debug.Log("������ ����! (" + ItemTable.GetItemInfo(itemCode).Value.Name);
        itemInfo.Value.UsageEvent.Raise(player);
        return true;
    }
    public void AddItemUsageListener(int itemCode, ItemUsageEventListener.ItemUsageEventHandler handler)
    {
        if (!ItemTable.LoadDone)
        {
            return;
        }
        ItemInfo? itemInfo = ItemTable.GetItemInfo(itemCode);
        if (itemInfo.HasValue)
        {
            ItemUsageEventListener listener = new ItemUsageEventListener(itemInfo.Value.UsageEvent, handler);
            itemInfo.Value.UsageEvent.RegisterListener(listener);
        }
        
    }
    

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            Destroy(this);
            return;
        }
        Instance = this;

        if (!ItemTable.LoadDone) ItemTable.LoadItems();

        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
