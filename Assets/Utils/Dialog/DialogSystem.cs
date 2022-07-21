using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    //File로 읽기:Asset 한 단계 상위 폴더에서 시작
    public static readonly string DIALOG_JSON_BASE_PATH = "Assets/Resources/Table/Dialogue/";



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

    public void StartDialog(string dialogFileName)
    {
        //새 바텀 다이얼로그 생성
        GameObject dialogCanvas = CreateDialogCanvas();
        if (dialogCanvas)
        {
            GameObject bottomDialog = CreateBottomDialog(dialogCanvas);

            //Json 파일에서 {dialogueFileCode}.json에 해당하는 대화 파일을 읽어옴.
            string jstring = File.ReadAllText(DIALOG_JSON_BASE_PATH + dialogFileName.ToString() + ".json", System.Text.Encoding.UTF8);
            DialogueData[] dialogPack = JsonUtility.FromJson<Serialization<DialogueData>>(jstring).list;


            bottomDialog.GetComponent<BottomDialog>().StartDialogue(dialogPack);
        }

    }

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
