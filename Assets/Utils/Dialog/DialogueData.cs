using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueData
{
    public int code = -1;
    public string speakerName = "";
    public string text = "";
    public int next=0;

    public int delayOffset = 0; //글자 사이사이 줄 추가 딜레이(기본 단위에 +연산됨)
    
    public string eventName; //트리거할 이벤트 이름
    public string speakerCode = null;
    public GameObject speakerImage = null;
    public string statusLabel = "기본";
    [SerializeField]
    public DialogueConditionData[] conditions;

}
