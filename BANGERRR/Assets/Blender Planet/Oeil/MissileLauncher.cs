using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public GameObject rocketPrefab;
    public List<GameObject> spawnPositions;
    public GameObject target;
    public float speed = 1f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("launch");
            Vector3 closestSpawnerPosition = GetClosestSpawnerPosition();
            GameObject rocket = Instantiate(rocketPrefab, closestSpawnerPosition, rocketPrefab.transform.rotation);
            StartCoroutine(SendHoming(rocket));
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

    public IEnumerator SendHoming(GameObject rocket)
    {
        while (rocket != null)
        {
            rocket.transform.position += rocket.transform.forward * speed * Time.deltaTime;
            rocket.transform.LookAt(target.transform);
            yield return null;
        }
    }
}
