using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// ATTENTION : RAPPEL
/// LE GRAVITYBODY N'EST PAS QUE POUR LE JOUEUR
/// </summary>

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    public float rotationSpeed = 3f;

    private Rigidbody _rigidbody;

    private bool isGravityForceApplied = true;

    private List<GravityArea> _gravityAreas;

    public Vector3 GravityDirection
    {
        get
        {
            if (_gravityAreas.Count == 0) return Vector3.zero;
            _gravityAreas.Sort((area1, area2) => area1.Priority.CompareTo(area2.Priority));
            return _gravityAreas.Last().GetGravityDirection(this).normalized;
        }
    }
    public float GravityForce
    {
        get
        {
            if (_gravityAreas.Count == 0) return 0;
            _gravityAreas.Sort((area1, area2) => area1.Priority.CompareTo(area2.Priority));
            return _gravityAreas.Last().GravityForce;
        }
    }
    public bool IsBreathable
    {
        get
        {
            if (_gravityAreas.Count == 0) return false;
            _gravityAreas.Sort((area1, area2) => area1.Priority.CompareTo(area2.Priority));
            return _gravityAreas.Last().IsBreathable;
        }
    }
    public Shader AreaShader
    {
        get
        {
            if (_gravityAreas.Count == 0) return null;
            _gravityAreas.Sort((area1, area2) => area1.Priority.CompareTo(area2.Priority));
            return _gravityAreas.Last().AreaShader;
        }
    }

    public Transform GravityTransform
    {
        get
        {
            if (_gravityAreas.Count == 0) return null;
            return _gravityAreas.Last().transform;
        }
    }

    public bool inGravityArea
    {
        get
        {
            if (_gravityAreas.Count == 0) return false;
            return true;
        }
    }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _gravityAreas = new List<GravityArea>();

        _rigidbody.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (isGravityForceApplied)
        {
            _rigidbody.AddForce(GravityDirection * (GravityForce * Time.fixedDeltaTime), ForceMode.Acceleration);
        }

        Quaternion upRotation = Quaternion.FromToRotation(transform.up, -GravityDirection);
        Quaternion newRotation = Quaternion.Lerp(_rigidbody.rotation, upRotation * _rigidbody.rotation, Time.fixedDeltaTime * rotationSpeed);
        _rigidbody.MoveRotation(newRotation);
    }

    public void AddGravityArea(GravityArea gravityArea)
    {
        _gravityAreas.Add(gravityArea);
    }

    public void RemoveGravityArea(GravityArea gravityArea)
    {
        _gravityAreas.Remove(gravityArea);
    }

    public void SetForceApplication(bool status)
    {
        isGravityForceApplied = status;
    }
}
