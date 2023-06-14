using KKSpeech;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceGame : MonoBehaviour {
    [SerializeField] private List<string> gameSentences = new List<string>();
    private System.Random random = new System.Random();

    // UI Components
    [SerializeField] private TMPro.TMP_Text firstSentence;
    [SerializeField] private TMPro.TMP_Text countdownText;


    // Game variables
    [SerializeField] private int countdownBeforeRecording = 3;
    private List<string> players = new();

    private string textToSpeech;
    private string gameSentence;

    private bool firstEnable = true;


    private void OnEnable() {
        if (firstEnable) {
            firstEnable = false;
            return;
        }
        Debug.Log("Hello World =D");
        initialize();
    }

    private void initialize() {
        // get random index from list
        gameSentence = gameSentences[random.Next(gameSentences.Count)];
        firstSentence.SetText(gameSentence);

        // prepare countdown
        countdownText.SetText(countdownBeforeRecording.ToString());

        // start recording countdown
        StartCoroutine(startRecordingCountdown());

    }

    private IEnumerator startRecordingCountdown() {
        // after the countdown is finished, disable the countdown text object
        for (int i = countdownBeforeRecording; i > 0; i--) {
            countdownText.SetText(i.ToString());
            yield return new WaitForSeconds(1);
        }

        countdownText.SetText("GO!");
        yield return new WaitForSeconds(1);

        countdownText.gameObject.SetActive(false);
        
        startRecording();
    }

    private void startRecording() {
        SpeechRecognizer.StartRecording(false);
        
    }

}