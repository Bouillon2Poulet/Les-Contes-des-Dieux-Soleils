using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyInteractionManager : MonoBehaviour
{
    public static KeyInteractionManager instance { get; private set; }

    public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;
    GameObject[] Slots;

    [Header("Menu")]
    [SerializeField] Sprite menuSprite;
    [SerializeField] Sprite backSprite;

    [Header("Menu")]
    [SerializeField] Sprite cosmoguideSprite;

    [Header("Actions")]
    [SerializeField] Sprite genericActionSprite;
    [SerializeField] Sprite bubbleSprite;
    [SerializeField] Sprite talkSprite;
    [SerializeField] Sprite noteSprite;
    private Dictionary<int, Sprite> actionSprites = new Dictionary<int, Sprite>();

    [Header("None")]
    [SerializeField] Sprite noneSprite;

    class KeyIcon
    {
        public Sprite sprite;
        public bool activated = false;
        public KeyIcon(Sprite sprite)
        {
            this.sprite = sprite;
        }
    }

    KeyIcon MenuIcon;
    KeyIcon CosmoguideIcon;
    KeyIcon ActionIcon;
    private void InitKeyIcons()
    {
        MenuIcon = new KeyIcon(menuSprite);
        CosmoguideIcon = new KeyIcon(cosmoguideSprite);
        ActionIcon = new KeyIcon(genericActionSprite);
    }
    List<KeyIcon> Queue;
    private void InitActionSprites()
    {
        actionSprites[0] = genericActionSprite;
        actionSprites[1] = bubbleSprite;
        actionSprites[2] = talkSprite;
        actionSprites[3] = noteSprite;
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        InitKeyIcons();
        InitActionSprites();
        Queue = new List<KeyIcon>();
        Slots = new GameObject[] { slot1, slot2, slot3 };

        MenuIcon.activated = true;

        UpdateSlots();
    }

    public void UpdateSlots()
    {
        //Debug.Log("Updating key interaction slots");
        Queue.Clear();
        foreach (GameObject slot in Slots)
        {
            slot.GetComponent<Image>().sprite = noneSprite;
        }

        if (MenuIcon.activated)
            Queue.Add(MenuIcon);
        if (CosmoguideIcon.activated)
            Queue.Add(CosmoguideIcon);
        if (ActionIcon.activated)
            Queue.Add(ActionIcon);

        int slotIndex = 0;
        foreach (KeyIcon keyIcon in Queue)
        {
            Slots[slotIndex++].GetComponent<Image>().sprite = keyIcon.sprite;
        }
    }

    public void ChangeMenuIcon(bool inMenu)
    {
        MenuIcon.sprite = (inMenu) ? backSprite : menuSprite;
        UpdateSlots();
    }

    public void ToggleCosmoguideIcon(bool state)
    {
        CosmoguideIcon.activated = state;
        UpdateSlots();
    }

    public void ToggleActionIcon(int action, bool state)
    {
        ActionIcon.sprite = actionSprites.TryGetValue(action, out Sprite sprite) ? sprite : genericActionSprite;
        ActionIcon.activated = state;
        UpdateSlots();
    }
}
