using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemInfo
{
    public int Code;
    public string Name;
    public string Description;
    public ItemUsageEvent UsageEvent; //로드 시 초기화

}
