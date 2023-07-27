using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastJumpPosition : MonoBehaviour
{
    public static LastJumpPosition instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void SetParentPlanet(Transform parent)
    {
        transform.SetParent(parent);
    }
}
