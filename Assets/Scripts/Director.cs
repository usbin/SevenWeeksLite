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
            ItemUsageSystem.Instance.AddItemUsageListener(1, (Player player) =>
            {
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
