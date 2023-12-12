using UnityEngine;
using TMPro;

public class SystemDayCounter : MonoBehaviour
{
    public static SystemDayCounter instance;

    public float oneDayDurationInIRLSeconds = 600f;
    private float pauseValue = 1048575f;
    private float speedUpValue = 60f;
    private float goingValue;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI debugTime; // TO DELETE
    public TextMeshProUGUI debugTimeMin; // TO DELETE

    public float systemTime;
    private int dayCounter;
    public int hour;
    public int minutes;

    private string dayString;

    public float offset;
    [Header("Debug")]
    public bool ConstantSpeedUpdate = false;

    private float TimeAtWhichTheSceneStarted;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        goingValue = oneDayDurationInIRLSeconds;
        TimeAtWhichTheSceneStarted = Time.time;
        /// DEBUG ONLY, TO DELETE
        /*goingValue = 60f;
        UpdateSystemSpeed(goingValue);*/
    }

    private void Start()
    {
        LanguageManager.Lang lang = (LanguageManager.Lang)GlobalVariables.Get<int>("lang");
        if (lang == LanguageManager.Lang.French)
        {
            dayString = "Jour";
        }
        else
        {
            dayString = "Day";
        }
    }

    void Update()
    {
        float appTime = Time.time - TimeAtWhichTheSceneStarted;
        float convertFactor = 86400f / oneDayDurationInIRLSeconds;
        systemTime = ((appTime  + offset) * convertFactor) % 86400;
        dayCounter = (int)((appTime) * convertFactor) / 86400;
        hour = (int)(systemTime / 3600f);
        minutes = (int)(systemTime / 60f);

        string minutesString = (minutes%60 < 10) ? "0" + minutes%60 : "" + minutes%60;
        string timeString = string.Format(dayString + " {0} - {1} h {2}", dayCounter, hour, minutesString);
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

    public void SpeedUpSystem()
    {
        if (!LarmeToAmphipolis.instance.IsAnimated())
            UpdateSystemSpeed(speedUpValue);
    }
}
