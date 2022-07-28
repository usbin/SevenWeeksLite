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
    public bool OnItemUse(Player player, int itemCode)
    {

        if (!ItemTable.LoadDone)
        {
            Debug.Log("아이템 사용 에러: 아이템테이블이 아직 로드되지 않았습니다.");
            return false;
        }

        ItemInfo? itemInfo = ItemTable.GetItemInfo(itemCode);
        if(!itemInfo.HasValue)
        {
            Debug.Log("아이템 사용 에러: 아이템테이블에 없는 아이템 코드입니다.");
            return false;
            
        }
        Debug.Log("아이템 사용됨! (" + ItemTable.GetItemInfo(itemCode).Value.Name);
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
