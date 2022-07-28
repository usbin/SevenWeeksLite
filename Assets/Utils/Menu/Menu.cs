using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    public GameObject InventoryPrefab;
    private GameObject _lastSelectedTab;
    private GameObject _focusedWindow;
    private Player _owner;

    public void OnClickExit()
    {
        if (MenuSystem.Instance != null)
        {
            MenuSystem.Instance.OnCloseMenu();
        }
    }
    private void OnOpenInventory(Inventory inventory)
    {
        _focusedWindow = inventory.gameObject;
    }
    private void OpenInventory()
    {
        Player player = FindObjectOfType<Player>();
        if(player != null)
        {
            Inventory inventory = Instantiate(InventoryPrefab).GetComponent<Inventory>();
            inventory.transform.SetParent(transform.GetChild(1), false);
            inventory.Initialize(_owner, _owner.InventoryData);
            OnOpenInventory(inventory);
        }
    }
    public void OnClickInventoryTab()
    {
        //메뉴탭 비활성화하기
        SetEnableAllTabs(false);
        //인벤토리 탭 선택한 상태로 변경
        Button inventoryButton = transform.GetChild(0).GetChild(0).GetComponent<Button>();
        OnTabSelected(inventoryButton);
        //Area에 인벤토리 열기
        OpenInventory();

    }
    private void OnTabSelected(Button tab)
    {
        tab.interactable = false;
        _lastSelectedTab = tab.gameObject;
    }
    private void SetInteractiveAllTabs(bool b)
    {
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            GameObject tab = transform.GetChild(0).GetChild(i).gameObject;
            tab.GetComponent<UnityEngine.UI.Button>().interactable = b;
        }
    }
    private void SetEnableAllTabs(bool b)
    {
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            GameObject tab = transform.GetChild(0).GetChild(i).gameObject;
            tab.GetComponent<UnityEngine.UI.Button>().enabled = b;
        }
    }
    private void PreventFocusOut()
    {
        //열려있던 창이 닫혀서 포커스를 잃은 상태일 때
        //or
        //빈 곳을 클릭해서 포커스를 잃은 상태일 때
        if (_focusedWindow == null && EventSystem.current.currentSelectedGameObject == null)
        {
            //메뉴탭 활성화하기
            SetEnableAllTabs(true);
            SetInteractiveAllTabs(true);

            if (_lastSelectedTab != null)
                EventSystem.current.SetSelectedGameObject(_lastSelectedTab);
            else
            {
                EventSystem.current.SetSelectedGameObject(transform.GetChild(0).GetChild(0).gameObject);
            }
        }
    }
    private void ListenKeyInput()
    {
        if (Control.IsPressDown(Control.KeyList.Esc))
        {
            //아무 탭도 열려있지 않을 때
            if(_focusedWindow == null)
            {
                if (MenuSystem.Instance != null)
                {
                    MenuSystem.Instance.OnCloseMenu();
                }
            }
        }
    }
    public void Initialize(Player player)
    {
        _owner = player;
    }
    private void Awake()
    {
        //첫번째 버튼에 포커스 주기
        GameObject firstTab = transform.GetChild(0).GetChild(0).gameObject;//transform->MenuTab->첫번째탭
        EventSystem.current.SetSelectedGameObject(firstTab);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        ListenKeyInput();
        PreventFocusOut();
    }
}
