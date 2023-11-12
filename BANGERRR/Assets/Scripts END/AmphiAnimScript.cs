using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmphiAnimScript : MonoBehaviour
{
    public static bool laserHasHitSoleilRouge;
    public static bool finito;

    public void HitSoleilROuge()
    {
        laserHasHitSoleilRouge = true;
    }

    public void CestFinito()
    {
        finito = true;
    }
}
