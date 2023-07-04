using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleurbulleManager : MonoBehaviour
{
    private Fleurbulle[] fleurbulles;

    // Start is called before the first frame update
    void Awake()
    {
        fleurbulles = FindObjectsByType<Fleurbulle>(FindObjectsSortMode.None);
    }

    public void resetFleurbulles()
    {
        foreach (Fleurbulle fleurbulle in fleurbulles)
        {
            if (!fleurbulle.isBubbleAvailable)
            {
                fleurbulle.retrieveBubble();
            }
        }
    }
}
