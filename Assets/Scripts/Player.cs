using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float walkSpeed = 5f;
    public Vector3 velocity;
    private PlayerData _playerData;
    public InventoryData InventoryData
    {
        get => _playerData.InventoryData;
    }

    public void OnClickButton()
    {
        ItemInfo ItemInfo = new ItemInfo();
        ItemInfo.Code = 1;
        ItemInfo.Name = "abc";
        ItemData item = new ItemData(ItemInfo, 1);
        item.Use(this);


    }
    public void OnPressMenuKey()
    {
        if (Control.IsPressDown(Control.KeyList.Esc))
        {
            if (MenuSystem.Instance != null)
            {
                MenuSystem.Instance.OpenMenu(this);
            }
        }
    }
    void Move()
    {
        int vectorX = Control.IsPressed(Control.KeyList.Left) ? -1
                : Control.IsPressed(Control.KeyList.Right) ? 1
                    : 0;
        int vectorY = Control.IsPressed(Control.KeyList.Down) ? -1
                : Control.IsPressed(Control.KeyList.Up) ? 1
                    : 0;
        /*CombatSystem.Direction direction = vectorY == 1 ? CombatSystem.Direction.Back
                                                : vectorX == -1 ? CombatSystem.Direction.Left
                                                               : vectorX == 1 ? CombatSystem.Direction.Right
                                                                               : CombatSystem.Direction.Front;*/
        if (vectorX != 0 || vectorY != 0)
        {
            //SetPlayerSpriteDirection(direction);
            //�̵�
            Vector2 velocity = new Vector2(vectorX * walkSpeed - vectorX * Mathf.Abs(vectorY) * Mathf.Cos(Mathf.Deg2Rad * 45), vectorY * walkSpeed - vectorY * Mathf.Abs(vectorX) * Mathf.Cos(Mathf.Deg2Rad * 45));

            GetComponent<Rigidbody2D>().velocity = velocity;
            this.velocity = velocity;
            //_animator.SetBool("IsWalking", true);
            //_lastDirection = direction;

        }
        else
        {
            //_animator.SetBool("IsWalking", false);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        }
    }

    private void Awake()
    {
        //PlayerData �ҷ�����(�̸�, �κ��丮 ���� ��)
        _playerData = SaveFileManager.LoadPlayerData();
        if(_playerData != null)
        {
            Debug.Log("�÷��̾� ������ �ҷ���!");
        }

        DontDestroyOnLoad(this);

    }

    void Start()
    {

    }

    void Update()
    {
        if (!GameFlags.PlayerFreezed)
        {
            Move();
            OnPressMenuKey();
        }
    }
}
