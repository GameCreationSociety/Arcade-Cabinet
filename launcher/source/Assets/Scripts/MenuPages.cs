using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuPages : MonoBehaviour
{
    RectTransform rt;

    float SCREEN_X_BOUND = 9;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null
            || EventSystem.current.currentSelectedGameObject.transform.parent.parent.parent != rt)
            return;
        else
            PageButton.instance.SetCurrentPages(this);

        if (EventSystem.current.currentSelectedGameObject.transform.position.x > SCREEN_X_BOUND)
            Page(true);
        else if (EventSystem.current.currentSelectedGameObject.transform.position.x < -SCREEN_X_BOUND)
            Page(false);

    }

    public void Page(bool right)
    {
        if (right)
        {
            if (GetPage() < transform.childCount)
                rt.anchoredPosition -= Vector2.right * 750f;
        }
        else
        {
            if (GetPage() > 1)
                rt.anchoredPosition += Vector2.right * 750f;
        }

        PageButton.instance.UpdateLook();
    }

    public int GetPage()
    {
        return (int)(rt.anchoredPosition.x / -750) + 1;
    }
}
