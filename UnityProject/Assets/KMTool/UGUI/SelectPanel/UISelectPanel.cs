using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

public class UISelectPanel : MonoBehaviour {

    public Scrollbar scrollbar;
    public ScrollRect scrollRect;

    float targetValue;

    bool needMove = true;

    const float SMOOTH_TIME = 0.2f;

    float moveSpeed = 0f;

    public int Count = 5;

    int _value = 1;

    int Value { get { return _value; } set { _value = value; } }

    public GameObject leftButton;
    public GameObject rightButton;
    public GameObject Content;
    public GameObject Template;

    private void Start()
    {
        for (int i = 0; i < Count; ++i)
        {
            var g = Instantiate(Template, Content.transform);
            g.SetActive(true);
            g.GetComponent<Image>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
        }
    }


    public void OnPointerDown()
    {
        needMove = false;
    }

    public void OnPointerUp()
    {
        if(1 == Count)
        {
            targetValue = 0;
        }
        else
        {
            float spaceMid = .5f / (Count - 1);
            float space = 2 * spaceMid;
            for (int i = 1; i <= Count; ++i)
            {
                if (scrollbar.value <= space * i - spaceMid)
                {
                    targetValue = space * (i - 1);
                    Value = i;
                    break;
                }
            }
        }
        needMove = true;
        moveSpeed = 0f;
    }

    public void OnButtonClick(int value)
    {
        float space = 1f / (Count - 1);
        targetValue = space * (value - 1);
        needMove = true;
    }

    public void OnClickLeftButton()
    {
        Value -= 1;
        if (Value < 1)
            Value = 1;
        OnButtonClick(Value);
    }

    public void OnClickRightButton()
    {
        Value += 1;
        if (Value > Count)
            Value = Count;
        OnButtonClick(Value);
    }

    void Update()
    {
        if (Value > 1 && Value < Count)
        {
            leftButton.SetActive(true);
            rightButton.SetActive(true);
        }
        else
        {
            if (Value == 1)
            {
                leftButton.SetActive(false);
            }
            if (Count == Value)
            {
                rightButton.SetActive(false);
            }
        }

        if (needMove)
        {
            if (Mathf.Abs(scrollbar.value - targetValue) < 0.01f)
            {
                scrollbar.value = targetValue;
                needMove = false;
                return;
            }
            scrollbar.value = Mathf.SmoothDamp(scrollbar.value, targetValue, ref moveSpeed, SMOOTH_TIME);
        }
    }
}
