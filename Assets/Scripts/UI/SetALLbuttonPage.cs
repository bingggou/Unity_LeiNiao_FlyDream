using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetALLbuttonPage : MonoBehaviour
{
    public ShowDifferentPage sdp;

    private Button[] buttons;
    // Start is called before the first frame update
    void Start()
    {
        buttons = transform.GetComponentsInChildren<Button>();
       
        for (int i=0;i<buttons.Length;i++)
        {
            int index = i + 1;
            buttons[i].onClick.AddListener(()=> {
                sdp.ChangePageTo(index);
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
