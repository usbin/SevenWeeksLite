using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Control
{
    public enum KeyList
    {
        Interact = KeyCode.Space,
        Up = KeyCode.UpArrow,
        Right = KeyCode.RightArrow,
        Down = KeyCode.DownArrow,
        Left = KeyCode.LeftArrow,
        Esc = KeyCode.Escape,
        Inventory = KeyCode.Tab
    }
    public static Dictionary<KeyList, KeyCode> KeyBinds = new Dictionary<KeyList, KeyCode>()
    {
        { KeyList.Interact, KeyCode.Space },
        { KeyList.Up, KeyCode.UpArrow },
        { KeyList.Right, KeyCode.RightArrow },
        { KeyList.Down, KeyCode.DownArrow },
        { KeyList.Left, KeyCode.LeftArrow },
        { KeyList.Esc, KeyCode.Escape },
        { KeyList.Inventory, KeyCode.Tab }
    };
    public static bool IsPressed(KeyList key)
    {
        return Input.GetKey(KeyBinds[key]);
    }
    public static bool IsPressDown(KeyList key)
    {
        return Input.GetKeyDown(KeyBinds[key]);
    }
    public static bool IsPressUp(KeyList key)
    {
        return Input.GetKeyUp(KeyBinds[key]);
    }

}
