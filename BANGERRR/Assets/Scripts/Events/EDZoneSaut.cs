using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDZoneSaut : MonoBehaviour
{
    public Transform gaSolisede;
    public bool isDesaut;

    float s = 130f;
    float d = 50f;

    private void OnTriggerEnter(Collider other)
    {
        if (isDesaut)
            gaSolisede.localScale = new Vector3(d, d, d);
        else
            gaSolisede.localScale = new Vector3(s, s, s);
    }
}
