using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemUsageEventListener
{
    private ItemUsageEvent _event;
    private ItemUsageEventHandler _handler;
    public void OnEventRaise(Player player)
    {
        _handler(player);
    }
    public ItemUsageEventListener(ItemUsageEvent __event, ItemUsageEventHandler handler)
    {
        _event = __event;
        _handler = handler;
    }

    public delegate void ItemUsageEventHandler(Player player);
}
