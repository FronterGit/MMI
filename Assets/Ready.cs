using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ready : MonoBehaviour
{
    public TMP_Text gameName;
    public TMP_Text player;

    public void ChangeTexts(string gamename, string playername)
    {
        gameName.text = gamename;
        player.text = playername;
    }
}
