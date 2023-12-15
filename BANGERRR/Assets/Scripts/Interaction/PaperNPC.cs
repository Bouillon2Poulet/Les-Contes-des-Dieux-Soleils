using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperNPC : MonoBehaviour, IInteractable
{
    private GameObject noteSprite;

    [Header("Rotation")]
    public float rotationSpeed = 100f;
    private float angle = 0f;

    [Header("Up and Down")]
    public float amplitude = .1f;
    public float frequency = .5f;
    private Vector3 startPos;

    [Header("Paper")]
    public GameObject Paper;

    bool justClosed = false;
    bool justOpened = false;

    private void FixedUpdate()
    {
        IdleRotateNote();
        IdleMoveNote();

        if (Input.GetKeyDown(KeyCode.E) && Paper.activeSelf && !justOpened)
        {
            Debug.Log("PAPER Close");
            AudioManager.instance.Play("paper");
            Paper.SetActive(false);
            PlayerStatus.instance.GameMenuCursor(false);
            PlayerStatus.instance.isAnimated = false;
            FindAnyObjectByType<ThirdPersonMovement>().unblockPlayerMoveInputs();
            justClosed = true;
            StartCoroutine(nameof(DisableJustClosed));
        }
    }

    IEnumerator DisableJustClosed()
    {
        yield return new WaitForSecondsRealtime(.1f);
        justClosed = false;
    }

    IEnumerator DisableJustOpened()
    {
        yield return new WaitForSecondsRealtime(.1f);
        justOpened = false;
    }

    private void IdleRotateNote()
    {
        angle += Time.fixedDeltaTime * rotationSpeed;
        noteSprite.transform.localRotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void IdleMoveNote()
    {
        float verticalOffset = amplitude * Mathf.Sin(Time.time * 2 * Mathf.PI * frequency);
        noteSprite.transform.localPosition = startPos + new Vector3(0f, verticalOffset, 0f);
    }

    private void Start()
    {
        noteSprite = GetComponentInChildren<SpriteRenderer>().gameObject;
        startPos = noteSprite.transform.localPosition;

        Paper.SetActive(false);
    }

    public void Interact()
    {
        if (!Paper.activeSelf && !justClosed)
        {
            Debug.Log("PAPER Open");
            AudioManager.instance.Play("paper");
            Paper.SetActive(true);
            PlayerStatus.instance.GameMenuCursor(true);
            PlayerStatus.instance.isAnimated = true;
            FindAnyObjectByType<ThirdPersonMovement>().blockPlayerMoveInputs();
            justOpened = true;
            StartCoroutine(nameof(DisableJustOpened));
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
