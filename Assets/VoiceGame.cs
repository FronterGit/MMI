using KKSpeech;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VoiceGame : MonoBehaviour {
    [SerializeField] private List<string> gameSentences = new List<string>();
    [SerializeField] private SpeechRecognizerListener speechRecognizerListener;
    private System.Random random = new System.Random();

    // UI Components
    [SerializeField] private TMP_Text firstSentence;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private TMP_Text speechText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button nextPlayer;
    [SerializeField] private TMP_Text nextText;
    [SerializeField] private Slider countdownSlider;
    [SerializeField] private List<Image> countdownImages = new();


    // Game variables
    [SerializeField] private int countdownBeforeRecording = 3;
    [SerializeField] private int gameDuration = 5;
    [SerializeField] private List<string> playersQueue = new List<string>();
    [SerializeField] private string turnPlayer;
    [SerializeField] private string finalResult;
    [SerializeField] private GameObject end;
    [SerializeField] private TMP_Text endText;

    private string gameSentence;
    private bool firstEnable = true;

    private void OnEnable() {
        //make LEDs black
        countdownImages.ForEach(led => {
            led.color = Color.black;
        });

        //dupe players from game manager
        playersQueue = GameManager.instance.players;

        //set turn player
        turnPlayer = GameManager.instance.currentPlayer;

        //set next player text
        nextText.SetText("It is now" + turnPlayer + "'s turn.");

        //set slider value to game duration * 10
        countdownSlider.maxValue = gameDuration * 10;
    }

    public void Initialize() {

        // disable start button once it has been pressed
        startButton.gameObject.SetActive(false);

        // add listeners to speech recognizer
        speechRecognizerListener.onFinalResults.AddListener(onFinalResult);
        speechRecognizerListener.onPartialResults.AddListener(onPartialResult);

        // get random senctence from list. This is the sentence that the player will have to say
        gameSentence = gameSentences[random.Next(gameSentences.Count)];
        firstSentence.SetText(gameSentence);

        // start recording countdown
        StartCoroutine(startRecordingCountdown());
    }

    private IEnumerator startRecordingCountdown() {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < countdownImages.Count; i++) {
            countdownImages[i].color = Color.red;
            yield return new WaitForSeconds(1);
        }

        countdownText.gameObject.SetActive(false);

        StartCoroutine(startRecording());
        StartCoroutine(sliderDown());
    }

    private IEnumerator startRecording() {
        nextPlayer.gameObject.SetActive(false);
        SpeechRecognizer.StartRecording(true);
        Vibration.Vibrate();

        yield return new WaitForSeconds(gameDuration);

        SpeechRecognizer.StopIfRecording();
        nextPlayer.gameObject.SetActive(true);
        Vibration.Vibrate();
        countdownImages.ForEach(c =>
        {
            c.color = Color.black;
        });
        yield return new WaitForSeconds(1);

        //Handle next turn

        nextPlayer.gameObject.SetActive(true);

        playersQueue.Remove(turnPlayer);

        //if the list is empty, end the game
        if (playersQueue.Count == 0) {
            EndGame();
        } 
        else 
        {
            turnPlayer = playersQueue[Random.Range(0, playersQueue.Count)];
            nextText.SetText("It is now " + turnPlayer + "'s turn.");
        }

    }

    private IEnumerator sliderDown() {
        //set slider to max value
        countdownSlider.value = countdownSlider.maxValue;

        //remove one value every 0.1 seconds
        while (countdownSlider.value > 0) {
            countdownSlider.value -= 1;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void onPartialResult(string result) {
        Debug.Log("Partial result: " + result);
        speechText.SetText(result);
    }

    private void onFinalResult(string result) {
        Debug.Log("String result: " + result);
        speechText.SetText("[Your result here]");
        firstSentence.text = result;
    }

    public void RecordNext() {
        StartCoroutine(startRecordingCountdown());
    }

    public void EndGame() {
        finalResult = firstSentence.text;
        end.SetActive(true);
        string[] gameSentenceArray = gameSentence.Split(' ');
        string[] resultSentenceArray = finalResult.Split(' ');

        //turn the arrays into lists
        List<string> gameSentenceList = new List<string>(gameSentenceArray);
        List<string> resultSentenceList = new List<string>(resultSentenceArray);

        int penalties = gameSentenceList.Count;
        //compare game sentence to result sentence and remove words that are the same
        foreach (string word in gameSentenceList) {
            if (resultSentenceList.Contains(word)) {
                penalties--;
            }
        }

        endText.SetText("The original sentence was: " + gameSentence + "Your sentence was:" + finalResult + ". Your sentence was " + penalties + " words different! Everyone takes that many sips.");
    }

}