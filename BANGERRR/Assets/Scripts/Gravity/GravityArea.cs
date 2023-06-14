using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class GravityArea : MonoBehaviour
{
    [SerializeField] private int _priority;
    public int Priority => _priority;
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<Collider>().isTrigger = true;
    }

    public abstract Vector3 GetGravityDirection(GravityBody _gravityBody);

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out GravityBody gravityBody))
        {
            gravityBody.AddGravityArea(this); // à implémenter dans gravity body
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out GravityBody gravityBody))
        {
            gravityBody.RemoveGravityArea(this); // à implémenter dans gravity body
        }
    }

    internal object GetGravityDirection(ThirdPersonMovement thirdPersonMovement)
    {
        throw new NotImplementedException();
    }
}
