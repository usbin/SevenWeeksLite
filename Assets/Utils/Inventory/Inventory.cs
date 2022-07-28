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
    private GameObject _lastSelectedItem; //마지막으로 클릭한 항목
    private GameObject _focusedView;

    private void ProtectFocusOut()
    {
        if (_focusedView == null
            && EventSystem.current != null && EventSystem.current.currentSelectedGameObject == null)
        {
            SetEnableAllItemCells(true);
            SetInteractiveAllItemCells(true);
            //열려있던 디테일뷰가 닫혀서 포커스를 잃은 경우
            if (_lastSelectedItem != null)
            {
                EventSystem.current.SetSelectedGameObject(_lastSelectedItem);
            }
            //빈 곳을 클릭했거나 포커스된 아이템을 전부 사용해서 포커스를 잃은 경우
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
        //**************************** 주의사항 **************************
        //*** Content 직속 child의 pivot은 반드시 y가 1일 것           ***
        //*** childRect는 반드시 Content 직속 child의 Rect로 설정할 것 ***
        //****************************************************************
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
        ScrollRect scrollRect = ScrollViewRect;
        GameObject content = scrollRect.content.gameObject;
        //선택된 게 버튼일 때+위아래 중 버튼 하나라도 눌렸을 때
        if ((selectedObject != null && selectedObject.transform.IsChildOf(content.transform))
            && (Control.IsPressed(Control.KeyList.Up) || Control.IsPressed(Control.KeyList.Down)))
        {
            //selectedRect = Content 직속 child로 있는 오브젝트의 RectTransform으로 계산
            RectTransform childRect = selectedObject.transform.parent.GetComponent<RectTransform>();
            float heightUnit = Math.Abs(childRect.rect.height); //한 개 높이
            float wholeHeight = content.GetComponent<RectTransform>().rect.height; //전체 높이
            float viewHeight = scrollRect.viewport.rect.height; //한 화면의 높이(한 번에 노출되는 화면의 높이)
            float countPerWindow = (int)viewHeight / (int)heightUnit;//한 화면에 보이는 개수
            float currentY = childRect.anchoredPosition.y;
            //내리고 있을 때, 맨 위 영역이 아니면서 스크롤이 가용범위보다 위일 때(클 때)
            if (currentY <= -(heightUnit * (countPerWindow))
                && Control.IsPressed(Control.KeyList.Down)
                && scrollRect.verticalScrollbar.value > (1 + (currentY + heightUnit * (countPerWindow - 1)) / (wholeHeight - viewHeight)))
            {
                scrollRect.verticalScrollbar.value = (1 + (currentY + heightUnit * (countPerWindow - 1)) / (wholeHeight - viewHeight));
            }
            //올리고 있을 때, 맨 아래 영역이 아니면서 스크롤이 가용범위보다 아래일 때(작을 때)
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
            //스프라이트가 없을 경우
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
        item.transform.GetChild(0).GetComponent<Text>().text = itemData.Amount.ToString(); //Amount 텍스트
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
                //인벤토리에 있을 경우 amount만 새로고침함.
                GameObject item = ScrollViewRect.GetComponent<ScrollRect>().content.GetChild(index).GetChild(0).gameObject;
                item.transform.GetChild(0).GetComponent<Text>().text = itemData.Amount.ToString(); //Amount 텍스트
            }
            else
            {
                //인벤토리에 없을 경우 추가함.
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
        //인벤토리 칸에 빈 쉘 채우고
        //쉘 안에 인벤토리 아이템 넣고
        //인벤토리 아이템 눌렀을 때 디테일뷰 뜨도록

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
    //인벤토리에 아이템 채우기
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
