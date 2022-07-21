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

    public int delayOffset = 0; //���� ���̻��� �� �߰� ������(�⺻ ������ +�����)
    
    public string eventName; //Ʈ������ �̺�Ʈ �̸�
    public string speakerCode = null;
    public GameObject speakerImage = null;
    public string statusLabel = "�⺻";
    [SerializeField]
    public DialogueConditionData[] conditions;

}
