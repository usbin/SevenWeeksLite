using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogSystem : MonoBehaviour
{
    //File로 읽기:Asset 한 단계 상위 폴더에서 시작

    public static readonly string DIALOG_JSON_BASE_PATH = "Assets/Utils/Dialog/Jsons/";

    public static DialogSystem Instance;
    public GameObject DialogCanvasPrefab;
    public GameObject BottomDialogPrefab;
    

    public enum DialogType {Bottom, Condition}; //Bottom: 화면 하단의 대화창, Condition: 화면 중앙에 뜨는 선택지

    private GameObject CreateBottomDialog(GameObject dialogCanvas)
    {
        GameObject bottomDialog = Instantiate(BottomDialogPrefab);
        bottomDialog.transform.SetParent(dialogCanvas.transform, false);
        return bottomDialog;
    }
    private GameObject CreateDialogCanvas()
    {
        GameObject dialogCanvas = GameObject.FindGameObjectWithTag("DialogCanvas");
        if (dialogCanvas == null)
        {
            if (DialogCanvasPrefab) dialogCanvas = Instantiate(DialogCanvasPrefab);
            else
            {
                Debug.Log("BottomDialogPrefab이 null입니다.");
            }
        }
        return dialogCanvas;
    }
    private void CheckEventSystem()
    {
        //다이얼로그는 해당 씬에 EventSystem이 있어야 동작함.
        if (FindObjectOfType<EventSystem>() == null)
        {
            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }
    }

    public void StartDialog(string dialogFileName, Action callback)
    {
        //새 바텀 다이얼로그 생성
        GameObject dialogCanvas = CreateDialogCanvas();
        if (dialogCanvas)
        {
            CheckEventSystem();
            GameObject bottomDialog = CreateBottomDialog(dialogCanvas);

            //Json 파일에서 {dialogueFileCode}.json에 해당하는 대화 파일을 읽어옴.
            string jstring = File.ReadAllText(DIALOG_JSON_BASE_PATH + dialogFileName.ToString() + ".json", System.Text.Encoding.UTF8);
            DialogueData[] dialogPack = JsonUtility.FromJson<Serialization<DialogueData>>(jstring).list;


            StartCoroutine(bottomDialog.GetComponent<BottomDialog>().StartDialogue(dialogPack, callback));
        }

    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            Destroy(this);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
