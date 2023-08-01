using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmpElevatorTrigger : MonoBehaviour
{
    public GameObject bubble;

    private bool playerIsThere = false;

    private void Start()
    {
        bubble.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("IN");
            playerIsThere = true;
            bubble.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("OUT");
            playerIsThere = false;
            bubble.SetActive(false);
        }
    }

    public bool isPlayerThere()
    {
        return playerIsThere;
    }

    public static AmpElevatorTrigger instance { get; private set; }
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
}
