using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemUsageEventListener
{
    private ItemUsageEvent _event;
    private ItemUsageEventHandler _handler;
    private UnityEvent _response;
    public void OnEventRaise(Player player)
    {
        _handler(player);
    }
    //��ũ��Ʈ���� �������� �������͸� �߰��ؾ� �� �� ���.
    public ItemUsageEventListener(ItemUsageEvent __event, ItemUsageEventHandler handler)
    {
        _event = __event;
        _handler = handler;
    }

    public delegate void ItemUsageEventHandler(Player player);
}
