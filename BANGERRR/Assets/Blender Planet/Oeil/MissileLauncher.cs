using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public GameObject rocketPrefab;
    public List<GameObject> spawnPositions;
    public List<GameObject> spawnTargets;
    public GameObject target;
    public float speed = 10f;
    private bool triggered = false;

    public void Trigger()
    {
        triggered = true;
    }

    private void Update()
    {
        if (triggered)
        {
            triggered = false;

            Vector3 closestSpawnerPosition = GetClosestSpawnerPosition();

            GameObject rocket = Instantiate(rocketPrefab, closestSpawnerPosition, rocketPrefab.transform.rotation);

            Vector3 closestSpawnTarget = GetClosestSpawnTarget();

            StartCoroutine(SendHoming(rocket, closestSpawnTarget));
        }
    }

    private Vector3 GetClosestSpawnerPosition()
    {
        Vector3 targetPosition = target.transform.position;
        Vector3 closestPosition = spawnPositions[0].transform.position;
        float closestDistance = Vector3.Distance(targetPosition, closestPosition);

        foreach (GameObject spawner in spawnPositions)
        {
            float distanceToTarget = Vector3.Distance(targetPosition, spawner.transform.position);
            if (distanceToTarget < closestDistance)
            {
                closestDistance = distanceToTarget;
                closestPosition = spawner.transform.position;
            }
        }

        return closestPosition;
    }

    private Vector3 GetClosestSpawnTarget()
    {
        Vector3 targetPosition = target.transform.position;
        Vector3 closestPosition = spawnTargets[0].transform.position;
        float closestDistance = Vector3.Distance(targetPosition, closestPosition);

        foreach (GameObject spawnTarget in spawnTargets)
        {
            float distanceToTarget = Vector3.Distance(targetPosition, spawnTarget.transform.position);
            if (distanceToTarget < closestDistance)
            {
                closestDistance = distanceToTarget;
                closestPosition = spawnTarget.transform.position;
            }
        }

        return closestPosition;
    }

    public IEnumerator SendHoming(GameObject rocket, Vector3 firstTarget)
    {
        float goingUpTimer = 1f;
        rocket.transform.LookAt(firstTarget);
        float rocketRotationSpeed = 1.5f;

        Transform Targeter = rocket.transform.GetChild(0);
        Targeter.SetParent(null);

        Transform player = GameObject.Find("Third Person Player").transform;

        bool timerEnded = false;

        while (rocket != null)
        {
            // Rocket Going Forward
            rocket.transform.position += rocket.transform.forward * speed * Time.deltaTime;

            // Targeter management
            Targeter.position = rocket.transform.position;
            Targeter.LookAt(player);
            Quaternion targetRot = Targeter.rotation;

            // Rocket Rotation (only after end of timer)
            if (!timerEnded)
            {
                //Debug.Log("Missile Phase 1...");
                goingUpTimer -= 0.01f;
                if (goingUpTimer < 0)
                {
                    //Debug.Log("Missile Phase 2 begins");
                    timerEnded = true;
                    rocket.GetComponent<MissileDestroy>().invincible = false;
                }
            }
            else
            {
                //Debug.Log("Missile Phase 2...");
                rocket.transform.rotation = Quaternion.Slerp(rocket.transform.rotation, targetRot, rocketRotationSpeed * Time.deltaTime);
            }

            //debug
            //test_cube2.rotation = targetRot;
            //rocket.transform.rotation = Quaternion.Slerp(initialRot, targetRot, 1f);

            yield return null;
        }
    }
}
