using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameFlags.PlayerFreeze();
        DialogSystem.Instance.StartDialog("Test", () => {
            GameFlags.PlayerUnfreeze();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
