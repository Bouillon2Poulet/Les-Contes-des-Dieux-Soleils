using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOmnio : MonoBehaviour
{
    public void DoIt()
    {
        transform.gameObject.SetActive(false);
    }
}
