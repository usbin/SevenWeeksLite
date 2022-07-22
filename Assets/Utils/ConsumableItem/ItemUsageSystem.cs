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
    public void OnItemUse(Player player, int itemCode)
    {
        if (!ItemTable.LoadDone)
        {
            return;
        }

        ItemData? itemData = ItemTable.GetItemData(itemCode);
        if(itemData.HasValue)
        {
            itemData.Value.UsageEvent.Raise(player);
        }
    }
    public void AddListener(int itemCode, ItemUsageEventListener.ItemUsageEventHandler handler)
    {
        if (!ItemTable.LoadDone)
        {
            return;
        }
        ItemData? itemData = ItemTable.GetItemData(itemCode);
        if (itemData.HasValue)
        {
            ItemUsageEventListener listener = new ItemUsageEventListener(itemData.Value.UsageEvent, handler);
            itemData.Value.UsageEvent.RegisterListener(listener);
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
