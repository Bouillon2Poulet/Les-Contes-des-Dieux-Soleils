using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingTargetMore : MonoBehaviour
{
    public GameObject target;
    public float speed = 4.0f;

    void Update()
    {
        if (target != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        }
    }
}