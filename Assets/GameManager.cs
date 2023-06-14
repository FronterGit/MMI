using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public GameObject gameCreator;
    public GameObject ready;

    public List<GameObject> games = new List<GameObject>();

    public List<string> players = new List<string>();
    private string currentGame;
    private string currentPlayer;

    public void ReadyCheck()
    {
        ready.SetActive(true);

        //initialize new string array equal to the length of the tmp text list
        string[] playersQueue = gameCreator.GetComponent<GameCreator>().ReturnPlayers();

        //for loop through the list of player names in the game creator and add the tmp text to the players list
        for (int i = 0; i < playersQueue.Length; i++)
        {
            if (playersQueue[i] != "") players.Add(playersQueue[i]);
        }

        //select random game and player
        int randomGame = Random.Range(0, games.Count);
        int randomPlayer = Random.Range(0, players.Count);

        //set current game and player to the randomly selected game and player
        currentGame = games[randomGame].name;
        games[randomGame].SetActive(false);
        currentPlayer = players[randomPlayer];

        //set the ready screen text to the according game and player names
        ready.GetComponent<Ready>().ChangeTexts(currentGame, currentPlayer);
    }

    public void StartGame()
    {
        ready.SetActive(false);
        //take the gamename and loop through the game list to see if the name matches any of the game names
        for (int i = 0; i < games.Count; i++)
        {
            if (games[i].name == currentGame)
            {
                //if the name matches, set the game to active
                games[i].SetActive(true);
            }
            else
            {
                //if the name does not match, set the game to inactive
                games[i].SetActive(false);
            }
        }
    }

    public void ClearGame()
    {
        players.Clear();
    }
}
