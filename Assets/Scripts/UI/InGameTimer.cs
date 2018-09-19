using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InGameTimer : MonoBehaviour
{

    public Text timerText;

    private int _minutes = 0;
    private int _seconds = 0;

    void Start()
    {
        RefreshTimer();
    }

    public void StartTimer()
    {
        RefreshTimer();

        DisplayTimerText();

        StartCoroutine(Timer());
    }

    public void StopTimer()
    {
        StopAllCoroutines();
    }

    public void RefreshTimer()
    {
        _minutes = 0;
        _seconds = 0;

        DisplayTimerText();
    }

    private void DisplayTimerText()
    {
        string timeText = string.Format("{0}:{1}", _minutes.ToString("D2"), _seconds.ToString("D2"));
        timerText.text = timeText;
    }

    private IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            _seconds++;

            if(_seconds == 60)
            {
                _seconds = 0;
                _minutes++;
            }

            DisplayTimerText();
        }
    }
}
