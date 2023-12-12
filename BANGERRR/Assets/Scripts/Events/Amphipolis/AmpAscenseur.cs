using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmpAscenseur : MonoBehaviour
{
    public Transform elevator;
    public Transform elevatorUpPos;
    public Transform elevatorDownPos;
    public Transform door;
    public Transform doorUpPos;
    public Transform doorDownPos;

    public GameObject intGAs;
    private bool intGAsHaveBeenActivated;

    private ThirdPersonMovement playerMovement;

    private bool openTheDoor = false;
    private float doorOpeningProgression = 0;
    [SerializeField] private float doorOpeningSpeed = .01f;
    private bool doorIsDown = false;

    private bool takeElevatorDown = false;
    private float elevatorProgression = 0;
    [SerializeField]  private float elevatorSpeed = .001f;
    private bool elevatorIsDown = false;

    private void FixedUpdate()
    {
        if (openTheDoor && !doorIsDown)
        {
            door.position = Vector3.Lerp(doorUpPos.position, doorDownPos.position, doorOpeningProgression);
            doorOpeningProgression += doorOpeningSpeed;
            if (doorOpeningProgression >= 1)
            {
                doorIsDown = true;
            }
        }
        if (takeElevatorDown && !elevatorIsDown)
        {
            if (!intGAsHaveBeenActivated)
            {
                intGAs.SetActive(true);
                AmpSoleilRougeDialogue.instance.gameObject.SetActive(false);
                intGAsHaveBeenActivated = true;
                AmpNPCManager.instance.ToggleInteriorObjects(true);
                AudioManager.instance.FadeOut("amphipolis", 120);
                AudioManager.instance.FadeIn("amphipolis_int", 120);
                playerMovement.gameObject.transform.SetParent(elevator);
            }

            playerMovement.blockPlayerMoveInputs();

            elevator.position = Vector3.Lerp(elevatorUpPos.position, elevatorDownPos.position, elevatorProgression);
            elevatorProgression += elevatorSpeed;
            if (elevatorProgression >= 1)
            {
                elevatorIsDown = true;
                AudioManager.instance.Stop("elevatorloop");
                AudioManager.instance.Play("elevatordone");
                playerMovement.unblockPlayerMoveInputs();
                playerMovement.gameObject.transform.SetParent(null);
            }
        }
    }

    public void OpenTheDoor()
    {
        AudioManager.instance.Play("hydraulicdown");
        openTheDoor = true;
    }

    public void TakeElevatorDown()
    {
        takeElevatorDown = true;
        AudioManager.instance.Play("elevatorloop");
    }

    private void Start()
    {
        playerMovement = FindAnyObjectByType<ThirdPersonMovement>();
    }

    public static AmpAscenseur instance { get; private set; }
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
