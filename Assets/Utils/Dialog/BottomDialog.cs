using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BottomDialog : MonoBehaviour
{

    private bool _isVisible;
    public bool IsVisible
    {
        get
        {
            return _isVisible;
        }
        set
        {
            transform.parent.GetComponent<CanvasGroup>().alpha = value ? 1.0f : 0.0f;
            _isVisible = value;
        }
    }
    private string _name;
    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            UnityEngine.UI.Text text = gameObject.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>();
            text.text = value;
            _name = value;
        }
    }
    private string _text;
    public string Text
    {
        get
        {
            return _text;
        }
        set
        {
            UnityEngine.UI.Text text = gameObject.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>();
            text.text = value;
            _text = value;
        }
    }
    private Vector2 _size;
    public Vector2 Size
    {
        get
        {
            return _size;
        }
        set
        {
            GetComponent<RectTransform>().sizeDelta = value;
            _size = value;
        }
    }

    public GameObject conditionDialogPrefab;
    public GameObject conditionDialogBtnPrefab;
    private DialogueData[] _dialogPack;
    private IEnumerator dialogTask;

    private bool _skipDialogue = false;
    public void StartDialogue(DialogueData[] dialoguePack)
    {
        this._dialogPack = dialoguePack;
        IEnumerator task = StartDialogueAsync(dialoguePack);
        StartCoroutine(task);
        
    }
    
    IEnumerator WaitInteract()
    {
        do
        {
            yield return null;
        } while (!Control.IsPressDown(Control.KeyList.Interact));
        
    }
    IEnumerator WaitInput()
    {
        yield return null;
        bool up = false;
        bool down = false;
        bool interact = false;
        do
        {
            up = Control.IsPressDown(Control.KeyList.Up);
            down = Control.IsPressDown(Control.KeyList.Down);
            interact = Control.IsPressDown(Control.KeyList.Interact);
            yield return null;
        }
        while (!up && !down && !interact);

        if (up) yield return Control.KeyList.Up;
        else if (down) yield return Control.KeyList.Down;
        else yield return Control.KeyList.Interact;
    }
    IEnumerator WaitSkipDialogue()
    {
        while (true)
        {
            _skipDialogue = Control.IsPressDown(Control.KeyList.Interact);
            yield return null;
        }

    }
    IEnumerator StartDialogueAsync(DialogueData[] dialoguePack)
    {
        int cursor = 0;

        while (cursor != -1)
        {
            if(dialoguePack[cursor].conditions == null)
            {
                //[1] �Ϲ� �ؽ�Ʈ�� ���

                // - cursor�� �ش��ϴ� ��ȭ ����(�̸�, �ؽ�Ʈ) ä���
                transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = dialoguePack[cursor].speakerName;
                yield return StartCoroutine(TypeDialog(transform.GetChild(1).GetComponent<UnityEngine.UI.Text>(), dialoguePack[cursor].text, 0.03f+dialoguePack[cursor].delayOffset*0.001f));

                // - ���� ��ȭ�� ���� �� �����Ÿ� ����
                GameObject nextArrow = transform.GetChild(0).GetChild(2).gameObject;
                if (dialoguePack[cursor].next != -1)
                {
                    
                }

                // - �Է� ���
                yield return StartCoroutine(WaitInteract());
                // - �̺�Ʈ ����(�Է´�� ��)
                if (dialoguePack[cursor].eventName != null)
                {
                    GameEvent __event = Resources.Load<GameEvent>(GameEvent.EVENT_BASE_PATH + dialoguePack[cursor].eventName);
                    __event.Raise();
                    Debug.Log("�̺�Ʈ ����:"+__event);
                }
                // - Ŀ�� �̵�
                if (dialoguePack[cursor].next == 0) cursor++;
                else if (dialoguePack[cursor].next == -1) cursor = -1;
                else
                {
                    cursor = Array.FindIndex(dialoguePack, value => value.code == dialoguePack[cursor].next);
                    if (cursor == -1) Debug.LogError("�������� �ʴ� ��ȭ ����!");
                }
                Destroy(nextArrow);
            }
            else
            {
                //[2] �������� ���
                // - cursor�� text,name �� �ش��ϴ� ���� ä���
                transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = dialoguePack[cursor].speakerName;
                yield return StartCoroutine(TypeDialog(transform.GetChild(1).GetComponent<UnityEngine.UI.Text>(), dialoguePack[cursor].text, 0.03f + dialoguePack[cursor].delayOffset * 0.001f));
                // - ConditionDialog ����, cursor.conditions�� �ش��ϴ� ���� ä���
                
                GameObject conditionDialog = GameObject.Instantiate(conditionDialogPrefab);
                conditionDialog.transform.SetParent(transform.parent, false);
                for (int i= 0; i < dialoguePack[cursor].conditions.Length; i++)
                {
                    int iIndex = i;
                    GameObject conditionDialogBtn = GameObject.Instantiate(conditionDialogBtnPrefab);
                    conditionDialogBtn.transform.GetChild(0).GetComponent<Text>().text = dialoguePack[cursor].conditions[iIndex].text;
                    conditionDialogBtn.transform.SetParent(conditionDialog.transform, false);
                    conditionDialogBtn.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        // - �Է¿� �´� �̺�Ʈ ����
                        if (dialoguePack[cursor].conditions[iIndex].eventName != null)
                        {
                            GameEvent __event = Resources.Load<GameEvent>(GameEvent.EVENT_BASE_PATH + dialoguePack[cursor].conditions[iIndex].eventName);
                            __event.Raise();
                            Debug.Log("�̺�Ʈ ����:", __event);
                        }

                        // - �Է¿� �´� Ŀ�� �̵�
                        if (dialoguePack[cursor].conditions[iIndex].next == 0) cursor++;
                        else if (dialoguePack[cursor].conditions[iIndex].next == -1) cursor = -1;
                        else
                        {
                            cursor = Array.FindIndex(dialoguePack, value => value.code == dialoguePack[cursor].conditions[iIndex].next);
                            if (cursor == -1) Debug.LogError("�������� �ʴ� ��ȭ ����!");
                        }
                    });
                }
                // - 0�� �ε����� Ŀ�� �α�
                int conditionCursor = 0;
                Button cursoredBtn = conditionDialog.transform.GetChild(conditionCursor).GetComponent<Button>();
                
                // - ��ư Ŀ�� ��Ŀ��
                cursoredBtn.Select();

                // - �Է� ���
                IEnumerator waitConditionSelection = WaitInput();
                yield return StartCoroutine(waitConditionSelection);
                //��ȣ�ۿ� Ű�� ������ ������ Ŀ�� �̵���.
                while ((Control.KeyList)waitConditionSelection.Current != Control.KeyList.Interact)
                {
                    if ((Control.KeyList)waitConditionSelection.Current == Control.KeyList.Up)
                    {
                        conditionCursor = conditionCursor > 0 ? conditionCursor - 1 : 0;
                        
                    }
                    if ((Control.KeyList)waitConditionSelection.Current == Control.KeyList.Down)
                    {
                        conditionCursor =
                            conditionCursor < dialoguePack[cursor].conditions.Length - 1
                            ? conditionCursor + 1
                            : dialoguePack[cursor].conditions.Length - 1;

                    }
                    cursoredBtn = conditionDialog.transform.GetChild(conditionCursor).GetComponent<Button>();

                    // - ��ư Ŀ�� ��Ŀ��
                    cursoredBtn.Select();
                    Debug.Log("Ŀ��: " + conditionCursor);
                    waitConditionSelection = WaitInput(); //�Է� �ڷ�ƾ �ʱ�ȭ
                    yield return StartCoroutine(waitConditionSelection);
                }
                
                
                
                Destroy(conditionDialog.gameObject);
            }
            

        }
        Destroy(gameObject);
        Destroy(transform.parent.gameObject);
    }
    IEnumerator TypeDialog(Text textObject, string text, float delay)
    {
        textObject.text = "";
        // Ư�� �ʸ��� �� ���ھ� �Է�
        for(int i=0; i< text.Length; i++)
        {
            textObject.text = textObject.text+text[i].ToString();
            yield return StartCoroutine(WaitForSecondsUntilSkip(delay));
            if (_skipDialogue)
            {
                textObject.text = text;
                break;
            }

        }
        yield return new WaitForSeconds(0.05f); //��Ÿ�ϴ� �Ѿ�� �� ����
    }
    IEnumerator WaitForSecondsUntilSkip(float seconds)
    {
        //������ �ð� �Ǵ� �� �ȿ� ��ȣ�ۿ� Ű�� ������ _skipDialogue�� true�� ����� �����.
        yield return null;
        _skipDialogue = false;
        long start = (long)(DateTime.Now.ToLocalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime()).TotalMilliseconds;
        long now = (long)(DateTime.Now.ToLocalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime()).TotalMilliseconds;
        while ((now-start < seconds * 1000) && !Control.IsPressDown(Control.KeyList.Interact))
        {
            now =(long)(DateTime.Now.ToLocalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime()).TotalMilliseconds;
            yield return null;
        }
        _skipDialogue = Control.IsPressDown(Control.KeyList.Interact);
    }

    void OnClickDialog()
    {

    }
    void Start()
    {
        //
    }

    // Update is called once per frame
    void Update()
    {

    }


}
