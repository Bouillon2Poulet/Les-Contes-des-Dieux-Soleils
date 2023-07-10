using UnityEngine;
using TMPro;

public class SystemDayCounter : MonoBehaviour
{
    public float oneDayDurationInIRLSeconds = 86400f;
    public TextMeshProUGUI timeText;

    public float systemTime;
    private int dayCounter;
    public int hour;

    public float offset;

    void Update()
    {
        float appTime = Time.time;
        float convertFactor = 86400f / oneDayDurationInIRLSeconds;
        systemTime = ((appTime  + offset) * convertFactor) % 86400;
        dayCounter = (int)((appTime) * convertFactor) / 86400;
        hour = (int)(systemTime / 3600f);
        float seconds = systemTime - (hour * 60f);

        string timeString = string.Format("Jour {0} - {1}h", dayCounter, hour);
        timeText.text = timeString;
    }
}
