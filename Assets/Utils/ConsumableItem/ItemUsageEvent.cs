using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemUsageEvent
{
    private ItemInfo _itemInfo;
    private List<ItemUsageEventListener> _listeners = new List<ItemUsageEventListener>();
    public ItemUsageEvent(ItemInfo item)
    {
        _itemInfo = item;
    }
    public void RegisterListener(ItemUsageEventListener listener)
    {
        _listeners.Add(listener);
    }
    public void UnregisterListener(ItemUsageEventListener listener)
    {
        _listeners.Remove(listener);
    }

    public void Raise(Player player)
    {
        foreach(ItemUsageEventListener listener in _listeners)
        {
            listener.OnEventRaise(player);
        }
    }

}
