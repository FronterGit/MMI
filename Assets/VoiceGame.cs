using KKSpeech;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceGame : MonoBehaviour {
    [SerializeField] private List<string> gameSentences = new List<string>();
    [SerializeField] private SpeechRecognizerListener speechRecognizerListener;
    private System.Random random = new System.Random();

    // UI Components
    [SerializeField] private TMPro.TMP_Text firstSentence;
    [SerializeField] private TMPro.TMP_Text countdownText;
    [SerializeField] private TMPro.TMP_Text speechText;

    [SerializeField] private GameObject listenToResultsButton;

    [SerializeField] private List<Image> countdownImages = new();


    // Game variables
    [SerializeField] private int countdownBeforeRecording = 3;
    [SerializeField] private int gameDuration = 5;
    private List<string> players = new();

    private string textToSpeech;
    private string gameSentence;

    private bool firstEnable = true;


    private void OnEnable() {
        // if (firstEnable) {
        //     firstEnable = false;
        //     Debug.Log("This was the first enable.", this);
        //     return;
        // }
        Debug.Log("Hello World =D");
        initialize();
    }

    private void initialize() {
        listenToResultsButton.SetActive(false);
        countdownImages.ForEach(c =>
        {
            c.color = Color.red;
        });

        speechRecognizerListener.onFinalResults.AddListener(onFinalResult);
        speechRecognizerListener.onPartialResults.AddListener(onPartialResult);

        // get random index from list
        gameSentence = gameSentences[random.Next(gameSentences.Count)];
        firstSentence.SetText(gameSentence);

        // prepare countdown
        countdownText.SetText(countdownBeforeRecording.ToString());

        // start recording countdown
        StartCoroutine(startRecordingCountdown());

    }

    private IEnumerator startRecordingCountdown() {
        yield return new WaitForSeconds(1);
        // after the countdown is finished, disable the countdown text object
        for (int i = 0; i < countdownImages.Count; i++) {
            countdownImages[i].color = Color.green;
            yield return new WaitForSeconds(1);
        }

        countdownText.gameObject.SetActive(false);

        StartCoroutine(startRecording());
    }

    private IEnumerator startRecording() {
        SpeechRecognizer.StartRecording(false);

        yield return new WaitForSeconds(5);

        SpeechRecognizer.StopIfRecording();
        countdownImages.ForEach(c =>
        {
            c.color = Color.red;
        });
        
        listenToResultsButton.SetActive(true);
    }

    public void listenToResults() {
        
    }

    private void onPartialResult(string result) {
        Debug.Log("Partial result: " + result);
        speechText.SetText(result);
    }

    private void onFinalResult(string result) {
        Debug.Log("String result: " + result);
        speechText.SetText("Final result: " + result);
    }

}