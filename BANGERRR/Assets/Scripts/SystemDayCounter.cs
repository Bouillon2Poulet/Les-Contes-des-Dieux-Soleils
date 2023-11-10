using UnityEngine;
using TMPro;

public class SystemDayCounter : MonoBehaviour
{
    public float oneDayDurationInIRLSeconds = 600f;
    private float pauseValue = 1048575f;
    private float goingValue;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI debugTime; // TO DELETE
    public TextMeshProUGUI debugTimeMin; // TO DELETE

    public float systemTime;
    private int dayCounter;
    public int hour;
    public int minutes;

    public float offset;
    [Header("Debug")]
    public bool ConstantSpeedUpdate = false;

    private float TimeAtWhichTheSceneStarted;

    private void Awake()
    {
        goingValue = oneDayDurationInIRLSeconds;
        TimeAtWhichTheSceneStarted = Time.time;
        /// DEBUG ONLY, TO DELETE
        /*goingValue = 60f;
        UpdateSystemSpeed(goingValue);*/
    }

    void Update()
    {
        float appTime = Time.time - TimeAtWhichTheSceneStarted;
        float convertFactor = 86400f / oneDayDurationInIRLSeconds;
        systemTime = ((appTime  + offset) * convertFactor) % 86400;
        dayCounter = (int)((appTime) * convertFactor) / 86400;
        hour = (int)(systemTime / 3600f);
        minutes = (int)(systemTime / 60f);

        string timeString = string.Format("Jour {0} - {1}h", dayCounter, hour);
        timeText.text = timeString;
        debugTime.text = ""+ hour; // DEBUG - TO DELETE
        debugTimeMin.text = "" + minutes; // DEBUG - TO DELETE

        if (ConstantSpeedUpdate)
        {
            goingValue = oneDayDurationInIRLSeconds;
            UpdateSystemSpeed(goingValue);
        }
    }

    private void UpdateSystemSpeed(float speed)
    {
        oneDayDurationInIRLSeconds = speed;
        var ellispseScripts = FindObjectsByType<SimpleEllipseRotationTristan>(FindObjectsSortMode.None);
        foreach (var script in ellispseScripts)
        {
            script.updateVitesseRadiale();
        }
    }

    public void pauseSystem()
    {
        UpdateSystemSpeed(pauseValue);
    }

    public void resumeSystem()
    {
        UpdateSystemSpeed(goingValue);
    }
}
