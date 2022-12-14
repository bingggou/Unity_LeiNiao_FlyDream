using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MutiButtonAimOne : MonoBehaviour
{

    public Transform aimImage;
    public Transform aimImageParent;
    private Button[] allButtons;
    // Start is called before the first frame update
 public   void Start()
    {

        allButtons = transform.GetComponentsInChildren<Button>();
        Debug.Log(allButtons.Length);
        foreach (Button b in allButtons)
        {
            b.onClick.AddListener(() =>
            {
                if (aimImageParent == null)
                {
                    aimImage.parent = b.transform.parent;
                    aimImage.position = b.transform.position;
                }
                else
                {
                    aimImage.parent = aimImageParent;
                    aimImage.position = b.transform.position;
                }
            });
        }
    }

    public void SetAimPosByIndex(int index)
    {
        if (aimImageParent == null)
        {
            aimImage.parent = allButtons[index].transform.parent;
            aimImage.position = allButtons[index].transform.position;
        }
        else
        {
            aimImage.parent = aimImageParent;
            aimImage.position = allButtons[index].transform.position;
        }
    }
}
