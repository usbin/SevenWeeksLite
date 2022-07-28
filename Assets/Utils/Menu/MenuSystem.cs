using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSystem : MonoBehaviour
{
    public static MenuSystem Instance;

    public GameObject MenuCanvasPrefab;
    public GameObject MenuPrefab;

    private void CheckEventSystem()
    {
        //다이얼로그는 해당 씬에 EventSystem이 있어야 동작함.
        if (FindObjectOfType<EventSystem>() == null)
        {
            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }
    }
    public void OnCloseMenu()
    {
        MenuCanvas menuCanvas = FindObjectOfType<MenuCanvas>();
        if (menuCanvas != null)
        {
            Destroy(menuCanvas.gameObject);
            GameFlags.PlayerUnfreeze();
        }
    }
    public void OpenMenu(Player player)
    {

        CheckEventSystem();
        GameFlags.PlayerFreeze();
        
        MenuCanvas menuCanvas = FindObjectOfType<MenuCanvas>();
        if(menuCanvas == null)
        {
            menuCanvas = Instantiate(MenuCanvasPrefab).GetComponent<MenuCanvas>();
            Menu menu = Instantiate(MenuPrefab).GetComponent<Menu>();
            menu.Initialize(player);
            menu.transform.SetParent(menuCanvas.transform.GetChild(0), false);
        }

    }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
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
