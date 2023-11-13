using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBubble : MonoBehaviour
{
    public enum InteractionType
    {
        generic,
        bubble,
        talk,
        note
    }

    public GameObject bubble;
    public InteractionType interactionType;
    public bool Off = false;

    private static Rigidbody player;
    private bool previousToggleState = true;

    private void Start()
    {
        player = PlayerStatus.instance.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!Off)
        {
            bool toggle = Vector3.Distance(player.transform.position, transform.position) < GlobalVariables.Get<float>("interactRange") + 1;
            if (toggle != previousToggleState)
            {
                ToggleBubble(toggle);
                ToggleActionIcon(toggle);
            }
            previousToggleState = toggle;
        }
    }

    public void ToggleBubble(bool state)
    {
        bubble.SetActive(state);
    }

    public void ToggleActionIcon(bool state)
    {
        KeyInteractionManager.instance.ToggleActionIcon((int)interactionType, state);
    }

    public void TurnOff()
    {
        Off = true;
        ToggleBubble(false);
        ToggleActionIcon(false);
    }

    public void TurnOn()
    {
        Off = false;
    }
}
