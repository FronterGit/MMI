using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;

public class GameCreator : MonoBehaviour
{
    //initialize public list of tmp text objects
    public List<TMP_Text> tmpTextList = new List<TMP_Text>();
    public GameObject Input;
    public TMP_InputField inputField;

    public int listPos;

    private void Start()
    {
        for(int i = 0; i < tmpTextList.Count; i++)
        {
            tmpTextList[i].text = "";
        }
        listPos = -1;
    }

    public void AddPlayer()
    {
        Input.SetActive(true);
        inputField.text = "";
        listPos++;
    }
    public void ConfirmAddPlayer()
    {
        tmpTextList[listPos].text = inputField.text;
        Input.SetActive(false);
    }

    public void RemovePlayer()
    {
        tmpTextList[listPos].text = "";
        listPos--;
    }

    //public function that returns the tmp text contents of the tmpTextList as a string array
    public string[] ReturnPlayers()
    {
        string[] players = new string[tmpTextList.Count];
        for(int i = 0; i < tmpTextList.Count; i++)
        {
            players[i] = tmpTextList[i].text;
        }
        return players;
    }
}