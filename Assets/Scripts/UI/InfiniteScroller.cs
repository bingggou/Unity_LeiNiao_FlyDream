using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InfiniteScroller : MonoBehaviour
{
    [Header("离中心点的最大距离,用于缩放调整，0为不缩放")]
    [Range(0, 1000)]
    public int maxDistenceFromMiddle = 7;
    public Vector3 normalScale = Vector3.one;

    private Button leftButton;
    private Button rightButton;

    MoveTransition moveT;
    RectTransform content;
    List<Transform> allTrans = new List<Transform>();

    List<GameObject> allScrollUiGameObj = new List<GameObject>();
    GridLayoutGroup glg;
    float xMaxValue;

    

    // Start is called before the first frame update
    void Awake()
    {

        leftButton = TransformHelper.GetChild(transform, nameof(leftButton)).GetComponent<Button>();
        rightButton = TransformHelper.GetChild(transform, nameof(rightButton)).GetComponent<Button>();
        leftButton.onClick.AddListener(PlayLeftAnimation);
        rightButton.onClick.AddListener(PlayRightAnimation);

        content = TransformHelper.GetChild(transform, nameof(content)) as RectTransform;

        for (int i = 0; i < content.childCount; i++)
        {
            allTrans.Add(content.GetChild(i));
        }
        int nowTransCount = allTrans.Count;
        if (nowTransCount < 7)
        {
            int aimNum = 7 - nowTransCount;
            for (int i = 0; i < aimNum; i++)
            {

                Transform temp = Instantiate(allTrans[i % nowTransCount].gameObject, content).transform;
                allTrans.Add(temp);
                allScrollUiGameObj.Add(temp.gameObject);
            }

        }
        if (allTrans.Count % 2 == 0)
        {
            Transform temp = Instantiate(allTrans[0].gameObject, content).transform;
            allTrans.Add(temp);
            allScrollUiGameObj.Add(temp.gameObject);
        }


        glg = content.GetComponent<GridLayoutGroup>();
        //content.sizeDelta = new Vector2(glg.cellSize.x * allTrans.Count, content.sizeDelta.y);

        moveT = content.gameObject.GetComponent<MoveTransition>();
        moveT.start_Vector3 = Vector3.zero;

        xMaxValue = glg.cellSize.x + glg.spacing.x;

        moveT.to_Vector3 = new Vector3(xMaxValue, 0, 0);
        moveT.out_Vector3 = new Vector3(-xMaxValue, 0, 0);
        moveT.toEvents.onTransitionEnd.AddListener(() =>
        {
            SetLastUi2First();
        });

        moveT.outEvents.onTransitionEnd.AddListener(() =>
        {
            SetFirstUi2Last();
        });

        allScrollUiGameObj.Add(content.gameObject);



        bool isUI = RectTransformUtility.RectangleContainsScreenPoint(content, Input.mousePosition);
        if (isUI) isMouseIn = true;


        EventTrigger eventTrigger = content.gameObject.AddComponent<EventTrigger>();
        SetEvent(eventTrigger, OnMouseIn, EventTriggerType.PointerEnter);
        SetEvent(eventTrigger, OnMouseOut, EventTriggerType.PointerExit);       
        moveT.Awake();
      

    }
   


    bool isMouseIn = false;
    void OnMouseIn()
    {
        //Debug.Log("进来了");
        isMouseIn = true;
    }
    void OnMouseOut()
    {
       // Debug.Log("走了");
        isMouseIn = false;
    }

    private void SetLastUi2First()
    {
        //下置顶
        Transform aimTrans = allTrans[allTrans.Count - 1];
        allTrans.RemoveAt(allTrans.Count - 1);
        aimTrans.SetAsFirstSibling();
        allTrans.Insert(0, aimTrans);
        content.transform.localPosition = Vector3.zero;
    }


    private void SetFirstUi2Last()
    {
        //上置尾
        Transform aimTrans = allTrans[0];
        allTrans.RemoveAt(0);
        aimTrans.SetAsLastSibling();
        allTrans.Add(aimTrans);
        content.transform.localPosition = Vector3.zero;
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



    Vector3 pointDownPos;
    bool isDraging = false;


    Coroutine contentBackToPosRoutine;

    void OnDragBegin()
    {
        if (moveT.isPlay == false)
        {
            if (contentBackToPosRoutine != null)
            {
                StopCoroutine(contentBackToPosRoutine);
            }
            isDraging = true;
            pointDownPos = Input.mousePosition;

            if (content.localPosition != Vector3.zero)
            {
                pointDownPos.x -= content.localPosition.x;
            }


        }
    }



    void OnDragOver()
    {
        if (isDraging == true)
        {
            isDraging = false;
            contentBackToPosRoutine = StartCoroutine(ContentBackToPosRoutine());
        }
    }


    IEnumerator ContentBackToPosRoutine()
    {
        float aimX;
        if (Mathf.Abs(content.localPosition.x) > xMaxValue / 2)
        {
            if (content.localPosition.x > 0)
            {
                aimX = xMaxValue;
            }
            else
            {
                aimX = -xMaxValue;
            }
        }
        else
        {
            aimX = 0;
        }
        while (Mathf.Abs(content.localPosition.x - aimX) > 0.1f)
        {
            content.localPosition = new Vector3(Mathf.LerpUnclamped(content.localPosition.x, aimX, 0.1f), 0, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        content.localPosition = Vector3.zero;
        if (aimX != 0)
        {
            if (aimX > 0)
            {
                SetLastUi2First();
            }
            else
            {
                SetFirstUi2Last();
            }
        }
        yield return null;
    }


    void ContentBackToPosImmediately()
    {
        float aimX;
        if (Mathf.Abs(content.localPosition.x) > xMaxValue / 2)
        {
            if (content.localPosition.x > 0)
            {
                aimX = xMaxValue;
            }
            else
            {
                aimX = -xMaxValue;
            }
        }
        else
        {
            aimX = 0;
        }
       

        content.localPosition = Vector3.zero;
        if (aimX != 0)
        {
            if (aimX > 0)
            {
                SetLastUi2First();
            }
            else
            {
                SetFirstUi2Last();
            }
        }
       
    }

    [ContextMenu("PlayRightAnimation")]
    void PlayRightAnimation()
    {
        // moveT.to_Vector3 = new Vector3(xMoveValue, 0, 0);
        if (moveT.isPlay == false && isDraging == false )
        {
            if (content.localPosition != Vector3.zero)
            {
                StopAllCoroutines();
                ContentBackToPosImmediately();
            }
            moveT.ManualRePlay();
        }
    }
    [ContextMenu("PlayLeftAnimation")]
    void PlayLeftAnimation()
    {
        //  moveT.out_Vector3 = new Vector3(-xMoveValue, 0, 0);
        if (moveT.isPlay == false && isDraging == false)
        {
            if (content.localPosition != Vector3.zero)
            {
                StopAllCoroutines();
                ContentBackToPosImmediately();
            }
            moveT.PlayOutTrans();
        }
    }
    // Update is called once per frame
    void Update()
    {
        ScaleFromMiddle(0.2f);
        CheckDragAction();
        CheckScrollAction();
        //  Debug.Log(content.GetChild(3).position);

    }
    float scrollValue = 0;
    float ScrollValue
    {
        set
        {
            if (scrollValue == 0 && value != 0)
            {
                if (value > 0)
                {
                    PlayRightAnimation();
                }
                else
                {
                    PlayLeftAnimation();
                }
            }
            scrollValue = value;
        }
        get { return scrollValue; }
    }

    void CheckDragAction()
    {

        if (isMouseIn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnDragBegin();
            }
            
        }

       


        if (isDraging == true)
        {

            if (Input.GetMouseButtonUp(0))
            {
                OnDragOver();
            }
            else
            {
                Vector3 mousePos = Input.mousePosition;
                Vector3 aimPos = new Vector3(mousePos.x - pointDownPos.x, 0, 0);

                float aimXvalue = aimPos.x;
                float aimXabsValue = Mathf.Abs(aimXvalue);
                if (aimXabsValue >= xMaxValue)
                {
                    aimXvalue = aimXvalue % xMaxValue;
                    //  Debug.Log(aimXvalue);
                    aimPos.x = aimXvalue;
                    if (aimXvalue > 0)
                    {
                        SetLastUi2First();
                    }
                    else
                    {
                        SetFirstUi2Last();
                    }
                    pointDownPos = mousePos;
                }
                content.localPosition = aimPos;
            }
           
        }

    }



    


    void CheckScrollAction()
    {
        //Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
        if (isMouseIn == true)
        {

            ScrollValue = Input.GetAxis("Mouse ScrollWheel");
        }
        else
        {
            ScrollValue = 0;
        }
    }


    void ScaleFromMiddle(float lerpValue)
    {

        if (maxDistenceFromMiddle > 0)
        {

            for (int i = 0; i < allTrans.Count; i++)
            {
                float percent = 1;
                float temDis = Vector3.Distance(allTrans[i].position, transform.position);
                if (temDis == 0)
                {
                    percent = 1;
                }
                else
                {
                    percent = 1 - temDis / maxDistenceFromMiddle;
                }
                percent = Mathf.Abs(percent);
                Vector3 aimScale = normalScale * percent;
                allTrans[i].localScale = Vector3.Lerp(allTrans[i].localScale, aimScale, lerpValue); 

            }
        }
        else
        {
            for (int i = 0; i < allTrans.Count; i++)
            {
                Vector3 aimScale = normalScale;
                allTrans[i].localScale = Vector3.Lerp(allTrans[i].localScale, aimScale, lerpValue);
            }
        }
    }
}


