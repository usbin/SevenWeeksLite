using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    private InventoryData _inventoryData;
    public GameObject EmptyCellPrefab;
    public GameObject InventoryItemPrefab;
    public GameObject ConsumableItemDetailViewPrefab;
    public Scrollbar VerticalScrollbar;
    public ScrollRect ScrollViewRect;
    private bool _loadDone = false;
    private Player _owner;
    private GameObject _lastSelectedItem; //���������� Ŭ���� �׸�
    private GameObject _focusedView;

    private void ProtectFocusOut()
    {
        if (_focusedView == null
            && EventSystem.current != null && EventSystem.current.currentSelectedGameObject == null)
        {
            SetEnableAllItemCells(true);
            SetInteractiveAllItemCells(true);
            //�����ִ� �����Ϻ䰡 ������ ��Ŀ���� ���� ���
            if (_lastSelectedItem != null)
            {
                EventSystem.current.SetSelectedGameObject(_lastSelectedItem);
            }
            //�� ���� Ŭ���߰ų� ��Ŀ���� �������� ���� ����ؼ� ��Ŀ���� ���� ���
            else
            {
                if (ScrollViewRect.content.childCount > 0)
                {
                    _lastSelectedItem = ScrollViewRect.content.GetChild(0).GetChild(0).gameObject;
                    EventSystem.current.SetSelectedGameObject(_lastSelectedItem);
                }

            }
        }
    }
    public void UpdateScroll()
    {
        //**************************** ���ǻ��� **************************
        //*** Content ���� child�� pivot�� �ݵ�� y�� 1�� ��           ***
        //*** childRect�� �ݵ�� Content ���� child�� Rect�� ������ �� ***
        //****************************************************************
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
        ScrollRect scrollRect = ScrollViewRect;
        GameObject content = scrollRect.content.gameObject;
        //���õ� �� ��ư�� ��+���Ʒ� �� ��ư �ϳ��� ������ ��
        if ((selectedObject != null && selectedObject.transform.IsChildOf(content.transform))
            && (Control.IsPressed(Control.KeyList.Up) || Control.IsPressed(Control.KeyList.Down)))
        {
            //selectedRect = Content ���� child�� �ִ� ������Ʈ�� RectTransform���� ���
            RectTransform childRect = selectedObject.transform.parent.GetComponent<RectTransform>();
            float heightUnit = Math.Abs(childRect.rect.height); //�� �� ����
            float wholeHeight = content.GetComponent<RectTransform>().rect.height; //��ü ����
            float viewHeight = scrollRect.viewport.rect.height; //�� ȭ���� ����(�� ���� ����Ǵ� ȭ���� ����)
            float countPerWindow = (int)viewHeight / (int)heightUnit;//�� ȭ�鿡 ���̴� ����
            float currentY = childRect.anchoredPosition.y;
            //������ ���� ��, �� �� ������ �ƴϸ鼭 ��ũ���� ����������� ���� ��(Ŭ ��)
            if (currentY <= -(heightUnit * (countPerWindow))
                && Control.IsPressed(Control.KeyList.Down)
                && scrollRect.verticalScrollbar.value > (1 + (currentY + heightUnit * (countPerWindow - 1)) / (wholeHeight - viewHeight)))
            {
                scrollRect.verticalScrollbar.value = (1 + (currentY + heightUnit * (countPerWindow - 1)) / (wholeHeight - viewHeight));
            }
            //�ø��� ���� ��, �� �Ʒ� ������ �ƴϸ鼭 ��ũ���� ����������� �Ʒ��� ��(���� ��)
            if (currentY >= (-wholeHeight + (heightUnit * (countPerWindow + 1)))
                && Control.IsPressed(Control.KeyList.Up)
                && scrollRect.verticalScrollbar.value < (1 + (currentY) / (wholeHeight - viewHeight)))
            {
                scrollRect.verticalScrollbar.value = (1 + (currentY / (wholeHeight - viewHeight)));
            }

        }
    }
    private void ScrollbarSyncronize()
    {
        
        if(EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            UpdateScroll();
           
        }

    }
    private void OnPressEsc()
    {
        if (Control.IsPressDown(Control.KeyList.Esc))
        {
            if(_focusedView == null)
                Destroy(gameObject);
        }
    }

    private void SetInteractiveAllItemCells(bool b)
    {
        for (int i = 0; i < ScrollViewRect.content.transform.childCount; i++)
        {
            UnityEngine.UI.Button item = ScrollViewRect.content.transform.GetChild(i).GetChild(0).GetComponent<UnityEngine.UI.Button>();
            item.interactable = b;
        }
    }
    private void SetEnableAllItemCells(bool b)
    {
        for (int i = 0; i < ScrollViewRect.content.transform.childCount; i++)
        {
            UnityEngine.UI.Button item = ScrollViewRect.content.transform.GetChild(i).GetChild(0).GetComponent<UnityEngine.UI.Button>();
            item.enabled = b;
        }
    }
    private void OnItemUse(ItemData itemData)
    {
        itemData.Use(_owner);
        itemData.Amount--;
        if (itemData.Amount <= 0)
        {
            int index = _inventoryData.ItemList.FindIndex(match=>match.Code == itemData.Code);
            _inventoryData.ItemList.RemoveAt(index);
            GameObject item = ScrollViewRect.GetComponent<ScrollRect>().content.GetChild(index).GetChild(0).gameObject;
            Destroy(item.transform.parent.gameObject);
            Destroy(item.transform.gameObject);
        }
    }
    private GameObject ShowDetailView(ItemData itemData)
    {
        ConsumableItemDetailView consumableItemDetailView = Instantiate(ConsumableItemDetailViewPrefab).GetComponent<ConsumableItemDetailView>();
        consumableItemDetailView.Initialize(_owner, itemData, ()=>
        {
            OnItemUse(itemData);
            ReloadInventoryItems();
        });
        consumableItemDetailView.transform.SetParent(transform, false);
        return consumableItemDetailView.gameObject;
    }
    private Sprite LoadSprite(int itemCode)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(ItemTable.ITEM_SPRITE_PATH + itemCode);
        if(sprites != null && sprites.Length > 0)
        {
            return sprites[0];
        }
        else
        {
            //��������Ʈ�� ���� ���
            Sprite[] unknownSprites = Resources.LoadAll<Sprite>(ItemTable.ITEM_SPRITE_PATH + itemCode);
            if (unknownSprites != null && unknownSprites.Length > 0)
                return unknownSprites[0];
            else return null;
        }
        
    }
    private GameObject CreateItemCell(ItemData itemData)
    {
        GameObject cell = Instantiate(EmptyCellPrefab);
        GameObject item = Instantiate(InventoryItemPrefab);
        
        item.GetComponent<UnityEngine.UI.Image>().sprite = LoadSprite(itemData.Code);
        item.transform.GetChild(0).GetComponent<Text>().text = itemData.Amount.ToString(); //Amount �ؽ�Ʈ
        cell.transform.SetParent(ScrollViewRect.GetComponent<ScrollRect>().content.transform);
        item.transform.SetParent(cell.transform);
        return item;
    }
    private int FindInInventory(ItemData itemData)
    {
        return _inventoryData.ItemList.FindIndex(match => match.Code == itemData.Code);
    }
    private void ReloadInventoryItems()
    {
        for (int i = 0; i < _inventoryData.ItemList.Count; i++)
        {
            ItemData itemData = _inventoryData.ItemList[i];
            int index = FindInInventory(itemData);
            if (index != -1)
            {
                //�κ��丮�� ���� ��� amount�� ���ΰ�ħ��.
                GameObject item = ScrollViewRect.GetComponent<ScrollRect>().content.GetChild(index).GetChild(0).gameObject;
                item.transform.GetChild(0).GetComponent<Text>().text = itemData.Amount.ToString(); //Amount �ؽ�Ʈ
            }
            else
            {
                //�κ��丮�� ���� ��� �߰���.
                GameObject item = CreateItemCell(itemData);
                item.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                {
                    SetEnableAllItemCells(false);
                    item.GetComponent<UnityEngine.UI.Button>().interactable = false;
                    GameObject detailView = ShowDetailView(itemData);
                    _lastSelectedItem = item;
                    _focusedView = detailView;

                });
            }
            


        }
    }
    private void LoadInventoryItems()
    {
        //�κ��丮 ĭ�� �� �� ä���
        //�� �ȿ� �κ��丮 ������ �ְ�
        //�κ��丮 ������ ������ �� �����Ϻ� �ߵ���

        for (int i=0; i<_inventoryData.ItemList.Count; i++)
        {
            ItemData itemData = _inventoryData.ItemList[i];
            GameObject item = CreateItemCell(itemData);
            item.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                SetEnableAllItemCells(false);
                //item.GetComponent<UnityEngine.UI.Button>().interactable = false;
                GameObject detailView = ShowDetailView(itemData);
                _lastSelectedItem = item;
                _focusedView = detailView;
                
            });
            
            
        }
        _loadDone = true;

    }
    //�κ��丮�� ������ ä���
    public void Initialize(Player player, InventoryData inventoryData)
    {
        _owner = player;
        _inventoryData = inventoryData;
        LoadInventoryItems();


        if (ScrollViewRect.content.childCount > 0)
        {
            EventSystem.current.SetSelectedGameObject(ScrollViewRect.content.transform.GetChild(0).GetChild(0).gameObject);
            _lastSelectedItem = EventSystem.current.currentSelectedGameObject;
        }

    }
    

    private void Awake()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OnPressEsc();
        ScrollbarSyncronize();
        ProtectFocusOut();
    }
}
