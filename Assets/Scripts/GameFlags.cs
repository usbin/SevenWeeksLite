using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class GameFlags
{
    //플레이어 프리징 관리 변수 및 이벤트들
    //플레이어 프리징: 유저가 플레이어를 움직일 수 없게됨.
    private static int _playerFreeze=0;
    private static UnityEvent _onPlayerFreezed = new UnityEvent();
    private static UnityEvent _onPlayerUnfreezed = new UnityEvent();

    public static bool PlayerFreezed
    {
        get => _playerFreeze > 0;
    }
    public static void AddPlayerFreezeListener(UnityAction listener)
    {
        _onPlayerFreezed.AddListener(listener);
    }
    public static void AddPlayerUnfreezeListener(UnityAction listener)
    {
        _onPlayerUnfreezed.AddListener(listener);
    }
    public static void PlayerFreeze()
    {
        _playerFreeze++;
        if(_playerFreeze == 1)
        {
            _onPlayerFreezed.Invoke();
        }
    }
    public static void PlayerUnfreeze()
    {
        _playerFreeze--;
        if(_playerFreeze == 0)
        {
            _onPlayerUnfreezed.Invoke();
        }
    }

}
