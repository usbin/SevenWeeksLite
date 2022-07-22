using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class ItemUsageSystem : MonoBehaviour
{
    
    public static ItemUsageSystem Instance;
    //----- 외부에서 호출하는 함수
    //이벤트 등록 함수.(아이템코드, Action)
    // - 이벤트를 등록하는 시점에 아이템 사용 이벤트가 초기화되어있지 않을 경우 고려.
    // - 이벤트 등록과 아이템 사용 이벤트의 초기화 시점을 독립적으로 구현해야함.
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
