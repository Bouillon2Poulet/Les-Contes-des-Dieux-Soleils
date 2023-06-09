using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCenter : MonoBehaviour
{
    public GameObject center;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = center.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = center.transform.position;
    }
}
