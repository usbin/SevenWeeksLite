using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConsumableItemDetailView : MonoBehaviour
{
    private ItemData _itemData;
    public Button UseButton;
    private GameObject _lastSelected;
    public void Exit()
    {
        Destroy(gameObject);
    }
    private void ListenKeyInput()
    {
        if (Control.IsPressDown(Control.KeyList.Esc))
        {
            Exit();
        }
    }
    private void PreventFocusOut()
    {
        if(EventSystem.current.currentSelectedGameObject == null
            &&_lastSelected != null)
        {
            EventSystem.current.SetSelectedGameObject(_lastSelected);
        }
    }
    public void Initialize(Player player, ItemData itemData, Action callback)
    {
        if (ItemTable.GetItemInfo(itemData.Code).HasValue)
        {
            _itemData = itemData;
            transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = ItemTable.GetItemInfo(itemData.Code).Value.Description;
            UseButton.onClick.AddListener(() =>
            {
                callback();
                Destroy(gameObject);
            });
        }
        else
        {
            Debug.Log("존재하지 않는 아이템입니다.");
        }
       
    }
    private void Awake()
    {
        if(EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(UseButton.gameObject);
            _lastSelected = UseButton.gameObject;
        }
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
