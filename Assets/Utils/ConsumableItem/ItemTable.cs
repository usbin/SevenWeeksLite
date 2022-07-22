using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ItemTable
{
    public static readonly string ITEM_JSON_PATH = "Assets/Utils/ConsumableItem/Jsons/";

    public static bool LoadDone = false;
    [SerializeField]
    private static IDictionary<int, Item> _itemList = new Dictionary<int, Item>();

    public static ItemData? GetItemData(int itemCode)
    {
        if (_itemList.ContainsKey(itemCode))
            return _itemList[itemCode].ItemData;
        else return null;
    }
    public static void LoadItems()
    {
        //Json 파일에서 Items.json 파일을 읽어옴.
        string jstring = File.ReadAllText(ITEM_JSON_PATH + "Items.json", System.Text.Encoding.UTF8);
        ItemData[] itemDataList = JsonUtility.FromJson<Serialization<ItemData>>(jstring).list;

        foreach (ItemData itemData in itemDataList)
        {
            Item item = new Item(itemData);
            item.ItemData.UsageEvent = new ItemUsageEvent(item);
            _itemList.Add(itemData.Code, item);
        }
        LoadDone = true;
    }
}
