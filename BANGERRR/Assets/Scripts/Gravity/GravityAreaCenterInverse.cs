using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAreaCenterInverse : GravityArea
{
    public override Vector3 GetGravityDirection(GravityBody _gravityBody)
    {
        return (_gravityBody.transform.position - transform.position).normalized;
    }
}
