using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{

    private void Awake()
    {
        
    }
    void Start()
    {
        if (ItemUsageSystem.Instance)
        {
            ItemUsageSystem.Instance.AddListener(1, (Player player) =>
            {
                Debug.Log("아이템 사용함!");
            });
        }
        if (DialogSystem.Instance)
        {
            DialogSystem.Instance.StartDialog("Test", () => {
                Debug.Log("asdf");

            });
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
