using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    //File�� �б�:Asset �� �ܰ� ���� �������� ����
    public static readonly string DIALOG_JSON_BASE_PATH = "Assets/Resources/Table/Dialogue/";



    public static DialogSystem Instance;
    public GameObject DialogCanvasPrefab;
    public GameObject BottomDialogPrefab;
    

    public enum DialogType {Bottom, Condition}; //Bottom: ȭ�� �ϴ��� ��ȭâ, Condition: ȭ�� �߾ӿ� �ߴ� ������

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
                Debug.Log("BottomDialogPrefab�� null�Դϴ�.");
            }
        }
        return dialogCanvas;
    }

    public void StartDialog(string dialogFileName)
    {
        //�� ���� ���̾�α� ����
        GameObject dialogCanvas = CreateDialogCanvas();
        if (dialogCanvas)
        {
            GameObject bottomDialog = CreateBottomDialog(dialogCanvas);

            //Json ���Ͽ��� {dialogueFileCode}.json�� �ش��ϴ� ��ȭ ������ �о��.
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
