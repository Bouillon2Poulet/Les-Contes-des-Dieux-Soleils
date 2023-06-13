using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometOrbitGrow : MonoBehaviour
{
    float minSemiMajorAxis;
    float minSemiMinorAxis;
    float startTime;

    public float factor;
    // Start is called before the first frame update
    void Start()
    {
        minSemiMajorAxis = GetComponent<SimpleEllipseRotation>().semiMajorAxis;
        minSemiMinorAxis = GetComponent<SimpleEllipseRotation>().semiMinorAxis;
        startTime = GetComponentInParent<SystemDayCounter>().systemTime;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SimpleEllipseRotation>().semiMajorAxis = ((GetComponentInParent<SystemDayCounter>().systemTime*minSemiMajorAxis)/startTime);
        GetComponent<SimpleEllipseRotation>().semiMinorAxis = ((GetComponentInParent<SystemDayCounter>().systemTime*minSemiMinorAxis)/startTime);
    }
}
