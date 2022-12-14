using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetAllSelectableSound : MonoBehaviour
{
    public string enterSoundName;
    public string downSoundName1;
    public string downSoundName2;
   

    // Start is called before the first frame update
    void Start()
    {
        Selectable[] allSelectable = transform.GetComponentsInChildren<Selectable>(true);
        foreach (var item in allSelectable)
        {
            SetSoundEvent(item);
        }

        //Slider[] allSlider = transform.GetComponentsInChildren<Slider>(true);
        //foreach (var item in allSlider)
        //{
          

        //}
        
    }



    private void SetSoundEvent(Selectable item)
    {
        EventTrigger et = item.GetComponent<EventTrigger>();
        if (et == null)
        {
            et = item.gameObject.AddComponent<EventTrigger>();
        }


        SetEvent(et, () => {

            MusicManager.instance.ChangeAndPlaySound(enterSoundName);

        }, EventTriggerType.PointerEnter);
        SetEvent(et, () => {
            if (item.interactable == true)
            {

                MusicManager.instance.ChangeAndPlaySound(downSoundName1);
            }
            else
            {
                MusicManager.instance.ChangeAndPlaySound(downSoundName2);
            }

        }, EventTriggerType.PointerDown);
    }

    private delegate void MouseFun();
    void SetEvent(EventTrigger et, MouseFun mouseFun, EventTriggerType ett)
    {
        EventTrigger.Entry ete = new EventTrigger.Entry();
        ete.eventID = ett;
        ete.callback.AddListener((baseEventData) =>
        {
            mouseFun();
        });
        et.triggers.Add(ete);
    }

   
}
