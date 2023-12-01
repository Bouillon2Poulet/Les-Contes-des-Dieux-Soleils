using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneEphemeralMessage : MonoBehaviour
{
    [SerializeField] string text;
    [SerializeField] string engText;
    [SerializeField] string actor;
    [SerializeField] string skin;
    [SerializeField] float duration;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(DialogManager.instance.EphemeralMessage(actor, text, engText, duration, skin));
    }
}
