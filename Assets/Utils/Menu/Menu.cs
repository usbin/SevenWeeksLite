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
        //�޴��� ��Ȱ��ȭ�ϱ�
        SetEnableAllTabs(false);
        //�κ��丮 �� ������ ���·� ����
        Button inventoryButton = transform.GetChild(0).GetChild(0).GetComponent<Button>();
        OnTabSelected(inventoryButton);
        //Area�� �κ��丮 ����
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
        //�����ִ� â�� ������ ��Ŀ���� ���� ������ ��
        //or
        //�� ���� Ŭ���ؼ� ��Ŀ���� ���� ������ ��
        if (_focusedWindow == null && EventSystem.current.currentSelectedGameObject == null)
        {
            //�޴��� Ȱ��ȭ�ϱ�
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
            //�ƹ� �ǵ� �������� ���� ��
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
        //ù��° ��ư�� ��Ŀ�� �ֱ�
        GameObject firstTab = transform.GetChild(0).GetChild(0).gameObject;//transform->MenuTab->ù��°��
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
