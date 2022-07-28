using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ItemTable
{
    public static readonly string ITEM_JSON_PATH = "Assets/Utils/ConsumableItem/Jsons/"; //프로젝트 루트폴더부터 시작
    public static readonly string ITEM_SPRITE_PATH = "Sprites/Item/"; //Resources/부터 시작
    private static bool _loadDone = false;
    public static bool LoadDone
    {
        get => _loadDone;
    }
    [SerializeField]
    private static IDictionary<int, ItemInfo> _itemDictionary = new Dictionary<int, ItemInfo>();

    public static ItemData GenerateItemData(int itemCode, int amount)
    {
        if (!LoadDone) LoadItems();
        if (_itemDictionary.ContainsKey(itemCode))
        {
            return new ItemData(_itemDictionary[itemCode], amount);
        }
        else return null;
    }
    public static ItemInfo? GetItemInfo(int itemCode)
    {
        if (!LoadDone) LoadItems();

        if (_itemDictionary.ContainsKey(itemCode))
            return _itemDictionary[itemCode];
        else return null;
    }
    public static void LoadItems()
    {
        //Json 파일에서 Items.json 파일을 읽어옴.
        string jstring = File.ReadAllText(ITEM_JSON_PATH + "Items.json", System.Text.Encoding.UTF8);
        ItemInfo[] itemInfoList = JsonUtility.FromJson<Serialization<ItemInfo>>(jstring).list;

        for(int i=0; i<itemInfoList.Length; i++)
        {
            itemInfoList[i].UsageEvent = new ItemUsageEvent(itemInfoList[i]);
            _itemDictionary.Add(itemInfoList[i].Code, itemInfoList[i]);
        }
        _loadDone = true;
    }
}
