using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PageButton : MonoBehaviour
{
    public static PageButton instance;

    public TMP_Text text;

    MenuPages currentPages;

    private void Awake()
    {
        instance = this;
    }

    public void SetCurrentPages(MenuPages p)
    {
        currentPages = p;
        UpdateLook();
    }

    public MenuPages GetCurrentPages()
    {
        return currentPages;
    }

    public void UpdateLook()
    {
        text.text = "Page " + currentPages.GetPage() + " of " + currentPages.transform.childCount;
    }
}
