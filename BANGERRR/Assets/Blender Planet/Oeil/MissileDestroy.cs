using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileDestroy : MonoBehaviour
{
    public bool invincible = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!invincible)
        {
            //Debug.Log("Collision !");
            GameObject otherGameObject = other.gameObject;
            if (otherGameObject.name == "Centre")
            {
                //Debug.Log("Centre touché");
                Centre.instance.Hit();
                DestroyAllAndSelf();
            }
            else if (otherGameObject.name == "Third Person Player")
            {
                //Debug.Log("Target touchée");
                PhaseManager.instance.hitByMissile = true;
                DestroyAllAndSelf();
            }
            else if (otherGameObject.name == "Paupière_up" || otherGameObject.name == "Paupière_down")
            {
                //Debug.Log("Paupière touchée");
                Centre.instance.Hit();
                DestroyAllAndSelf();
            }
        }
    }

    private void DestroyAllAndSelf()
    {
        MissileDestroy[] missiles = FindObjectsByType<MissileDestroy>(FindObjectsSortMode.None);
        foreach (MissileDestroy missile in missiles)
        {
            Destroy(missile.gameObject);
        }

        Destroy(gameObject);
    }
}

