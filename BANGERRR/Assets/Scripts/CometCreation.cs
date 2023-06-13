using UnityEngine;

public class CometCreation : MonoBehaviour
{
    public GameObject cometPrefab; // Préfabriqué de la "Larme"
    bool canComet = true;
    void Start()
    {
    }

    void Update()
    {
        if(GetComponentInParent<SystemDayCounter>().hour==12 && canComet)
        {
            Instantiate(cometPrefab, transform.position, Quaternion.identity, transform.parent);
            canComet = false;

        }
        if(GetComponentInParent<SystemDayCounter>().hour==0)
        {
            canComet = true;
        }
    }
    
    void createComet()
    {

    }
}
