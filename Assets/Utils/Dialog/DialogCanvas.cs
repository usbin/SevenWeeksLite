using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogCanvas : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        for(int i=0; i<transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    IEnumerator RightToLeftAnim(GameObject canvas, int start, int end, int time)
    {
        int delay = 10;
        for(int i=0; i<= (time / delay); i++)
        {
            canvas.GetComponent<RectTransform>().anchoredPosition = new Vector2(start+((end - start) / (time / delay)) * i, canvas.transform.localPosition.y);
            yield return new WaitForSeconds(delay / 1000f);
        }
    }

}
