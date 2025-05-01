using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameButton : MonoBehaviour
{
    public TMP_Text gameNameText;

    public string gameProfile;
    public string gameName;

    public string playerCount;
    public string year;
    public string genre;

    [TextArea(4, 10)]
    public string description;
    [TextArea(4, 10)]
    public string credits;
    [TextArea(4, 10)]
    public string controls;

    private void Start()
    {
        gameNameText.text = gameName;
    }

    public void GameSelected()
    {
        Manager.instance.OpenInfoScreen(this);
    }
}
