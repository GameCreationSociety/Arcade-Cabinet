using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public enum SelectedRange { None = 0, Top = 1, Games = 2, Page = 3 };

    public static Manager instance;

    public MenuPages[] menus;
    public GameObject[] menuButtons;
    public int menu;
    public SelectedRange selectedRange;

    public GameObject[] pageNav;

    [HideInInspector] public int inputDelay = 30;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // set default parameters
        Cursor.visible = false;
        Application.wantsToQuit += WantsToQuit;

        menu = 0;
        selectedRange = SelectedRange.Top;

        OpenMenu(0);
    }

    private void Update()
    {
        if (inputDelay > 0)
            inputDelay--;

        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(menuButtons[0]);
        }

        // control redirection
        if (EventSystem.current.currentSelectedGameObject.transform.parent == PageButton.instance.transform)
        {
            if (selectedRange == SelectedRange.Top)
            {
                try
                {
                    // should select game first over the page button
                    EventSystem.current.SetSelectedGameObject(
                        menus[menu].transform.GetChild(menus[menu].GetPage() - 1).GetChild(0
                        ).GetComponentInChildren<Button>().gameObject);
                    selectedRange = SelectedRange.Games;

                    return;
                }
                catch (System.Exception)
                {
                    // no games page to select
                }
            }

            selectedRange = SelectedRange.Page;

            if (EventSystem.current.currentSelectedGameObject == pageNav[1])
            {
                PageButton.instance.GetCurrentPages().Page(true);
                EventSystem.current.SetSelectedGameObject(PageButton.instance.transform.GetChild(0).gameObject);
            }
            else if (EventSystem.current.currentSelectedGameObject == pageNav[0])
            {
                PageButton.instance.GetCurrentPages().Page(false);
                EventSystem.current.SetSelectedGameObject(PageButton.instance.transform.GetChild(0).gameObject);
            }
        }
        else if (menuButtons.Contains(EventSystem.current.currentSelectedGameObject))
        {
            // handled by openmenu
        }
        else
        {
            selectedRange = SelectedRange.Games;
        }
    }

    public void OpenMenu(int index)
    {
        // this needs to be a coreq
        StartCoroutine(OpenMenuC(index));
    }

    IEnumerator OpenMenuC(int index)
    {
        yield return null;

        if (selectedRange == SelectedRange.Games)
        {
            EventSystem.current.SetSelectedGameObject(menuButtons[menu]);
            selectedRange = SelectedRange.Top;
            yield break;
        }
        else if (selectedRange == SelectedRange.Page)
        {
            try
            {
                // should select game first over the page button
                EventSystem.current.SetSelectedGameObject(
                        menus[menu].transform.GetChild(menus[menu].GetPage() - 1).GetChild(0
                        ).GetComponentInChildren<Button>().gameObject);
                yield break;
            }
            catch
            {
                selectedRange = SelectedRange.Top;
            }
        }

        menu = index;

        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].transform.parent.gameObject.SetActive(i == menu);
        }

        PageButton.instance.SetCurrentPages(menus[index].GetComponentInChildren<MenuPages>());
    }

    public void OpenInfoScreen(GameButton game)
    {
        GameInfoScreen.selectedGame = game;
        GameInfoScreen.instance.OpenInfoScreen(true);
    }

    // disable Alt + F4
    // close using Escape + Alt + F4 or 5 + Quit
    static bool WantsToQuit()
    {
        return Input.GetButton("Cancel");
    }
}
