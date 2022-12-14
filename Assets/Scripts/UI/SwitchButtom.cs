using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SwitchButtom : MonoBehaviour
{
      public UnityEvent to0Event;
      public UnityEvent to1Event;
      private GameObject[] twoAppearances=new GameObject[2];
      private Button buttom;
    // Start is called before the first frame update
    void Start()
    {
            twoAppearances[0] = transform.GetChild(0).gameObject;
            twoAppearances[1] = transform.GetChild(1).gameObject;
            buttom = transform.GetChild(2).GetComponent<Button>();
            buttom.onClick.AddListener(OnButtomClick);
      }

    // Update is called once per frame
    void OnButtomClick()
    {
            if (twoAppearances[0].activeSelf == true)
            {
                  twoAppearances[0].SetActive(false) ;
                  twoAppearances[1].SetActive(true);
                  to1Event.Invoke();
            }
            else
            {
                  twoAppearances[0].SetActive(true);
                  twoAppearances[1].SetActive(false);
                  to0Event.Invoke();
            }
    }
}
