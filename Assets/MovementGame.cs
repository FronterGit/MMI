using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MovementGame : MonoBehaviour
{
    public float x, y, z, w;
    public TMP_Text xText, yText,zText, wText;
    public TMP_Text countdownText;
    public int countdown;
    public Image[] LEDs;
    public Slider healthbar;

    public GameObject endScreen;
    public TMP_Text endText;

    private bool gameRunning = false;

    void OnEnable()
    {
        endScreen.SetActive(false);
        healthbar.value = healthbar.maxValue;
        Input.gyro.enabled = true;
        countdownText.text = "Ready " + GameManager.instance.currentPlayer + "?";
        //forloop through leds and set color to black
        for (int i = 0; i < LEDs.Length; i++)
        {
            LEDs[i].color = Color.black;
        }

    }
    private void Update()
    {
        //set gyro values
        x = Input.gyro.attitude.x;
        y = Input.gyro.attitude.y;
        z = Input.gyro.attitude.z;
        w = Input.gyro.attitude.w;

        //set the text to the gyro values
        xText.text = "X: " + x.ToString();
        yText.text = "Y: " + y.ToString();
        zText.text = "Z: " + z.ToString();
        wText.text = "W: " + w.ToString();

        if (gameRunning)
        {
            if (x < -0.1 || x > 0.1 || y < -0.1 || y > 0.1)
            {
                RedColor();
                Vibration.Vibrate();
                healthbar.value -= 1f;
            }
            else
            {
                GreenColor();
            }
        }
    }

    public void StartGame()
    {
        StartCoroutine(ChangeColor());
    }
    public void EndGame()
    {
        gameRunning = false;
        endScreen.SetActive(true);
        endText.text = GameManager.instance.currentPlayer + "your final score was: " + healthbar.value.ToString() + " out of " + healthbar.maxValue.ToString() + ". You must take " + (10 - Mathf.Ceil(healthbar.value / 10)) + " sips!";
    }

    IEnumerator ChangeColor()
    {
        RedColor();

        //wait a sec
        yield return new WaitForSeconds(1);

        //forloop through the LEDs and set the color to green
        for (int i = 0; i < LEDs.Length; i++)
        {
            LEDs[i].color = Color.green;
            yield return new WaitForSeconds(1);
        }
        StartCoroutine(Countdown());
        gameRunning = true;

    }

    public void RedColor()
    {
        //folorloop through the leds and set color to red
        for (int i = 0; i < LEDs.Length; i++)
        {
            LEDs[i].color = Color.red;
        }
    }

    public void GreenColor()
    {
        //forloop through the leds and set color to green
        for (int i = 0; i < LEDs.Length; i++)
        {
            LEDs[i].color = Color.green;
        }
    }

    //coroutine that does countdown
    IEnumerator Countdown()
    {
        for (int i = countdown; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        EndGame();
    }

}
