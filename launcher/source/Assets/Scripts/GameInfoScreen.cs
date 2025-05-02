using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameInfoScreen : MonoBehaviour
{
    public static GameInfoScreen instance;

    public string gamesPath;

    public GameObject loadingScreen;
    public TMP_Text gameName;
    public TMP_Text gameSubtitle;
    public TMP_Text header;
    public TMP_Text body;

    public Image controlsImage;
    public TMP_Text controlsBody;

    Animator animator;
    Button[] buttons;
    public Button[] gameInfoButtons;

    public GameObject previousSelection;
    public static GameButton selectedGame;

    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
    }

    public void OpenInfoScreen(bool open)
    {
        animator.SetBool("Open", open);

        if (open)
        {
            StartCoroutine(MenuOpened());

            gameName.text = selectedGame.gameName;
            gameSubtitle.text = selectedGame.playerCount + " | " + selectedGame.year + " | " + selectedGame.genre;
            OpenSubMenu(0);
        }
        else
            StartCoroutine(MenuClosed());
    }

    IEnumerator MenuOpened()
    {
        buttons = FindObjectsOfType<Button>();

        foreach (Button button in buttons)
            button.enabled = false;

        foreach (Button button in gameInfoButtons)
            button.enabled = true;

        yield return new WaitForSeconds(0.35f);

        previousSelection = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(gameInfoButtons[0].gameObject);
    }

    IEnumerator MenuClosed()
    {
        yield return new WaitForSeconds(0.35f);

        foreach (Button button in buttons)
            button.enabled = true;

        foreach (Button button in gameInfoButtons)
            button.enabled = false;

        EventSystem.current.SetSelectedGameObject(previousSelection);
    }

    public void OpenSubMenu(int i)
    {
        switch (i) {
            case 0:
                header.text = "About";
                body.text = selectedGame.description;
                controlsImage.enabled = false;
                controlsBody.text = "";
                break;

            case 1:
                header.text = "Credits";
                body.text = selectedGame.credits;
                controlsImage.enabled = false;
                controlsBody.text = "";
                break;

            case 2:
                header.text = "Controls";
                body.text = "";
                controlsImage.enabled = true;
                controlsBody.text = (selectedGame.controls?.Length > 0
                    ? selectedGame.controls : "Controls can be found in-game.");
                break;
        }
    }

    public void OpenClicked()
    {
        // prevent reading inputs from launched applications
        if (Manager.instance.inputDelay > 0)
            return;

        loadingScreen.SetActive(true);

        Invoke("OpenGame", 0.1f);
    }

    void RunGameFromFolder(string extension, string executable)
    {
        // save current directory
        // this will usually be \launcher\source or \launcher\build
        string wdir = System.IO.Directory.GetCurrentDirectory();

        // run game executable from games folder
        // need to do this for some older games that require logging
        try
        {
            System.IO.Directory.SetCurrentDirectory("..\\..\\games\\" + extension);
            Utils.Run("\"" + executable + "\"");
        }
        catch (System.IO.DirectoryNotFoundException)
        {
            Debug.LogWarning("Could not find the specified game path: \\games\\" + extension);
        }

        // reset wdir or else Unity will get mad
        System.IO.Directory.SetCurrentDirectory(wdir);
    }

    protected void OpenGame()
    {
        switch (selectedGame.gameProfile)
        {
            // overrides for special profiles
            case "buggygame":
                RunGameFromFolder("Buggy", "Buggy_2021_ver2.exe");
                break;

            case "DashEraser":
                RunGameFromFolder("DashEraserDust", "DED.exe");
                break;

            case "gamebytes2019":
                RunGameFromFolder("GameBytesF2019", "Game Bytes.exe");
                break;

            case "gamebytes2020":
                RunGameFromFolder("GameBytesF2020", "Game Bytes.exe");
                break;

            case "gamebytes2021":
                RunGameFromFolder("GameBytesF2021", "Game Bytes.exe");
                break;

            case "gamebytes2024":
                RunGameFromFolder("GameBytesF2024", "Game Bytes Fall 2024.exe");
                break;

            case "gamebytes2025":
                RunGameFromFolder("GameBytesS2025", "Game Bytes.exe");
                break;

            case "KnightNight":
                RunGameFromFolder("Knight Night", "KnightNight.exe");
                break;

            case "MonkCombat":
                RunGameFromFolder("MonkCombat.win32", "MK3.exe");
                break;

            case "TanksInAdvance":
                RunGameFromFolder("Tanks In Advance", "Tanks In Advance.exe");
                break;

            case "TanksOnPlanks":
                RunGameFromFolder("TanksOnPlanks", "Tanks On Planks.exe");
                break;

            case "VertoAnimo":
                RunGameFromFolder("VertoAnimo", "Verto Animo.exe");
                break;

            // default profile
            // run game from \games\GameName\GameName.exe
            // with no extra commands
            default:
                RunGameFromFolder(selectedGame.gameProfile, selectedGame.gameProfile + ".exe");
                break;
        }

        Manager.instance.inputDelay = 30;
        loadingScreen.SetActive(false);
        OpenInfoScreen(false);
    }
}
